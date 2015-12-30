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
	public abstract class MultiChildPath<T> : PropertyPath
		where T : PropertyPath
	{
		internal MultiChildPath(T firstPart, params T[] moreParts) : this(new[] { firstPart }.Concat(moreParts))
		{
		}
		
		internal MultiChildPath(IEnumerable<T> parts)
		{
			if (parts == null) {
			    throw new ArgumentNullException("parts");
			}
			if (parts.Contains(null)) {
				throw new ArgumentNullException("parts contained a null element.");
			}
			
			this.parts = parts.ToArray();
		}
		
		private readonly T[] parts;
		
		public IList<T> Parts {
			get {
				return parts.ToList().AsReadOnly();
			}
		}
		
		protected abstract string PartDelimiter { get; }
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			var result = new System.Text.StringBuilder();
			foreach (var part in this.Parts) {
				if (result.Length > 0) {
					result.Append(PartDelimiter);
				}
				result.Append(ToSparqlChildString(part, settings));
			}
			return result.ToString();
		}
		
		public override bool Equals(PropertyPath other)
		{
			var typedOther = other as MultiChildPath<T>;
			if (typedOther == null) {
				return false;
			} else {
				if (typedOther.parts.Length == this.parts.Length) {
					for (int i = 0; i < parts.Length; i++) {
						if (!this.parts[i].Equals(typedOther.parts[i])) {
							return false;
						}
					}
					return true;
				} else {
					return false;
				}
			}
		}
		
		public override int GetHashCode()
		{
			uint result = 0;
			for (int i = 0; i < parts.Length; i++) {
				uint hc = (uint)parts[i].GetHashCode();
				result ^= (hc << ((i * 17) % 32)) | (hc >> (32 - ((i * 17) % 32)));
			}
			return GetType().Name.GetHashCode() ^ (int)result;
		}
		
		public override PropertyPath SimplifyPropertyPath()
		{
			if (parts.Length == 1) {
				return parts[0].SimplifyPropertyPath();
			} else {
				return CreateWithSimplifiedChildren();
			}
		}
		
		protected abstract MultiChildPath<T> CreateWithSimplifiedChildren();
	}
}
