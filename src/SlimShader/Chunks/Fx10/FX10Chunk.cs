using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Looks vaguely similar to D3D10_EFFECT_DESC
	/// </summary>
	public class FX10Chunk : BytecodeChunk
	{
		public byte[] Data;
		uint unknownOffset;
		public static FX10Chunk Parse(BytecodeReader reader, uint size)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var dataReader = reader.CopyAtCurrentPosition();
			var result = new FX10Chunk();
			var probablyVersion = chunkReader.ReadUInt32();
			var bufferCount = chunkReader.ReadUInt32();
			//Global Variable Count
			var variableCount = chunkReader.ReadUInt32();
			var localObjectCount = chunkReader.ReadUInt32();
			var unknown1 = chunkReader.ReadUInt32();
			var unknown2 = chunkReader.ReadUInt32();
			var unknown3 = chunkReader.ReadUInt32();
			var techniqueCount = chunkReader.ReadUInt32();
			//probably a size or offset
			var unknown4 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown5 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown6 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown7 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown8 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown9 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown10 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown11 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown12 = result.unknownOffset = chunkReader.ReadUInt32();
			var numberOfShaders1 = result.unknownOffset = chunkReader.ReadUInt32();
			var numberOfShaders2 = result.unknownOffset = chunkReader.ReadUInt32();
			var unknown15 = result.unknownOffset = chunkReader.ReadUInt32();

			result.Data = dataReader.ReadBytes((int)size);
			return result;
		}
		public static string FormatReadable(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
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
