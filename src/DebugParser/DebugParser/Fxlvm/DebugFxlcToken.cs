using SlimShader.Chunks.Fxlvm;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugFxlcToken
	{
		public FxlcTokenType Type { get; private set; }
		public List<DebugFxlcOperand> Operands { get; private set; }

		public uint Token0;
		public uint OperandCount;

		public DebugFxlcToken()
		{
			Operands = new List<DebugFxlcOperand>();
		}

		public static DebugFxlcToken Parse(DebugBytecodeReader reader, DebugBytecodeContainer container)
		{
			var result = new DebugFxlcToken();
			var token = reader.PeakUint32();
			var type = (FxlcTokenType)token.DecodeValue(20, 30);

			result.Token0 = reader.ReadUInt32($"Token0({type})");
			var tokenComponentCount = token.DecodeValue(0, 2);
			result.Type = (FxlcTokenType)token.DecodeValue(20, 30);
			var singleFirstComponent = token.DecodeValue(31, 31);

			Debug.Assert(Enum.IsDefined(typeof(FxlcTokenType), result.Type),
				$"Unknown FxlcTokenType {result.Type}");

			Debug.Assert(token.DecodeValue(3, 19) == 0,
				$"Unexpected data in FxlcToken bits 3-19 {token.DecodeValue(3, 19)}");

			var operandCount = result.OperandCount = reader.ReadUInt32("OperandCount");
			for(int i = 0; i < operandCount; i++)
			{
				var componentCount = i == 0 && singleFirstComponent == 1 ?
					1 : tokenComponentCount;
				result.Operands.Add(DebugFxlcOperand.Parse(reader, container, componentCount));
			}
			// destination operand
			result.Operands.Insert(0, DebugFxlcOperand.Parse(reader, container, tokenComponentCount));
			return result;
		}
	}
}
