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
		public string Value = "";
		public bool IsShader => Shader != null;
		public ShaderModel Shader;
		public string Version
		{
			get
			{
				if (!IsShader)
				{
					return "";
				}
				var type = Shader.Type;
				var major = Shader.MajorVersion;
				var minor = Shader.MinorVersion;
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
		static bool _IsShader(byte[] data)
		{
			if (data.Length < 4) return false;
			var type = (ShaderType)BitConverter.ToUInt16(data, 2);
			switch (type)
			{
				case ShaderType.Fx:
				case ShaderType.Pixel:
				case ShaderType.Tx:
				case ShaderType.Vertex:
					return true;
				default:
					return false;
			}
		}
		public static VariableBlob Parse(BytecodeReader reader, BytecodeReader dataReader)
		{
			var result = new VariableBlob();
			result.Index = dataReader.ReadUInt32();
			result.Size = dataReader.ReadUInt32();
			var paddedSize = result.Size + (result.Size % 4 == 0 ? 0 : 4 - result.Size % 4);
			var shaderReader = dataReader.CopyAtCurrentPosition();
			var data = dataReader.ReadBytes((int)paddedSize);
			if (!_IsShader(data))
			{
				if (result.Size == 0)
				{
					result.Value = "";
				}
				else
				{
					result.Value = Encoding.UTF8.GetString(data, 0, (int)(result.Size - 1));
				}
			} else
			{
				result.Shader = ShaderModel.Parse(shaderReader);
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    BinaryData.Index: {Index} {Index.ToString("X4")}");
			sb.AppendLine($"    BinaryData.Size: {Size} {Size.ToString("X4")}");
			sb.AppendLine($"    BinaryData.Data: {Value}");
			return sb.ToString();
		}
	}
}
