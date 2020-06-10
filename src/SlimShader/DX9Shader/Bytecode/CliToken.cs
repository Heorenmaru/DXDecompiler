using System;
using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode
{
	public class CliToken
	{
		public static CliToken Parse(BytecodeReader reader)
		{
			var result = new CliToken();
			return result;
		}

		internal string GetLiteral(uint elementIndex, uint componentCount)
		{
			return "?";
		}
	}
}
