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

using SparqlHelper.ColumnExpressions;

namespace SparqlHelper
{
	/// <summary>
	/// Represents a SPARQL SELECT query.
	/// </summary>
	public sealed partial class SelectQuery : Query
	{
		public SelectQuery()
		{
		}
		
		private readonly List<Column> resultColumns = new List<Column>();
		
		public IList<Column> ResultColumns {
			get {
				return resultColumns;
			}
		}
		
		/// <summary>
		/// Stores whether distinct results are desired.
		/// </summary>
		/// <seealso cref="DistinctResults"/>
		private bool distinctResults = true;
		
		/// <summary>
		/// Stores whether distinct results are desired.
		/// </summary>
		/// <value>
		/// <para>Gets or sets a value that determines whether the results should be distinct.
		///   The default value is <see langword="true"/>.</para>
		/// </value>
		public bool DistinctResults {
			get {
				return distinctResults;
			}
			set {
				distinctResults = value;
			}
		}
		
		protected override IEnumerable<Variable> ProjectedVariables {
			get {
				foreach (var column in resultColumns) {
					foreach (var r in column.References) {
						yield return r;
					}
				}
			}
		}
	}
}
