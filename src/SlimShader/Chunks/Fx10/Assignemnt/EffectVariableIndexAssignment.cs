using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectVariableIndexAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public string VariableName { get; private set; }

		public uint VariableNameOffset;
		uint ArrayNameOffset;
		public static EffectVariableIndexAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectVariableIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();
			var variableNameOffset = result.VariableNameOffset = assignmentReader.ReadUInt32();
			var variableNameReader = reader.CopyAtOffset((int)variableNameOffset);
			result.VariableName = variableNameReader.ReadString();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectVariableIndexAssignment.ArrayName: {ArrayName}: {ArrayNameOffset.ToString("X4")}");
			sb.AppendLine($"    EffectVariableIndexAssignment.VariableName: {VariableName}: {VariableNameOffset.ToString("X4")}");
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(ArrayName);
			sb.Append("[");
			sb.Append(VariableName);
			sb.Append("];");
			return sb.ToString();
		}
	}
}