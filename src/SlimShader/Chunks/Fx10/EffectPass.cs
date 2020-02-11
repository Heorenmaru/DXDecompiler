using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Based on D3D10_PASS_DESC
	/// </summary>
	public class EffectPass
	{
		public uint NameOffset;
		public string Name { get; private set; }
		public uint ShaderCount;
		public uint AnnotationCount;
		public List<EffectAssignment> Assignments { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public EffectPass()
		{
			Assignments = new List<EffectAssignment>();
			Annotations = new List<EffectAnnotation>();
		}
		public static EffectPass Parse(BytecodeReader reader, BytecodeReader passReader)
		{
			var result = new EffectPass();
			var nameOffset = result.NameOffset = passReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			result.ShaderCount = passReader.ReadUInt32();
			result.AnnotationCount = passReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, passReader));
			}
			for (int i = 0; i < result.ShaderCount; i++)
			{
				result.Assignments.Add(EffectAssignment.Parse(reader, passReader));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectPass");
			sb.AppendLine($"  Name {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  ShaderCount {ShaderCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			foreach (var assignment in Assignments)
			{
				sb.Append(assignment.Dump());
			}
			return sb.ToString();
		}
		public string ToString(int indent)
		{
			var indentString = new string(' ', indent * 4);
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("{0}pass {1}", indentString, Name));
			sb.AppendLine(string.Format("{0}{{", indentString));
			foreach(var shader in Assignments)
			{
				sb.AppendLine(string.Format("{0}    {1}", indentString, shader.ToString()));
			}
			sb.AppendLine(string.Format("{0}}}", indentString));
			return sb.ToString();
		}
		public override string ToString()
		{
			return ToString(0);
		}
	}
}
