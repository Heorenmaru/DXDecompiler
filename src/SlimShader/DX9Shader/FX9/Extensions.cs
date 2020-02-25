using SlimShader.Util;
using System;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public static class Extensions
	{
		public static bool IsSampler(this ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Sampler:
				case ParameterType.Sampler1D:
				case ParameterType.Sampler2D:
				case ParameterType.Sampler3D:
				case ParameterType.SamplerCube:
					return true;
				default:
					return false;
			}
		}
		public static string TryReadString(this BytecodeReader reader)
		{
			try
			{
				var length = reader.ReadUInt32();
				if(length == 0)
				{
					return "";
				}
				var bytes = reader.ReadBytes((int)length);
				return Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
			} catch(Exception ex)
			{
				return "Error reading string";
			}
		}
	}
}
