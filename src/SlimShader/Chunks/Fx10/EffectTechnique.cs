using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Based on D3D10_TECHNIQUE_DESC
	/// </summary>
	public class EffectTechnique
	{
		public uint NameOffset;
		public string Name { get; private set; }
		public uint PassCount;
		public uint AnnotationCount;

		private ShaderVersion m_Version;
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<EffectPass> Passes { get; private set; }
		public EffectTechnique(ShaderVersion version)
		{
			Annotations = new List<EffectAnnotation>();
			Passes = new List<EffectPass>();
			m_Version = version;
		}
		public static EffectTechnique Parse(BytecodeReader reader, BytecodeReader techniqueReader, ShaderVersion version)
		{
			var result = new EffectTechnique(version);
			var nameOffset = result.NameOffset = techniqueReader.ReadUInt32();
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
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectTechnique");
			sb.AppendLine($"  Name {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  PassCount {PassCount}");
			sb.AppendLine($"  AnnotationCount {AnnotationCount}");
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			foreach (var pass in Passes)
			{
				sb.Append(pass.Dump());
			}
			return sb.ToString();
		}
		public string ToString(int indent)
		{
			var techniqueVersion = m_Version.MajorVersion == 5 ? "11" : "10";
			var sb = new StringBuilder();
			var indentString = new string(' ', indent * 4);
			sb.Append(indentString);
			sb.AppendLine(string.Format("technique{0} {1}", techniqueVersion, Name));
			sb.Append(indentString);
			sb.AppendLine("{");
			foreach(var pass in Passes)
			{
				sb.Append(pass.ToString(indent + 1));
			}
			sb.Append(indentString);
			sb.AppendLine("}");
			sb.AppendLine();
			return sb.ToString();
		}
		public override string ToString()
		{
			return ToString(0);
		}
	}
}
