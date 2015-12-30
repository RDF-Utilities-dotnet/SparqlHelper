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

namespace SparqlHelper.Expressions
{
	/// <summary>
	/// An expression type that uses the results of sub-expressions.
	/// </summary>
	public abstract class ParentExpression : Expression
	{
		internal ParentExpression(IEnumerable<Expression> subExpressions)
		{
			if (subExpressions == null) {
			    throw new ArgumentNullException("subExpressions");
			}
			if (subExpressions.Contains(null)) {
				throw new ArgumentNullException("subExpressions contained a null element.", (Exception)null);
			}
			
			this.subExpressions = subExpressions.ToList().AsReadOnly();
		}
		
		internal ParentExpression(Expression firstSubExpression, params Expression[] moreSubExpressions)
		{
			if (firstSubExpression == null) {
			    throw new ArgumentNullException("firstSubExpression");
			}
			if (moreSubExpressions == null) {
			    throw new ArgumentNullException("moreSubExpressions");
			}
			if (moreSubExpressions.Contains(null)) {
				throw new ArgumentNullException("moreSubExpressions contained a null element.", (Exception)null);
			}
			
			this.subExpressions = new Expression[] { firstSubExpression }.Concat(moreSubExpressions).ToList().AsReadOnly();
		}
		
		private readonly IList<Expression> subExpressions;
		
		public IList<Expression> SubExpressions {
			get {
				return subExpressions;
			}
		}
		
		protected string SubExpressionToSparqlString(int index, SparqlConversion.SparqlSettings settings)
		{
			bool requiresBrackets = subExpressions[index].Precedence(settings) <= this.Precedence(settings);
			if (requiresBrackets) {
				return "(" + subExpressions[index].ToSparqlString(settings) + ")";
			} else {
				return subExpressions[index].ToSparqlString(settings);
			}
		}
		
		public override IEnumerable<NamedThing> References {
			get {
				foreach (var se in subExpressions) {
					foreach (var r in se.References) {
						yield return r;
					}
				}
			}
		}
	}
}
