using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectGSSOInitializer
	{
		public string SODecl { get; private set; }

		public uint ShaderOffset;
		public uint SODeclOffset;
		public static EffectGSSOInitializer Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectGSSOInitializer();
			result.ShaderOffset = variableReader.ReadUInt32();
			var SODeclOffset = result.SODeclOffset = variableReader.ReadUInt32();
			var declReader = reader.CopyAtOffset((int)result.SODeclOffset);
			result.SODecl = declReader.ReadString();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectObject.SODecl: {SODecl} ({SODeclOffset.ToString("X4")})");
			return sb.ToString();
		}
	}
}
