using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInlineShaderAssignment : EffectAssignment
	{
		public string SODecl { get; private set; }

		uint ShaderOffset;
		uint SODeclOffset;
		public static EffectInlineShaderAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectInlineShaderAssignment();
			var shaderOffset = result.ShaderOffset = assignmentReader.ReadUInt32();

			var SODeclOffset = result.SODeclOffset = assignmentReader.ReadUInt32();
			var SODeclReader = reader.CopyAtOffset((int)SODeclOffset);
			result.SODecl = SODeclReader.ReadString();
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    ShaderOffset: {ShaderOffset}: {ShaderOffset.ToString("X4")}");
			sb.AppendLine($"    SODecl: {SODecl}: {SODeclOffset.ToString("X4")}");
			return sb.ToString();
		}
	}
}
