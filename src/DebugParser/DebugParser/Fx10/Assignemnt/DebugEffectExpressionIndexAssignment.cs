using SlimShader.Chunks.Fxlvm;
using SlimShader.Util;
using System;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectExpressionIndexAssignment : DebugEffectAssignment
	{
		public string ArrayName { get; private set; }
		public BytecodeContainer Shader { get; private set; }

		public uint ShaderOffset;
		public uint ShaderSize;
		uint ArrayNameOffset;
		public static DebugEffectExpressionIndexAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectExpressionIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32("ArrayNameOffset");
			var arrayNameReader = reader.CopyAtOffset("ArrayNameReader", assignmentReader, (int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString("ArrayName");

			var shaderOffset = result.ShaderOffset = assignmentReader.ReadUInt32("ShaderOffset");
			var shaderReader = reader.CopyAtOffset("ShaderReader", assignmentReader, (int)shaderOffset);
			var shaderSize = result.ShaderSize = shaderReader.ReadUInt32("ShaderSize");
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes("Shader", (int)shaderSize));
			}
			return result;
		}
	}
}
