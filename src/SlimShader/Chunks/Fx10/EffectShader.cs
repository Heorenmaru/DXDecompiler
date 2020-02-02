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
		public uint ShaderType;
		public uint Unknown1;
		public uint Unknown2;
		public uint Unknown3;
		public string Name;
		public static EffectShader Parse(BytecodeReader reader, BytecodeReader passReader)
		{
			var result = new EffectShader();
			//6 = vertex, 7 = pixel, 8 = geometry
			result.ShaderType = passReader.ReadUInt32();
			result.Unknown1 = passReader.ReadUInt32();
			result.Unknown2 = passReader.ReadUInt32();
			//String for assigned variable, offset to DXBC otherwise i think
			var nameOffset = result.Unknown3 = passReader.ReadUInt32();
			//2 for shader with variable, 7 for compiled inside pass
			if (result.Unknown2 == 2)
			{
				var nameReader = reader.CopyAtOffset((int)nameOffset);
				result.Name = nameReader.ReadString();
			}
			else
			{
				result.Name = "NoName";
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectShader");
			sb.AppendLine($"  ShaderType {ShaderType} ({(EffectShaderType)ShaderType})");
			sb.AppendLine($"  Unknown1 {Unknown1}");
			sb.AppendLine($"  Unknown2 {Unknown2}");
			sb.AppendLine($"  Unknown3 {Unknown3} ({Unknown3.ToString("X4")})");
			sb.AppendLine($"  Name {Name}");
			return sb.ToString();
		}
	}
}
