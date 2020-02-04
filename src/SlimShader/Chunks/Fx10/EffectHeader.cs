using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectHeader
	{
		/// <summary>
		/// Based on D3D10_EFFECT_DESC and maybe D3D10_STATE_BLOCK_MASK?
		/// fx_4_x has a stride of 80 bytes
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
		public uint StringCount;
		public uint LocalTextureCount;
		public uint DepthStencilStateCount;
		public uint BlendStateCount;
		public uint RasterizerStateCount;
		public uint LocalSamplerCount;
		public uint RenderTargetViewCount;
		public uint DepthStencilViewCount;
		public uint ShaderCount;
		public uint InlineShaderCount;

		/// <summary>
		/// Start of fx_5_0 members
		/// </summary>
		public uint GroupCount;
		public uint UAVCount;
		public uint InterfaceVariableCount;
		public uint InterfaceVariableElementCount;
		public uint ClassInstanceElementCount;
		public static EffectHeader Parse(BytecodeReader chunkReader)
		{
			var result = new EffectHeader();
			result.Version = ShaderVersion.ParseFX(chunkReader);
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
			var unknown5 = result.StringCount = chunkReader.ReadUInt32();
			var TextureCount = result.LocalTextureCount = chunkReader.ReadUInt32();
			var DepthStencilStateCount = result.DepthStencilStateCount = chunkReader.ReadUInt32();
			var BlendStateCount = result.BlendStateCount = chunkReader.ReadUInt32();
			var RasterizerStateCount = result.RasterizerStateCount = chunkReader.ReadUInt32();
			var SamplerCount = result.LocalSamplerCount = chunkReader.ReadUInt32();
			result.RenderTargetViewCount = chunkReader.ReadUInt32();
			result.DepthStencilViewCount = chunkReader.ReadUInt32();
			var ShaderCount = result.ShaderCount = chunkReader.ReadUInt32();
			var UsedShaderCount = result.InlineShaderCount = chunkReader.ReadUInt32();
			if(result.Version.MajorVersion >= 5)
			{
				result.GroupCount = chunkReader.ReadUInt32();
				result.UAVCount = chunkReader.ReadUInt32();
				result.InterfaceVariableCount = chunkReader.ReadUInt32();
				result.InterfaceVariableElementCount = chunkReader.ReadUInt32();
				result.ClassInstanceElementCount = chunkReader.ReadUInt32();
			}
			return result;

		}
		public string Dump()
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
			sb.AppendLine($"  StringCount: {StringCount}");
			sb.AppendLine($"  LocalTextureCount: {LocalTextureCount}");
			sb.AppendLine($"  DepthStencilStateCount: {DepthStencilStateCount}");
			sb.AppendLine($"  BlendStateCount: {BlendStateCount}");
			sb.AppendLine($"  RasterizerStateCount: {RasterizerStateCount}");
			sb.AppendLine($"  LocalSamplerCount: {LocalSamplerCount}");
			sb.AppendLine($"  RenderTargetViewCount: {RenderTargetViewCount}");
			sb.AppendLine($"  DepthStencilViewCount: {DepthStencilViewCount}");
			sb.AppendLine($"  ShaderCount: {ShaderCount}");
			sb.AppendLine($"  InlineShaderCount: {InlineShaderCount}");
			if (Version.MajorVersion >= 5)
			{
				sb.AppendLine($"  GroupCount: {GroupCount}");
				sb.AppendLine($"  UAVCount: {UAVCount}");
				sb.AppendLine($"  InterfaceVariableCount: {InterfaceVariableCount}");
				sb.AppendLine($"  InterfaceVariableElementCount: {InterfaceVariableElementCount}");
				sb.AppendLine($"  ClassInstanceElementCount: {ClassInstanceElementCount}");
			}
			return sb.ToString();
		}
	}
}
