using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	/// <summary>
	/// VariableSize 4
	/// TechniqueSize 3 (No Pass)
	/// PassSized 3 (No Shader)
	/// InlineShaderSize 4
	/// AnnotationSize2
	/// StringSize = 7 (4 Variable, 3 Object, value="def")
	/// PixelShaderSize = 6 (4 Varaible, 2 Object)
	/// </summary>
	public class Fx9Chunk
	{
		const int headerLength = 4;
		public uint VariableCount;
		public uint TechniqueCount;
		public uint PassCount;
		public uint ShaderCount;
		public uint UnknownCount;
		public uint InlineShaderCount;
		List<Variable> Variables = new List<Variable>();
		List<Technique> Techniques = new List<Technique>();
		List<BinaryData> BinaryDataList = new List<BinaryData>();
		List<InlineShader> InlineShaders = new List<InlineShader>();
		public uint length;
		byte[] HeaderData;
		byte[] BodyData;
		byte[] FooterData;
		byte[] Data;
		uint unknownOffset;
		uint headerUnknown;
		string DebugError;
		public static Fx9Chunk Parse(BytecodeReader reader, uint length)
		{
			var result = new Fx9Chunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var footerOffset = result.unknownOffset = chunkReader.ReadUInt32();
			try
			{
				result.length = length;
				var bodyReader = chunkReader.CopyAtCurrentPosition();
				var footerReader = reader.CopyAtOffset((int)footerOffset + 4);
				var variableCount = result.VariableCount = footerReader.ReadUInt32();
				var techniqueCount = result.TechniqueCount = footerReader.ReadUInt32();
				result.PassCount = footerReader.ReadUInt32();
				result.ShaderCount = footerReader.ReadUInt32();
				for (int i = 0; i < variableCount; i++)
				{
					result.Variables.Add(Variable.Parse(bodyReader, footerReader));
				}
				for (int i = 0; i < techniqueCount; i++)
				{
					result.Techniques.Add(Technique.Parse(bodyReader, footerReader));
				}
				result.UnknownCount = footerReader.ReadUInt32();
				result.InlineShaderCount = footerReader.ReadUInt32();
				for (int i = 0; i < result.UnknownCount; i++)
				{
					result.BinaryDataList.Add(BinaryData.Parse(bodyReader, footerReader));
				}
				for (int i = 0; i < result.InlineShaderCount; i++)
				{
					result.InlineShaders.Add(InlineShader.Parse(bodyReader, footerReader));
				}
			} catch(Exception ex)
			{
				result.DebugError = ex.ToString();
			}
			var dataReader = reader.CopyAtCurrentPosition();
			result.HeaderData = dataReader.ReadBytes((int)headerLength);
			var bodyLength = (int)(Math.Max(footerOffset - headerLength, 0));
			result.BodyData = dataReader.ReadBytes(bodyLength);
			result.FooterData = dataReader.ReadBytes((int)(length - bodyLength - headerLength));
			result.Data = reader.ReadBytes((int)length);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Fx9Chunk");
			sb.AppendLine($"Length: {length} {length.ToString("X4")}");
			sb.AppendLine($"UnknownOffset: {unknownOffset} {unknownOffset.ToString("X4")}");
			sb.AppendLine($"HeaderUnknown: {headerUnknown} {headerUnknown.ToString("X4")}");
			sb.AppendLine($"VariableCount: {VariableCount} {VariableCount.ToString("X4")}");
			sb.AppendLine($"TechniqueCount: {TechniqueCount} {TechniqueCount.ToString("X4")}");
			sb.AppendLine($"PassCount?: {PassCount} {PassCount.ToString("X4")}");
			sb.AppendLine($"ShaderCount?: {ShaderCount} {ShaderCount.ToString("X4")}");
			for(int i = 0; i < Variables.Count; i++)
			{
				sb.AppendLine($"Variable {i}");
				sb.Append(Variables[i].Dump());
			}
			for (int i = 0; i < Techniques.Count; i++)
			{
				sb.AppendLine($"Technique {i}");
				sb.Append(Techniques[i].Dump());
			}
			sb.AppendLine($"UnknownCount?: {UnknownCount} {UnknownCount.ToString("X4")}");
			sb.AppendLine($"InlineShaderCount?: {InlineShaderCount} {InlineShaderCount.ToString("X4")}");
			for (int i = 0; i < BinaryDataList.Count; i++)
			{
				sb.AppendLine($"BinaryData {i}");
				sb.Append(BinaryDataList[i].Dump());
			}
			for (int i = 0; i < InlineShaders.Count; i++)
			{
				sb.AppendLine($"InlineShader {i}");
				sb.Append(InlineShaders[i].Dump());
			}
			sb.AppendLine($"Footer");
			if(FooterData != null)sb.Append(FormatUtil.FormatBytes(FooterData));
			sb.AppendLine($"Header");
			if (HeaderData != null) sb.Append(FormatUtil.FormatBytes(HeaderData));
			sb.AppendLine($"Body");
			if (BodyData != null) sb.Append(FormatUtil.FormatBytes(BodyData));
			sb.AppendLine($"Data");
			if (Data != null) sb.Append(FormatUtil.FormatBytes(Data));
			sb.Append(DebugError);
			return sb.ToString();
		}
	}
}
