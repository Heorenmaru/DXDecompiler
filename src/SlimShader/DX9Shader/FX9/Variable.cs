using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	/*
	 * 
	 * 
	 */ 
	public class Variable
	{
		public uint DataOffset;
		public uint DefaultValueOffset;
		public uint IsShared;
		public uint AnnotationCount;
		public Parameter Parameter;
		public List<Number> DefaultValue = new List<Number>();
		public List<SamplerState> SamplerStates = new List<SamplerState>();
		public List<Annotation> Annotations = new List<Annotation>();
		public static Variable Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Variable();
			result.DataOffset = variableReader.ReadUInt32();
			result.DefaultValueOffset = variableReader.ReadUInt32();
			result.IsShared = variableReader.ReadUInt32();
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			var dataReader = reader.CopyAtOffset((int)result.DataOffset);
			result.Parameter = Parameter.Parse(reader, dataReader);
			if (result.Parameter.ParameterType.IsSampler())
			{
				var stateReader = reader.CopyAtOffset((int)result.DefaultValueOffset);
				var stateCount = stateReader.ReadUInt32();
				for(int i = 0; i < stateCount; i++)
				{
					result.SamplerStates.Add(SamplerState.Parse(reader, stateReader));
				}
			}
			else
			{
				var valueReader = reader.CopyAtOffset((int)result.DefaultValueOffset);
				result.DefaultValue = result.Parameter.ReadParameterValue(valueReader);
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    {ToString()}");
			sb.AppendLine($"    Variable.DataOffset: {DataOffset} {DataOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.DefaultValueOffset: {DefaultValueOffset} {DefaultValueOffset.ToString("X4")}");
			sb.AppendLine($"    Variable.IsShared: {IsShared} {IsShared.ToString("X4")}");
			sb.AppendLine($"    Variable.AnnotationCount: {AnnotationCount} {AnnotationCount.ToString("X4")}");
			sb.Append(Parameter.Dump());
			if (Parameter.ParameterType == ParameterType.Sampler)
			{
				foreach(var state in SamplerStates)
				{
					sb.Append(state.Dump());
				}
			}
			else
			{
				var defaultValueText = string.Join(", ", DefaultValue);
				sb.AppendLine($"    Variable.DefaultValue: {defaultValueText}");
			}
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			if(IsShared == 1)
			{
				sb.Append("shared ");
			}
			sb.Append(Parameter.GetTypeName());
			sb.Append(" ");
			sb.Append(Parameter.Name);
			if (!string.IsNullOrEmpty(Parameter.Semantic))
			{
				sb.Append(string.Format(" : {0}", Parameter.Semantic));
			}
			return sb.ToString();
		}
	}
}
