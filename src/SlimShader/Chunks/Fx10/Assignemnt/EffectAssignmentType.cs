using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public enum EffectAssignmentType
	{
		RasterizerState = 0,
		DepthStencilState = 1,
		BlenderState = 2,
		VertexShader = 6,
		PixelShader = 7,
		GeometryShader = 8,
		DS_StencilRef = 9,
		AB_BlendFactor = 10,
		AB_SampleMask = 11,
		/// <summary>
		/// Rasterizer State
		/// Based on D3D10_RASTERIZER_DESC for list of types
		/// </summary>
		FillMode = 12,
		CullMode = 13,
		FrontCounterClockwise = 14,
		DepthBias = 15,
		DepthBiasClamp = 16,
		SlopeScaledDepthBias = 17,
		DepthClipEnable = 18,
		ScissorEnable = 19,
		MultiSampleEnable = 20,
		AntialiasedLineEnable = 21,
		/// <summary>
		/// Depth Stencil State
		/// Based on D3D10_DEPTH_STENCIL_DESC for list of types
		/// </summary>
		DepthEnable = 22,
		DepthWriteMask = 23,
		DepthFunc = 24,
		StencilEnable = 25,
		StencilReadMask = 26,
		StencilWriteMask = 27,
		FrontFaceStencilFail = 28,
		FrontFaceStencilDepthFail = 29,
		FrontFaceStencilPass = 30,
		FrontFaceStencilFunc = 31,
		BackFaceStencilFail = 32,
		BackFaceStencilDepthFail = 33,
		BackFaceStencilPass = 34,
		BackFaceStencilFunc = 35,
		/// <summary>
		/// Blend State
		/// Based on D3D10_BLEND_DESC for list of types
		/// </summary>
		AlphaToCoverageEnable = 36,
		BlendEnable = 37,
		SrcBlend = 38,
		DestBlend = 39,
		BlendOp = 40,
		SrcBlendAlpha = 41,
		DestBlendAlpha = 42,
		BlendOpAlpha= 43,
		RenderTargetWriteMask = 44,
		/// <summary>
		/// SamplerState
		/// Based on D3D10_SAMPLER_DESC for list of types
		/// </summary>
		Filter = 45,
		AddressU = 46,
		AddressV = 47,
		AddressW = 48,
		MipLODBias = 49,
		MaxAnisotropy = 50,
		ComparisonFunc = 51,
		BorderColor = 52,
		MinLOD = 53,
		MaxLOD = 54,
		Texture = 55,
		/// <summary>
		/// FX5 Shaders
		/// </summary>
		HullShader = 56,
		DomainShader = 57,
		ComputeShader = 58,
	}
}
