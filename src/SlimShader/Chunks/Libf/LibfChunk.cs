using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Libf
{
	/// <summary>
	/// Might be related to D3D11_FUNCTION_DESC
	/// </summary>
	public class LibfChunk : BytecodeChunk
	{
		public byte[] Data;
		BytecodeContainer Container;
		public static LibfChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new LibfChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var data = chunkReader.ReadBytes((int)chunkSize);
			result.Data = data;
			result.Container = new BytecodeContainer(data);
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
			sb.AppendLine("LibfChunk");
			//sb.Append(FormatReadable(Data));
			sb.AppendLine(Container.ToString());
			return sb.ToString();
		}
	}
}
