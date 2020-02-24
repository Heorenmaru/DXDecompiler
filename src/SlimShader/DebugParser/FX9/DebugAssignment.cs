using SlimShader.DX9Shader.FX9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.FX9
{
	public class DebugAssignment
	{
		public StateType Type;
		public uint ArrayIndex;
		public uint TypeOffset;
		public uint Unknown1;
		public DebugVariableData Variable;
		public UnknownObject Unknown2;
		public static DebugAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugAssignment();
			result.Type = assignmentReader.ReadEnum32<StateType>("Type");
			result.ArrayIndex = assignmentReader.ReadUInt32("ArrayIndex");
			result.TypeOffset = assignmentReader.ReadUInt32("TypeOffset");
			result.Unknown1 = assignmentReader.ReadUInt32("UnknownOffset");

			var variableReader = reader.CopyAtOffset("AssignmentTypeReader", assignmentReader, (int)result.TypeOffset);
			result.Variable = DebugVariableData.Parse(reader, variableReader);

			return result;
		}
	}
}
