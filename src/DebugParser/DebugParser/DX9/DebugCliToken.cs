using SlimShader.DebugParser;
using System.Collections.Generic;
using System.Text;

namespace DebugParser.DebugParser.DX9
{
	public class DebugCliToken
	{
		List<DebugNumber> Numbers = new List<DebugNumber>();
		public static DebugCliToken Parse(DebugBytecodeReader reader)
		{
			var result = new DebugCliToken();
			var count = reader.ReadUInt32("Count");
			for(int i = 0; i < count; i++)
			{
				reader.AddIndent($"Number {i}");
				result.Numbers.Add(DebugNumber.Parse(reader));
				reader.RemoveIndent();
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
