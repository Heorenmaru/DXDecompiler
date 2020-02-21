using SlimShader.Util;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectExpressionIndexAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public BytecodeContainer Shader { get; private set; }

		public uint ShaderOffset;
		public uint ShaderSize;
		uint ArrayNameOffset;
		public static EffectExpressionIndexAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectExpressionIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();

			var shaderOffset = result.ShaderOffset = assignmentReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = result.ShaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectExpressionIndexAssignment.ArrayName: {ArrayName}: {ArrayNameOffset.ToString("X4")}");
			sb.AppendLine($"    EffectExpressionAssignment.ShaderOffset: {ShaderOffset}: {ShaderOffset.ToString("X4")}");
			sb.AppendLine($"    EffectExpressionAssignment.ShaderSize: {ShaderSize}: {ShaderSize.ToString("X4")}");
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(ArrayName);
			sb.Append("[0]; // TODO Expression index assignment not current supported");
			if (Shader != null)
			{
				foreach (var chunk in Shader.Chunks)
				{
					sb.AppendLine(chunk.ToString());
				}
			}
			return sb.ToString();
		}
	}
}
