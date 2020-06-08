using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class VariableBlob
	{
		public uint Index;
		public uint Size;
		public byte[] Data;
		public string Value = "";
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
				switch (type) {
					case ShaderType.Pixel:
						return $"ps_{major}_{minor}";
					case ShaderType.Vertex:
						return $"vs_{major}_{minor}";
					case ShaderType.Fx:
						return $"fx_{major}_{minor}";
					case ShaderType.Tx:
						return $"tx_{major}_{minor}";
					default:
						return "unknown_version";
				}
			}
		}
		public static VariableBlob Parse(BytecodeReader reader, BytecodeReader dataReader)
		{
			var result = new VariableBlob();
			result.Index = dataReader.ReadUInt32();
			result.Size = dataReader.ReadUInt32();
			var toRead = result.Size + (result.Size % 4 == 0 ? 0 : 4 - result.Size % 4);
			result.Data = dataReader.ReadBytes((int)toRead);
			if (!result.IsShader)
			{
				if (result.Size == 0)
				{
					result.Value = "";
				}
				else
				{
					result.Value = Encoding.UTF8.GetString(result.Data, 0, (int)(result.Size - 1));
				}
			}

			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    BinaryData.Index: {Index} {Index.ToString("X4")}");
			sb.AppendLine($"    BinaryData.Size: {Size} {Size.ToString("X4")}");
			sb.AppendLine($"    BinaryData.DataSize: {Data.Length} {Data.Length.ToString("X4")}");
			sb.AppendLine($"    BinaryData.Data: {Value}");
			return sb.ToString();
		}
	}
}
