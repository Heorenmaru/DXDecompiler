using SlimShader.Chunks.Rdef;
using System.Collections.Generic;

namespace SlimShader.DebugParser.Rdef
{
	public class DebugShaderType
	{
		private readonly int _indent;
		private readonly bool _isFirst;
		public ShaderVariableClass VariableClass { get; private set; }
		public ShaderVariableType VariableType { get; private set; }
		public ushort Rows { get; private set; }
		public ushort Columns { get; private set; }
		public ushort ElementCount { get; private set; }
		public List<DebugShaderTypeMember> Members { get; private set; }
		public string BaseTypeName { get; private set; }

		public DebugShaderType(int indent, bool isFirst)
		{
			_indent = indent;
			_isFirst = isFirst;
			Members = new List<DebugShaderTypeMember>();
		}

		public static DebugShaderType Parse(DebugBytecodeReader reader, DebugBytecodeReader typeReader, DebugShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var result = new DebugShaderType(indent, isFirst)
			{
				VariableClass = typeReader.ReadEnum16<ShaderVariableClass>("VariableClass"),
				VariableType = typeReader.ReadEnum16<ShaderVariableType>("VariableType"),
				Rows = typeReader.ReadUInt16("Rows"),
				Columns = typeReader.ReadUInt16("Columns"),
				ElementCount = typeReader.ReadUInt16("ElementCount")
			};

			var memberCount = typeReader.ReadUInt16("memberCount");
			var memberOffset = typeReader.ReadUInt32("memberOffset");

			if (target.MajorVersion >= 5)
			{
				var parentTypeOffset = typeReader.ReadUInt32("parentTypeOffset"); // Guessing
				if (parentTypeOffset != 0)
				{
					var parentTypeReader = reader.CopyAtOffset("parentTypeReader", typeReader,(int)parentTypeOffset);
					var parentTypeClass = (ShaderVariableClass)parentTypeReader.ReadUInt16("parentTypeClass");
					var unknown1 = parentTypeReader.ReadUInt16("unknown1");
				}

				var unknown2 = typeReader.ReadUInt32("unknown2");
				if (unknown2 != 0)
				{
					var unknownReader = reader.CopyAtOffset("unknownReader", typeReader, (int)unknown2);
					uint unknown3 = unknownReader.ReadUInt32("unknownReader");
				}

				var unknown4 = typeReader.ReadUInt32("unknown4");
				var unknown5 = typeReader.ReadUInt32("unknown5");
				if (unknown5 != 0)
				{
					var unknownReader = reader.CopyAtOffset("unknownReader", typeReader, (int)unknown5);
					var unknown6 = unknownReader.ReadUInt32("unknown6");
				}

				var parentNameOffset = typeReader.ReadUInt32("parentNameOffset");
				if (parentNameOffset > 0)
				{
					var parentNameReader = reader.CopyAtOffset("parentNameOffset", typeReader, (int)parentNameOffset);
					result.BaseTypeName = parentNameReader.ReadString("BaseTypeName");
				}
			}

			if (memberCount > 0)
			{
				var memberReader = reader.CopyAtOffset("memberReader", typeReader, (int)memberOffset);
				for (int i = 0; i < memberCount; i++)
					result.Members.Add(DebugShaderTypeMember.Parse(reader, memberReader, target, indent + 4, i == 0,
						parentOffset));
			}

			return result;
		}
	}
}