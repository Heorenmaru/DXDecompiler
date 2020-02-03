using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectStateAnnotation
	{
		public EffectStateAnnotationType MemberType;
		public uint MemberIndex;
		public uint GuessAnnotationClass;
		public uint ValueOffset;

		public uint ValueSize;
		public uint GuessAnotationFormat;
		public uint GuessAnotationValue;

		public string TextureName { get; private set; }
		public static EffectStateAnnotation Parse(BytecodeReader reader, BytecodeReader annotationReader)
		{
			var result = new EffectStateAnnotation();
			//MemberType type, such as AddressV or Filter
			result.MemberType = (EffectStateAnnotationType)annotationReader.ReadUInt32();
			Debug.Assert(Enum.IsDefined(typeof(EffectStateAnnotationType), result.MemberType),
				$"EffectStateAnnotationType is {result.MemberType}");
			//MemberIndex is 1 for BlendEnable[1] = TRUE;
			result.MemberIndex = annotationReader.ReadUInt32();
			//Is 1 for normal sampler state, Is 2 when for
			//Texture2D g_txDiffuse;
			//sampler2D DiffuseSampler = sampler_state {
			//	Texture = (g_txDiffuse);
			//};
			result.GuessAnnotationClass = annotationReader.ReadUInt32();
			result.ValueOffset = annotationReader.ReadUInt32();
			if (result.GuessAnnotationClass == 1)
			{
				var unknownReader = reader.CopyAtOffset((int)result.ValueOffset);
				result.ValueSize = unknownReader.ReadUInt32();

				/* Could be formating flags, for example: 
				 * DepthEnable = TRUE; = 2				bool(TRUE / 1 /);
				 * BackFaceStencilFunc = Always; = 2
				 * StencilReadMask = 255; = 2
				 * StencilReadMask = 0xFF; = 3
				 * DepthEnable = true; = 4				bool(TRUE / true /);
				* 
				 */
				result.GuessAnotationFormat = unknownReader.ReadUInt32();
				//Value such as Wrap in AddressU = WRAP
				result.GuessAnotationValue = unknownReader.ReadUInt32();
			} else
			{
				var textureNameReader = reader.CopyAtOffset((int)result.ValueOffset);
				result.TextureName = textureNameReader.ReadString();
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("  EffectStateAnnotation");
			sb.AppendLine($"    Annotation.MemberType {MemberType} ({(uint)MemberType})");
			sb.AppendLine($"    Annotation.MemberIndex {MemberIndex} ({MemberIndex.ToString("X4")})");
			sb.AppendLine($"    Annotation.GuessAnnotationClass {GuessAnnotationClass} ({GuessAnnotationClass.ToString("X4")})");
			sb.AppendLine($"    Annotation.ValueOffset {ValueOffset} ({ValueOffset.ToString("X4")})");
			if (GuessAnnotationClass == 1)
			{
				sb.AppendLine($"    Annotation.ValueSize {ValueSize} ({ValueSize.ToString("X4")})");
				sb.AppendLine($"    Annotation.GuessAnotationFormat {GuessAnotationFormat} ({GuessAnotationFormat.ToString("X4")})");
				sb.AppendLine($"    Annotation.GuessAnotationValue {GuessAnotationValue} ({GuessAnotationValue.ToString("X4")})");
			}
			else
			{
				sb.AppendLine($"    Annotation.TextureName {TextureName} ({ValueOffset.ToString("X4")})");
			}
			return sb.ToString();
		}
	}
}
