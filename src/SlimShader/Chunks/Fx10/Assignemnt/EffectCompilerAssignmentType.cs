using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public enum EffectCompilerAssignmentType
	{
		Invalid,
		Constant,
		Variable,
		CosntantIndex,
		VariableIndex,
		ExpressionIndex,
		Expression,
		InlineShader,
		InlineShader5
	}
}
