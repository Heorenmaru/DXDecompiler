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
		public uint GuessNameOffset;
		public string Name { get; private set; }
		public uint ShaderCount;
		public uint AnnotationCount;
		public List<EffectShader> Shaders { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public EffectPass()
		{
			Shaders = new List<EffectShader>();
			Annotations = new List<EffectAnnotation>();
		}
		public static EffectPass Parse(BytecodeReader reader, BytecodeReader passReader)
		{
			var result = new EffectPass();
			var nameOffset = result.GuessNameOffset = passReader.ReadUInt32();
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
				result.Shaders.Add(EffectShader.Parse(reader, passReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectPass");
			sb.AppendLine($"  Name {Name} ({GuessNameOffset.ToString("X4")})");
			sb.AppendLine($"  ShaderCount {ShaderCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.ToString());
			}
			foreach (var shader in Shaders)
			{
				sb.Append(shader.ToString());
			}
			return sb.ToString();
		}
	}
}
