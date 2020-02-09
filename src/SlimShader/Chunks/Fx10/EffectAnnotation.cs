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
		public List<Number> DefaultNumericValue { get; private set; }
		public List<string> DefaultStringValue { get; private set; }

		public string Semantic => "";
		public uint Flags => 2; //Annotation Flag. TODO: Fix Type
		public uint AnnotationCount => 0;
		public uint BufferOffset => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IEffectVariable> Annotations => new IEffectVariable[0];

		public uint NameOffset;
		public uint TypeOffset;
		public uint ValueOffset;
		private uint ElementCount => Type.ElementCount == 0 ? 1 : Type.ElementCount;
		public EffectAnnotation()
		{
			DefaultNumericValue = new List<Number>();
			DefaultStringValue = new List<string>();
		}
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
			var valueOffset = result.ValueOffset = annotationReader.ReadUInt32();
			var defaultValueReader = reader.CopyAtOffset((int)valueOffset);
			if(result.Type.EffectVariableType == EffectVariableType.Numeric)
			{
				for(int i = 0; i < result.Type.PackedSize; i++)
				{
					result.DefaultNumericValue.Add(Number.Parse(defaultValueReader));
				}
			} else
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					result.DefaultStringValue.Add(defaultValueReader.ReadString());
				}
			}
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
			string defaultValue = "";
			if (Type.EffectVariableType == EffectVariableType.Numeric)
			{
				defaultValue = string.Join(", ", DefaultNumericValue);
			} else {
				defaultValue = string.Join(", ", DefaultStringValue.Select(v => $"\"{v}\"" ));
			}
			return string.Format("{0} {1} = {2};",
				Type.TypeName, Name, defaultValue);
		}
	}
}
