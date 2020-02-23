using SlimShader.DX9Shader;
using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugVariableData
	{
		public ParameterType ParameterType { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint ElementCount { get; private set; }
		public uint Rows { get; private set; }
		public uint Columns { get; private set; }
		public uint StructMemberCount { get; private set; }
		public List<DebugVariableData> StructMembers = new List<DebugVariableData>();
		public uint NameOffset;
		public uint SemanticOffset;
		public uint Unknown1;
		public uint Unknown2;
		public static DebugVariableData Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugVariableData();
			result.ParameterType = variableReader.ReadEnum32<ParameterType>("ParameterType");
			result.ParameterClass = variableReader.ReadEnum32<ParameterClass>("ParameterClass");
			result.NameOffset = variableReader.ReadUInt32("NameOffset");

			var nameReader = reader.CopyAtOffset("NameReader", variableReader, (int)result.NameOffset);
			result.Name = nameReader.TryReadString("Name");

			result.SemanticOffset = variableReader.ReadUInt32("SemanticOffset");

			var semanticReader = reader.CopyAtOffset("SemanticReader", variableReader, (int)result.SemanticOffset);
			result.Semantic = semanticReader.TryReadString("Semantic");

			if (result.ParameterClass == ParameterClass.Scalar ||
				result.ParameterClass == ParameterClass.Vector ||
				result.ParameterClass == ParameterClass.MatrixRows ||
				result.ParameterClass == ParameterClass.MatrixColumns)
			{
				result.ElementCount = variableReader.ReadUInt32("ElementCount");
				result.Rows = variableReader.ReadUInt32("Rows");
				result.Columns = variableReader.ReadUInt32("Columns");
			}
			if (result.ParameterClass == ParameterClass.Struct)
			{
				result.ElementCount = variableReader.ReadUInt32("ElementCount");
				result.StructMemberCount = variableReader.ReadUInt32("StructMemberCount");
				for (int i = 0; i < result.StructMemberCount; i++)
				{
					result.StructMembers.Add(DebugVariableData.Parse(reader, variableReader));
				}

			}
			return result;
		}
	}
}