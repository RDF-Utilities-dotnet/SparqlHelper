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

namespace SparqlHelper.SparqlConversion
{
	/// <summary>
	/// An interface for objects that manage a list of prefixes and can insert IRI prefixes to IRI strings.
	/// </summary>
	public interface IIriPrefixingService
	{
		/// <summary>
		/// Attempts to shorten a given IRI and returns a string that can be used in a SPARQL query.
		/// </summary>
		/// <param name="iri">The IRI to shorten.</param>
		/// <returns>The shortened IRI (with a prefix), or the original value of <paramref name="iri"/> within angled brackets so it can be directly used in a SPARQL string.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="iri"/> is <see langword="null"/>.</exception>
		string Shorten(string iri);
		
		/// <summary>
		/// Enumerates all known prefixes.
		/// </summary>
		IEnumerable<IriPrefix> KnownPrefixes { get; }
	}
}
