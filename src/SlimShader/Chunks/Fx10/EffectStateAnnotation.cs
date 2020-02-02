using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectStateAnnotation
	{
		public uint SamplerUnknown1;
		public uint SamplerUnknown2;
		public uint SamplerUnknown3;
		public uint SamplerUnknown4;

		public uint SamplerUnknown5;
		public uint SamplerUnknown6;
		public uint SamplerUnknown7;
		public static EffectStateAnnotation Parse(BytecodeReader reader, BytecodeReader annotationReader)
		{
			var result = new EffectStateAnnotation();
			//Annotation type, such as AddressV or Filter
			result.SamplerUnknown1 = annotationReader.ReadUInt32();
			result.SamplerUnknown2 = annotationReader.ReadUInt32();
			result.SamplerUnknown3 = annotationReader.ReadUInt32();
			result.SamplerUnknown4 = annotationReader.ReadUInt32();
			var unknownReader = reader.CopyAtOffset((int)result.SamplerUnknown4);
			result.SamplerUnknown5 = unknownReader.ReadUInt32();
			result.SamplerUnknown6 = unknownReader.ReadUInt32();
			//Value such as Wrap in AddressU = WRAP
			result.SamplerUnknown7 = unknownReader.ReadUInt32();
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("  EffectStateAnnotation");
			sb.AppendLine($"    Annotation.SamplerUnknown1 {SamplerUnknown1} ({SamplerUnknown1.ToString("X4")})");
			sb.AppendLine($"    Annotation.SamplerUnknown2 {SamplerUnknown2} ({SamplerUnknown2.ToString("X4")})");
			sb.AppendLine($"    Annotation.SamplerUnknown3 {SamplerUnknown3} ({SamplerUnknown3.ToString("X4")})");
			sb.AppendLine($"    Annotation.SamplerUnknown4 {SamplerUnknown4} ({SamplerUnknown4.ToString("X4")})");

			sb.AppendLine($"    Annotation.SamplerUnknown5 {SamplerUnknown5} ({SamplerUnknown5.ToString("X4")})");
			sb.AppendLine($"    Annotation.SamplerUnknown6 {SamplerUnknown6} ({SamplerUnknown6.ToString("X4")})");
			sb.AppendLine($"    Annotation.SamplerUnknown7 {SamplerUnknown7} ({SamplerUnknown7.ToString("X4")})");
			return sb.ToString();
		}
	}
}
