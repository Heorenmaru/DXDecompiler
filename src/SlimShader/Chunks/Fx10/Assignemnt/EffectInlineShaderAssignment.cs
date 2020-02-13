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
		public BytecodeContainer Shader { get; private set; }
		uint ShaderOffset;
		uint SODeclOffset;
		public static EffectInlineShaderAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectInlineShaderAssignment();
			var shaderOffset = result.ShaderOffset = assignmentReader.ReadUInt32();
			var SODeclOffset = result.SODeclOffset = assignmentReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
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
		public override string ToString()
		{
			if(Shader == null)
			{
				return string.Format("{0} = NULL;", MemberType);
			}
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("{0} = asm {{", MemberType));
			sb.AppendLine(Shader.ToString());
			sb.Append("};");
			return sb.ToString();
		}
	}
}
