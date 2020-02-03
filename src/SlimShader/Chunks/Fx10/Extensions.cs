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
				case EffectVariableType.Sampler:
					return ShaderVariableType.Sampler;
				case EffectVariableType.Buffer:
					return ShaderVariableType.Buffer;
				case EffectVariableType.TextureCubeArray:
					return ShaderVariableType.TextureCubeArray;
				default:
					throw new Exception($"Unknown effect variable type {effectType}");
			}
		}
	}
}
