﻿using SlimShader.Chunks.Fxlvm;
using SlimShader.Util;
using System;
using System.Diagnostics;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugFxlcOperand
	{
		public uint IsArray { get; private set; }
		public FxlcOperandType OpType { get; private set; }
		public uint OpIndex { get; private set; }
		public FxlcOperandType ArrayType { get; private set; }
		public uint ArrayIndex { get; private set; }

		private uint ComponentCount;
		private DebugBytecodeContainer Container;
		public static DebugFxlcOperand Parse(DebugBytecodeReader reader, DebugBytecodeContainer container, uint componentCount)
		{
			var result = new DebugFxlcOperand() {
				Container = container,
				ComponentCount = componentCount,
				IsArray = reader.ReadUInt32("IsArray"),
				OpType = (FxlcOperandType)reader.ReadUInt32("OpType"),
				OpIndex = reader.ReadUInt32("OpIndex"),
			};
			Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.OpType),
				$"Unexpected FxlcOperandType OpType {result.OpType}");
			if(result.IsArray == 1)
			{
				result.ArrayType = (FxlcOperandType)reader.ReadUInt32("ArrayType");
				result.ArrayIndex = reader.ReadUInt32("ArrayIndex");

				Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.ArrayType),
					$"Unexpected FxlcOperandType ArrayType {result.ArrayType}");
			}

			return result;
		}
	}
}