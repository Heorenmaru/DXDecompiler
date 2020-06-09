
namespace SlimShader.DebugParser.DX9
{
	public class DebugPresToken
	{
		DebugShaderModel Shader;
		public static DebugPresToken Parse(DebugBytecodeReader reader)
		{
			var result = new DebugPresToken();
			result.Shader = DebugShaderModel.Parse(reader);
			return result;
		}
	}
}
