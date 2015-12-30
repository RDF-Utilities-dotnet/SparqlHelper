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

namespace SparqlHelper.Expressions
{
	public abstract class InExpressionBase : NAryOperatorExpression
	{
		internal InExpressionBase(IEnumerable<Expression> operands) : base(operands)
		{
		}
		
		internal InExpressionBase(Expression firstOperand, params Expression[] moreOperands) : base(firstOperand, moreOperands)
		{
		}
		
		public sealed override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			
			result.Append(SubExpressionToSparqlString(0, settings));
			result.Append(" " + OperatorSymbol + " (");
			
			for (int i = 1; i < Operands.Count; i ++) {
				if (i > 1) {
					result.Append(", ");
				}
				result.Append(Operands[i].ToSparqlString(settings));
			}
			
			result.Append(")");
			
			return result.ToString();
		}
		
		internal override byte Precedence(SparqlConversion.SparqlSettings settings)
		{
			return 120;
		}
	}
}
