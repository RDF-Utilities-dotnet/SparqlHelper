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
	public abstract class FunctionExpression : ParentExpression
	{
		internal FunctionExpression(string functionName, IEnumerable<Expression> arguments) : base(arguments)
		{
			if (functionName == null) {
			    throw new ArgumentNullException("functionName");
			}
			
			this.functionName = functionName;
		}
		
		internal FunctionExpression(string functionName, Expression firstArgument, params Expression[] moreArguments) : base(firstArgument, moreArguments)
		{
			if (functionName == null) {
			    throw new ArgumentNullException("functionName");
			}
			
			this.functionName = functionName;
		}
		
		private readonly string functionName;
		
		public string FunctionName {
			get {
				return functionName;
			}
		}
		
		public IList<Expression> Arguments {
			get {
				return SubExpressions;
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			
			foreach (var arg in SubExpressions) {
				if (result.Length > 0) {
					result.Append(", ");
				}
				
				result.Append(arg.ToSparqlString(settings));
			}
			
			result.Insert(0, functionName + "(");
			result.Append(")");
			
			return result.ToString();
		}
		
		internal override byte Precedence(SparqlConversion.SparqlSettings settings)
		{
			return 255;
		}
	}
}
