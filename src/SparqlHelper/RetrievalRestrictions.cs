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

namespace SparqlHelper
{
	public sealed class RetrievalRestrictions
	{
		#region constructors
		public RetrievalRestrictions()
		{
		}
		
		public RetrievalRestrictions(int limit)
		{
			this.limit = limit;
		}
		
		public RetrievalRestrictions(int offset, int limit)
		{
			this.offset = offset;
			this.limit = limit;
		}
		#endregion
		
		private int? offset;
		
		public bool HasOffset {
			get {
				return offset.HasValue;
			}
		}
		
		public int Offset {
			get {
				return offset.Value;
			}
			set {
				offset = value;
			}
		}
		
		public void RemoveOffset()
		{
			offset = null;
		}
		
		private int? limit;
		
		public bool HasLimit {
			get {
				return limit.HasValue;
			}
		}
		
		public int Limit {
			get {
				return limit.Value;
			}
			set {
				limit = value;
			}
		}
		
		public void RemoveLimit()
		{
			limit = null;
		}
		
		public bool IsRestricted {
			get {
				return offset.HasValue || limit.HasValue;
			}
		}
		
		/// <summary>
		/// Copies the settings from another <see cref="SelectRestrictions"/> instance.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
		public void CopyFrom(RetrievalRestrictions other)
		{
			if (other == null) {
			    throw new ArgumentNullException("other");
			}
			
			this.offset = other.offset;
			this.limit = other.limit;
		}
	}
}
