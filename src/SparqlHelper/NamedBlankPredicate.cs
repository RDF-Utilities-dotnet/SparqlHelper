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

namespace SparqlHelper
{
	public sealed class NamedBlankPredicate : TriplePredicate
	{
		public NamedBlankPredicate(NamedBlankNode node)
		{
			if (node == null) {
			    throw new ArgumentNullException("node");
			}
			
			this.node = node;
		}
		
		private readonly NamedBlankNode node;
		
		public NamedBlankNode Node {
			get {
				return node;
			}
		}
		
		public override string ToSparqlString(SparqlHelper.SparqlConversion.SparqlSettings settings)
		{
			return node.ToSparqlString(settings);
		}
		
		public override IEnumerable<SparqlHelper.NamedThing> References {
			get {
				yield return node;
			}
		}
	}
}
