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
using System.Text;

using SparqlHelper.SparqlConversion;

namespace SparqlHelper.GraphTemplates
{
	partial class GraphTemplate
	{
		public string ToSparqlString(SparqlSettings settings)
		{
			return ToSparqlString(settings, "");
		}
		
		public string ToSparqlString(SparqlSettings settings, string indentation)
		{
			if (settings == null) {
			    throw new ArgumentNullException("settings");
			}
			
			var result = new StringBuilder();
			ToSparqlString(settings, result, indentation);
			return result.ToString();
		}
		
		private void ToSparqlString(SparqlSettings settings, StringBuilder dest, string indentation)
		{
			foreach (var triple in this.triples) {
				dest.AppendLine(indentation + triple.ToSparqlString(settings));
			}
		}
	}
}
