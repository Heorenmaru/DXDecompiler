using SlimShader.Chunks.Fx10.FXLVM;
using SlimShader.DX9Shader;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class FxlcChunk : BytecodeChunk
	{
		public ShaderModel ShaderModel;
		public List<FxlcToken> Tokens = new List<FxlcToken>();

		uint TokenCount;
		byte[] Data;
		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new FxlcChunk();
			result.ShaderModel = new ShaderModel(5, 0, ShaderType.Fx);
			var chunkReader = reader.CopyAtCurrentPosition();
			var tokenCount = result.TokenCount = chunkReader.ReadUInt32();
			try
			{
				for (int i = 0; i < tokenCount; i++)
				{
					result.Tokens.Add(FxlcToken.Parse(chunkReader));
				}
			} catch(Exception ex)
			{

			}
			result.Data = reader.ReadBytes((int)chunkSize);
			return result;
		}
		public static string FormatReadable(byte[] data, bool endian = false)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
				for (int j = i; j < i + 16; j++)
				{
					var index = endian ? j : j + (3 - (j % 4) * 2);
					if (index < data.Length)
					{
						sb.Append(data[index].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append("  ");
					}
				}
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (char.IsControl(c))
					{
						sb.Append("_");
					}
					else if (c > 0x7E)
					{
						sb.Append('.');
					}
					else if (char.IsWhiteSpace(c))
					{
						sb.Append('.');
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetType().Name);
			sb.AppendLine($"Tokens TokenCount {TokenCount} TokensRead {Tokens.Count} ExpectedTokenCount {(Data.Length-16)/(4*8)} ChunkSize {Data.Length}");
			foreach (var token in Tokens)
			{
				sb.AppendLine(token.ToString());
			}
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
