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
using System.Collections.Generic;
using System.Linq;

using SparqlHelper.PropertyPaths;
using SparqlHelper.Filters;
using SparqlHelper.Expressions;

namespace SparqlHelper
{
	/// <summary>
	/// A graph that can be used in a query.
	/// </summary>
	public sealed partial class QueryGraph
	{
		/// <summary>
		/// Enumerates all <see cref="NamedThing">named things</see> locally referenced in this graph.
		/// </summary>
		/// <value>
		/// <para>This property returns an enumeration of all named things that are referenced in this graph.
		///   Named things in subgraphs are not considered.
		///   To retrieve those, use the <see cref="AllNamedThings"/> property.</para>
		/// <para>Note that the resulting enumeration may contain duplicates.</para>
		/// </value>
		/// <seealso cref="AllNamedThings"/>
		public IEnumerable<NamedThing> LocalNamedThings {
			get {
				foreach (var stmt in statements) {
					foreach (var nt in stmt.References) {
						yield return nt;
					}
				}
			}
		}
		
		/// <summary>
		/// Enumerates all <see cref="NamedThing">named things</see> referenced in this graph or any of its subgraphs.
		/// </summary>
		/// <value>
		/// <para>This property returns an enumeration of all named things in this graph and any of its subgraphs.
		///   To consider only named things used in this graph, use the <see cref="LocalNamedThings"/> property.</para>
		/// <para>Note that the resulting enumeration may contain duplicates.</para>
		/// </value>
		/// <seealso cref="LocalNamedThings"/>
		public IEnumerable<NamedThing> AllNamedThings {
			get {
				foreach (var nt in LocalNamedThings) {
					yield return nt;
				}
				foreach (var a in alternativeGraphs) {
					foreach (var g in a) {
						foreach (var nt in g.AllNamedThings) {
							yield return nt;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// The internal list of statements in the graph.
		/// </summary>
		private readonly List<GraphStatement> statements = new List<GraphStatement>();
		
		public void Add(GraphStatement statement)
		{
			if (statement == null) {
			    throw new ArgumentNullException("statement");
			}
			
			statements.Add(statement);
		}
		
		public IEnumerable<NamedThing> ReachableNamedThings(GraphStatement fromStatement, params NamedThing[] ignoredNamedThings)
		{
			if (fromStatement == null) {
			    throw new ArgumentNullException("fromStatement");
			}
			if (ignoredNamedThings == null) {
			    throw new ArgumentNullException("ignoredNamedThings");
			}
			
			var ignoredNTs = new HashSet<NamedThing>(ignoredNamedThings.Where(nt => nt != null));
			
			var reachableNTs = new HashSet<NamedThing>(fromStatement.References);
			reachableNTs.RemoveWhere(ignoredNTs.Contains);
			
			var allReferences = statements.Where(s => s != fromStatement).Select(s => new HashSet<NamedThing>(s.References)).Where(r => r.Count > 0).ToList();
			
			for (int i = allReferences.Count - 1; i >= 0; i--) {
				var refs = allReferences[i];
				if (refs.Any(reachableNTs.Contains)) {
					refs.RemoveWhere(ignoredNTs.Contains);
					reachableNTs.UnionWith(refs);
					allReferences.RemoveAt(i);
					i = allReferences.Count - 1;
				}
			}
			
			return reachableNTs.Select(nt => nt);
		}
		
		#region triples
		/// <summary>
		/// Adds a new triple to the graph.
		/// </summary>
		/// <param name="subject">The triple subject.</param>
		/// <param name="predicate">The triple predicate.</param>
		/// <param name="object">The triple object.</param>
		/// <returns>The newly added triple.</returns>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public Triple AddTriple(PropertyPathEnd subject, TriplePredicate predicate, PropertyPathEnd @object)
		{
			Triple newTriple = new Triple(subject, predicate, @object);
			newTriple.Owner = this;
			statements.Add(newTriple);
			return newTriple;
		}
		
		/// <summary>
		/// Adds a new triple, whose subject is a variable reference, to the graph.
		/// </summary>
		/// <param name="subject">The triple subject.</param>
		/// <param name="predicate">The triple predicate.</param>
		/// <param name="object">The triple object.</param>
		/// <returns>The newly added triple.</returns>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public Triple AddTriple(Variable subject, TriplePredicate predicate, PropertyPathEnd @object)
		{
			return AddTriple(new VariableEnd(subject),
			                 predicate,
			                 @object);
		}
		
		/// <summary>
		/// Adds a new triple, whose object is a variable reference, to the graph.
		/// </summary>
		/// <param name="subject">The triple subject.</param>
		/// <param name="predicate">The triple predicate.</param>
		/// <param name="object">The triple object.</param>
		/// <returns>The newly added triple.</returns>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public Triple AddTriple(PropertyPathEnd subject, TriplePredicate predicate, Variable @object)
		{
			return AddTriple(subject,
			                 predicate,
			                 new VariableEnd(@object));
		}
		
		/// <summary>
		/// Adds a new triple, whose subject and object are variable references, to the graph.
		/// </summary>
		/// <param name="subject">The triple subject.</param>
		/// <param name="predicate">The triple predicate.</param>
		/// <param name="object">The triple object.</param>
		/// <returns>The newly added triple.</returns>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public Triple AddTriple(Variable subject, TriplePredicate predicate, Variable @object)
		{
			return AddTriple(new VariableEnd(subject),
			                 predicate,
			                 new VariableEnd(@object));
		}
		
		/// <summary>
		/// Ensures that a triple consisting of a particular variable, a given property path and any other variable is a part of the graph.
		/// </summary>
		/// <param name="fromVariable">The variable that will be used as the triple subject.</param>
		/// <param name="path">The property path that forms the triple predicate.</param>
		/// <returns>The variable that serves as the triple object.</returns>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public Variable AddVariableTriple(Variable fromVariable, PropertyPath path)
		{
			if (fromVariable == null) {
			    throw new ArgumentNullException("fromVariable");
			}
			if (path == null) {
			    throw new ArgumentNullException("path");
			}
			
			foreach (var t in statements.OfType<Triple>()) {
				var varSubject = t.Subject as VariableEnd;
				if (varSubject != null) {
					if (varSubject.Variable == fromVariable) {
						var varObject = t.Object as VariableEnd;
						if (varObject != null) {
							var pathPredicate = t.Predicate as PropertyPath;
							if (pathPredicate != null) {
								if (path.Equals(pathPredicate)) {
									return varObject.Variable;
								}
							}
						}
					}
				}
			}
			
			var newVar = new Variable();
			AddTriple(new VariableEnd(fromVariable),
			          path,
			          new VariableEnd(newVar));
			return newVar;
		}
		#endregion
		
		#region filters
		public bool ContainsOnlyExpressionFilters {
			get {
				if (statements.All(stmt => stmt is ExpressionFilter)) {
					if (alternativeGraphs.All(gg => gg.All(g => g.ContainsOnlyExpressionFilters))) {
						return true;
					}
				}
				
				return false;
			}
		}
		
		public Expression AsFilterExpression()
		{
			var conjunction = new List<Expression>();
			
			try {
				conjunction.AddRange(statements.Cast<ExpressionFilter>().Select(ef => ef.Expression));
			}
			catch (InvalidCastException ex) {
				throw new InvalidOperationException("The graph does not contain only filter expressions.", ex);
			}
			
			for (int i = 0; i < alternativeGraphs.Count; i++) {
				if (!optionalAlternativeIndices.Contains(i)) {
					var disjunction = new OrExpression(alternativeGraphs[i].Select(g => g.AsFilterExpression()));
					conjunction.Add(disjunction);
				}
			}
			
			switch (conjunction.Count) {
				case 0:
					return new BooleanLiteralExpression(true);
				case 1:
					return conjunction[0];
				default:
					return new AndExpression(conjunction);
			}
		}
		#endregion
		
		#region subgraphs
		private readonly List<QueryGraph[]> alternativeGraphs = new List<QueryGraph[]>();
		
		/// <summary>
		/// Stores the indices from <see cref="alternativeGraphs"/> that can be considered optional.
		/// </summary>
		private readonly HashSet<int> optionalAlternativeIndices = new HashSet<int>();
		
		public void AddAlternativeGraphs(QueryGraph firstGraph, params QueryGraph[] moreGraphs)
		{
			AddAlternativeGraphs(new[] { firstGraph }.Concat(moreGraphs));
		}
		
		public void AddAlternativeGraphs(IEnumerable<QueryGraph> graphs)
		{
			if (graphs == null) {
			    throw new ArgumentNullException("graphs");
			}
			if (graphs.Contains(null)) {
				throw new ArgumentNullException("graphs contained a null element.");
			}
			
			alternativeGraphs.Add(graphs.ToArray());
		}
		
		public void AddOptionalGraphs(QueryGraph firstGraph, params QueryGraph[] moreGraphs)
		{
			AddAlternativeGraphs(firstGraph, moreGraphs);
			optionalAlternativeIndices.Add(alternativeGraphs.Count - 1);
		}
		
		public void AddOptionalGraphs(IEnumerable<QueryGraph> graphs)
		{
			AddAlternativeGraphs(graphs);
			optionalAlternativeIndices.Add(alternativeGraphs.Count - 1);
		}
		#endregion
		
		/// <summary>
		/// Copies the data (the same instances!) from anotehr graph.
		/// </summary>
		/// <param name="otherGraph">The other graph.</param>
		/// <exception cref="ArgumentNullException"><paramref name="otherGraph"/> is <see langword="null"/>.</exception>
		/// <remarks>
		/// <para>This method copies the contents of the graph, including all of its subgraphs, from another <see cref="QueryGraph"/> instance.
		///   This is meant only for temporary copies, as the same instances are used.</para>
		/// </remarks>
		private void UseSameData(QueryGraph otherGraph)
		{
			if (otherGraph == null) {
			    throw new ArgumentNullException("otherGraph");
			}
			
			this.statements.AddRange(otherGraph.statements);
			this.alternativeGraphs.AddRange(otherGraph.alternativeGraphs);
			this.optionalAlternativeIndices.UnionWith(otherGraph.optionalAlternativeIndices);
		}
		
		public bool IsSufficientlyBound(Variable variable)
		{
			foreach (var stmt in statements) {
				if (stmt.CanBind(variable)) {
					return true;
				}
			}
			
			return false;
		}
	}
}
