using SlimShader.DebugParser.Chunks.Fxlvm;
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
				result.Tokens.Add(DebugFxlcToken.Parse(reader, null));
			}
			return result;
		}
	}
}
