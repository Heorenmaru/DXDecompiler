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
		uint probablyVersion;
		uint variableCount;
		uint bufferCount;
		uint localObjectCount;
		uint unknown1;
		uint unknown2;
		uint unknown3;
		uint techniqueCount;
		uint unknown4;
		uint unknown5;
		uint unknown6;
		uint unknown7;
		uint unknown8;
		uint unknown9;
		uint unknown10;
		uint unknown11;
		uint unknown12;
		uint numberOfShaders1;
		uint numberOfShaders2;
		uint unknown15;
		public static FX10Chunk Parse(BytecodeReader reader, uint size)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var dataReader = reader.CopyAtCurrentPosition();
			var result = new FX10Chunk();
			var probablyVersion = result.probablyVersion = chunkReader.ReadUInt32();
			var bufferCount = result.bufferCount = chunkReader.ReadUInt32();
			//Global Variable Count
			var variableCount = result.variableCount = chunkReader.ReadUInt32();
			var localObjectCount = result.localObjectCount = chunkReader.ReadUInt32();
			var unknown1 = result.unknown1 = chunkReader.ReadUInt32();
			var unknown2 = result.unknown2 = chunkReader.ReadUInt32();
			var unknown3 = result.unknown3 = chunkReader.ReadUInt32();
			var techniqueCount = result.techniqueCount = chunkReader.ReadUInt32();
			//probably a size or offset
			var unknown4 = result.unknown4 = chunkReader.ReadUInt32();
			var unknown5 = result.unknown5 = chunkReader.ReadUInt32();
			var unknown6 = result.unknown6 = chunkReader.ReadUInt32();
			var unknown7 = result.unknown7 = chunkReader.ReadUInt32();
			var unknown8 = result.unknown8 = chunkReader.ReadUInt32();
			var unknown9 = result.unknown9 = chunkReader.ReadUInt32();
			var unknown10 = result.unknown10 = chunkReader.ReadUInt32();
			var unknown11 = result.unknown11 = chunkReader.ReadUInt32();
			var unknown12 = result.unknown12 = chunkReader.ReadUInt32();
			var numberOfShaders1 = result.numberOfShaders1 = chunkReader.ReadUInt32();
			var numberOfShaders2 = result.numberOfShaders2 = chunkReader.ReadUInt32();
			var unknown15 = result.unknown15 = chunkReader.ReadUInt32();

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

			sb.AppendLine($"probablyVersion: {probablyVersion}");
			sb.AppendLine($"variableCount: {variableCount}");
			sb.AppendLine($"bufferCount: {bufferCount}");
			sb.AppendLine($"localObjectCount: {localObjectCount}");
			sb.AppendLine($"unknown1: {unknown1}");
			sb.AppendLine($"unknown2: {unknown2}");
			sb.AppendLine($"unknown3: {unknown3}");
			sb.AppendLine($"techniqueCount: {techniqueCount}");
			sb.AppendLine($"unknown4: {unknown4} {unknown4.ToString("X4")}");
			sb.AppendLine($"unknown5: {unknown5}");
			sb.AppendLine($"unknown6: {unknown6}");
			sb.AppendLine($"unknown7: {unknown7}");
			sb.AppendLine($"unknown8: {unknown8}");
			sb.AppendLine($"unknown9: {unknown9}");
			sb.AppendLine($"unknown10: {unknown10}");
			sb.AppendLine($"unknown12: {unknown11}");
			sb.AppendLine($"unknown11: {unknown12}");
			sb.AppendLine($"numberOfShaders1: {numberOfShaders1}");
			sb.AppendLine($"numberOfShaders2: {numberOfShaders2}");
			sb.AppendLine($"unknown15: {unknown15}");
			sb.AppendLine("FX10 Data:");
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
