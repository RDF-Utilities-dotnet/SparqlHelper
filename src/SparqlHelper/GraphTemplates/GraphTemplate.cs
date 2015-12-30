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

namespace SparqlHelper.GraphTemplates
{
	public sealed partial class GraphTemplate
	{
		private readonly List<TemplateTriple> triples = new List<TemplateTriple>();
		
		public TemplateTriple AddTriple(Variable subject, TemplateTriplePredicate predicate, TemplateTripleEnd @object)
		{
			return AddTriple(new TemplateVariableEnd(subject),
			                 predicate,
			                 @object);
		}
		
		public TemplateTriple AddTriple(TemplateTripleEnd subject, TemplateTriplePredicate predicate, Variable @object)
		{
			return AddTriple(subject,
			                 predicate,
			                 new TemplateVariableEnd(@object));
		}
		
		public TemplateTriple AddTriple(Variable subject, TemplateTriplePredicate predicate, Variable @object)
		{
			return AddTriple(new TemplateVariableEnd(subject),
			                 predicate,
			                 new TemplateVariableEnd(@object));
		}
		
		public TemplateTriple AddTriple(TemplateTripleEnd subject, TemplateTriplePredicate predicate, TemplateTripleEnd @object)
		{
			TemplateTriple newTriple = new TemplateTriple(subject, predicate, @object);
			triples.Add(newTriple);
			return newTriple;
		}
		
		public IEnumerable<NamedThing> References {
			get {
				foreach (var t in triples) {
					foreach (var nt in t.References) {
						yield return nt;
					}
				}
			}
		}
	}
}
