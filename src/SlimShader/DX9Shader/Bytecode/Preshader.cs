using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode
{
	public class Preshader
	{
		public ShaderModel Shader;
		public static Preshader Parse(BytecodeReader reader)
		{
			var result = new Preshader();
			result.Shader = ShaderModel.Parse(reader);
			return result;
		}
	}
}
