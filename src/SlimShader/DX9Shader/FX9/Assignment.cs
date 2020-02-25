using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Assignment
	{
		public StateType Type;
		public uint ArrayIndex;
		public uint TypeOffset;
		public uint ValueOffset;
		public Parameter Variable;
		public UnknownObject Value;
		public static Assignment Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{ 
			var result = new Assignment();
			result.Type = (StateType)shaderReader.ReadUInt32();
			result.ArrayIndex = shaderReader.ReadUInt32();
			result.TypeOffset = shaderReader.ReadUInt32();
			result.ValueOffset = shaderReader.ReadUInt32();

			var variableReader = reader.CopyAtOffset((int)result.TypeOffset);
			result.Variable = Parameter.Parse(reader, variableReader);

			var unknownReader = reader.CopyAtOffset((int)result.ValueOffset);
			result.Value = UnknownObject.Parse(unknownReader, 2);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Assignment.StateType: {Type} {((uint)Type).ToString("X4")}");
			sb.AppendLine($"    Assignment.ArrayIndex: {ArrayIndex} {ArrayIndex.ToString("X4")}");
			sb.AppendLine($"    Assignment.TypeOffset: {TypeOffset} {TypeOffset.ToString("X4")}");
			sb.AppendLine($"    Assignment.ValueOffset: {ValueOffset} {ValueOffset.ToString("X4")}");
			sb.Append(Variable.Dump());
			sb.Append(Value.Dump());
			return sb.ToString();
		}
	}
}
