using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public enum EffectVariableType
	{
		String = 1,
		Blend = 2,
		DepthStencil = 3,
		Rasterizer = 4,
		PixelShader = 5,
		VertexShader = 6,
		GeometryShader = 7,
		GeometryShaderWithStream = 8,
		Texture1D = 10,
		Texture1DArray = 11,
		Texture2D = 12,
		Texture2DArray = 13,
		Texture2DMultiSampled = 14,
		Texture2DMultiSampledArray = 15,
		Texture3D = 16,
		TextureCube = 17,
		Sampler = 21,
		Buffer = 22,
		TextureCubeArray = 23,

		Void = 0x100,
		Float = 0x101,
		Int = 0x102,
		UInt = 0x104,
		Bool = 0x105,
	}
}
