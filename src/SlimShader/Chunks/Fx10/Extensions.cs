using SlimShader.Chunks.Rdef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public static class Extensions
	{
		public static ShaderVariableType ToShaderVariableType(this EffectVariableType effectType)
		{
			switch (effectType)
			{
				case EffectVariableType.Void:
					return ShaderVariableType.Void;
				case EffectVariableType.Float:
					return ShaderVariableType.Float;
				case EffectVariableType.UInt:
					return ShaderVariableType.UInt;
				case EffectVariableType.Int:
					return ShaderVariableType.Int;
				case EffectVariableType.Bool:
					return ShaderVariableType.Bool;

				case EffectVariableType.String:
					return ShaderVariableType.String;
				case EffectVariableType.Blend:
					return ShaderVariableType.Blend;
				case EffectVariableType.DepthStencil:
					return ShaderVariableType.DepthStencil;
				case EffectVariableType.Rasterizer:
					return ShaderVariableType.Rasterizer;
				case EffectVariableType.PixelShader:
					return ShaderVariableType.PixelShader;
				case EffectVariableType.VertexShader:
					return ShaderVariableType.VertexShader;
				case EffectVariableType.GeometryShader:
					return ShaderVariableType.GeometryShader;
				case EffectVariableType.GeometryShaderWithStream:
					return ShaderVariableType.GeometryShader;
				case EffectVariableType.Texture:
					return ShaderVariableType.Texture;
				case EffectVariableType.Texture1D:
					return ShaderVariableType.Texture1D;
				case EffectVariableType.Texture1DArray:
					return ShaderVariableType.Texture1DArray;
				case EffectVariableType.Texture2D:
					return ShaderVariableType.Texture2D;
				case EffectVariableType.Texture2DArray:
					return ShaderVariableType.Texture2DArray;
				case EffectVariableType.Texture2DMultiSampled:
					return ShaderVariableType.Texture2DMultiSampled;
				case EffectVariableType.Texture2DMultiSampledArray:
					return ShaderVariableType.Texture2DMultiSampledArray;
				case EffectVariableType.Texture3D:
					return ShaderVariableType.Texture3D;
				case EffectVariableType.TextureCube:
					return ShaderVariableType.TextureCube;
				case EffectVariableType.RenderTargetView:
					return ShaderVariableType.RenderTargetView;
				case EffectVariableType.DepthStencilView:
					return ShaderVariableType.DepthStencilView;
				case EffectVariableType.Sampler:
					return ShaderVariableType.Sampler;
				case EffectVariableType.Buffer:
					return ShaderVariableType.Buffer;
				case EffectVariableType.TextureCubeArray:
					return ShaderVariableType.TextureCubeArray;
				case EffectVariableType.ComputeShader:
					return ShaderVariableType.ComputeShader;
				case EffectVariableType.HullShader:
					return ShaderVariableType.HullShader;
				case EffectVariableType.DomainShader:
					return ShaderVariableType.DomainShader;
				//TODO: Bring RW and ReadWrite notation into alignment
				case EffectVariableType.RWTexture1D:
					return ShaderVariableType.ReadWriteTexture1D;
				case EffectVariableType.RWTexture1DArray:
					return ShaderVariableType.ReadWriteTexture1DArray;
				case EffectVariableType.RWTexture2D:
					return ShaderVariableType.ReadWriteTexture2D;
				case EffectVariableType.RWTexture2DArray:
					return ShaderVariableType.ReadWriteTexture2DArray;
				case EffectVariableType.RWTexture3D:
					return ShaderVariableType.ReadWriteTexture3D;
				case EffectVariableType.RWBuffer:
					return ShaderVariableType.ReadWriteBuffer;
				case EffectVariableType.ByteAddressBuffer:
					return ShaderVariableType.ByteAddressBuffer;
				case EffectVariableType.RWByteAddressBuffer:
					return ShaderVariableType.ReadWriteByteAddressBuffer;
				case EffectVariableType.StructuredBuffer:
					return ShaderVariableType.StructuredBuffer;
				case EffectVariableType.RWStructuredBuffer:
					return ShaderVariableType.ReadWriteStructuredBuffer;
				case EffectVariableType.AppendStructuredBuffer:
					return ShaderVariableType.AppendStructuredBuffer;
				case EffectVariableType.ConsumeStructuredBuffer:
					return ShaderVariableType.ConsumeStructuredBuffer;
				default:
					throw new Exception($"Unknown effect variable type {effectType}");
			}
		}
	}
}
