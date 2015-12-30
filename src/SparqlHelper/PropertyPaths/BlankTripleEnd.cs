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

namespace SparqlHelper.PropertyPaths
{
	public sealed class BlankTripleEnd : PropertyPathEnd
	{
		public BlankTripleEnd(TriplePredicate predicate, PropertyPathEnd @object) : this(new Tuple<TriplePredicate, PropertyPathEnd>(predicate, @object))
		{
		}
		
		public BlankTripleEnd(Tuple<TriplePredicate, PropertyPathEnd> firstTriple, params Tuple<TriplePredicate, PropertyPathEnd>[] moreTriples) : this(new[] { firstTriple }.Concat(moreTriples))
		{
		}
		
		public BlankTripleEnd(IEnumerable<Tuple<TriplePredicate, PropertyPathEnd>> triples)
		{
			if (triples == null) {
			    throw new ArgumentNullException("triples");
			}
			if (triples.Contains(null)) {
				throw new ArgumentNullException("triples contained a null element.", (Exception)null);
			}
			if (triples.Any(t => (t.Item1 == null) || (t.Item2 == null))) {
				throw new ArgumentNullException("triples contained an element whose predicate and/or object was null.", (Exception)null);
			}
			if (!triples.Any()) {
				throw new ArgumentException("triples is empty.");
			}
			
			this.triples = triples.ToArray();
		}
		
		private readonly Tuple<TriplePredicate, PropertyPathEnd>[] triples;
		
		public IList<Tuple<TriplePredicate, PropertyPathEnd>> Triples {
			get {
				return triples.ToList().AsReadOnly();
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			foreach (var t in triples) {
				if (result.Length > 0) {
					result.Append("; ");
				}
				result.Append(t.Item1.ToSparqlString(settings) + " " + t.Item2.ToSparqlString(settings));
			}
			return "[ " + result.ToString() + " ]";
		}
		
		public override IEnumerable<NamedThing> References {
			get {
				foreach (var t in triples) {
					foreach (var nt in t.Item1.References) {
						yield return nt;
					}
					foreach (var nt in t.Item2.References) {
						yield return nt;
					}
				}
			}
		}
	}
}
