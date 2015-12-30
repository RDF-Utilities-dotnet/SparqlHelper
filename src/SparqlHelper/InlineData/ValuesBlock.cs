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

using SparqlHelper.Expressions;

namespace SparqlHelper.InlineData
{
	public sealed class ValuesBlock : GraphStatement
	{
		#region constructors
		public ValuesBlock(IEnumerable<ValuesValue> values)
		{
			if (values == null) {
			    throw new ArgumentNullException("values");
			}
			
			var vals = values.ToArray();
			
			if (vals.Length <= 0) {
				throw new ArgumentException("values did not contain any elements.");
			}
			
			variables = CreateVariables(vals.Length);
			this.values = new ValuesValue[1, variables.Length];
			
			for (int i = 0; i < variables.Length; i++) {
				this.values[0, i] = vals[i] ?? new ValuesUndefined();
			}
		}
		
		public ValuesBlock(ValuesValue firstValue, params ValuesValue[] moreValues)
		{
			if (moreValues == null) {
			    throw new ArgumentNullException("moreValues");
			}
			
			var vals = new ValuesValue[] { firstValue }.Concat(moreValues).ToArray();
			
			variables = CreateVariables(vals.Length);
			this.values = new ValuesValue[1, variables.Length];
			
			for (int i = 0; i < variables.Length; i++) {
				this.values[0, i] = vals[i] ?? new ValuesUndefined();
			}
		}
		
		public ValuesBlock(IEnumerable<IEnumerable<ValuesValue>> values)
		{
			if (values == null) {
			    throw new ArgumentNullException("values");
			}
			if (values.Contains(null)) {
				throw new ArgumentNullException("values contained unassigned data rows.", (Exception)null);
			}
			
			var rows = values.Select(row => row.ToArray()).ToArray();
			if (rows.Length <= 0) {
				throw new ArgumentException("values did not contain any data rows.");
			}
			
			int varCount = rows.Max(row => row.Length);
			if (varCount <= 0) {
				throw new ArgumentException("values indicated a total of zero variables.");
			}
			
			this.variables = CreateVariables(varCount);
			
			this.values = new ValuesValue[rows.Length, varCount];
			
			for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++) {
				var row = rows[rowIndex];
				for (int varIndex = 0; varIndex < varCount; varIndex++) {
					if (varIndex < row.Length) {
						this.values[rowIndex, varIndex] = row[varIndex] ?? new ValuesUndefined();
					} else {
						this.values[rowIndex, varIndex] = new ValuesUndefined();
					}
				}
			}
		}
		
		public ValuesBlock(IEnumerable<ValuesValue> firstDataRow, params IEnumerable<ValuesValue>[] moreDataRows) : this(new IEnumerable<ValuesValue>[] { firstDataRow }.Concat(moreDataRows))
		{
		}
		#endregion
		
		private static Variable[] CreateVariables(int count)
		{
			var result = new Variable[count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = new Variable();
			}
			return result;
		}
		
		private readonly Variable[] variables;
		
		public Variable[] GetVariables()
		{
			return (Variable[])variables.Clone();
		}
		
		private readonly ValuesValue[,] values;
		
		public override IEnumerable<NamedThing> References {
			get {
				return variables.Cast<NamedThing>();
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			if (variables.Length == 1) {
				result.Append("VALUES ");
				result.Append(variables[0].ToSparqlString(settings));
				result.Append(" {");
				
				for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++) {
					result.Append(" ");
					var val = values[rowIndex, 0];
					result.Append(val.ToSparqlString(settings));
				}
				
				result.Append(" }");
			} else {
				result.Append("VALUES (");
				result.Append(variables[0].ToSparqlString(settings));
				for (int i = 1; i < variables.Length; i++) {
					result.Append(" ");
					result.Append(variables[i].ToSparqlString(settings));
				}
				result.Append(") {");
				
				for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++) {
					result.Append(" (");
					for (int i = 0; i < variables.Length; i++) {
						if (i > 0) {
							result.Append(" ");
						}
						
						var val = values[rowIndex, i];
						result.Append(val.ToSparqlString(settings));
					}
					result.Append(")");
				}
				
				result.Append(" }");
			}
			return result.ToString();
		}
	}
}
