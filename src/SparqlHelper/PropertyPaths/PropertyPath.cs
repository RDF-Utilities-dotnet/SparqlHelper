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

namespace SparqlHelper.PropertyPaths
{
	/// <summary>
	/// The base class for any part of a property path.
	/// </summary>
	public abstract class PropertyPath : TriplePredicate, IEquatable<PropertyPath>, ICloneable
	{
		internal PropertyPath()
		{
		}
		
		internal abstract byte Precedence { get; }
		
		protected string ToSparqlChildString(PropertyPath child, SparqlConversion.SparqlSettings settings)
		{
			if (child == null) {
			    throw new ArgumentNullException("child");
			}
			
			if (child.Precedence <= this.Precedence) {
				return "(" + child.ToSparqlString(settings) + ")";
			} else {
				return child.ToSparqlString(settings);
			}
		}
		
		public sealed override bool Equals(object obj)
		{
			return Equals(obj as PropertyPath);
		}
		
		public abstract bool Equals(PropertyPath other);
		
		public override int GetHashCode()
		{
			throw new NotSupportedException("This class does not implement GetHashCode.");
		}
		
		public sealed override TriplePredicate Simplify()
		{
			return SimplifyPropertyPath();
		}
		
		public virtual PropertyPath SimplifyPropertyPath()
		{
			return this;
		}
		
		public abstract object Clone();
	}
}
