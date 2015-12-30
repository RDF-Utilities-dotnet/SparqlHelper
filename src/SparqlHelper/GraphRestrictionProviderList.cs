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
	public class GraphRestrictionProviderList : IList<IGraphRestrictionProvider>, IGraphRestrictionProvider
	{
		private readonly List<IGraphRestrictionProvider> items = new List<IGraphRestrictionProvider>();
		
		public IGraphRestrictionProvider this[int index] {
			get {
				return items[index];
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value");
				}
				
				items[index] = value;
			}
		}
		
		public int Count {
			get {
				return items.Count;
			}
		}
		
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		
		public int IndexOf(IGraphRestrictionProvider item)
		{
			return items.IndexOf(item);
		}
		
		public void Insert(int index, IGraphRestrictionProvider item)
		{
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			
			items.Insert(index, item);
		}
		
		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}
		
		public void Add(IGraphRestrictionProvider item)
		{
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			
			items.Add(item);
		}
		
		public void Clear()
		{
			items.Clear();
		}
		
		public bool Contains(IGraphRestrictionProvider item)
		{
			return items.Contains(item);
		}
		
		public void CopyTo(IGraphRestrictionProvider[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}
		
		public bool Remove(IGraphRestrictionProvider item)
		{
			return items.Remove(item);
		}
		
		public IEnumerator<IGraphRestrictionProvider> GetEnumerator()
		{
			return items.GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}
		
		public void AddRestrictions(DataRecordGraph graph)
		{
			if (graph == null) {
				throw new ArgumentNullException("graph");
			}
			
			foreach (var rp in items) {
				rp.AddRestrictions(graph);
			}
		}
	}
}
