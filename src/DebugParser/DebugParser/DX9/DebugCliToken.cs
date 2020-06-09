using SlimShader.DebugParser;

namespace DebugParser.DebugParser.DX9
{
	public class DebugCliToken
	{
		public static DebugCliToken Parse(DebugBytecodeReader reader)
		{
			var result = new DebugCliToken();
			var count = reader.ReadUInt32("Count");
			return result;
		}
	}
}
