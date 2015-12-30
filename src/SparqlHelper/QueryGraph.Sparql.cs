/*
------------------------------------------------------------------------------
This source file is a part of SparqlHelper.

Copyright (c) 2015 VIS/University of Stuttgart

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
------------------------------------------------------------------------------
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SparqlHelper.Filters;
using SparqlHelper.SparqlConversion;

namespace SparqlHelper
{
	partial class QueryGraph
	{
		public bool IsEmpty {
			get {
				return (statements.Count <= 0)
					&& alternativeGraphs.All(graphs => graphs.All(g => g.IsEmpty));
			}
		}
		
		public string ToSparqlString(SparqlSettings settings, ISet<Variable> exportedVariables)
		{
			return ToSparqlString(settings, exportedVariables, "");
		}
		
		public string ToSparqlString(SparqlSettings settings, ISet<Variable> exportedVariables, string indentation)
		{
			if (settings == null) {
			    throw new ArgumentNullException("settings");
			}
			
			var result = new StringBuilder();
			ToSparqlString(settings, result, exportedVariables, new HashSet<Variable>(), indentation);
			return result.ToString();
		}
		
		private void ToSparqlString(SparqlSettings settings, StringBuilder dest, ISet<Variable> exportedVariables, ISet<Variable> subqueriedVars, string indentation)
		{
			QueryGraph g = new QueryGraph();
			g.UseSameData(this);
			
			var restrictedVars = g.LocalNamedThings.OfType<Variable>().Distinct().Where(v => v.IsRestricted).Where(v => !subqueriedVars.Contains(v)).ToArray();
			switch (restrictedVars.Length) {
				case 0:
					break;
				case 1:
					{
						var variable = restrictedVars[0];
						
						var varR = variable.Restrictions;
						variable.Restrictions = null;
						try {
							var subQuery = new SelectQuery();
							subQuery.Restrictions.CopyFrom(varR);
							subQuery.Graph.UseSameData(this);
							var newOptionalAlternativeIndices = new List<int>();
							for (int i = 0; i < subQuery.Graph.alternativeGraphs.Count; i++) {
								if (subQuery.Graph.optionalAlternativeIndices.Contains(i)) {
									var possiblyObsoleteGraphs = subQuery.Graph.alternativeGraphs[i];
									var namedThingsInPossiblyObsoleteGraphs = new HashSet<NamedThing>();
									foreach (var pog in possiblyObsoleteGraphs) {
										foreach (var nt in pog.AllNamedThings) {
											namedThingsInPossiblyObsoleteGraphs.Add(nt);
										}
									}
									
									var namedThingsNotInPossiblyObsoleteGraphs = new HashSet<NamedThing>();
									foreach (var stmt in subQuery.Graph.statements) {
										foreach (var nt in stmt.References) {
											namedThingsNotInPossiblyObsoleteGraphs.Add(nt);
										}
									}
									foreach (var otherAltGraphs in subQuery.Graph.alternativeGraphs) {
										if (otherAltGraphs != possiblyObsoleteGraphs) {
											foreach (var otherAltGraph in otherAltGraphs) {
												foreach (var nt in otherAltGraph.AllNamedThings) {
													namedThingsNotInPossiblyObsoleteGraphs.Add(nt);
												}
											}
										}
									}
									
									namedThingsInPossiblyObsoleteGraphs.IntersectWith(namedThingsNotInPossiblyObsoleteGraphs);
									if (namedThingsInPossiblyObsoleteGraphs.Count < 2) {
										subQuery.Graph.alternativeGraphs.RemoveAt(i);
										i--;
									} else {
										newOptionalAlternativeIndices.Add(i);
									}
								}
							}
							subQuery.Graph.optionalAlternativeIndices.Clear();
							foreach (int newIdx in newOptionalAlternativeIndices) {
								subQuery.Graph.optionalAlternativeIndices.Add(newIdx);
							}
							subQuery.ResultColumns.Add(new ColumnExpressions.VariableColumn(variable));
							
							dest.AppendLine(indentation + "{");
							dest.Append(subQuery.ToSparqlString(settings, false, indentation + settings.BlockIndentation));
							dest.AppendLine(indentation + "}.");
						}
						finally {
							variable.Restrictions = varR;
						}
						
						subqueriedVars.Add(variable);
						
						var ignore = subqueriedVars.ToArray();
						for (int i = g.statements.Count - 1; i >= 0; i--) {
							if (!g.ReachableNamedThings(g.statements[i], ignore).OfType<Variable>().Any(exportedVariables.Contains)) {
								g.statements.RemoveAt(i);
							}
						}
					}
					break;
				default:
					throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture,
					                                                  "Only one variable can be restricted, but the graph contains {0}.",
					                                                  restrictedVars.Length));
			}
			
			var filterConjunction = new List<Expressions.Expression>();
			foreach (var stmt in g.statements) {
				var f = stmt as Filters.ExpressionFilter;
				if (f == null) {
					dest.AppendLine(indentation + stmt.ToSparqlString(settings));
				} else {
					filterConjunction.Add(f.Expression);
				}
			}
			if (filterConjunction.Count > 0) {
				var f = new Filters.ExpressionFilter(new Expressions.AndExpression(filterConjunction));
				dest.AppendLine(indentation + f.ToSparqlString(settings));
			}
			
			// simplify subgraphs to combined filter expressions, where possible
			var altGraphs = new List<Tuple<QueryGraph[], Expressions.Expression[], bool>>();
			var altFilters = new List<Expressions.Expression>();
			
			for (int i = 0; i < g.alternativeGraphs.Count; i++) {
				var alternatives = g.alternativeGraphs[i].Where(gr => !gr.IsEmpty).ToArray();
				if (alternatives.Length > 0) {
					bool isOptional = g.optionalAlternativeIndices.Contains(i);
					
					var graphs = new List<QueryGraph>();
					var filterGraphs = new List<QueryGraph>();
					foreach (var alt in alternatives) {
						if (alt.ContainsOnlyExpressionFilters) {
							filterGraphs.Add(alt);
						} else {
							graphs.Add(alt);
						}
					}
					
					var filters = filterGraphs.Select(fg => fg.AsFilterExpression());
					if (graphs.Count > 0) {
						altGraphs.Add(new Tuple<QueryGraph[], Expressions.Expression[], bool>(graphs.ToArray(), filters.ToArray(), isOptional));
					} else {
						if (!isOptional) {
							altFilters.Add(new Expressions.OrExpression(filters));
						}
					}
				}
			}
			
			// output subgraph contents
			foreach (var ag in altGraphs) {
				QueryGraph[] alternatives;
				if (ag.Item2.Length > 0) {
					var qg = new QueryGraph();
					qg.Add(new Filters.ExpressionFilter(new Expressions.OrExpression(ag.Item2)));
					alternatives = ag.Item1.Concat(new QueryGraph[] { qg }).ToArray();
				} else {
					alternatives = ag.Item1;
				}
				
				if (ag.Item3) {
					dest.AppendLine(indentation + "OPTIONAL {");
					AlternativesToSparqlString(alternatives, settings, dest, exportedVariables, subqueriedVars, indentation + settings.BlockIndentation);
					dest.AppendLine(indentation + "}.");
				} else {
					AlternativesToSparqlString(alternatives, settings, dest, exportedVariables, subqueriedVars, indentation);
				}
			}
			
			if (altFilters.Count> 0) {
				var f = new Filters.ExpressionFilter(new Expressions.AndExpression(altFilters));
				dest.AppendLine(indentation + f.ToSparqlString(settings));
			}
		}
		
		private static void AlternativesToSparqlString(QueryGraph[] alternatives, SparqlSettings settings, StringBuilder dest, ISet<Variable> exportedVariables, ISet<Variable> subqueriedVars, string indentation)
		{
			var nextSubqueriedVars = new HashSet<Variable>();
			
			for (int j = 0; j < alternatives.Length; j++) {
				string subgraphBodyIndentation;
				
				var currentSubqueriedVars = new HashSet<Variable>(subqueriedVars);
				
				if (j > 0) {
					dest.AppendLine(" UNION {");
					subgraphBodyIndentation = indentation + settings.BlockIndentation;
				} else {
					if (alternatives.Length > 1) {
						dest.AppendLine(indentation + "{");
						subgraphBodyIndentation = indentation + settings.BlockIndentation;
					} else {
						subgraphBodyIndentation = indentation;
					}
				}
				var g = alternatives[j];
				g.ToSparqlString(settings, dest, exportedVariables, currentSubqueriedVars, subgraphBodyIndentation);
				if (alternatives.Length > 1) {
					dest.Append(indentation + "}");
				}
				
				nextSubqueriedVars.UnionWith(currentSubqueriedVars);
			}
			if (alternatives.Length > 1) {
				dest.AppendLine(".");
			}
			
			subqueriedVars.UnionWith(nextSubqueriedVars);
		}
	}
}
