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
	/// <summary>
	/// A helper class that stores a variable representing a data record along with the graph it is defined in.
	/// </summary>
	public class DataRecordGraph
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="graph">The graph.</param>
		/// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
		public DataRecordGraph(Variable variable, QueryGraph graph)
		{
			if (variable == null) {
			    throw new ArgumentNullException("variable");
			}
			if (graph == null) {
			    throw new ArgumentNullException("graph");
			}
			
			this.variable = variable;
			this.graph = graph;
		}
		
		private readonly Variable variable;
		
		public Variable Variable {
			get {
				return variable;
			}
		}
		
		private readonly QueryGraph graph;
		
		public QueryGraph Graph {
			get {
				return graph;
			}
		}
	}
}
