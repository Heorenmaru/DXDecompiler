using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Annotation
	{
		public Parameter Parameter { get; private set; }
		public List<Number> Value { get; private set; }
		public uint ParameterOffset;
		public uint ValueOffset;

		public static Annotation Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Annotation();
			result.ParameterOffset = variableReader.ReadUInt32();
			result.ValueOffset = variableReader.ReadUInt32();
			var parameterReader = reader.CopyAtOffset((int)result.ParameterOffset);
			result.Parameter = Parameter.Parse(reader, parameterReader);

			var valueReader = reader.CopyAtOffset((int)result.ValueOffset);
			result.Value = result.Parameter.ReadParameterValue(valueReader);

			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Annotation.ParameterOffset: {ParameterOffset} {ParameterOffset.ToString("X4")}");
			sb.AppendLine($"    Annotation.ValueOffset: {ValueOffset} {ValueOffset.ToString("X4")}");
			sb.Append(Parameter.Dump());
			sb.AppendLine($"    Annotation.DefaultValueOffset: {ValueOffset} {ValueOffset.ToString("X4")}");
			return sb.ToString();
		}
	}
}
