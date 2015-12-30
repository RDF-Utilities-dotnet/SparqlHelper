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

using SparqlHelper.PropertyPaths;

namespace SparqlHelper.Expressions
{
	public abstract class ExistsExpressionBase : Expression
	{
		#region constructors
		internal ExistsExpressionBase(Triple triple)
		{
			if (triple == null) {
			    throw new ArgumentNullException("triple");
			}
			
			this.triple = triple;
		}
		
		internal ExistsExpressionBase(PropertyPathEnd subject, TriplePredicate predicate, PropertyPathEnd @object)
		{
			this.triple = new Triple(subject, predicate, @object);
		}
		
		internal ExistsExpressionBase(Variable subject, TriplePredicate predicate, Variable @object)
		{
			this.triple = new Triple(new VariableEnd(subject),
			                         predicate,
			                         new VariableEnd(@object));
		}
		
		internal ExistsExpressionBase(Variable subject, TriplePredicate predicate) : this(subject, predicate, new Variable())
		{
		}
		#endregion
		
		private readonly Triple triple;
		
		public override IEnumerable<NamedThing> References {
			get {
				return triple.References;
			}
		}
		
		public sealed override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			return Operator + " { " + triple.ToSparqlString(settings) + " }";
		}
		
		protected abstract string Operator { get; }
		
		internal sealed override byte Precedence(SparqlConversion.SparqlSettings settings)
		{
			return 255;
		}
	}
}
