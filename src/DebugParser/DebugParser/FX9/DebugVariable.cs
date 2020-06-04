using SlimShader.DX9Shader.FX9;
using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugVariable
	{
		public uint IsShared { get; private set;  }
		public DebugParameter Parameter { get; private set; }
		public List<Number> Value { get; private set; }
		public List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		public List<DebugAssignment> SamplerStates = new List<DebugAssignment>();

		public uint AnnotationCount;
		public uint ParameterOffset;
		public uint ValueOffset;
		public static DebugVariable Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugVariable();
			result.ParameterOffset = variableReader.ReadUInt32("DataOffset");
			result.ValueOffset = variableReader.ReadUInt32("DefaultValueOffset");
			result.IsShared = variableReader.ReadUInt32("IsShared");
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(DebugAnnotation.Parse(reader, variableReader));
			}
			var paramterReader = reader.CopyAtOffset("ParameterReader", variableReader, (int)result.ParameterOffset);
			result.Parameter = DebugParameter.Parse(reader, paramterReader);

			if (result.Parameter.ParameterType.IsSampler())
			{
				var stateReader = reader.CopyAtOffset("SamplerStateReader", variableReader, (int)result.ValueOffset);
				var stateCount = stateReader.ReadUInt32("StateCount");
				for (int i = 0; i < stateCount; i++)
				{
					result.SamplerStates.Add(DebugAssignment.Parse(reader, stateReader));
				}
			}
			else
			{
				var valueReader = reader.CopyAtOffset("ValueReader", variableReader, (int)result.ValueOffset);
				result.Value = result.Parameter.ReadParameterValue(valueReader);
			}
			return result;
		}
	}
}