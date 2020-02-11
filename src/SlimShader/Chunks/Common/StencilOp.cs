using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Common
{
	/// <summary>
	/// Based on D3D12_STENCIL_OP
	/// </summary>
	public enum StencilOp
	{
		Keep = 1,
		Zero = 2,
		Replace = 3,
		IncrStat = 4,
		DecrStat = 5,
		Invert = 6,
		Incr = 7,
		Decr = 8
	}
}
