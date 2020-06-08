using SlimShader.DebugParser;
using System.Diagnostics;

namespace DebugParser.DebugParser.DX9
{
	public class DebugClit
	{
		public static DebugClit Parse(DebugBytecodeReader reader)
		{
			var result = new DebugClit();
			var count = reader.ReadUInt32("Count");
			Debug.Assert(count == 0, $"Clit.Count is {count}");
			return result;
		}
	}
}
