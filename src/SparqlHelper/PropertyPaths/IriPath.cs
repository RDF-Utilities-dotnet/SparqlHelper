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

namespace SparqlHelper.PropertyPaths
{
	public sealed class IriPath : PredicatePath
	{
		public IriPath(string iri)
		{
			if (iri == null) {
				throw new ArgumentNullException("iri");
			}
			
			this.iri = iri;
		}
		
		private readonly string iri;
		
		public string Iri {
			get {
				return iri;
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			if (settings == null) {
			    throw new ArgumentNullException("settings");
			}
			
			return settings.PrefixingService.Shorten(iri);
		}
		
		internal override byte Precedence {
			get {
				return 100;
			}
		}
		
		public override bool Equals(PropertyPath other)
		{
			var typedOther = other as IriPath;
			if (typedOther == null) {
				return false;
			} else {
				return this.iri == typedOther.iri;
			}
		}
		
		public override int GetHashCode()
		{
			return iri.GetHashCode();
		}
		
		public override object Clone()
		{
			return new IriPath(this.iri);
		}
	}
}
