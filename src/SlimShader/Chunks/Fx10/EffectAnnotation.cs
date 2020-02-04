using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectAnnotation : IEffectVariable
	{
		public string Name { get; private set; }
		public EffectType Type { get; private set; }

		public string Semantic => "";
		public uint Flags => 2; //Annotation Flag. TODO: Fix Type
		public uint AnnotationCount => 0;
		public uint BufferOffset => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IEffectVariable> Annotations => new IEffectVariable[0];

		public uint NameOffset;
		public uint TypeOffset;
		public uint ValueOffset;
		public static EffectAnnotation Parse(BytecodeReader reader, BytecodeReader annotationReader)
		{
			var result = new EffectAnnotation();
			var nameOffset = result.NameOffset = annotationReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var typeOffset = result.TypeOffset = annotationReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = EffectType.Parse(reader, typeReader);
			//Note: Points to 27 and "foo" in Texture2D tex<int bla=27;string blu="foo";>;
			/// Value format and stride depends on Type
			result.ValueOffset = annotationReader.ReadUInt32();
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("  Annotation");
			sb.AppendLine($"    Annotation {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"    Annotation.TypeOffset {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"    Annotation.ValueOffset {ValueOffset} ({ValueOffset.ToString("X4")})");
			sb.Append(Type.ToString());
			return sb.ToString();
		}
		public override string ToString()
		{
			return Name;
		}
	}
}
