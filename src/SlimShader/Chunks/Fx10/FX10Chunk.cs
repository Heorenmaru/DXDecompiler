using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class FX10Chunk : BytecodeChunk
	{
		public byte[] Data;
		public static FX10Chunk Parse(BytecodeReader reader, uint size)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var result = new FX10Chunk();
			result.Data = reader.ReadBytes((int)size);
			return result;
		}
		public static string FormatReadable(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				for (int j = i; j < i + 16; j++)
				{
					if (j < data.Length)
					{
						sb.Append(data[j].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append(" ");
					}
				}
				sb.Append("\t");
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (!char.IsControl(c))
					{
						sb.Append(c);
					}
					else
					{
						sb.Append('.');
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("FX10 Data:");
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
