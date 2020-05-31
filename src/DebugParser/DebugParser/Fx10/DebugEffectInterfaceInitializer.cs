﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectInterfaceInitializer
	{
		public string Name { get; private set; }
		public uint Index { get; private set; }
		public static DebugEffectInterfaceInitializer Parse(DebugBytecodeReader reader, DebugBytecodeReader initializerReader)
		{
			var nameOffset = initializerReader.ReadUInt32("NameOffset");
			var index = initializerReader.ReadUInt32("Index");
			var nameReader = reader.CopyAtOffset("NameReader", initializerReader, (int)nameOffset);
			var name = nameReader.ReadString("Name");
			return new DebugEffectInterfaceInitializer()
			{
				Name = name,
				Index = index
			};
		}
	}
}
