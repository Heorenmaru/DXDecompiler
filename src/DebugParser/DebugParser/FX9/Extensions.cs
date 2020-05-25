using SlimShader.DX9Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.FX9
{
	public static class Extensions
	{
		public static List<Number> ReadParameterValue(this DebugParameter parameter, DebugBytecodeReader valueReader)
		{
			var result = new List<Number>();
			if (parameter.ParameterClass == ParameterClass.Object)
			{
				var data = valueReader.ReadBytes("ParameterValue", 4);
				result.Add(new Number(data));
			}
			else
			{
				var defaultValueCount = parameter.GetSize() / 4;
				var data = valueReader.ReadBytes("ParameterValue", (int)defaultValueCount * 4);
				for (int i = 0; i < defaultValueCount; i++)
				{

					result.Add(Number.FromByteArray(data, i * 4));
				}
			}
			return result;
		}
	}
}
