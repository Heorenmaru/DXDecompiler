using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public static EffectGroup Parse(BytecodeReader reader, BytecodeReader groupReader)
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
				result.Techniques.Add(EffectTechnique.Parse(reader, groupReader));
			}
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, groupReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("EffectGroup");
			sb.AppendLine($"  {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  TechniqueCount {TechniqueCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach(var technique in Techniques)
			{
				sb.Append(technique.ToString());
			}
			foreach(var annotation in Annotations)
			{
				sb.Append(annotation.ToString());
			}
			return sb.ToString();
		}
	}
}
