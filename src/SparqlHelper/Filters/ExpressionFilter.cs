﻿/*
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

using SparqlHelper.Expressions;

namespace SparqlHelper.Filters
{
	public sealed class ExpressionFilter : Filter
	{
		public ExpressionFilter(Expression expression)
		{
			if (expression == null) {
			    throw new ArgumentNullException("expression");
			}
			
			this.expression = expression;
		}
		
		private readonly Expression expression;
		
		public Expression Expression {
			get {
				return expression;
			}
		}
		
		protected internal override string ToSparqlFilterString(SparqlConversion.SparqlSettings settings)
		{
			return expression.ToSparqlString(settings);
		}
		
		public override IEnumerable<NamedThing> References {
			get {
				return expression.References;
			}
		}
	}
}
