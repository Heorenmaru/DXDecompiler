using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.FXLVM
{
	public enum OperandType
	{
		Literal = 1,
		Variable = 2,
		Expr = 4,
		Temp = 7,
	}
}
