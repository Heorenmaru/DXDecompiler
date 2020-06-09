﻿using SlimShader.DX9Shader;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	public class FxlcChunk : BytecodeChunk
	{
		public List<FxlcToken> Tokens = new List<FxlcToken>();

		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize, BytecodeContainer container)
		{
			var result = new FxlcChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var tokenCount = chunkReader.ReadUInt32();
			for (int i = 0; i < tokenCount; i++)
			{
				result.Tokens.Add(FxlcToken.Parse(chunkReader, container));
			}
			return result;
		}
	}
}
