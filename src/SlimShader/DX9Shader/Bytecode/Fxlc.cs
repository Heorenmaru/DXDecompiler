using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode
{
	public class Fxlc
	{
		public List<Chunks.Fxlvm.FxlcToken> Tokens = new List<Chunks.Fxlvm.FxlcToken>();
		public static Fxlc Parse(BytecodeReader reader)
		{
			var result = new Fxlc();
			var tokenCount = reader.ReadUInt32();
			for (int i = 0; i < tokenCount; i++)
			{
				result.Tokens.Add(Chunks.Fxlvm.FxlcToken.Parse(reader, null));
			}
			return result;
		}
	}
}
