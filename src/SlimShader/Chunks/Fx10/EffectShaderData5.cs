﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectShaderData5
	{
		public BytecodeContainer Shader { get; private set; }
		public List<string> SODecls { get; private set; }

		public uint[] SODeclsOffset { get; private set; }
		public uint SODeclsCount { get; private set; }
		public uint RasterizedStream { get; private set; }
		public uint InterfaceBindingCount { get; private set; }
		public uint InterfaceBindingOffset { get; private set; }

		uint ShaderOffset;
		public EffectShaderData5()
		{
			SODeclsOffset = new uint[4];
			SODecls = new List<string>();
		}
		public static EffectShaderData5 Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectShaderData5();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32();
			result.SODeclsOffset[0] = variableReader.ReadUInt32();
			result.SODeclsOffset[1] = variableReader.ReadUInt32();
			result.SODeclsOffset[2] = variableReader.ReadUInt32();
			result.SODeclsOffset[3] = variableReader.ReadUInt32();
			var SoDeclsCount = result.SODeclsCount = variableReader.ReadUInt32();
			result.RasterizedStream = variableReader.ReadUInt32();
			result.InterfaceBindingCount = variableReader.ReadUInt32();
			result.InterfaceBindingOffset = variableReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			for(int i = 0; i < 4; i++)
			{
				var offset = result.SODeclsOffset[i];
				if (offset != 0)
				{
					var soDeclReader = reader.CopyAtOffset((int)offset);
					result.SODecls.Add(soDeclReader.ReadString());
				}
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectObject.SODecls0: {SODeclsOffset[0]} ({SODeclsOffset[0].ToString("X4")})");
			sb.AppendLine($"  EffectObject.SODecls1: {SODeclsOffset[1]} ({SODeclsOffset[1].ToString("X4")})");
			sb.AppendLine($"  EffectObject.SODecls2: {SODeclsOffset[2]} ({SODeclsOffset[2].ToString("X4")})");
			sb.AppendLine($"  EffectObject.SODecls3: {SODeclsOffset[3]} ({SODeclsOffset[3].ToString("X4")})");
			sb.AppendLine($"  EffectObject.SODeclsCount: {SODeclsCount} ({SODeclsCount.ToString("X4")})");
			sb.AppendLine($"  EffectObject.RasterizedStream: {RasterizedStream} ({RasterizedStream.ToString("X4")})");
			sb.AppendLine($"  EffectObject.InterfaceBindingCount: {InterfaceBindingCount} ({InterfaceBindingCount.ToString("X4")})");
			sb.AppendLine($"  EffectObject.InterfaceBindingOffset: {InterfaceBindingOffset} ({InterfaceBindingOffset.ToString("X4")})");
			return sb.ToString();
		}
		public override string ToString()
		{
			if (Shader == null) return "NULL";
			var sb = new StringBuilder();
			sb.AppendLine("    asm {");
			sb.Append("        ");
			var shaderText = Shader.ToString()
				.Replace(Environment.NewLine, $"{Environment.NewLine}        ");
			sb.AppendLine(shaderText);
			sb.Append("}");
			if (SODecls.Count > 0)
			{
				for (int i = 0; i < SODecls.Count; i++)
				{
					if (string.IsNullOrEmpty(SODecls[i]))
					{
						break;
					}
					sb.AppendLine();
					sb.Append(string.Format("/* Stream {0} out decl: \"{1}\" */", i, SODecls[i]));
				}
				sb.AppendLine();
				sb.Append(string.Format("/* Stream {0} to rasterizer */", RasterizedStream));
			}
			if(InterfaceBindingCount != 0 || InterfaceBindingOffset != 0)
			{
				sb.AppendLine();
				sb.Append("interfacebind");
			}
			return sb.ToString();
		}
	}
}
