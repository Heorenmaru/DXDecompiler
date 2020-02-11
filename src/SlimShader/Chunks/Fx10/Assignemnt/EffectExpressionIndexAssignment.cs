using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectExpressionIndexAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public uint CodeOffset { get; private set; }

		uint ArrayNameOffset;
		public static EffectExpressionIndexAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectExpressionIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();
			result.CodeOffset = assignmentReader.ReadUInt32();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectExpressionIndexAssignment.ArrayName: {ArrayName}: {ArrayNameOffset.ToString("X4")}");
			sb.AppendLine($"    EffectExpressionIndexAssignment.CodeOffset: {CodeOffset}: {CodeOffset.ToString("X4")}");
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(ArrayName);
			sb.Append("[0]; // TODO Expression index assignment not current supported");
			return sb.ToString();
		}
	}
}
