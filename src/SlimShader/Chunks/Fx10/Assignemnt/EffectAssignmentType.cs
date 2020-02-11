using SlimShader.Chunks.Common;
using SlimShader.Chunks.Fx10.Assignemnt;
using SlimShader.Chunks.RTS0;
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
		[AssignmentType(typeof(bool))]
		DepthEnable = 22,
		[AssignmentType(typeof(DepthWriteMask))]
		DepthWriteMask = 23,
		[AssignmentType(typeof(ComparisonFunc))]
		DepthFunc = 24,
		[AssignmentType(typeof(bool))]
		StencilEnable = 25,
		[AssignmentType(typeof(byte))]
		StencilReadMask = 26,
		[AssignmentType(typeof(byte))]
		StencilWriteMask = 27,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilFail = 28,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilDepthFail = 29,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilPass = 30,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilFunc = 31,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilFail = 32,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilDepthFail = 33,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilPass = 34,
		[AssignmentType(typeof(StencilOp))]
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
		[AssignmentType(typeof(TextureAddressMode))]
		AddressU = 46,
		[AssignmentType(typeof(TextureAddressMode))]
		AddressV = 47,
		[AssignmentType(typeof(TextureAddressMode))]
		AddressW = 48,
		[AssignmentType(typeof(float))]
		MipLODBias = 49,
		[AssignmentType(typeof(uint))]
		MaxAnisotropy = 50,
		[AssignmentType(typeof(ComparisonFunc))]
		ComparisonFunc = 51,
		[AssignmentType(typeof(float))]
		BorderColor = 52,
		[AssignmentType(typeof(float))]
		MinLOD = 53,
		[AssignmentType(typeof(float))]
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
