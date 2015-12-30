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
using System.ComponentModel;
using System.Collections.Generic;

namespace SparqlHelper.SparqlConversion
{
	/// <summary>
	/// Stores some settings for the conversion to SPARQL.
	/// </summary>
	public class SparqlSettings : ICloneable
	{
		private class NopPrefixingService : IIriPrefixingService
		{
			public IEnumerable<IriPrefix> KnownPrefixes {
				get {
					yield break;
				}
			}
			
			public string Shorten(string iri)
			{
				if (iri == null) {
				    throw new ArgumentNullException("iri");
				}
				
				return "<" + iri + ">";
			}
		}
		
		private IIriPrefixingService prefixingService = new NopPrefixingService();
		
		public IIriPrefixingService PrefixingService {
			get {
				return prefixingService;
			}
			set {
				prefixingService = value ?? new NopPrefixingService();;
			}
		}
		
		private string varPrefix = "v";
		
		public string VarPrefix {
			get {
				return varPrefix;
			}
			set {
				if (value == null) {
				    throw new ArgumentNullException("value");
				}
				
				varPrefix = value;
			}
		}
		
		private string blankNodePrefix = "b";
		
		public string BlankNodePrefix {
			get {
				return blankNodePrefix;
			}
			set {
				if (value == null) {
				    throw new ArgumentNullException("value");
				}
				
				blankNodePrefix = value;
			}
		}
		
		private string blockIndentation = "  ";
		
		public string BlockIndentation {
			get {
				return blockIndentation;
			}
			set {
				blockIndentation = value ?? "";
			}
		}
		
		private VariablePrefixSymbol varPrefixSymbol = VariablePrefixSymbol.QuestionMark;
		
		public VariablePrefixSymbol VarPrefixSymbol {
			get {
				return varPrefixSymbol;
			}
			set {
				switch (value) {
					case VariablePrefixSymbol.QuestionMark:
					case VariablePrefixSymbol.Dollar:
						varPrefixSymbol = value;
						break;
					default:
						throw new InvalidEnumArgumentException("value", (int)value, typeof(VariablePrefixSymbol));
				}
			}
		}
		
		public string FormatVarReference(string varName)
		{
			if (varName == null) {
			    throw new ArgumentNullException("varName");
			}
			
			switch (varPrefixSymbol) {
				case VariablePrefixSymbol.Dollar:
					return "$" + varName;
				default:
					return "?" + varName;
			}
		}
		
		private bool explicitLiteralTypes;
		
		/// <summary>
		/// Gets or sets a value that determines whether literal types should be explicitly indicated whenever possible.
		/// </summary>
		/// <value>
		/// <para>This property gets or sets a value that determines whether literal types should be explicitly indicated whenever possible.
		///   The default value is <see langword="false"/>.</para>
		/// </value>
		public bool ExplicitLiteralTypes {
			get {
				return explicitLiteralTypes;
			}
			set {
				explicitLiteralTypes = value;
			}
		}
		
		public virtual object Clone()
		{
			SparqlSettings result = new SparqlSettings();
			result.prefixingService = this.prefixingService;
			result.varPrefix = this.varPrefix;
			return result;
		}
	}
}
