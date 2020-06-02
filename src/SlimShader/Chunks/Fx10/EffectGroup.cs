using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectGroup
	{
		public string Name { get; private set; }
		public List<EffectTechnique> Techniques { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }

		public uint NameOffset;
		public uint TechniqueCount;
		public uint AnnotationCount;
		public EffectGroup()
		{
			Techniques = new List<EffectTechnique>();
			Annotations = new List<EffectAnnotation>();
		}
		public static EffectGroup Parse(BytecodeReader reader, BytecodeReader groupReader, ShaderVersion version)
		{
			var result = new EffectGroup();
			result.NameOffset = groupReader.ReadUInt32();
			result.TechniqueCount = groupReader.ReadUInt32();
			result.AnnotationCount = groupReader.ReadUInt32();
			if(result.NameOffset != 0)
			{
				var nameReader = reader.CopyAtOffset((int)result.NameOffset);
				result.Name = nameReader.ReadString();
			} else
			{
				result.Name = "";
			}
			for(int i = 0; i < result.TechniqueCount; i++)
			{
				result.Techniques.Add(EffectTechnique.Parse(reader, groupReader, version));
			}
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, groupReader, version));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("EffectGroup");
			sb.AppendLine($"  {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  TechniqueCount {TechniqueCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach(var technique in Techniques)
			{
				sb.Append(technique.Dump());
			}
			foreach(var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("fxgroup");
			if (!string.IsNullOrEmpty(Name))
			{
				sb.Append(" ");
				sb.Append(Name);
			}
			sb.AppendLine();
			sb.AppendLine("{");
			sb.AppendLine("    //");
			sb.AppendLine(string.Format("    // {0} technique(s)",
				Techniques.Count));
			sb.AppendLine("    //");
			foreach(var technique in Techniques)
			{
				sb.Append(technique.ToString(1));
			}
			sb.Append("}");
			return sb.ToString();

		}
	}
}
