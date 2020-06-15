using System;
using System.Collections.Generic;
using System.Text;
using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode
{
	public class CliToken
	{
		List<double> Numbers = new List<double>();
		public byte[] Data;
		public static CliToken Parse(BytecodeReader reader, uint size)
		{
			var result = new CliToken();
			var dataReader = reader.CopyAtCurrentPosition();
			result.Data = dataReader.ReadBytes((int)size * 4 - 4);
			var count = reader.ReadUInt32();
			for (int i = 0; i < count; i++)
			{
				result.Numbers.Add(reader.ReadDouble());
			}
			return result;
		}

		public string GetLiteral(uint index)
		{
			return Numbers[(int)index].ToString();
		}

		public string Dump()
		{
			return FormatUtil.FormatBytes(Data);
		}
	}
}
