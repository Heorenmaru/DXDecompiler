using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	/// <summary>
	/// Parameter 4
	/// TechniqueSize 3 (No Pass)
	/// PassSized 3 (No Shader)
	/// InlineShaderSize 4
	/// AnnotationSize2
	/// StringSize = 7 (4 Variable, 3 Object, value="def")
	/// PixelShaderSize = 6 (4 Varaible, 2 Object)
	/// InlineShaderSize BodySize = 6 (5 Parameter, only 4 used?, 1 defaultvalue)
	/// </summary>
	public class Fx9Chunk
	{
		const int headerLength = 4;
		public uint VariableCount;
		public uint TechniqueCount;
		public uint PassCount;
		public uint ShaderCount;
		public uint VariableBlobCount;
		public uint StateBlobCount;
		public List<Variable> Variables = new List<Variable>();
		public List<Technique> Techniques = new List<Technique>();
		public List<VariableBlob> VariableBlobs = new List<VariableBlob>();
		public List<StateBlob> StateBlobs = new List<StateBlob>();

		public Dictionary<Parameter, VariableBlob> VariableBlobLookup = new Dictionary<Parameter, VariableBlob>();
		public Dictionary<Assignment, StateBlob> StateBlobLookup = new Dictionary<Assignment, StateBlob>();
		public static Fx9Chunk Parse(BytecodeReader reader, uint length)
		{
			var result = new Fx9Chunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var footerOffset = chunkReader.ReadUInt32() + 4;
			var bodyReader = chunkReader.CopyAtCurrentPosition();
			var footerReader = reader.CopyAtOffset((int)footerOffset);
			var variableCount = result.VariableCount = footerReader.ReadUInt32();
			var techniqueCount = result.TechniqueCount = footerReader.ReadUInt32();
			result.PassCount = footerReader.ReadUInt32();
			result.ShaderCount = footerReader.ReadUInt32();
			for (int i = 0; i < variableCount; i++)
			{
				result.Variables.Add(Variable.Parse(bodyReader, footerReader));
			}
			for (int i = 0; i < techniqueCount; i++)
			{
				result.Techniques.Add(Technique.Parse(bodyReader, footerReader));
			}
			result.VariableBlobCount = footerReader.ReadUInt32();
			result.StateBlobCount = footerReader.ReadUInt32();
			for (int i = 0; i < result.VariableBlobCount; i++)
			{
				var data = VariableBlob.Parse(bodyReader, footerReader);
				result.VariableBlobs.Add(data);

			}
			for (int i = 0; i < result.StateBlobCount; i++)
			{
				var data = StateBlob.Parse(bodyReader, footerReader);
				result.StateBlobs.Add(data);
			}
			result.BuildBlobLookup();
			return result;
		}
		private void AnnotationBlobLookup(List<Annotation> annotations)
		{
			foreach (var annotation in annotations)
			{
				if (annotation.Parameter.ParameterType.HasVariableBlob())
				{
					var index = annotation.Value[0].UInt;
					var blob = VariableBlobs.FirstOrDefault(b => b.Index == index);
					VariableBlobLookup[annotation.Parameter] = blob;
				}
			}
		}
		public void BuildBlobLookup()
		{
			for(int i = 0; i < Variables.Count; i++)
			{
				var variable = Variables[i];
				if(variable.Parameter.ParameterType.HasVariableBlob())
				{
					var index = variable.DefaultValue[0].UInt;
					var blob = VariableBlobs.FirstOrDefault(b => b.Index == index);
					VariableBlobLookup[variable.Parameter] = blob;
				}
				AnnotationBlobLookup(variable.Annotations);
				if (variable.Parameter.ParameterType.IsSampler())
				{
					for(int j = 0; j < variable.SamplerStates.Count; j++)
					{
						var samplerState = variable.SamplerStates[j];
						for (int k = 0; k < samplerState.Assignments.Count; k++)
						{
							var assignment = samplerState.Assignments[k];
							if (assignment.Type.HasStateBlob())
							{
								StateBlobLookup[assignment] = StateBlobs.FirstOrDefault(b => 
									b.PassIndex == i &&
									b.SamplerStateIndex == j &&
									b.AssignmentIndex == k);
							}
						}
					}
				}
			}
			for (int i = 0; i < Techniques.Count; i++)
			{
				var technique = Techniques[i];
				AnnotationBlobLookup(technique.Annotations);
				for (int j = 0; j < technique.Passes.Count; j++)
				{
					var pass = technique.Passes[j];
					AnnotationBlobLookup(pass.Annotations);
					for (int k = 0; k < pass.Assignments.Count; k++)
					{
						var assignment = pass.Assignments[k];
						var blob = StateBlobs.FirstOrDefault(b => 
								b.TechniqueIndex == i &&
								b.PassIndex == j &&
								b.AssignmentIndex == k);
						if(blob != null || assignment.Type.HasStateBlob())
						{
							StateBlobLookup[assignment] = blob;
						}
					}
				}
			}
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Fx9Chunk");
			sb.AppendLine($"VariableCount: {VariableCount} {VariableCount.ToString("X4")}");
			sb.AppendLine($"TechniqueCount: {TechniqueCount} {TechniqueCount.ToString("X4")}");
			sb.AppendLine($"PassCount?: {PassCount} {PassCount.ToString("X4")}");
			sb.AppendLine($"ShaderCount?: {ShaderCount} {ShaderCount.ToString("X4")}");
			for(int i = 0; i < Variables.Count; i++)
			{
				sb.AppendLine($"Variable {i}");
				sb.Append(Variables[i].Dump());
			}
			for (int i = 0; i < Techniques.Count; i++)
			{
				sb.AppendLine($"Technique {i}");
				sb.Append(Techniques[i].Dump());
			}
			sb.AppendLine($"VariableBlobCount: {VariableBlobCount} {VariableBlobCount.ToString("X4")}");
			sb.AppendLine($"StateBlobCount: {StateBlobCount} {StateBlobCount.ToString("X4")}");
			for (int i = 0; i < VariableBlobs.Count; i++)
			{
				sb.AppendLine($"VariableBlob {i}");
				sb.Append(VariableBlobs[i].Dump());
			}
			for (int i = 0; i < StateBlobs.Count; i++)
			{
				sb.AppendLine($"StateBlob {i}");
				sb.Append(StateBlobs[i].Dump());
			}
			return sb.ToString();
		}
	}
}
