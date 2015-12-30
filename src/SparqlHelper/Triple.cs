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

namespace SparqlHelper
{
	/// <summary>
	/// A single triple in a <see cref="QueryGraph"/>.
	/// </summary>
	public sealed class Triple : GraphStatement
	{
		internal Triple(PropertyPathEnd subject, TriplePredicate predicate, PropertyPathEnd @object)
		{
			if (subject == null) {
			    throw new ArgumentNullException("subject");
			}
			if (predicate == null) {
			    throw new ArgumentNullException("predicate");
			}
			if (@object == null) {
			    throw new ArgumentNullException("object");
			}
			
			this.tripleSubject = subject;
			this.triplePredicate = predicate;
			this.tripleObject = @object;
		}
		
		private QueryGraph owner;
		
		public QueryGraph Owner {
			get {
				return owner;
			}
			internal set {
				owner = value;
			}
		}
		
		private readonly PropertyPathEnd tripleSubject;
		
		public PropertyPathEnd Subject {
			get {
				return tripleSubject;
			}
		}
		
		private readonly TriplePredicate triplePredicate;
		
		public TriplePredicate Predicate {
			get {
				return triplePredicate;
			}
		}
		
		private readonly PropertyPathEnd tripleObject;
		
		public PropertyPathEnd Object {
			get {
				return tripleObject;
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
			                     "{0} {1} {2}.",
			                     tripleSubject.ToSparqlString(settings),
			                     triplePredicate.ToSparqlString(settings),
			                     tripleObject.ToSparqlString(settings));
		}
		
		public sealed override IEnumerable<NamedThing> References {
			get {
				foreach (var t in tripleSubject.References) {
					yield return t;
				}
				foreach (var t in triplePredicate.References) {
					yield return t;
				}
				foreach (var t in tripleObject.References) {
					yield return t;
				}
			}
		}
		
		public override bool CanBind(Variable variable)
		{
			base.CanBind(variable);
			
			var varEnd = tripleSubject as VariableEnd;
			if (varEnd != null) {
				if (varEnd.Variable == variable) {
					return true;
				}
			}
			
			var varPredicate = triplePredicate as VariablePredicate;
			if (varPredicate != null) {
				if (varPredicate.Variable == variable) {
					return true;
				}
			}
			
			varEnd = tripleObject as VariableEnd;
			if (varEnd != null) {
				if (varEnd.Variable == variable) {
					return true;
				}
			}
			
			return false;
		}
	}
}
