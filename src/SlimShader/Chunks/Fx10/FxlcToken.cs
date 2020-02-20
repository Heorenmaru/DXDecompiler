using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class FxlcToken
	{
		public uint Unknown1;
		public uint Unknown2;
		public uint Unknown3;
		public uint Unknown4;
		public uint Unknown5;
		public uint Unknown6;
		public uint Unknown7;
		public uint Unknown8;
		public uint Unknown9;
		public uint Unknown10;
		public uint Unknown11;
		public static FxlcToken Parse(BytecodeReader reader)
		{
			var result = new FxlcToken();
			result.Unknown1 = reader.ReadUInt32();
			result.Unknown2 = reader.ReadUInt32();
			result.Unknown3 = reader.ReadUInt32();
			result.Unknown4 = reader.ReadUInt32();
			result.Unknown5 = reader.ReadUInt32();
			result.Unknown6 = reader.ReadUInt32();
			result.Unknown7 = reader.ReadUInt32();
			result.Unknown8 = reader.ReadUInt32();
			result.Unknown9 = reader.ReadUInt32();
			result.Unknown10 = reader.ReadUInt32();
			result.Unknown11 = reader.ReadUInt32();
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  Unknown1: {Unknown1} {Unknown1.ToString("X4")}");
			var type = Unknown1.DecodeValue(20, 28);
			sb.AppendLine($"    {type} {type.ToString("X4")}");
			sb.AppendLine($"  Unknown2: {Unknown2} {Unknown2.ToString("X4")}");
			sb.AppendLine($"  Unknown3: {Unknown3} {Unknown3.ToString("X4")}");
			sb.AppendLine($"  Unknown4: {Unknown4} {Unknown4.ToString("X4")}");
			sb.AppendLine($"  Unknown5: {Unknown5} {Unknown5.ToString("X4")}");
			sb.AppendLine($"  Unknown6: {Unknown6} {Unknown6.ToString("X4")}");
			sb.AppendLine($"  Unknown7: {Unknown7} {Unknown7.ToString("X4")}");
			sb.AppendLine($"  Unknown8: {Unknown8} {Unknown8.ToString("X4")}");
			sb.AppendLine($"  Unknown9: {Unknown9} {Unknown9.ToString("X4")}");
			sb.AppendLine($"  Unknown10: {Unknown10} {Unknown10.ToString("X4")}");
			sb.AppendLine($"  Unknown11: {Unknown11} {Unknown11.ToString("X4")}");
			return sb.ToString();
		}
	}
}
