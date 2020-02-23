using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Variable
	{
		public uint DataOffset;
		public uint DefaultValueOffset;
		public uint IsShared;
		public uint AnnotationCount;
		public VariableData Data;
		public UnknownObject DefaultValue;
		public List<Annotation> Annotations = new List<Annotation>();
		public static Variable Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Variable();
			result.DataOffset = variableReader.ReadUInt32();
			result.DefaultValueOffset = variableReader.ReadUInt32();
			result.IsShared = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			var dataReader = reader.CopyAtOffset((int)result.DataOffset);
			result.Data = VariableData.Parse(reader, dataReader);

			var unknownReader = reader.CopyAtOffset((int)result.DefaultValueOffset);
			result.DefaultValue = UnknownObject.Parse(unknownReader, 2);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    {ToString()}");
			sb.AppendLine($"    Variable.DataOffset: {DataOffset} {DataOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.DefaultValueOffset: {DefaultValueOffset} {DefaultValueOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.IsShared: {IsShared} {IsShared.ToString("X4")}");
			sb.AppendLine($"    Variable.AnnotationCount: {AnnotationCount} {AnnotationCount.ToString("X4")}");
			sb.Append(Data.Dump());
			sb.Append(DefaultValue.Dump());
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			if(IsShared == 1)
			{
				sb.Append("shared ");
			}
			sb.Append(Data.GetTypeName());
			sb.Append(" ");
			sb.Append(Data.Name);
			if (!string.IsNullOrEmpty(Data.Semantic))
			{
				sb.Append(string.Format(" : {0}", Data.Semantic));
			}
			return sb.ToString();
		}
	}
}
