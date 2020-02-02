using SlimShader.Chunks.Common;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class FX10Header
	{
		/// <summary>
		/// Based on D3D10_EFFECT_DESC and maybe D3D10_STATE_BLOCK_MASK?
		/// </summary>
		public ShaderVersion Version { get; private set; }
		/// <summary>
		/// Number of global variables in this effect, excluding the effect pool.
		/// </summary>
		public uint GlobalVariables;
		public uint ConstantBuffers;
		public uint ObjectCount;
		public uint SharedConstantBuffers;
		public uint SharedGlobalVariables;
		/// <summary>
		/// Resources such as textures and SamplerState
		/// </summary>
		public uint SharedObjectCount;
		public uint Techniques;
		public uint FooterOffset;
		public uint unknown5;
		public uint LocalTextureCount;
		public uint DepthStencilStateCount;
		public uint BlendStateCount;
		public uint RasterizerStateCount;
		public uint LocalSamplerCount;
		public uint unknown11;
		public uint unknown12;
		public uint ShaderCount;
		public uint UsedShaderCount;
		public uint unknown15;
		public static FX10Header Parse(BytecodeReader chunkReader)
		{
			var result = new FX10Header();
			result.Version = ShaderVersion.ParseRdef(chunkReader);
			var bufferCount = result.ConstantBuffers = chunkReader.ReadUInt32();
			//Global Variable Count
			var variableCount = result.GlobalVariables = chunkReader.ReadUInt32();
			var localObjectCount = result.ObjectCount = chunkReader.ReadUInt32();
			var SharedConstantBuffers = result.SharedConstantBuffers = chunkReader.ReadUInt32();
			var SharedVariableCount = result.SharedGlobalVariables = chunkReader.ReadUInt32();
			var SharedObjectCount = result.SharedObjectCount = chunkReader.ReadUInt32();
			var techniqueCount = result.Techniques = chunkReader.ReadUInt32();
			//probably a size or offset
			var unknown4 = result.FooterOffset = chunkReader.ReadUInt32();
			var unknown5 = result.unknown5 = chunkReader.ReadUInt32();
			Debug.Assert(unknown5 == 0, $"FX10Chunkl.unknown5 is {unknown5}");
			var TextureCount = result.LocalTextureCount = chunkReader.ReadUInt32();
			var DepthStencilStateCount = result.DepthStencilStateCount = chunkReader.ReadUInt32();
			var BlendStateCount = result.BlendStateCount = chunkReader.ReadUInt32();
			var RasterizerStateCount = result.RasterizerStateCount = chunkReader.ReadUInt32();
			var SamplerCount = result.LocalSamplerCount = chunkReader.ReadUInt32();
			var unknown11 = result.unknown11 = chunkReader.ReadUInt32();
			Debug.Assert(unknown11 == 0, $"FX10Chunkl.unknown11 is {unknown11}");
			var unknown12 = result.unknown12 = chunkReader.ReadUInt32();
			Debug.Assert(unknown12 == 0, $"FX10Chunkl.unknown12 is {unknown12}");
			var ShaderCount = result.ShaderCount = chunkReader.ReadUInt32();
			var UsedShaderCount = result.UsedShaderCount = chunkReader.ReadUInt32();
			var unknown15 = result.unknown15 = chunkReader.ReadUInt32();
			Debug.Assert(unknown15 == 0, $"FX10Chunkl.unknown15 is {unknown15}");
			return result;

		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  probablyVersion: {Version}");
			sb.AppendLine($"  VariableCount: {GlobalVariables}");
			sb.AppendLine($"  ConstantBufferCount: {ConstantBuffers}");
			sb.AppendLine($"  LocalObjectCount: {ObjectCount}");
			sb.AppendLine($"  SharedConstantBuffers: {SharedConstantBuffers}");
			sb.AppendLine($"  SharedVariableCount: {SharedGlobalVariables}");
			sb.AppendLine($"  SharedObjectCount: {SharedObjectCount}");
			sb.AppendLine($"  TechniqueCount: {Techniques}");
			sb.AppendLine($"  FooterOffset: {FooterOffset} - {FooterOffset.ToString("X4")}");
			sb.AppendLine($"  unknown5: {unknown5}");
			sb.AppendLine($"  LocalTextureCount: {LocalTextureCount}");
			sb.AppendLine($"  DepthStencilStateCount: {DepthStencilStateCount}");
			sb.AppendLine($"  BlendStateCount: {BlendStateCount}");
			sb.AppendLine($"  RasterizerStateCount: {RasterizerStateCount}");
			sb.AppendLine($"  LocalSamplerCount: {LocalSamplerCount}");
			sb.AppendLine($"  unknown12: {unknown11}");
			sb.AppendLine($"  unknown11: {unknown12}");
			sb.AppendLine($"  ShaderCount: {ShaderCount}");
			sb.AppendLine($"  UsedShaderCount: {UsedShaderCount}");
			sb.AppendLine($"  unknown15: {unknown15}");
			return sb.ToString();
		}
	}
}
