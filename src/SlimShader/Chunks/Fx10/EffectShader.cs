using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Could be based on D3D10_EFFECT_SHADER_DESC?
	/// </summary>
	public class EffectShader
	{
		public EffectShaderType ShaderType;
		public uint Unknown1;
		public uint GuessType;
		public uint GuessShaderOffset;
		public string Name;
		public static EffectShader Parse(BytecodeReader reader, BytecodeReader passReader)
		{
			var result = new EffectShader();
			result.ShaderType = (EffectShaderType)passReader.ReadUInt32();
			result.Unknown1 = passReader.ReadUInt32();
			result.GuessType = passReader.ReadUInt32();
			//String for assigned variable, offset to DXBC otherwise i think
			var nameOffset = result.GuessShaderOffset = passReader.ReadUInt32();
			//2 for shader with variable, 7 for compiled inside pass, 1 for StateParamter
			if (result.GuessType == 2)
			{
				var nameReader = reader.CopyAtOffset((int)nameOffset);
				result.Name = nameReader.ReadString();
			}
			else
			{
				//TODO: GetValue
				result.Name = "$Anonymous";
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectShader");
			sb.AppendLine($"  ShaderType {ShaderType} ({(uint)ShaderType})");
			sb.AppendLine($"  EffectShader.Unknown1 {Unknown1} ({Unknown1.ToString("X4")})");
			sb.AppendLine($"  EffectShader.GuessType {GuessType} ({GuessType.ToString("X4")})");
			sb.AppendLine($"  EffectShader.GuessShaderOffset {GuessShaderOffset} ({GuessShaderOffset.ToString("X4")})");
			sb.AppendLine($"  Name {Name}");
			return sb.ToString();
		}
	}
}
