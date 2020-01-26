using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugEntry : IDumpable
	{
		public string Name;
		public string Value;
		public int Indent;
		public uint RelativeIndex;
		public uint AbsoluteIndex;
		public uint Size;
		public string Dump()
		{
			var member = this;
			var indent = new string(' ', (int)member.Indent * 2);
			var sb = new StringBuilder();
			sb.Append(indent);
			if (DebugBytecodeReader.DumpOffsets)
			{
				sb.Append($"{member.AbsoluteIndex}:{member.AbsoluteIndex + member.Size - 1}[{member.RelativeIndex}:{member.RelativeIndex + member.Size - 1}] - ");
			}
			sb.Append($"{member.Name}={member.Value}\n");
			return sb.ToString();
		}
	}
}
