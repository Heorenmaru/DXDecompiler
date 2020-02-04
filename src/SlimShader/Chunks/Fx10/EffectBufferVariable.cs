using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Base on D3D10_EFFECT_VARIABLE_DESC
	/// </summary>
	public class EffectBufferVariable : IEffectVariable
	{
		public string Name { get; private set; }
		public EffectType Type { get; private set; }
		public uint SemanticOffset { get; private set; }
		public string Semantic { get; private set; }
		public uint BufferOffset { get; private set; }
		public uint Unknown1 { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }

		//TODO
		public uint Flags => 0;
		uint IEffectVariable.AnnotationCount => AnnotationCount;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();

		uint AnnotationCount;
		uint NameOffset;
		uint TypeOffset;
		public uint DefaultValueOffset;
		public EffectBufferVariable()
		{
			Annotations = new List<EffectAnnotation>();
		}
		internal static EffectBufferVariable Parse(BytecodeReader reader, BytecodeReader variableReader, bool isShared)
		{
			var result = new EffectBufferVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var typeOffset = result.TypeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = EffectType.Parse(reader, typeReader);
			var semanticOffset = result.SemanticOffset = variableReader.ReadUInt32();
			if (semanticOffset != 0)
			{
				var semanticReader = reader.CopyAtOffset((int)semanticOffset);
				result.Semantic = semanticReader.ReadString();
			} else
			{
				result.Semantic = "";
			}
			result.BufferOffset = variableReader.ReadUInt32();
			result.DefaultValueOffset = variableReader.ReadUInt32();
			if (!isShared)
			{
				result.Unknown1 = variableReader.ReadUInt32();
				//TODO: Unknown1
				//Debug.Assert(result.Unknown1 == 0, $"EffectBufferVariable.Unknown1 {result.Unknown1}");
			}
			result.AnnotationCount = variableReader.ReadUInt32();
			for(int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectBufferVariable");
			sb.AppendLine($"    Name: {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"    TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"    Semantic: {Semantic} ({SemanticOffset.ToString("X4")})");
			sb.AppendLine($"    BufferOffset: {BufferOffset}");
			sb.AppendLine($"    EffectBufferVariable.DefaultValueOffset: {DefaultValueOffset}");
			sb.AppendLine($"    EffectBufferVariable.Unknown1: {Unknown1}");
			sb.AppendLine($"    EffectBufferVariable.AnnotationCount: {AnnotationCount}");
			sb.Append(Type.Dump());
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			string elements = "";
			if(Type.ElementCount > 0)
			{
				elements = string.Format("[{0}]", Type.ElementCount);
			}
			string name = string.Format("{0,-7} {1}{2};", Type.TypeName, Name, elements);
			return string.Format("    {0,-35} // Offset: {1, 4}, size: {2, 4}",
				name, BufferOffset, Type.GuessUnpackedSize);
		}
	}
}
