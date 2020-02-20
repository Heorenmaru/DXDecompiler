﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	// D3DXSHADER
	[Flags]
	public enum ShaderFlags
	{
		Debug = 1,
		SkipValidation = 2,
		SkipOptimization = 4,
		RowMajor = 8,
		ColumnMajor = 16,
		PartialPrecision = 32,
		ForceVSSoftwareNoOpt = 64,
		ForcePSSoftwareNoOpt = 128,
		NoPreShader = 256,
		AvoidFlowControl = 512,
		PreferFlowControl = 1024,
		EnableBackwardsCompatibility = 2048,
		IEEEStrictness = 4096,
		UseLegacyD3DX931Dll = 8192
	}
}
