﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Based on D3D10_TECHNIQUE_DESC
	/// </summary>
	public class EffectTechnique
	{
		public uint GuessNameOffset;
		public string Name { get; private set; }
		public uint PassCount;
		public uint AnnotationCount;
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<EffectPass> Passes { get; private set; }
		public EffectTechnique()
		{
			Annotations = new List<EffectAnnotation>();
			Passes = new List<EffectPass>();
		}
		public static EffectTechnique Parse(BytecodeReader reader, BytecodeReader techniqueReader)
		{
			var result = new EffectTechnique();
			var nameOffset = result.GuessNameOffset = techniqueReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			result.PassCount = techniqueReader.ReadUInt32();
			result.AnnotationCount = techniqueReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, techniqueReader));
			}
			for (int i = 0; i < result.PassCount; i++)
			{
				result.Passes.Add(EffectPass.Parse(reader, techniqueReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectTechnique");
			sb.AppendLine($"  Name {Name} ({GuessNameOffset.ToString("X4")})");
			sb.AppendLine($"  PassCount {PassCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.ToString());
			}
			foreach (var pass in Passes)
			{
				sb.Append(pass.ToString());
			}
			return sb.ToString();
		}
	}
}