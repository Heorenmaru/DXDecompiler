using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugVariable
	{
		public uint DataOffset;
		public uint DefaultValueOffset;
		public uint IsShared;
		public uint AnnotationCount;
		public DebugVariableData Data;
		public DebugUnknownObject DefaultValue;
		public List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		public static DebugVariable Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugVariable();
			result.DataOffset = variableReader.ReadUInt32("DataOffset");
			result.DefaultValueOffset = variableReader.ReadUInt32("DefaultValueOffset");
			result.IsShared = variableReader.ReadUInt32("IsShared");
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(DebugAnnotation.Parse(reader, variableReader));
			}
			var dataReader = reader.CopyAtOffset("VariableData", variableReader, (int)result.DataOffset);
			result.Data = DebugVariableData.Parse(reader, dataReader);

			var unknownReader = reader.CopyAtOffset("DefaultValue", variableReader, (int)result.DefaultValueOffset);
			result.DefaultValue = DebugUnknownObject.Parse(unknownReader, 2);
			return result;
		}
	}
}