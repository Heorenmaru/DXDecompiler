using SlimShader.Chunks.Common;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Looks vaguely similar to D3D10_EFFECT_DESC and D3D10_STATE_BLOCK_MASK
	/// </summary>
	public class FX10Chunk : BytecodeChunk
	{
		public byte[] Data;
		public ShaderVersion Version { get; private set; }
		/// <summary>
		/// Number of global variables in this effect, excluding the effect pool.
		/// </summary>
		uint GlobalVariables;
		uint ConstantBuffers;
		uint ObjectCount;
		uint IsChildEffect;
		uint SharedGlobalVariables;
		/// <summary>
		/// Resources such as textures and SamplerState
		/// </summary>
		uint SharedObjectCount;
		uint Techniques;
		uint unknown4;
		uint unknown5;
		uint LocalTextureCount;
		uint DepthStencilStateCount;
		uint BlendStateCount;
		uint RasterizerStateCount;
		uint LocalSamplerCount;
		uint unknown11;
		uint unknown12;
		uint ShaderCount;
		uint UsedShaderCount;
		uint unknown15;
		uint Size;

		public byte[] UnknownData;
		public static FX10Chunk Parse(BytecodeReader reader, uint size)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var dataReader = reader.CopyAtCurrentPosition();
			var result = new FX10Chunk();
			result.Size = size;
			result.Version = ShaderVersion.ParseRdef(chunkReader);
			var bufferCount = result.ConstantBuffers = chunkReader.ReadUInt32();
			//Global Variable Count
			var variableCount = result.GlobalVariables = chunkReader.ReadUInt32();
			var localObjectCount = result.ObjectCount = chunkReader.ReadUInt32();
			var IsChildEffect = result.IsChildEffect = chunkReader.ReadUInt32();
			var SharedVariableCount = result.SharedGlobalVariables = chunkReader.ReadUInt32();
			var SharedObjectCount = result.SharedObjectCount = chunkReader.ReadUInt32();
			var techniqueCount = result.Techniques = chunkReader.ReadUInt32();
			//probably a size or offset
			var unknown4 = result.unknown4 = chunkReader.ReadUInt32();
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
			
			//proper offset is unknown4 + 48?
			var unknownReader = reader.CopyAtOffset((int)unknown4);
			result.UnknownData = unknownReader.ReadBytes((int)(size - unknown4));
			result.Data = dataReader.ReadBytes((int)size);
			return result;
		}
		public static string FormatReadable(byte[] data, bool endian = false)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
				for (int j = i; j < i + 16; j++)
				{
					var index = endian ? j : j + (3 - (j % 4) * 2);
					if (index < data.Length)
					{
						sb.Append(data[index].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append("  ");
					}
				}
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (char.IsControl(c))
					{
						sb.Append("_");
					}
					else if (c > 0x7E)
					{
						sb.Append('.');
					}
					else if (char.IsWhiteSpace(c))
					{
						sb.Append('.');
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"Size: {Size}");
			sb.AppendLine($"probablyVersion: {Version}");
			sb.AppendLine($"variableCount: {GlobalVariables}");
			sb.AppendLine($"bufferCount: {ConstantBuffers}");
			sb.AppendLine($"localObjectCount: {ObjectCount}");
			sb.AppendLine($"IsChildEffect: {IsChildEffect}");
			sb.AppendLine($"SharedVariableCount: {SharedGlobalVariables}");
			sb.AppendLine($"SharedObjectCount: {SharedObjectCount}");
			sb.AppendLine($"techniqueCount: {Techniques}");
			sb.AppendLine($"unknown4: {unknown4} - {unknown4.ToString("X4")}, {Size - unknown4}");
			sb.AppendLine($"unknown5: {unknown5}");
			sb.AppendLine($"TextureCount: {LocalTextureCount}");
			sb.AppendLine($"DepthStencilStateCount: {DepthStencilStateCount}");
			sb.AppendLine($"BlendStateCount: {BlendStateCount}");
			sb.AppendLine($"RasterizerStateCount: {RasterizerStateCount}");
			sb.AppendLine($"SamplerCount: {LocalSamplerCount}");
			sb.AppendLine($"unknown12: {unknown11}");
			sb.AppendLine($"unknown11: {unknown12}");
			sb.AppendLine($"ShaderCount: {ShaderCount}");
			sb.AppendLine($"UsedShaderCount: {UsedShaderCount}");
			sb.AppendLine($"unknown15: {unknown15}");
			sb.AppendLine("UnknownData:");
			sb.AppendLine(FormatReadable(UnknownData));
			sb.AppendLine("FX10 Data:");
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
