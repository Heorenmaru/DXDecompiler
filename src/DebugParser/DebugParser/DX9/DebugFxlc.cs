using SlimShader.DebugParser.Chunks.Fxlvm;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using SlimShader.Util;
using System.Collections.Generic;

namespace SlimShader.DebugParser.DX9
{
	public class DebugFxlc
	{
		List<DebugFxlcToken> Tokens = new List<DebugFxlcToken>();
		public static DebugFxlc Parse(DebugBytecodeReader reader)
		{
			var result = new DebugFxlc();
			var tokenCount = reader.ReadUInt32("TokenCount");
			for (int i = 0; i < tokenCount; i++)
			{
				var token = reader.PeakUint32();
				var type = (FxlcTokenType)token.DecodeValue(20, 30);
				reader.AddIndent($"Token{i}({type})");
				result.Tokens.Add(DebugFxlcToken.Parse(reader));
				reader.RemoveIndent();
			}
			return result;
		}
	}
}
