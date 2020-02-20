using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectExpressionAssignment : EffectAssignment
	{
		public BytecodeContainer Shader { get; private set; }
		public uint CodeOffset;
		public static EffectExpressionAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectExpressionAssignment();
			var codeOffset = result.CodeOffset = assignmentReader.ReadUInt32();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectExpressionAssignment.CodeOffset: {CodeOffset}: {CodeOffset.ToString("X4")}");
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = 0; // TODO Expression assignment not current supported");
			return sb.ToString();
		}
	}
}
