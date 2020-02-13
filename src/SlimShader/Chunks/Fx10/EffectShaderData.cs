using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectShaderData
	{
		public BytecodeContainer Shader { get; private set; }
		uint ShaderOffset;
		public static EffectShaderData Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectShaderData();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32();
			var bytecodeReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  ShaderOffset: {ShaderOffset} ({ShaderOffset.ToString("X4")})");
			return sb.ToString();
		}
		public override string ToString()
		{
			if (Shader == null) return "NULL";
			var sb = new StringBuilder();
			sb.AppendLine("asm {");
			sb.AppendLine(Shader.ToString());
			sb.Append("}");
			return sb.ToString();
		}
	}
}
