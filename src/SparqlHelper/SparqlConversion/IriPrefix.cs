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

namespace SparqlHelper.SparqlConversion
{
	/// <summary>
	/// Stores a base IRI along with a prefix.
	/// </summary>
	public sealed class IriPrefix : IEquatable<IriPrefix>
	{
		public IriPrefix(string prefix, string iri)
		{
			if (prefix == null) {
			    throw new ArgumentNullException("prefix");
			}
			if (iri == null) {
			    throw new ArgumentNullException("iri");
			}
			
			this.prefix = prefix;
			this.iri = iri;
		}
		
		private readonly string prefix;
		
		public string Prefix {
			get {
				return prefix;
			}
		}
		
		private readonly string iri;
		
		public string Iri {
			get {
				return iri;
			}
		}
		
		public override bool Equals(object obj)
		{
			return Equals(obj as IriPrefix);
		}
		
		public override int GetHashCode()
		{
			return prefix.GetHashCode() ^ iri.GetHashCode();
		}

		
		public bool Equals(IriPrefix other)
		{
			if (other == null) {
				return false;
			} else {
				return (other.prefix == this.prefix) && (other.iri == this.iri);
			}
		}
	}
}
