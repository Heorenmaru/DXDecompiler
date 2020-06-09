using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode
{
	public class PresToken
	{
		public ShaderModel Shader;
		public static PresToken Parse(BytecodeReader reader)
		{
			var result = new PresToken();
			result.Shader = ShaderModel.Parse(reader);
			return result;
		}
	}
}
