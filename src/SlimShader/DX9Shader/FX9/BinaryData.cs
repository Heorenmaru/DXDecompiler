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
		public string DataPreview
		{
			get
			{
				string dataPreview = "";
				/*for (int i = 0; i < Data.Length && i < 8 * 4; i += 4)
				{
					var val = BitConverter.ToUInt32(Data, i);
					dataPreview += val.ToString("X4") + " ";
				}*/
				for (int i = 0; i < Data.Length; i++)
				{
					var val = (char)Data[i];
					dataPreview += val;
				}
				return dataPreview;
			}
		}
		public bool IsShader => Data.Length >= 4 && Data[0] == 0 && Data[1] == 2;
		public string Version
		{
			get
			{
				if (!IsShader)
				{
					return "";
				}
				var minor = Data[0];
				var major = Data[1];
				var type = (ShaderType)BitConverter.ToUInt16(Data, 2);
				return $"{type}_{major}_{minor}";
			}
		}
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
			sb.AppendLine($"    BinaryData.Data: {DataPreview}");
			return sb.ToString();
		}
	}
}
