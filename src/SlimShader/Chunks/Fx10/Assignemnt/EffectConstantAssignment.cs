using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectConstantAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public uint Index { get; private set; }

		uint ArrayNameOffset;
		public static EffectConstantAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectConstantAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();
			result.Index = assignmentReader.ReadUInt32();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    ArrayName: {ArrayName}: {ArrayNameOffset.ToString("X4")}");
			sb.AppendLine($"    Index: {Index}: {Index.ToString("X4")}");
			return sb.ToString();
		}
	}
}
