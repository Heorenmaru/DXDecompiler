﻿using SlimShader.Util;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectConstantIndexAssignment : DebugEffectAssignment
	{
		public string ArrayName { get; private set; }
		public uint Index { get; private set; }

		uint ArrayNameOffset;
		public static DebugEffectConstantIndexAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectConstantIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32("ArrayNameOffset");
			var arrayNameReader = reader.CopyAtOffset("ArrayNameReader", assignmentReader, (int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString("ArrayName");
			result.Index = assignmentReader.ReadUInt32("Index");
			return result;
		}
	}
}
