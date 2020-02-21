using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInlineShader5Assignment : EffectAssignment
	{
		public BytecodeContainer Shader { get; private set; }
		public List<string> SODecls { get; private set; }
		public uint[] SODeclsOffset { get; private set; }
		public uint SODeclsCount { get; private set; }
		public uint RasterizedStream { get; private set; }
		public List<EffectInterfaceInitializer> InterfaceBindings { get; private set; }

		public uint InterfaceBindingCount;
		public uint InterfaceBindingOffset;
		uint ShaderOffset;
		public EffectInlineShader5Assignment()
		{
			SODeclsOffset = new uint[4];
			SODecls = new List<string>();
			InterfaceBindings = new List<EffectInterfaceInitializer>();
		}
		public static EffectInlineShader5Assignment Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectInlineShader5Assignment();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32();
			result.SODeclsOffset[0] = variableReader.ReadUInt32();
			result.SODeclsOffset[1] = variableReader.ReadUInt32();
			result.SODeclsOffset[2] = variableReader.ReadUInt32();
			result.SODeclsOffset[3] = variableReader.ReadUInt32();
			var SoDeclsCount = result.SODeclsCount = variableReader.ReadUInt32();
			result.RasterizedStream = variableReader.ReadUInt32();
			var interfaceBindingCount = result.InterfaceBindingCount = variableReader.ReadUInt32();
			var interfaceBindingOffset = result.InterfaceBindingOffset = variableReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			for (int i = 0; i < 4; i++)
			{
				var offset = result.SODeclsOffset[i];
				if (offset != 0)
				{
					var soDeclReader = reader.CopyAtOffset((int)offset);
					result.SODecls.Add(soDeclReader.ReadString());
				}
			}
			var interfaceReader = reader.CopyAtOffset((int)interfaceBindingOffset);
			for (int i = 0; i < interfaceBindingCount; i++)
			{
				result.InterfaceBindings.Add(EffectInterfaceInitializer.Parse(reader, interfaceReader));
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
			foreach (var binding in InterfaceBindings)
			{
				sb.AppendLine(binding.Dump());
			}
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
			sb.Append("        ");
			var shaderText = Shader.ToString()
				.Replace(Environment.NewLine, $"{Environment.NewLine}        ");
			sb.AppendLine(shaderText);
			sb.Append("    }");
			for(int i = 0; i < InterfaceBindings.Count; i++)
			{
				var binding = InterfaceBindings[i];
				sb.AppendLine();
				sb.Append(string.Format("/* Interface parameter {0} bound to: {1} */", i, binding));
			}
			sb.Append(";");
			return sb.ToString();
		}
	}
}
