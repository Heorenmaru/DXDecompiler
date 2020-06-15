using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	public class FxlcToken
	{
		public FxlcOpcode Opcode { get; private set; }
		public List<FxlcOperand> Operands { get; private set; }

		public uint Token0;
		public uint OperandCount;

		public FxlcToken()
		{
			Operands = new List<FxlcOperand>();
		}

		public static FxlcToken Parse(BytecodeReader reader, BytecodeContainer container)
		{
			var result = new FxlcToken();
			var token = result.Token0 = reader.ReadUInt32();
			var tokenComponentCount = token.DecodeValue(0, 2);
			result.Opcode = (FxlcOpcode)token.DecodeValue(20, 30);
			var singleFirstComponent = token.DecodeValue(31, 31);

			Debug.Assert(Enum.IsDefined(typeof(FxlcOpcode), result.Opcode),
				$"Unknown FxlcTokenType {result.Opcode}");

			Debug.Assert(token.DecodeValue(3, 19) == 0,
				$"Unexpected data in FxlcToken bits 3-19 {token.DecodeValue(3, 19)}");

			var operandCount = result.OperandCount = reader.ReadUInt32();
			for(int i = 0; i < operandCount; i++)
			{
				var componentCount = i == 0 && singleFirstComponent == 1 ?
					1 : tokenComponentCount;
				result.Operands.Add(FxlcOperand.Parse(reader, container, componentCount));
			}
			// destination operand
			result.Operands.Insert(0, FxlcOperand.Parse(reader, container, tokenComponentCount));
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  Unknown1: {Token0} {Token0.ToString("X4")}");

			var bit0_19 = Token0.DecodeValue(0, 19);
			var bit31 = Token0.DecodeValue(31, 31);
			sb.AppendLine($"  Type {Opcode} {((uint)Opcode).ToString("X4")}, bit0_19 {bit0_19} {bit0_19.ToString("X4")}, bit31 {bit31} {bit31.ToString("X4")}");
			sb.AppendLine($"  OperandCount: {OperandCount} {OperandCount.ToString("X4")}");
			foreach (var operand in Operands)
			{
				sb.AppendLine($"  Operand {operand.Dump()}");
			}
			return sb.ToString();
		}

		public override string ToString()
		{
			return string.Format("{0} {1}",
				Opcode.ToString().ToLowerInvariant(),
				string.Join(", ", Operands));
		}
	}
}
