﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInterfaceInitializer
	{
		public string Name { get; private set; }
		public uint Index { get; private set; }
		public static EffectInterfaceInitializer Parse(BytecodeReader reader, BytecodeReader initializerReader)
		{
			var nameOffset = initializerReader.ReadUInt32();
			var index = initializerReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			var name = nameReader.ReadString();
			return new EffectInterfaceInitializer()
			{
				Name = name,
				Index = index
			};
		}
		public string Dump()
		{
			return $"    EffectInterfaceInitializer.Name: {Name}\n    EffectInterfaceInitializer.Index: {Index}";
		}
		public override string ToString()
		{
			if(Index != uint.MaxValue)
			{
				return string.Format("{0}[{1}]", Name, Index);
			}
			return Name;
		}
	}
}
