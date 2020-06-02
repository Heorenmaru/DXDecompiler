using SlimShader.Chunks.Fxlvm;
using SlimShader.Util;
using System;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectExpressionAssignment : DebugEffectAssignment
	{
		public DebugBytecodeContainer Shader { get; private set; }
		public uint ShaderSize;
		public static DebugEffectExpressionAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectExpressionAssignment();
			var shaderSize = result.ShaderSize = assignmentReader.ReadUInt32("ShaderSize");

			if (shaderSize != 0)
			{
				//result.Shader = DebugBytecodeContainer.Parse(assignmentReader.ReadBytes("Shader", (int)shaderSize));
				result.Shader = DebugBytecodeContainer.Parse(assignmentReader.CopyAtCurrentPosition("ExpressionReader", assignmentReader));
			}
			return result;
		}
	}
}
