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

namespace SparqlHelper.Filters
{
	public sealed class CustomTextFilter : Filter
	{
		public CustomTextFilter(string filterExpression) : this(filterExpression, new Variable[0])
		{
		}
		
		public CustomTextFilter(string filterExpression, Variable firstNamedThing, params Variable[] moreNamedThings) : this(filterExpression, new[] { firstNamedThing }.Concat(moreNamedThings))
		{
		}
		
		public CustomTextFilter(string filterExpression, IEnumerable<Variable> namedThings)
		{
			if (filterExpression == null) {
			    throw new ArgumentNullException("filterExpression");
			}
			if (namedThings == null) {
			    throw new ArgumentNullException("namedThings");
			}
			if (namedThings.Contains(null)) {
				throw new ArgumentNullException("namedThings contained a null element.");
			}
			
			this.filterExpression = filterExpression;
			this.namedThings = namedThings.ToArray();
		}
		
		private readonly string filterExpression;
		
		public string FilterExpression {
			get {
				return filterExpression;
			}
		}
		
		private readonly NamedThing[] namedThings;
		
		public IList<NamedThing> NamedThings {
			get {
				return namedThings.ToList().AsReadOnly();
			}
		}
		
		protected internal override string ToSparqlFilterString(SparqlConversion.SparqlSettings settings)
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
			                     filterExpression,
			                     namedThings.Select(v => v.ToSparqlString(settings)).Cast<object>().ToArray());
		}
		
		public override IEnumerable<NamedThing> References {
			get {
				return NamedThings;
			}
		}
	}
}
