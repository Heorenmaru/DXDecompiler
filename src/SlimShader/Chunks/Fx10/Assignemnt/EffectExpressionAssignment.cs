using SlimShader.Chunks.Fxlvm;
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
		public uint ShaderSize;
		public static EffectExpressionAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectExpressionAssignment();
			var shaderSize = result.ShaderSize = assignmentReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(assignmentReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectExpressionAssignment.CodeOffset: {ShaderSize}: {ShaderSize.ToString("X4")}");
			if (Shader != null)
			{
				foreach (var chunk in Shader.Chunks)
				{
					sb.AppendLine(chunk.ToString());
				}
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			if (Shader != null)
			{
				var chunk = Shader.GetChunk<FxlcChunk>();
				sb.Append(chunk.ToString());
			}
			sb.Append(";");
			return sb.ToString();
		}
	}
}
