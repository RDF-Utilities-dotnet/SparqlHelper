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

using SparqlHelper.SparqlConversion;
using SparqlHelper.Sorting;
using SparqlHelper.ColumnExpressions;

namespace SparqlHelper
{
	/// <summary>
	/// The base class for SPARQL queries.
	/// </summary>
	public abstract class Query
	{
		internal Query()
		{
		}
		
		private readonly QueryGraph graph = new QueryGraph();
		
		public QueryGraph Graph {
			get {
				return graph;
			}
		}
		
		public string ToSparqlString(SparqlSettings settings)
		{
			return ToSparqlString(settings, true);
		}
		
		public string ToSparqlString(SparqlSettings settings, bool includePrefixes)
		{
			return ToSparqlString(settings, includePrefixes, "");
		}
		
		public string ToSparqlString(SparqlSettings settings, string indentation)
		{
			return ToSparqlString(settings, true, indentation);
		}
		
		public string ToSparqlString(SparqlSettings settings, bool includePrefixes, string indentation)
		{
			if (settings == null) {
			    throw new ArgumentNullException("settings");
			}
			
			var result = new StringBuilder();
			
			var queryBody = new StringBuilder();
			ToSparqlString(settings, queryBody, indentation);
			
			if (includePrefixes && settings.PrefixingService.KnownPrefixes.Any()) {
				result.AppendLine(settings.PrefixingService.ToSparqlString(settings));
			}
			result.Append(queryBody.ToString());
			
			if (groupBy.Count > 0) {
				var groupByClause = new StringBuilder("GROUP BY ");
				foreach (var gc in groupBy) {
					groupByClause.Append(" ");
					groupByClause.Append(gc.ToSparqlString(settings));
				}
				result.AppendLine(groupByClause.ToString());
			}
			
			if (groupRestriction != null) {
				result.AppendLine("HAVING(" + groupRestriction.ToSparqlString(settings) + ")");
			}
			
			if (orderBy.Count > 0) {
				var orderByClause = new StringBuilder("ORDER BY");
				foreach (var oc in orderBy) {
					orderByClause.Append(" ");
					orderByClause.Append(oc.ToSparqlString(settings));
				}
				result.AppendLine(orderByClause.ToString());
			}
			
			if (restrictions.HasOffset) {
				result.AppendLine(indentation + "OFFSET " + restrictions.Offset.ToString());
			}
			if (restrictions.HasLimit) {
				result.AppendLine(indentation + "LIMIT " + restrictions.Limit.ToString());
			}
			
			return result.ToString();
		}
		
		protected abstract void ToSparqlString(SparqlSettings settings, StringBuilder dest, string indentation);
		
		protected abstract IEnumerable<Variable> ProjectedVariables { get; }
		
		private readonly RetrievalRestrictions restrictions = new RetrievalRestrictions();
		
		public RetrievalRestrictions Restrictions {
			get {
				return restrictions;
			}
		}
		
		private readonly SortingConstraintList orderBy = new SortingConstraintList();
		
		public IList<SortingConstraint> OrderBy {
			get {
				return orderBy;
			}
		}
		
		private readonly List<Column> groupBy = new List<Column>();
		
		public IList<Column> GroupBy {
			get {
				return groupBy;
			}
		}
		
		private Expressions.Expression groupRestriction;
		
		public Expressions.Expression GroupRestriction {
			get {
				return groupRestriction;
			}
			set {
				groupRestriction = value;
			}
		}
	}
}
