using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Parameter
	{
		public ParameterType ParameterType { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint ElementCount { get; private set; }
		public uint Rows { get; private set; }
		public uint Columns { get; private set; }
		public uint StructMemberCount { get; private set; }
		public List<Parameter> StructMembers = new List<Parameter>();

		public uint NameOffset;
		public uint SemanticOffset;

		public static Parameter Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Parameter();
			result.ParameterType = (ParameterType)variableReader.ReadUInt32();
			result.ParameterClass = (ParameterClass)variableReader.ReadUInt32();
			result.NameOffset = variableReader.ReadUInt32();
			result.SemanticOffset = variableReader.ReadUInt32();
			if (result.ParameterClass == ParameterClass.Scalar ||
				result.ParameterClass == ParameterClass.Vector ||
				result.ParameterClass == ParameterClass.MatrixRows ||
				result.ParameterClass == ParameterClass.MatrixColumns)
			{
				result.ElementCount = variableReader.ReadUInt32();
				result.Rows = variableReader.ReadUInt32();
				result.Columns = variableReader.ReadUInt32();
			}
			if (result.ParameterClass == ParameterClass.Struct)
			{
				result.ElementCount = variableReader.ReadUInt32();
				result.StructMemberCount = variableReader.ReadUInt32();
				for(int i = 0; i < result.StructMemberCount; i++)
				{
					result.StructMembers.Add(Parameter.Parse(reader, variableReader));
				}
			}

			var nameReader = reader.CopyAtOffset((int)result.NameOffset);
			result.Name = nameReader.TryReadString();

			var semanticReader = reader.CopyAtOffset((int)result.SemanticOffset);
			result.Semantic = semanticReader.TryReadString();
			return result;
		}
		public uint GetSize()
		{
			switch (ParameterClass)
			{
				case ParameterClass.Scalar:
					return 4;
				case ParameterClass.Vector:
					return Rows * 4;
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					return Rows * Columns * 4;
				case ParameterClass.Struct:
					return (uint)StructMembers.Sum(m => m.GetSize());
				default:
					return 0;
			}
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    VariableData.ParameterType: {ParameterType} {((uint)ParameterType).ToString("X4")}");
			sb.AppendLine($"    VariableData.ParameterClass: {ParameterClass} {((uint)ParameterClass).ToString("X4")}");
			sb.AppendLine($"    VariableData.Name: {Name} {NameOffset.ToString("X4")}");
			sb.AppendLine($"    VariableData.Semantic: {Semantic} {SemanticOffset.ToString("X4")}");
			if (ParameterClass == ParameterClass.Scalar ||
				ParameterClass == ParameterClass.Vector ||
				ParameterClass == ParameterClass.MatrixRows ||
				ParameterClass == ParameterClass.MatrixColumns)
			{
				sb.AppendLine($"    VariableData.ElementCount: {ElementCount} {ElementCount.ToString("X4")}");
				sb.AppendLine($"    VariableData.Rows: {Rows} {Rows.ToString("X4")}");
				sb.AppendLine($"    VariableData.Columns: {Columns} {Columns.ToString("X4")}");
			}
			if(ParameterClass == ParameterClass.Struct)
			{
				sb.AppendLine($"    VariableData.StructMembers: {StructMemberCount} {StructMemberCount.ToString("X4")}");
				foreach(var member in StructMembers)
				{
					sb.Append(member.Dump());
				}
			}
			return sb.ToString();
		}
		public string GetTypeName()
		{
			var sb = new StringBuilder();
			sb.Append(ParameterType.ToString().ToLower());
			switch (ParameterClass)
			{
				case ParameterClass.Vector:
					sb.Append(Rows);
					break;
				case ParameterClass.MatrixColumns:
					sb.Append(string.Format("{0}x{1}", Columns, Rows));
					break;
				case ParameterClass.MatrixRows:
					sb.Append(string.Format("{0}x{1}", Rows, Columns));
					break;
				default:
					break;
			}
			return sb.ToString();
		}
	}
}
