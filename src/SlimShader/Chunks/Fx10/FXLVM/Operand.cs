using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.FXLVM
{
	public class Operand
	{
		public uint IsArray;
		public uint OpType;
		public uint OpIndex;
		public uint ArrayType;
		public uint ArrayIndex;
		public static Operand Parse(BytecodeReader reader)
		{
			var result = new Operand();
			result.IsArray = reader.ReadUInt32();
			result.OpType = reader.ReadUInt32();
			result.OpIndex = reader.ReadUInt32();
			if(result.IsArray == 1)
			{
				result.ArrayType = reader.ReadUInt32();
				result.ArrayIndex = reader.ReadUInt32();
			}
			return result;
		}
		public override string ToString()
		{
			if (IsArray == 0)
			{
				return string.Format("IsArr {0} {1}, OpType {2} {3}, OpIndex {4} {5}",
					IsArray, IsArray.ToString("X4"),
					(OperandType)OpType, OpType.ToString("X4"),
					OpIndex, OpIndex.ToString("X4"));
			}else
			{
					return string.Format("IsArr {0} {1}, OpType {2} {3}, OpIndex {4} {5}, ArrType {6} {7}, ArrIndex {8} {9}",
						IsArray, IsArray.ToString("X4"),
						(OperandType)OpType, OpType.ToString("X4"),
						OpIndex, OpIndex.ToString("X4"),
						(OperandType)ArrayType, ArrayType.ToString("X4"),
						ArrayIndex, ArrayIndex.ToString("X4"));
				}
		}
	}
}
