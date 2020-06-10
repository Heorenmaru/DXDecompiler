using System.Diagnostics;

namespace SlimShader.DebugParser.DX9
{
	public class DebugPrsiToken
	{
		public static DebugPrsiToken Parse(DebugBytecodeReader reader, uint size)
		{
			//Size appears to be 12 for VS shaders and 14 for PS shaders
			var result = new DebugPrsiToken();
			if(size != 12) throw new System.Exception($"PRSI TOKEN Size {size}");
			Debug.Assert(true, "PRSI Token");
			return result;
		}
	}
}
