﻿using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectGroup
	{
		public string Name { get; private set; }
		public List<DebugEffectTechnique> Techniques { get; private set; }
		public List<DebugEffectAnnotation> Annotations { get; private set; }

		public uint NameOffset;
		public uint TechniqueCount;
		public uint AnnotationCount;
		public DebugEffectGroup()
		{
			Techniques = new List<DebugEffectTechnique>();
			Annotations = new List<DebugEffectAnnotation>();
		}
		public static DebugEffectGroup Parse(DebugBytecodeReader reader, DebugBytecodeReader groupReader, DebugShaderVersion version)
		{
			var result = new DebugEffectGroup();
			result.NameOffset = groupReader.ReadUInt32("NameOffset");
			result.TechniqueCount = groupReader.ReadUInt32("TechniqueCount");
			result.AnnotationCount = groupReader.ReadUInt32("AnnotationCount");
			if(result.NameOffset != 0)
			{
				var nameReader = reader.CopyAtOffset("NameReader", groupReader, (int)result.NameOffset);
				result.Name = nameReader.ReadString("Name");
			} else
			{
				result.Name = "";
			}
			for(int i = 0; i < result.TechniqueCount; i++)
			{
				result.Techniques.Add(DebugEffectTechnique.Parse(reader, groupReader, version));
			}
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, groupReader));
			}
			return result;
		}
	}
}
