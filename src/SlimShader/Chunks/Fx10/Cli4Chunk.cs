using SlimShader.Util;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class Cli4Chunk : BytecodeChunk
	{
		byte[] Data;
		public static BytecodeChunk Parse(BytecodeReader chunkReader, uint chunkSize)
		{
			var result = new Cli4Chunk();
			result.Data = chunkReader.ReadBytes((int)chunkSize);
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
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
