using System;
using System.Collections.Generic;
using System.Text;
using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode
{
	public class CliToken
	{
		List<Number> Numbers = new List<Number>();
		public static CliToken Parse(BytecodeReader reader)
		{
			var result = new CliToken();
			var count = reader.ReadUInt32();
			for (int i = 0; i < count; i++)
			{
				result.Numbers.Add(Number.Parse(reader));
			}
			return result;
		}

		public string GetLiteral(uint elementIndex, uint elementCount)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < elementCount; i++)
			{
				var index = elementIndex + i;
				var number = Numbers[(int)index];
				sb.Append(number.ToString());
				if (i < elementCount - 1)
				{
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}
	}
}
