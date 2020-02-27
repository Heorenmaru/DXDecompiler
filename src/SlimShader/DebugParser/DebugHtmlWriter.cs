using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugHtmlWriter
	{
		DebugBytecodeReader Reader;
		StringBuilder stringBuilder;
		int IndentLevel;
		public DebugHtmlWriter(DebugBytecodeReader reader)
		{
			this.Reader = reader;
			
		}
		public string ToHtml()
		{
			stringBuilder = new StringBuilder();
			IndentLevel = 0;
			return stringBuilder.ToString();
		}
	}
}
