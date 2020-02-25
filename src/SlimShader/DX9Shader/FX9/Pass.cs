using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Pass
	{
		public string Name { get; private set; }
		public uint NameOffset;
		public uint AnnotationCount;
		public uint AssignmentCount;
		public List<Annotation> Annotations = new List<Annotation>();
		public List<Assignment> Assignments = new List<Assignment>();
		public static Pass Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Pass();
			result.NameOffset = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			result.AssignmentCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			for (int i = 0; i < result.AssignmentCount; i++)
			{
				result.Assignments.Add(Assignment.Parse(reader, variableReader));
			}
			var nameReader = reader.CopyAtOffset((int)result.NameOffset);
			result.Name = nameReader.TryReadString();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Pass.Name: {Name}");
			sb.AppendLine($"    Pass.NameOffset: {NameOffset} {NameOffset.ToString("X4")}");
			sb.AppendLine($"    Pass.AnnotationCount: {AnnotationCount} {AnnotationCount.ToString("X4")}");
			sb.AppendLine($"    Pass.UnknownCount: {AssignmentCount} {AssignmentCount.ToString("X4")}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			foreach (var obj in Assignments)
			{
				sb.Append(obj.Dump());
			}
			return sb.ToString();
		}
	}
}
