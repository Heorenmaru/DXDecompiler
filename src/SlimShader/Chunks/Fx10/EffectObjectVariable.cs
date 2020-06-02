using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
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
		public uint AssignmentCount { get; private set; }
		public uint AnnotationCount { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<List<EffectAssignment>> Assignments { get; private set; }
		public List<string> Strings { get; private set; }
		public List<EffectShaderData5> ShaderData5 { get; private set; }
		public List<EffectShaderData> ShaderData { get; private set; }
		public List<EffectGSSOInitializer> GSSOInitializers { get; private set; }

		//TODO
		public uint Flags => 0;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();

		private uint ElementCount => Type.ElementCount == 0 ? 1 : Type.ElementCount;
		public EffectObjectVariable()
		{
			Annotations = new List<EffectAnnotation>();
			Assignments = new List<List<EffectAssignment>>();
			Strings = new List<string>();
			ShaderData5 = new List<EffectShaderData5>();
			ShaderData = new List<EffectShaderData>();
			GSSOInitializers = new List<EffectGSSOInitializer>();
	}
		private static bool IfHasAssignments(EffectType type)
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
		public static EffectObjectVariable Parse(BytecodeReader reader, BytecodeReader variableReader, ShaderVersion version, bool isShared = false)
		{
			var result = new EffectObjectVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			result.TypeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)result.TypeOffset);
			result.Type = EffectType.Parse(reader, typeReader, version);
			var semanticOffset = result.SemanticOffset = variableReader.ReadUInt32();
			if (semanticOffset != 0)
			{
				var semanticReader = reader.CopyAtOffset((int)semanticOffset);
				result.Semantic = semanticReader.ReadString();
			}
			else
			{
				result.Semantic = "";
			}
			result.BufferOffset = variableReader.ReadUInt32();
			if (isShared)
			{
				return result;
			}
			// Initializer data
			if (result.Type.ObjectType == EffectObjectType.String)
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					var stringValueOffset = variableReader.ReadUInt32();
					var stringValueReader = reader.CopyAtOffset((int)stringValueOffset);
					result.Strings.Add(stringValueReader.ReadString());
				}
			}
			if (IfHasAssignments(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					var assignmentCount = variableReader.ReadUInt32();
					var assignments = new List<EffectAssignment>();
					result.Assignments.Add(assignments);
					for (int j = 0; j < assignmentCount; j++)
					{
						assignments.Add(EffectAssignment.Parse(reader, variableReader));
					}
				}
			}
			if (result.Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					result.GSSOInitializers.Add(EffectGSSOInitializer.Parse(reader, variableReader));
				}
			}
			else if (IsShader5(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					result.ShaderData5.Add(EffectShaderData5.Parse(reader, variableReader));
				}
			}
			else if (IsShader(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					result.ShaderData.Add(EffectShaderData.Parse(reader, variableReader));
				}
			}
			result.AnnotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader, version));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"EffectObjectVariable");
			sb.AppendLine($"  Name: '{Name}' ({NameOffset.ToString("X4")})");
			sb.AppendLine($"  TypeOffset: {TypeOffset} ({TypeOffset.ToString("X4")})");
			sb.AppendLine($"  EffectObject.Semantic: {Semantic} ({SemanticOffset.ToString("X4")})");
			sb.AppendLine($"  EffectObject.BufferOffset: {BufferOffset}");
			sb.AppendLine($"  AnnotationCount: {AnnotationCount}");
			if (Type.VariableType == ShaderVariableType.String)
			{
				foreach (var data in Strings)
				{
					sb.AppendLine($"  StringData: {data}");
				}
			}
			if (IsShader(Type))
			{
				foreach (var data in ShaderData)
				{
					sb.Append(data.Dump());
				}
			}
			if (IsShader5(Type))
			{
				foreach(var data in ShaderData5)
				{
					sb.Append(data.Dump());
				}
			}
			if (Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				foreach (var data in GSSOInitializers)
				{
					sb.Append(data.Dump());
				}
			}
			sb.AppendLine(Type.ToString());
			foreach (var assignments1 in Assignments)
			{
				sb.AppendLine($"  AssignmentCount: {assignments1.Count}");
				foreach (var assignments2 in assignments1) 
				{ 
					sb.Append(assignments2.Dump());
				}
			}
			foreach (var annotation in Annotations)
			{
				sb.Append(annotation.Dump());
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			string arrayFormat = "";
			if(Type.ElementCount > 1)
			{
				arrayFormat = string.Format("[{0}]", Type.ElementCount);
			}
			sb.Append(string.Format("{0,-7} {1}{2}", Type.TypeName, Name, arrayFormat));

			if (Annotations.Count > 0)
			{
				sb.AppendLine();
				sb.AppendLine("<");
				foreach (var annotation in Annotations)
				{
					sb.AppendLine($"    {annotation}");
				}
				sb.Append(">");
			}
			if (Assignments.Count == 1)
			{
				sb.AppendLine();
				sb.AppendLine("{");
				foreach (var subAssignment in Assignments[0])
				{
					sb.AppendLine($"    {subAssignment}");
				}
				sb.Append("}");
			} else if(Assignments.Count > 1) {
				sb.AppendLine();
				sb.AppendLine("{");
				for (int i = 0; i < Assignments.Count; i++)
				{
					var assignment = Assignments[i];
					sb.AppendLine("    {");
					foreach (var subAssignment in assignment)
					{
						sb.AppendLine($"        {subAssignment}");
					}
					sb.Append("    }");
					if (i < Assignments.Count - 1) sb.Append(",");
					sb.AppendLine();
				}
				sb.Append("}");
			}
			if (Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				sb.AppendLine(" =");
				foreach (var data in GSSOInitializers)
				{
					sb.Append(data.ToString());
				}
			}
			else if (IsShader5(Type))
			{
				sb.AppendLine(" =");
				foreach (var data in ShaderData5)
				{
					sb.Append(data.ToString());
				}
			}
			else if (IsShader(Type))
			{
				sb.AppendLine(" =");
				foreach (var data in ShaderData)
				{
					sb.Append(data.ToString());
				}
			}
			sb.Append(";");
			return sb.ToString();
		}
	}
}
