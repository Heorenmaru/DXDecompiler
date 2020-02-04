using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInterfaceVariable : IEffectVariable
	{
		public string Name { get; private set; }
		public EffectType Type { get; private set; }
		public uint Flags { get; private set; }
		public string InstanceName { get; private set; }
		public uint ArrayIndex { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }


		uint IEffectVariable.AnnotationCount => AnnotationCount;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();
		public string Semantic => "";
		public uint BufferOffset => 0;

		uint TypeOffset;
		uint NameOffset;
		uint AnnotationCount;
		uint InstanceNameOffset;
		public uint DefaultValueOffset;
		public EffectInterfaceVariable()
		{
			Annotations = new List<EffectAnnotation>();
		}
		internal static EffectInterfaceVariable Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectInterfaceVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var typeOffset = result.TypeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = EffectType.Parse(reader, typeReader);
			//Pointer to InterfaceInitializer
			result.DefaultValueOffset = variableReader.ReadUInt32();
			var initializerReader = reader.CopyAtOffset((int)result.DefaultValueOffset);
			var instanceNameOffset = result.InstanceNameOffset = initializerReader.ReadUInt32();
			var instanceNameReader = reader.CopyAtOffset((int)instanceNameOffset);
			result.InstanceName = instanceNameReader.ReadString();
			result.Flags = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader));
			}
	
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectInterfaceVariable");
			sb.AppendLine($"    Name: {Name} ({NameOffset.ToString("X4")})");
			sb.AppendLine($"    TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"    DefaultValueOffset: {DefaultValueOffset} {DefaultValueOffset.ToString("X4")}");
			sb.AppendLine($"    Flags: {Flags}");
			sb.AppendLine($"    InstanceName: {InstanceName} ({InstanceNameOffset.ToString("X4")})");
			sb.AppendLine($"    ArrayIndex: {ArrayIndex}");
			sb.Append(Type.ToString());
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation);
			}
			return sb.ToString();
		}
	}
}
