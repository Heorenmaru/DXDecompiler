using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode
{
	public class PrsiToken
	{
		public uint Size;
		public byte[] Data;
		public static PrsiToken Parse(BytecodeReader reader, uint size)
		{
			var result = new PrsiToken();
			result.Size = size;
			result.Data = reader.ReadBytes((int)size * 4 - 4);
			return result;
		}
	}
}
