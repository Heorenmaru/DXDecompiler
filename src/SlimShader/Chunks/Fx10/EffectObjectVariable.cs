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
	public class EffectObjectVariable : IEffectVariable
	{
		public uint NameOffset { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint TypeOffset { get; private set; }
		public EffectType Type { get; private set; }
		public uint SemanticOffset { get; private set; }
		public uint BufferOffset { get; private set; }
		public uint StateAnnotationCount { get; private set; }
		public uint AnnotationCount { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<EffectStateAnnotation> StateAnnotations { get; private set; }
		public uint GuessShaderChunkOffset { get; private set; }
		public string StreamOutputDecl0 { get; private set; }

		/// <summary>
		/// Shader5 Members
		/// </summary>
		public uint SODecls0;
		public uint SODecls1;
		public uint SODecls2;
		public uint SODecls3;
		public uint SODeclsCount;
		public uint RasterizedStream;
		public uint InterfaceBindingCount;
		public uint InterfaceBindingOffset;

		//TODO
		public uint Flags => 0;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();

		public uint StreamOutputDecl0Offset;
		public EffectObjectVariable()
		{
			Annotations = new List<EffectAnnotation>();
			StateAnnotations = new List<EffectStateAnnotation>();
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
				case ShaderVariableType.ComputeShader:
				case ShaderVariableType.HullShader:
				case ShaderVariableType.DomainShader:
					return true;
			}
			return false;
		}
		private static bool IsShader5(EffectType type)
		{
			switch (type.ObjectType)
			{
				case EffectObjectType.VertexShader5:
				case EffectObjectType.PixelShader5:
				case EffectObjectType.GeometryShader5:
				case EffectObjectType.ComputeShader5:
				case EffectObjectType.HullShader5:
				case EffectObjectType.DomainShader5:
					return true;
			}
			return false;
		}
		public static EffectObjectVariable Parse(BytecodeReader reader, BytecodeReader variableReader, bool isShared = false)
		{
			var result = new EffectObjectVariable();
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
				result.StateAnnotationCount = variableReader.ReadUInt32();
				for (int i = 0; i < result.StateAnnotationCount; i++)
				{
					result.StateAnnotations.Add(EffectStateAnnotation.Parse(reader, variableReader));
				}
			}
			if (IsShader5(result.Type))
			{
				result.GuessShaderChunkOffset = variableReader.ReadUInt32();
				result.SODecls0 = variableReader.ReadUInt32();
				result.SODecls1 = variableReader.ReadUInt32();
				result.SODecls2 = variableReader.ReadUInt32();
				result.SODecls3 = variableReader.ReadUInt32();
				result.SODeclsCount = variableReader.ReadUInt32();
				result.RasterizedStream = variableReader.ReadUInt32();
				result.InterfaceBindingCount = variableReader.ReadUInt32();
				result.InterfaceBindingOffset = variableReader.ReadUInt32();
			}
			else if (IsShader(result.Type))
			{
				result.GuessShaderChunkOffset = variableReader.ReadUInt32();
			}
			if (result.Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				result.StreamOutputDecl0Offset = variableReader.ReadUInt32();
				var declReader = reader.CopyAtOffset((int)result.StreamOutputDecl0Offset);
				result.StreamOutputDecl0 = declReader.ReadString();
			} else
			{
				result.StreamOutputDecl0 = "";
			}
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				//result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectObjectVariable");
			sb.AppendLine($"  Name: '{Name}' ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"  EffectObject.Semantic: {Semantic} ({SemanticOffset.ToString("X4")})");
			sb.AppendLine($"  EffectObject.BufferOffset: {BufferOffset}");
			sb.AppendLine($"  AnnotationCount: {AnnotationCount}");
			if (HasExtraAnnotations(Type))
			{
				sb.AppendLine($"  StateAnnotationCount: {StateAnnotationCount}");
			}
			if (IsShader(Type))
			{
				sb.AppendLine($"  GuessShaderChunkOffset: {GuessShaderChunkOffset} ({GuessShaderChunkOffset.ToString("X4")})");
			}
			if (IsShader5(Type))
			{
				sb.AppendLine($"  EffectObject.SODecls0: {SODecls0} ({SODecls0.ToString("X4")})");
				sb.AppendLine($"  EffectObject.SODecls1: {SODecls1} ({SODecls1.ToString("X4")})");
				sb.AppendLine($"  EffectObject.SODecls2: {SODecls2} ({SODecls2.ToString("X4")})");
				sb.AppendLine($"  EffectObject.SODecls3: {SODecls3} ({SODecls3.ToString("X4")})");
				sb.AppendLine($"  EffectObject.SODeclsCount: {SODeclsCount} ({SODeclsCount.ToString("X4")})");
				sb.AppendLine($"  EffectObject.RasterizedStream: {RasterizedStream} ({RasterizedStream.ToString("X4")})");
				sb.AppendLine($"  EffectObject.InterfaceBindingCount: {InterfaceBindingCount} ({InterfaceBindingCount.ToString("X4")})");
				sb.AppendLine($"  EffectObject.InterfaceBindingOffset: {InterfaceBindingOffset} ({InterfaceBindingOffset.ToString("X4")})");
			}
			if (Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				sb.AppendLine($"  EffectObject.StreamOutputDecl0: {StreamOutputDecl0} ({StreamOutputDecl0Offset.ToString("X4")})");
			}			
			sb.AppendLine(Type.ToString());
			foreach (var annotation in StateAnnotations)
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
