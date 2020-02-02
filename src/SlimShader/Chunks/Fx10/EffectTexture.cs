using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Type bits
	/// Shader Variable Type
	//	Texture1D = 6,
	//	Texture2D = 7,
	//	Texture3D = 8,
	//  TextureCube = 9,
	/// SamplerState:
	/// SamplerComparisonState:
	/// Based on D3D10_EFFECT_VARIABLE_DESC
	/// </summary>
	public class EffectTexture : IEffectVariable
	{
		public uint NameOffset { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint TypeOffset { get; private set; }
		public EffectType Type { get; private set; }
		public uint SemanticOffset { get; private set; }
		public uint BufferOffset { get; private set; }
		public uint SamplerAnnotationCount { get; private set; }
		public uint AnnotationCount { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<EffectStateAnnotation> SamplerAnnotations { get; private set; }
		public uint GuessShaderChunkOffset { get; private set; }


		//TODO
		public uint Flags => 0;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();

		public EffectTexture()
		{
			Annotations = new List<EffectAnnotation>();
			SamplerAnnotations = new List<EffectStateAnnotation>();
		}
		private static bool HasExtraAnnotations(EffectType type)
		{
			switch (type.VariableType)
			{
				case ShaderVariableType.Sampler:
				case ShaderVariableType.DepthStencil:
				case ShaderVariableType.Blend:
				case ShaderVariableType.Rasterizer:
					return true;
			}
			return false;
		}
		private static bool IsShader(EffectType type)
		{
			switch (type.VariableType)
			{
				case ShaderVariableType.VertexShader:
				case ShaderVariableType.PixelShader:
				case ShaderVariableType.GeometryShader:
					return true;
			}
			return false;
		}
		public static EffectTexture Parse(BytecodeReader reader, BytecodeReader variableReader, bool isShared = false)
		{
			var result = new EffectTexture();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			result.TypeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)result.TypeOffset);
			result.Type = EffectType.Parse(reader, typeReader);
			var semanticOffset = result.SemanticOffset = variableReader.ReadUInt32();
			if (semanticOffset != 0)
			{
				var semanticReader = reader.CopyAtOffset((int)semanticOffset);
				result.Semantic = semanticReader.ReadString();
			} else
			{
				result.Semantic = "";
			}
			result.BufferOffset = variableReader.ReadUInt32();
			if (isShared)
			{
				return result;
			}
			if (HasExtraAnnotations(result.Type))
			{
				result.SamplerAnnotationCount = variableReader.ReadUInt32();
				for (int i = 0; i < result.SamplerAnnotationCount; i++)
				{
					result.SamplerAnnotations.Add(EffectStateAnnotation.Parse(reader, variableReader));
				}
			}
			if (IsShader(result.Type))
			{
				result.GuessShaderChunkOffset = variableReader.ReadUInt32();
			}
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectTexture");
			sb.AppendLine($"  Name: '{Name}' ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"  EffectTexture.Semantic: {Semantic} ({SemanticOffset.ToString("X4")})");
			sb.AppendLine($"  EffectTexture.BufferOffset: {BufferOffset}");
			sb.AppendLine($"  EffectTexture.SamplerAnnotationCount: {SamplerAnnotationCount}");
			sb.AppendLine($"  AnnotationCount: {AnnotationCount}");
			sb.AppendLine($"  SamplerAnotationCount: {SamplerAnnotationCount}");
			sb.AppendLine($"  GuessShaderChunkOffset: {GuessShaderChunkOffset} ({GuessShaderChunkOffset.ToString("X4")})");
			sb.AppendLine(Type.ToString());
			foreach (var annotation in SamplerAnnotations)
			{
				sb.Append(annotation);
			}
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation);
			}
			return sb.ToString();
		}
	}
}
