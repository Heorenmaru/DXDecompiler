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
		public uint ParameterOffset;
		public uint ValueOffset;
		public Parameter Parameter;
		public List<Number> Value;
		public static Assignment Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{ 
			var result = new Assignment();
			result.Type = (StateType)shaderReader.ReadUInt32();
			result.ArrayIndex = shaderReader.ReadUInt32();
			result.ParameterOffset = shaderReader.ReadUInt32();
			result.ValueOffset = shaderReader.ReadUInt32();

			var parameterReader = reader.CopyAtOffset((int)result.ParameterOffset);
			result.Parameter = Parameter.Parse(reader, parameterReader);

			var valueReader = reader.CopyAtOffset((int)result.ValueOffset);
			result.Value = result.Parameter.ReadParameterValue(valueReader);

			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Assignment.StateType: {Type} {((uint)Type).ToString("X4")}");
			sb.AppendLine($"    Assignment.ArrayIndex: {ArrayIndex} {ArrayIndex.ToString("X4")}");
			sb.AppendLine($"    Assignment.ParameterOffset: {ParameterOffset} {ParameterOffset.ToString("X4")}");
			sb.AppendLine($"    Assignment.ValueOffset: {ValueOffset} {ValueOffset.ToString("X4")}");
			sb.Append(Parameter.Dump());
			sb.AppendLine($"    Assignment.Value: {string.Join(", ", Value)}");
			return sb.ToString();
		}
	}
}
