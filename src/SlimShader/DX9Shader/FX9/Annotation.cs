using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class Annotation
	{
		public uint DataOffset;
		public uint DefaultValueOffset;
		Parameter Unknown2;
		public static Annotation Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Annotation();
			result.DataOffset = variableReader.ReadUInt32();
			result.DefaultValueOffset = variableReader.ReadUInt32();
			var unknownReader = reader.CopyAtOffset((int)result.DataOffset);
			result.Unknown2 = Parameter.Parse(reader, unknownReader);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    Annotation.DataOffset: {DataOffset} {DataOffset.ToString("X4")}");
			sb.AppendLine($"    Annotation.DefaultValueOffset: {DefaultValueOffset} {DefaultValueOffset.ToString("X4")}");
			sb.Append(Unknown2.Dump());
			return sb.ToString();
		}
	}
}
