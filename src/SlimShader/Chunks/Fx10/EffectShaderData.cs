using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectShaderData
	{
		uint ShaderOffset;
		public static EffectShaderData Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectShaderData();
			result.ShaderOffset = variableReader.ReadUInt32();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  ShaderOffset: {ShaderOffset} ({ShaderOffset.ToString("X4")})");
			return sb.ToString();
		}
	}
}
