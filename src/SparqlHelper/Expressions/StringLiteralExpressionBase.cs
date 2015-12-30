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

namespace SparqlHelper.Expressions
{
	public abstract class StringLiteralExpressionBase : TypedLiteralExpression<string>
	{
		internal StringLiteralExpressionBase(string value, string literalTypeName) : base(value, literalTypeName)
		{
		}
		
		protected string EscapedValue {
			get {
				var result = new System.Text.StringBuilder(Value.Length * 2);
				
				bool containsSingleQuote = false;
				bool containsDoubleQuote = false;
				
				foreach (char ch in Value) {
					switch (ch) {
						case '\t':
							result.Append(@"\t");
							break;
						case '\n':
							result.Append(@"\n");
							break;
						case '\r':
							result.Append(@"\r");
							break;
						case '\b':
							result.Append(@"\b");
							break;
						case '\f':
							result.Append(@"\f");
							break;
						case '\'':
							if (containsDoubleQuote) {
								result.Append(@"\'");
							} else {
								result.Append('\'');
								containsSingleQuote = true;
							}
							break;
						case '"':
							if (containsSingleQuote) {
								result.Append("\\\"");
							} else {
								result.Append('"');
								containsDoubleQuote = true;
							}
							break;
						case '\\':
							result.Append(@"\\");
							break;
						default:
							result.Append(ch);
							break;
					}
				}
				
				if (containsDoubleQuote) {
					result.Insert(0, '\'');
					result.Append('\'');
				} else {
					result.Insert(0, '"');
					result.Append('"');
				}
				
				return result.ToString();
			}
		}
	}
}
