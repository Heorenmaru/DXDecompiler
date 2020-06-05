using SlimShader.DX9Shader.FX9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class EffectHLSLWriter
	{
		Fx9Chunk EffectChunk;
		StreamWriter writer;
		int indent = 0;
		public EffectHLSLWriter(Fx9Chunk effectChunk)
		{
			EffectChunk = effectChunk;
		}
		public static string Decompile (Fx9Chunk effectChunk)
		{
			var asmWriter = new EffectHLSLWriter(effectChunk);
			using (var stream = new MemoryStream())
			{
				asmWriter.Write(stream);
				asmWriter.writer.Flush();
				stream.Position = 0;
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}
		public void Write(Stream stream)
		{
			writer = new StreamWriter(stream);
			foreach(var variable in EffectChunk.Variables)
			{
				WriteVariable(variable);
			}
			foreach (var technique in EffectChunk.Techniques)
			{
				WriteTechnique(technique);
			}
			WriteLine("//Errors: {0} ",
				EffectChunk.DebugError);
			WriteLine("//Entries {0} Binary {1}({2}) Inline {3}({4})", 
				EffectChunk.DataEntries.Count,
				EffectChunk.UnknownCount,
				EffectChunk.BinaryDataList.Count,
				EffectChunk.InlineShaderCount,
				EffectChunk.InlineShaders.Count);
			WriteLine("");
			for (int i = 0; i < EffectChunk.DataEntries.Count; i++)
			{
				WriteLine("//{0} {1}",
					i, DataEntryToString((uint)i + 1));
				if (EffectChunk.DataEntries[i].BinaryData != null)
				{
					var data = EffectChunk.DataEntries[i].BinaryData;
					WriteLine("//BinaryData{0}: Index? {1} Size: {2} DataLength {3} Version: {4}",
						i, data.Index, data.Size, data.Data.Length, data.Version);
				}
				if (EffectChunk.DataEntries[i].InlineShader != null)
				{
					var data = EffectChunk.DataEntries[i].InlineShader;
					WriteLine("//InlineShader{0}: Unk1 {1} Unk2 {2} Unk3 {3} Index? {4}",
						i - EffectChunk.UnknownCount, 
						data.Unknown1.ToString("X8"),
						data.Unknown2.ToString("X8"),
						data.Unknown3.ToString("X8"), 
						data.Index);
					WriteLine("//                IsVariable: {0} Size {1} Name: {2} Version: {3}",
							 data.IsVariable, data.ShaderSize, data.VariableName, data.Version);

				}
				WriteLine("");

			}
			/*
			for (int i = 0; i < EffectChunk.BinaryDataList.Count; i++)
			{
				var data = EffectChunk.BinaryDataList[i];
				WriteLine("//{0} BinaryData{0}: Index? {1} Size: {2} ", 
					dataCount++, i, data.Index, data.Size);
			}
			for (int i = 0; i < EffectChunk.InlineShaders.Count; i++)
			{
				var data = EffectChunk.InlineShaders[i];
				WriteLine("//{0} InlineShader{0}: Unk0 {1} Unk2 {2} Unk3 {3} Index? {4}",
					dataCount++, i, data.Unknown1.ToString("X8"), data.Unknown2.ToString("X8"), 
					data.Unknown3.ToString("X8"), data.Index);
				WriteLine("//                IsVariable: {0} Size {1} Name: {2} Version: {3}", 
						 data.IsVariable, data.ShaderSize, data.VariableName, data.Version);
			}*/
		}
		public string DataEntryToString(uint index)
		{
			index = index - 1;
			if(index >= EffectChunk.DataEntries.Count)
			{
				return $"Index out of range {index}";
			}
			var data = EffectChunk.DataEntries[(int)index];
			if(data.BinaryData != null)
			{
				if(data.BinaryData.Data.Length == 0)
				{
					return $"Variable lookup {data.BinaryData.Index}";
				} else if (data.BinaryData.IsShader)
				{
					return $"compile shader Index {data.BinaryData.Index} size {data.BinaryData.Size} data {data.BinaryData.Data.Length}";
				} else
				{
					return data.BinaryData.DataPreview;
				}
			}
			if (data.InlineShader != null)
			{
				if (!string.IsNullOrEmpty(data.InlineShader.VariableName))
				{
					return data.InlineShader.VariableName;
				} if(!string.IsNullOrEmpty(data.InlineShader.Version))
				{
					return $"compile {data.InlineShader.Version} Shader()";
				} else
				{
					return $"Unknown inline shader {data.Index}";
				}
			}
			throw new ArgumentException();
		}
		public void WriteVariable(Variable variable)
		{
			var param = variable.Parameter;
			WriteIndent();
			Write(param.GetDecleration());
			if(variable.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(variable.Annotations);
			}
			if(param.ParameterType.IsSampler())
			{
				WriteLine(" =");
				WriteLine("sampler_state");
				WriteLine("{");
				indent++;
				foreach (var state in variable.SamplerStates)
				{
					WriteIndent();
					if (state.Type == StateType.Texture)
					{
						var data = DataEntryToString(state.Value.Data[0]);
						WriteLine("{0} = {1}; // {2}", state.Type, data, state.Value.Data[0]);
					}
					else
					{
						WriteLine("{0} = {1};", state.Type, state.Value.Data[0]);
					}
				}
				indent--;
				WriteLine("};");
			} else
			{
				if (param.ParameterType.IsObjectType())
				{
					var data = DataEntryToString(variable.DefaultValue[0].UInt);
					WriteLine(" = {0} ; // {1}", data, variable.DefaultValue[0].UInt);
				}
				else if (variable.DefaultValue.All(d => d.UInt == 0))
				{
					WriteLine(";");
				}
				else
				{
					var defaultValue = string.Join(", ", variable.DefaultValue);
					WriteLine(" = {{ {0} }};", defaultValue);
				}
			}
		}
		public void WriteAnnotations(List<Annotation> annotations)
		{
			Write("<");
			for(int i = 0; i < annotations.Count; i++)
			{
				var annotation = annotations[i];
				var value = string.Join(", ", annotation.Value);
				Write("{0} {1} = {2};", annotation.Parameter.GetTypeName(), annotation.Parameter.Name, value);
				if (i < annotations.Count - 1) Write(" ");
			}
			Write(">");
		}
		public void WriteTechnique(Technique technique)
		{
			Write("technique {0}", technique.Name);
			if (technique.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(technique.Annotations);
			}
			WriteLine();
			WriteLine("{");
			indent++;
			foreach (var pass in technique.Passes)
			{
				WritePass(pass);
			}
			indent--;
			WriteLine("}");
			WriteLine("");
		}
		public void WritePass(Pass pass)
		{
			WriteIndent();
			Write("pass {0}", pass.Name);
			if (pass.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(pass.Annotations);
			}
			WriteLine();
			WriteIndent();
			WriteLine("{");
			indent++;
			var shaderAssignments = pass.Assignments;
			foreach (var assignment in shaderAssignments)
			{
				WriteAssignment(assignment);
			}
			indent--;
			WriteIndent();
			WriteLine("}");
		}
		public void WriteAssignment(Assignment assignment)
		{
			WriteIndent();
			string index = "";
			if (assignment.Type.RequiresIndex())
			{
				index = string.Format("[{0}]", assignment.ArrayIndex);
			}
			string value;
			if(assignment.Value.Count > 1)
			{
				value = string.Format("{{ {0} }}", string.Join(", ", assignment.Value));
			} else if(
				assignment.Type == StateType.PixelShader ||
				assignment.Type == StateType.VertexShader)
			{
				value = DataEntryToString(assignment.Value[0].UInt) + $" /* {assignment.Value[0].UInt} */";
			} else
			{
				value = assignment.Value[0].ToString();
			}
			WriteLine("{0}{1} = {2};", assignment.Type.ToString(), index, value);
		}
		public void WriteIndent()
		{
			writer.Write(new string(' ', indent * 4));
		}
		void WriteLine()
		{
			writer.WriteLine();
		}

		void WriteLine(string value)
		{
			writer.WriteLine(value);
		}

		void WriteLine(string format, params object[] args)
		{
			writer.WriteLine(format, args);
		}

		void Write(string value)
		{
			writer.Write(value);
		}

		void Write(string format, params object[] args)
		{
			writer.Write(format, args);
		}
	}
}
