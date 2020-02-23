using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class AnnotationData
	{
		public ParameterType ParameterType { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public string Name { get; private set; }
		public uint NameOffset;
		public uint Unknown4;
		public uint Unknown5;
		public uint ColumnsOrStuctMembers;
		public uint Rows;
		public uint Unknown8;
		public static AnnotationData Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new AnnotationData();
			result.ParameterType = (ParameterType)variableReader.ReadUInt32();
			result.ParameterClass = (ParameterClass)variableReader.ReadUInt32();
			result.NameOffset = variableReader.ReadUInt32();
			result.Unknown4 = variableReader.ReadUInt32();
			result.Unknown5 = variableReader.ReadUInt32();
			result.ColumnsOrStuctMembers = variableReader.ReadUInt32();
			result.Rows = variableReader.ReadUInt32();
			result.Unknown8 = variableReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)result.NameOffset);
			result.Name = nameReader.TryReadString();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    AnnotationData.ParameterType: {ParameterType} {((uint)ParameterType).ToString("X4")}");
			sb.AppendLine($"    AnnotationData.ParameterClass: {ParameterClass} {((uint)ParameterClass).ToString("X4")}");
			sb.AppendLine($"    AnnotationData.Name: {Name}");
			sb.AppendLine($"    AnnotationData.NameOffset: {NameOffset} {NameOffset.ToString("X4")}");
			sb.AppendLine($"    AnnotationData.Unknown4: {Unknown4} {Unknown4.ToString("X4")}");
			sb.AppendLine($"    AnnotationData.Unknown5: {Unknown5} {Unknown5.ToString("X4")}");
			sb.AppendLine($"    AnnotationData.ColumnsOrStuctMembers: {ColumnsOrStuctMembers} {ColumnsOrStuctMembers.ToString("X4")}");
			sb.AppendLine($"    AnnotationData.Rows: {Rows} {Rows.ToString("X4")}");
			sb.AppendLine($"    AnnotationData.Unknown8: {Unknown8} {Unknown8.ToString("X4")}");
			return sb.ToString();
		}
	}
}
