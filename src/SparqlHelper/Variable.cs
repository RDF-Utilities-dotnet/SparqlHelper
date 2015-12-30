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
	/// A single variable (that can be used as a subject, in a predicate or as an object) in a <see cref="QueryGraph"/>.
	/// </summary>
	public sealed class Variable : NamedThing
	{
		public Variable()
		{
		}
		
		private static int nextId = 1;
		
		private readonly int id = nextId++;
		
		public override int Id {
			get {
				return id;
			}
		}
		
		public override string ToSparqlString(SparqlConversion.SparqlSettings settings)
		{
			if (settings == null) {
			    throw new ArgumentNullException("settings");
			}
			
			return settings.FormatVarReference(settings.VarPrefix + id.ToString());
		}
		
		/// <summary>
		/// Stores restrictions concerning the number of values for this variable.
		/// </summary>
		/// <seealso cref="Restrictions"/>
		private RetrievalRestrictions restrictions;
		
		/// <summary>
		/// Gets or sets restrictions concerning the number of values for this variable.
		/// </summary>
		/// <value>
		/// <para>This property gets or sets restrictions for the number of values this variable is supposed to assume in the query results.
		///   The default value is <see langword="null"/>, in which case the property will be ignored.</para>
		/// </value>
		public RetrievalRestrictions Restrictions {
			get {
				return restrictions;
			}
			set {
				restrictions = value;
			}
		}
		
		public bool IsRestricted {
			get {
				if (restrictions == null) {
					return false;
				} else {
					return restrictions.HasOffset || restrictions.HasLimit;
				}
			}
		}
	}
}
