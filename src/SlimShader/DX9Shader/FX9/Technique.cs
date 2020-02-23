using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Technique
	{
		public string Name { get; private set; }
		public uint NameOffset;
		public uint AnnotationCount;
		public uint PassCount;
		List<Annotation> Annotations = new List<Annotation>();
		List<Pass> Passes = new List<Pass>();
		public static Technique Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Technique();
			result.NameOffset = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			result.PassCount = variableReader.ReadUInt32();
			for(int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			for (int i = 0; i < result.PassCount; i++)
			{
				result.Passes.Add(Pass.Parse(reader, variableReader));
			}
			var nameReader = reader.CopyAtOffset((int)result.NameOffset);
			result.Name = nameReader.TryReadString();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Variable.Name: {Name} {NameOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.NameOffset: {NameOffset} {NameOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.AnnotationCount: {AnnotationCount} {AnnotationCount.ToString("X4")}");
			sb.AppendLine($"    Variable.PassCount: {PassCount} {PassCount.ToString("X4")}");
			foreach(var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			foreach (var pass in Passes)
			{
				sb.Append(pass.Dump());
			}
			return sb.ToString();
		}
	}
}
