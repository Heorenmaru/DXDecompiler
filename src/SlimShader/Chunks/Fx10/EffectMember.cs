using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC
	/// </summary>
	public class EffectMember : IEffectVariable
	{

		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint BufferOffset { get; private set; }
		public EffectType Type { get; private set; }

		public uint Flags => 0;
		public uint AnnotationCount => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IEffectVariable> Annotations => new IEffectVariable[0];

		public uint TypeOffset;
		public uint NameOffset;
		public uint SemanticNameOffset;
		public static EffectMember Parse(BytecodeReader reader, BytecodeReader memberReader)
		{
			var result = new EffectMember();
			var nameOffset = result.NameOffset = memberReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			result.SemanticNameOffset = memberReader.ReadUInt32();
			if (result.SemanticNameOffset != 0)
			{
				var semanticNameReader = reader.CopyAtOffset((int)result.SemanticNameOffset);
				result.Semantic = semanticNameReader.ReadString();
			} else
			{
				result.Semantic = "";
			}
			result.BufferOffset = memberReader.ReadUInt32();
			result.TypeOffset = memberReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)result.TypeOffset);
			result.Type = EffectType.Parse(reader, typeReader);
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectStructMember");
			sb.AppendLine($"    Name: {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"    Semantic: {Semantic} ({SemanticNameOffset.ToString("X4")})");
			sb.AppendLine($"    EffectStructMember.BufferOffset: {BufferOffset} ({BufferOffset.ToString("X4")})");
			sb.AppendLine($"    EffectStructMember.TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.Append(Type.ToString());
			return sb.ToString();
		}
	}
}
