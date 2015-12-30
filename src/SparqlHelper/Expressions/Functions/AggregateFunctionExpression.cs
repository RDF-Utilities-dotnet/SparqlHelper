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

namespace SparqlHelper.Expressions.Functions
{
	public abstract class AggregateFunctionExpression : FunctionExpression
	{
		internal AggregateFunctionExpression(string functionName, bool distinct, IEnumerable<Expression> arguments) : base(functionName, arguments)
		{
			this.distinct = distinct;
		}
		
		internal AggregateFunctionExpression(string functionName, bool distinct, Expression firstArgument, params Expression[] moreArguments) : base(functionName, firstArgument, moreArguments)
		{
			this.distinct = distinct;
		}
		
		private readonly bool distinct;
		
		public bool Distinct {
			get {
				return distinct;
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			
			foreach (var arg in SubExpressions) {
				if (result.Length > 0) {
					result.Append(", ");
				} else {
					if (distinct) {
						result.Append("DISTINCT ");
					}
				}
				
				result.Append(arg.ToSparqlString(settings));
			}
			
			result.Insert(0, FunctionName + "(");
			result.Append(SpecialModifierParamsString(settings) + ")");
			
			return result.ToString();
		}
		
		protected virtual string SpecialModifierParamsString(SparqlConversion.SparqlSettings settings)
		{
			return "";
		}
	}
}
