﻿using SlimShader.DebugParser;
using SlimShader.DX9Shader;
using System;
using System.Collections.Generic;

namespace DebugParser.DebugParser.DX9
{
	public class DebugConstantType
	{
		public ParameterClass ParameterClass;
		public ParameterType ParameterType;
		public uint Rows;
		public uint Columns;
		public uint Elements;
		public uint MemberCount;
		public List<DebugConstantType> Members = new List<DebugConstantType>();
		public static DebugConstantType Parse(DebugBytecodeReader reader, DebugBytecodeReader typeReader)
		{
			var result = new DebugConstantType();
			result.ParameterClass = typeReader.ReadEnum16<ParameterClass>("ParameterClass");
			result.ParameterType = typeReader.ReadEnum16<ParameterType>("ParameterType");
			result.Rows = typeReader.ReadUInt16("Rows");
			result.Columns = typeReader.ReadUInt16("Columns");
			result.Elements = typeReader.ReadUInt16("Elements");
			result.MemberCount = typeReader.ReadUInt16("Members");
			var memberInfoOffset = typeReader.ReadUInt32("MemberInfoOffset");
			if (result.MemberCount > 0) {
				var memberInfoReader = reader.CopyAtOffset("MemberInfoReader", typeReader, (int)memberInfoOffset);
				result.Members.Add(DebugConstantType.Parse(reader, memberInfoReader));
			}
			return result;
		}
		public uint GetSize()
		{
			var elementCount = Math.Max(1, Elements);
			switch (ParameterClass)
			{
				case ParameterClass.Scalar:
					return 4 * elementCount;
				case ParameterClass.Vector:
					return Rows * 4 * elementCount;
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					return Rows * Columns * 4 * elementCount;
				case ParameterClass.Struct:
					return 0;
				case ParameterClass.Object:
					return 4 * elementCount;
				default:
					return 0;
			}
		}
	}
}
