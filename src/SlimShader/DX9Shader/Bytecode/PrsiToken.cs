using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode
{
	public class PrsiToken
	{
		public uint Size;
		public uint OutputRegisterOffset;
		public uint Unknown1;
		public uint Unknown2;
		public uint OutputRegisterCount;
		public uint Unknown3;
		public uint Unknown4;
		public uint Unknown5;
		public uint Unknown6;
		List<Tuple<uint, uint>> Mapping = new List<Tuple<uint, uint>>();
		public static PrsiToken Parse(BytecodeReader reader, uint size)
		{
			var result = new PrsiToken();
			result.Size = size;
			result.OutputRegisterOffset = reader.ReadUInt32();
			result.Unknown1 = reader.ReadUInt32();
			result.Unknown2 = reader.ReadUInt32();
			result.OutputRegisterCount = reader.ReadUInt32();
			result.Unknown3 = reader.ReadUInt32();
			result.Unknown4 = reader.ReadUInt32();
			var mappingCount = reader.ReadUInt32();
			result.Unknown5 = reader.ReadUInt32();
			result.Unknown6 = reader.ReadUInt32();
			Debug.Assert(result.Unknown1 == 0, $"Unknown1={result.Unknown1}");
			Debug.Assert(result.Unknown2 == 0, $"Unknown2={result.Unknown2}");
			Debug.Assert(result.Unknown3 == 0, $"Unknown3={result.Unknown3}");
			Debug.Assert(result.Unknown4 == 0, $"Unknown4={result.Unknown4}");

			Debug.Assert(result.Unknown5 == result.OutputRegisterOffset,
				$"Unknown5 ({result.Unknown5}) and OutputRegisterOffset ({result.OutputRegisterOffset}) differ");
			//Debug.Assert(unk6 == outputRegisterCount,
			//	$"unk6 ({unk6}) and OutputRegisterCount ({outputRegisterCount}) differ");
			for (int i = 0; i < mappingCount; i++)
			{
				result.Mapping.Add(new Tuple<uint, uint>(
					reader.ReadUInt32(),
					reader.ReadUInt32()));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"PRSI");
			sb.AppendLine($"  OutputRegisterOffset: {OutputRegisterOffset}");
			sb.AppendLine($"  Unknown1: {Unknown1}");
			sb.AppendLine($"  Unknown2: {Unknown2}");
			sb.AppendLine($"  OutputRegisterCount: {OutputRegisterCount}");
			sb.AppendLine($"  Unknown3: {Unknown3}");
			sb.AppendLine($"  Unknown4: {Unknown4}");
			sb.AppendLine($"  Unknown5: {Unknown5}");
			sb.AppendLine($"  Unknown6: {Unknown6}");
			sb.AppendLine($"  Mappings: {Mapping.Count}");
			for(int i = 0; i < Mapping.Count; i++)
			{
				var pair = Mapping[i];
				sb.AppendLine($"    {i} - ConstOutput: {pair.Item1} ConstInput {pair.Item2}");
			}
			return sb.ToString();
		}
	}
}
