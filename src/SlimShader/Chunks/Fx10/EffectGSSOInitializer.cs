using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectGSSOInitializer
	{
		public BytecodeContainer Shader { get; private set; }
		public string SODecl { get; private set; }

		public uint ShaderOffset;
		public uint SODeclOffset;
		public static EffectGSSOInitializer Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectGSSOInitializer();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32();
			var SODeclOffset = result.SODeclOffset = variableReader.ReadUInt32();

			var bytecodeReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes((int)shaderSize));
			}

			var declReader = reader.CopyAtOffset((int)SODeclOffset);
			result.SODecl = declReader.ReadString();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectObject.SODecl: {SODecl} ({SODeclOffset.ToString("X4")})");
			return sb.ToString();
		}
		public override string ToString()
		{
			if(Shader == null) return "NULL";
			var sb = new StringBuilder();
			sb.AppendLine("asm {");
			sb.AppendLine(Shader.ToString());
			sb.Append("}");
			sb.AppendLine();
			sb.Append(string.Format("/* Stream out decl: \"{0}\" */", SODecl));
			return sb.ToString();
		}
	}
}
