using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class BinaryData
	{
		public uint Index;
		public uint Size;
		public byte[] Data;
		public static BinaryData Parse(BytecodeReader reader, BytecodeReader dataReader)
		{
			var result = new BinaryData();
			result.Index = dataReader.ReadUInt32();
			result.Size = dataReader.ReadUInt32();
			var toRead = result.Size + (result.Size % 4 == 0 ? 0 : 4 - result.Size % 4);
			result.Data = dataReader.ReadBytes((int)toRead);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    BinaryData.Index: {Index} {Index.ToString("X4")}");
			sb.AppendLine($"    BinaryData.Size: {Size} {Size.ToString("X4")}");
			sb.AppendLine($"    BinaryData.DataSize: {Data.Length} {Data.Length.ToString("X4")}");
			return sb.ToString();
		}
	}
}
