using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode.Fxlvm
{
	public class FxlcBlock
	{
		public List<FxlcToken> Tokens = new List<FxlcToken>();
		public static FxlcBlock Parse(BytecodeReader reader)
		{
			var result = new FxlcBlock();
			var tokenCount = reader.ReadUInt32();
			for (int i = 0; i < tokenCount; i++)
			{
				result.Tokens.Add(FxlcToken.Parse(reader));
			}
			return result;
		}
	}
}
