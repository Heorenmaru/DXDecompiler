﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectShaderData5
	{
		public BytecodeContainer Shader { get; private set; }
		public List<string> SODecls { get; private set; }
		public uint[] SODeclsOffset { get; private set; }
		public uint SODeclsCount { get; private set; }
		public uint RasterizedStream { get; private set; }
		public List<DebugEffectInterfaceInitializer> InterfaceBindings { get; private set; }

		public uint InterfaceBindingCount;
		public uint InterfaceBindingOffset;
		uint ShaderOffset;
		public DebugEffectShaderData5()
		{
			SODeclsOffset = new uint[4];
			SODecls = new List<string>();
			InterfaceBindings = new List<DebugEffectInterfaceInitializer>();
		}
		public static DebugEffectShaderData5 Parse(DebugBytecodeReader reader, 
				DebugBytecodeReader variableReader)
		{
			var result = new DebugEffectShaderData5();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32("ShaderOffset");
			result.SODeclsOffset[0] = variableReader.ReadUInt32("SODeclsOffset0");
			result.SODeclsOffset[1] = variableReader.ReadUInt32("SODeclsOffset1");
			result.SODeclsOffset[2] = variableReader.ReadUInt32("SODeclsOffset2");
			result.SODeclsOffset[3] = variableReader.ReadUInt32("SODeclsOffset3");
			var SoDeclsCount = result.SODeclsCount = variableReader.ReadUInt32("SoDeclsCount");
			result.RasterizedStream = variableReader.ReadUInt32("RasterizedStream");
			var interfaceBindingCount = result.InterfaceBindingCount = variableReader.ReadUInt32("InterfaceBindingCount");
			var interfaceBindingOffset = result.InterfaceBindingOffset = variableReader.ReadUInt32("InterfaceBindingOffset");
			var shaderReader = reader.CopyAtOffset("ShaderReader", variableReader, (int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32("ShaderSize");
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes("Shader", (int)shaderSize));
			}
			for(int i = 0; i < 4; i++)
			{
				var offset = result.SODeclsOffset[i];
				if (offset != 0)
				{
					var soDeclReader = reader.CopyAtOffset("SODeclReader", variableReader, (int)offset);
					result.SODecls.Add(soDeclReader.ReadString("SODecls"));
				}
			}
			var interfaceReader = reader.CopyAtOffset("InterfaceReader", variableReader, (int)interfaceBindingOffset);
			for(int i = 0; i < interfaceBindingCount; i++)
			{
				result.InterfaceBindings.Add(DebugEffectInterfaceInitializer.Parse(reader, interfaceReader));
			}
			return result;
		}
	}
}
