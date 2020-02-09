﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Shex;
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
		public uint ExplicitBindPoint { get; private set; }
		public List<Number> DefaultValue { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }

		//TODO
		public uint Flags => 0;
		uint IEffectVariable.AnnotationCount => AnnotationCount;
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
			var defaultValueOffset = result.DefaultValueOffset = variableReader.ReadUInt32();

			List<Number> defaultValue = null;
			var size = result.Type.PackedSize;
			if (defaultValueOffset != 0)
			{
				defaultValue = new List<Number>();
				var defaultValueReader = reader.CopyAtOffset((int)defaultValueOffset);
				if (size % 4 != 0)
					throw new ParseException("Can only deal with 4-byte default values at the moment.");
				for (int i = 0; i < size; i += 4)
					defaultValue.Add(new Number(defaultValueReader.ReadBytes(4)));
			}
			result.DefaultValue = defaultValue;

			if (!isShared)
			{
				result.ExplicitBindPoint = variableReader.ReadUInt32();
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
			sb.AppendLine($"    EffectBufferVariable.Unknown1: {ExplicitBindPoint}");
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
			string defaultValue = "";
			if (DefaultValue != null)
			{
				var numberType = NumberType.Float;
				switch (Type.VariableType)
				{
					case Rdef.ShaderVariableType.Int:
						numberType = NumberType.Int;
						break;
					case Rdef.ShaderVariableType.UInt:
						numberType = NumberType.UInt;
						break;
					case Rdef.ShaderVariableType.Bool:
						numberType = NumberType.Bool;
						break;
				}
				if(Type.VariableClass == Rdef.ShaderVariableClass.Scalar)
				{
					defaultValue = $" = {DefaultValue[0].ToString(numberType)}";
				}
				else if(DefaultValue.Distinct().Count() == 1)
				{
					defaultValue = $" = {{ {DefaultValue[0].ToString(numberType)} }}";
				} else
				{
					var values = DefaultValue.Select(v => v.ToString(numberType));
					defaultValue = $" = {{ {string.Join(", ", values)} }}";
				}
			}
			string name = string.Format("{0,-7} {1}{2}{3};", Type.TypeName, Name, elements, defaultValue);
			return string.Format("    {0,-36}// Offset: {1, 4}, size: {2, 4}",
				name, BufferOffset, Type.UnpackedSize);
		}
	}
}
