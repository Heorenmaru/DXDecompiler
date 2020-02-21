using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.FXLVM
{
	public class FxlcToken
	{
		public FxTokenType Type { get; private set; }

		public uint Token0;
		public uint OperandCount;
		List<Operand> Operands = new List<Operand>();
		public static FxlcToken Parse(BytecodeReader reader)
		{
			var result = new FxlcToken();
			result.Token0 = reader.ReadUInt32();
			result.Type = (FxTokenType)result.Token0.DecodeValue(20, 30);
			var unknown13 = result.Token0.DecodeValue(28, 31);
			result.OperandCount = reader.ReadUInt32();
			for(int i = 0; i < result.OperandCount + 1; i++)
			{
				result.Operands.Add(Operand.Parse(reader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  Unknown1: {Token0} {Token0.ToString("X4")}");
			
			var bit0_19 = Token0.DecodeValue(0, 19);
			var bit31 = Token0.DecodeValue(31, 31);
			sb.AppendLine($"  Type {Type} {((uint)Type).ToString("X4")}, bit0_19 {bit0_19} {bit0_19.ToString("X4")}, bit31 {bit31} {bit31.ToString("X4")}");
			sb.AppendLine($"  OperandCount: {OperandCount} {OperandCount.ToString("X4")}");
			foreach(var operand in Operands)
			{
				sb.AppendLine($"  Operand {operand.ToString()}");
			}
			return sb.ToString();
		}
	}
}
