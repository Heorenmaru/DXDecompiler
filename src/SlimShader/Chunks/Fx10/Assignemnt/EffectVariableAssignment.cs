using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.Assignemnt
{
	public class EffectVariableAssignment : EffectAssignment
	{
		public string Value { get; private set; }
		public static EffectVariableAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectVariableAssignment();
			result.Value = assignmentReader.ReadString();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    Value: {Value}: {ValueOffset.ToString("X4")}");
			return sb.ToString();
		}
	}
}
