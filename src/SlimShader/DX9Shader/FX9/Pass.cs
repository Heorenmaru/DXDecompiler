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
		public uint UnknownCount;
		List<Annotation> Annotations = new List<Annotation>();
		List<Assignment> UnknownObjects = new List<Assignment>();
		public static Pass Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Pass();
			result.NameOffset = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			result.UnknownCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			for (int i = 0; i < result.UnknownCount; i++)
			{
				result.UnknownObjects.Add(Assignment.Parse(reader, variableReader));
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
			sb.AppendLine($"    Pass.UnknownCount: {UnknownCount} {UnknownCount.ToString("X4")}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			foreach (var obj in UnknownObjects)
			{
				sb.Append(obj.Dump());
			}
			return sb.ToString();
		}
	}
}
