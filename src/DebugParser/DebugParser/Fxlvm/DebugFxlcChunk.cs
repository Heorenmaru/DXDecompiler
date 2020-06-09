﻿using SlimShader.DX9Shader;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugFxlcChunk : DebugBytecodeChunk
	{
		public List<DebugFxlcToken> Tokens = new List<DebugFxlcToken>();

		public static DebugBytecodeChunk Parse(DebugBytecodeReader reader, uint chunkSize, DebugBytecodeContainer container)
		{
			var result = new DebugFxlcChunk();
			var chunkReader = reader.CopyAtCurrentPosition("FxlcChunkReader", reader);
			var tokenCount = chunkReader.ReadUInt32("TokenCount");
			for (int i = 0; i < tokenCount; i++)
			{
				result.Tokens.Add(DebugFxlcToken.Parse(chunkReader, container));
			}
			return result;
		}
	}
}
