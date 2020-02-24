using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class InlineShader
	{
		public uint Unknown1;
		public uint Unknown2;
		public uint Unknown3;
		public uint Index;
		public uint IsVariable;
		public uint ShaderSize;
		public string VariableName { get; private set; }
		public byte[] Data = new byte[0];
		public static InlineShader Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{
			var result = new InlineShader();
			result.Unknown1 = shaderReader.ReadUInt32();
			result.Unknown2 = shaderReader.ReadUInt32();
			result.Unknown3 = shaderReader.ReadUInt32();
			result.Index = shaderReader.ReadUInt32();
			result.IsVariable = shaderReader.ReadUInt32();
			var dataReader = shaderReader.CopyAtCurrentPosition();
			result.ShaderSize = shaderReader.ReadUInt32();
			var toRead = result.ShaderSize + (result.ShaderSize % 4 == 0 ? 0 : 4 - result.ShaderSize % 4);
			result.Data = shaderReader.ReadBytes((int)toRead);
			if (result.IsVariable == 1)
			{
				result.VariableName = dataReader.TryReadString();
			} else
			{
				result.VariableName = "";
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    InlineShader.Unk1: {Unknown1} {Unknown1.ToString("X4")}");
			sb.AppendLine($"    InlineShader.Unk2: {Unknown2} {Unknown2.ToString("X4")}");
			sb.AppendLine($"    InlineShader.Unk3: {Unknown3} {Unknown3.ToString("X4")}");
			sb.AppendLine($"    InlineShader.Index?: {Index} {Index.ToString("X4")}");
			sb.AppendLine($"    InlineShader.IsVariable: {IsVariable} {IsVariable.ToString("X4")}");
			sb.AppendLine($"    InlineShader.ShaderSize: {ShaderSize} {ShaderSize.ToString("X4")}");
			sb.AppendLine($"    InlineShader.DataLength: {Data.Length} {Data.Length.ToString("X4")}");
			if (IsVariable == 1)
			{
				sb.AppendLine($"    InlineShader.VariableName: {VariableName}");
			}
			return sb.ToString();
		}
	}
}
