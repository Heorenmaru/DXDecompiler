﻿using SlimShader.Util;
using System;
using System.Diagnostics;

namespace SlimShader.Chunks.Fxlvm
{
	public class FxlcOperand
	{
		public uint IsArray { get; private set; }
		public FxlcOperandType OpType { get; private set; }
		public uint OpIndex { get; private set; }
		public FxlcOperandType ArrayType { get; private set; }
		public uint ArrayIndex { get; private set; }

		private uint ComponentCount;
		private BytecodeContainer Container;
		public static FxlcOperand Parse(BytecodeReader reader, BytecodeContainer container, uint componentCount)
		{
			var result = new FxlcOperand() {
				Container = container,
				ComponentCount = componentCount,
				IsArray = reader.ReadUInt32(),
				OpType = (FxlcOperandType)reader.ReadUInt32(),
				OpIndex = reader.ReadUInt32(),
			};
			Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.OpType),
				$"Unexpected FxlcOperandType OpType {result.OpType}");
			if(result.IsArray == 1)
			{
				result.ArrayType = (FxlcOperandType)reader.ReadUInt32();
				result.ArrayIndex = reader.ReadUInt32();

				Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.ArrayType),
					$"Unexpected FxlcOperandType ArrayType {result.ArrayType}");
			}

			return result;
		}

		public string Dump()
		{
			if (IsArray == 0)
			{
				return string.Format("{0} IsArr {1} {2}, OpType {3} {4}, OpIndex {5} {6}",
					ToString(), 
					IsArray, IsArray.ToString("X4"),
					OpType, ((uint)OpType).ToString("X4"),
					OpIndex, OpIndex.ToString("X4"));
			}
			else
			{
				return string.Format("{0} IsArr {1} {2}, OpType {3} {4}, OpIndex {5} {6}, ArrType {7} {8}, ArrIndex {9} {10}",
					ToString(),
					IsArray, IsArray.ToString("X4"),
					OpType, ((uint)OpType).ToString("X4"),
					OpIndex, OpIndex.ToString("X4"),
					ArrayType, ((uint)ArrayType).ToString("X4"),
					ArrayIndex, ArrayIndex.ToString("X4"));
			}
		}
		private string FormatComponent(uint componentIndex, uint componentCount)
		{
			switch (componentCount)
			{
				case 1:
					switch (componentIndex)
					{
						case 0:
							return ".x";
						case 1:
							return ".y";
						case 2:
							return ".z";
						case 3:
							return ".w";
						default:
							return "";
					}
				case 2:
					switch (componentIndex)
					{
						case 0:
							return ".xy";
						case 1:
							return ".yz";
						case 2:
							return ".zw";
						default:
							return "";
					}
				case 3:
					switch (componentIndex)
					{
						case 0:
							return ".xyz";
						case 1:
							return ".yzw";
						default:
							return "";
					}
				default:
					return "";
			}
		}
		private string FormatOperand(FxlcOperandType type, uint index)
		{
			var elementIndex = index / 4;
			var componentIndex = index % 4;
			var component = FormatComponent(componentIndex, ComponentCount);
			var ctab = Container.GetChunk<CtabChunk>();
			var cli = Container.GetChunk<Cli4Chunk>();
			switch (type)
			{
				case FxlcOperandType.Literal:
					return string.Format("({0})", cli.GetLiteral(elementIndex, ComponentCount));
				case FxlcOperandType.Temp:
					return string.Format("r{0}{1}", elementIndex, component);
				case FxlcOperandType.Variable:
					return string.Format("{0}{1}", 
						ctab.GetVariable(elementIndex), component);
				case FxlcOperandType.Expr:
					if(ComponentCount == 1)
					{
						if(componentIndex == 0)
						{
							return string.Format("expr{0}", component);
						} else
						{
							return string.Format("expr0{0}", component);
						}
					}
					return string.Format("expr{0}", component);
				default:
					return string.Format("unknown{0}{1}", elementIndex, component);
			}
		}
		public override string ToString()
		{
			if (IsArray == 0)
			{
				return FormatOperand(OpType, OpIndex);
			}
			else
			{
				return string.Format("{0}[{1}]",
					FormatOperand(ArrayType, ArrayIndex),
					FormatOperand(OpType, OpIndex));
			}
		}
	}
}
