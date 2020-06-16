using SlimShader.Util;
using System;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	/*
	 * 
	 */ 
	public class StateBlob
	{
		public uint TechniqueIndex;
		public uint PassIndex;
		public uint SamplerStateIndex;
		public uint AssignmentIndex;
		public StateBlobType BlobType;
		public uint BlobSize;
		public string VariableName { get; private set; }
		public byte[] Data = new byte[0];
		public uint DataStart;
		public uint DataEnd;
		public ShaderModel Shader;
		public ShaderType ShaderType => Shader.Type;
		public string VersionString
		{
			get
			{
				if (BlobType != StateBlobType.Shader)
				{
					return "";
				}
				var minor = Data[0];
				var major = Data[1];
				var type = (ShaderType)BitConverter.ToUInt16(Data, 2);
				switch (type)
				{
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
		public static StateBlob Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{
			var result = new StateBlob();
			result.TechniqueIndex = shaderReader.ReadUInt32();
			result.PassIndex = shaderReader.ReadUInt32();
			result.SamplerStateIndex = shaderReader.ReadUInt32();
			result.AssignmentIndex = shaderReader.ReadUInt32();
			result.BlobType = (StateBlobType)shaderReader.ReadUInt32();
			var dataReader = shaderReader.CopyAtCurrentPosition();
			result.BlobSize = shaderReader.ReadUInt32();
			var paddedSize = result.BlobSize + (result.BlobSize % 4 == 0 ? 0 : 4 - result.BlobSize % 4);
			result.Data = shaderReader.ReadBytes((int)paddedSize);
			if(result.BlobType == StateBlobType.Shader)
			{
				result.Shader = ShaderReader.ReadShader(result.Data);
			}
			else if (result.BlobType == StateBlobType.Variable)
			{
				result.VariableName = dataReader.TryReadString();
			} 
			else if(result.BlobType == StateBlobType.IndexShader)
			{
				var blobSize = dataReader.ReadUInt32();
				var variableSize = dataReader.ReadUInt32();
				result.VariableName = dataReader.ReadString();
				if (variableSize > (result.VariableName.Length + 1)) {
					var paddingCount = variableSize - (result.VariableName.Length + 1);
					var padding = dataReader.ReadBytes((int)paddingCount);
				}
				result.Shader = result.Shader = ShaderModel.Parse(dataReader);
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    InlineShader.TechniqueIndex: {TechniqueIndex} {TechniqueIndex.ToString("X4")}");
			sb.AppendLine($"    InlineShader.StateIndex: {PassIndex} {PassIndex.ToString("X4")}");
			sb.AppendLine($"    InlineShader.SamplerIndex: {SamplerStateIndex} {SamplerStateIndex.ToString("X4")}");
			sb.AppendLine($"    InlineShader.AssignmentIndex: {AssignmentIndex} {AssignmentIndex.ToString("X4")}");
			sb.AppendLine($"    InlineShader.BlobType: {BlobType}");
			sb.AppendLine($"    InlineShader.BlobSize: {BlobSize} {BlobSize.ToString("X4")}");
			if (BlobType == StateBlobType.Variable)
			{
				sb.AppendLine($"    InlineShader.VariableName: {VariableName}");
			} else
			{
				sb.AppendLine($"    InlineShader.Version: {VersionString}");
				string dataPreview = "";
				for (int i = 0; i < Data.Length && i < 8 * 4; i += 4)
				{
					var val = BitConverter.ToUInt32(Data, i);
					dataPreview += val.ToString("X8") + " ";
				}
				sb.AppendLine($"    InlineShader.Data: {dataPreview}");
			}
			return sb.ToString();
		}
	}
}
