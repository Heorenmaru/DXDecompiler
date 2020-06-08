using DebugParser.DebugParser.DX9;
using SlimShader.DX9Shader;
using System.Collections.Generic;
using System.Reflection;

namespace SlimShader.DebugParser.DX9
{
	public class DebugConstantDeclaration
	{
		public DebugConstantType Type;
		public static DebugConstantDeclaration Parse(DebugBytecodeReader reader, DebugBytecodeReader decReader)
		{
			var result = new DebugConstantDeclaration();
			var nameOffset = decReader.ReadUInt32("NameOffset");
			var registerSet = decReader.ReadEnum16<RegisterSet>("RegisterSet");
			var registerIndex = decReader.ReadUInt16("RegisterIndex");
			var registerCount = decReader.ReadUInt16("RegisterCount");
			decReader.ReadUInt16("Reserved");
			var typeInfoOffset = decReader.ReadUInt32("TypeInfoOffset");
			var defaultValueOffset = decReader.ReadUInt32("DefaultValueOffset");

			List<float> defaultValue = new List<float>();
			var nameReader = reader.CopyAtOffset("NameReader", decReader, (int)nameOffset);
			var name = nameReader.ReadString("Name");

			var typeReader = reader.CopyAtOffset("TypeReader", decReader, (int)typeInfoOffset);
			result.Type = DebugConstantType.Parse(reader, typeReader);

			if (defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset("DefaultValueReader", decReader, (int)defaultValueOffset);
				var elementCount = result.Type.GetSize() / 4;
				for (int i = 0; i < elementCount; i++)
				{
					defaultValue.Add(defaultValueReader.ReadSingle($"DefaultValue {i}"));
				}
			}

			return result;
		}
	}
}
