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
		RenderTargetView = 19,
		DepthStencilView = 20,
		Sampler = 21,
		Buffer = 22,
		TextureCubeArray = 23,

		/// <summary>
		/// fx_5_0 types
		/// </summary>
		ComputeShader = 28,
		HullShader = 29,
		DomainShader = 30,
		RWTexture1D = 31,
		RWTexture1DArray = 32,
		RWTexture2D = 33,
		RWTexture2DArray = 34,
		RWTexture3D = 35,
		RWBuffer = 36,
		ByteAddressBuffer = 37,
		RWByteAddressBuffer = 38,
		StructuredBuffer = 39,
		RWStructuredBuffer = 40,
		AppendStructuredBuffer = 43,
		ConsumeStructuredBuffer = 44,
		

		Void = 0x1000,
		Float = 0x1001,
		Int = 0x1002,
		UInt = 0x1004,
		Bool = 0x1005,
	}
}
