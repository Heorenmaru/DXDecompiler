using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInlineShader5Assignment : EffectAssignment
	{
		public BytecodeContainer Shader { get; private set; }
		uint ShaderOffset;
		uint[] SODeclsOffset;
		uint SODeclsCount;
		uint RasterizedStream;
		uint InterfaceBindingCount;
		uint InterfaceBindingOffset;
		public EffectInlineShader5Assignment()
		{
			SODeclsOffset = new uint[4];
		}
		public static EffectInlineShader5Assignment Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectInlineShader5Assignment();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32();
			result.SODeclsOffset[0] = variableReader.ReadUInt32();
			result.SODeclsOffset[1] = variableReader.ReadUInt32();
			result.SODeclsOffset[2] = variableReader.ReadUInt32();
			result.SODeclsOffset[3] = variableReader.ReadUInt32();
			result.SODeclsCount = variableReader.ReadUInt32();
			result.RasterizedStream = variableReader.ReadUInt32();
			result.InterfaceBindingCount = variableReader.ReadUInt32();
			result.InterfaceBindingOffset = variableReader.ReadUInt32();

			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectInlineShader5Assignment.SODecls0: {SODeclsOffset[0]} ({SODeclsOffset[0].ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.SODecls1: {SODeclsOffset[1]} ({SODeclsOffset[1].ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.SODecls2: {SODeclsOffset[2]} ({SODeclsOffset[2].ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.SODecls3: {SODeclsOffset[3]} ({SODeclsOffset[3].ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.SODeclsCount: {SODeclsCount} ({SODeclsCount.ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.RasterizedStream: {RasterizedStream} ({RasterizedStream.ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.InterfaceBindingCount: {InterfaceBindingCount} ({InterfaceBindingCount.ToString("X4")})");
			sb.AppendLine($"  EffectInlineShader5Assignment.InterfaceBindingOffset: {InterfaceBindingOffset} ({InterfaceBindingOffset.ToString("X4")})");
			return sb.ToString();
		}
		public override string ToString()
		{
			if (Shader == null)
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
