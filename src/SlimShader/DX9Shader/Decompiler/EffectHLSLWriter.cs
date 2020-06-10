﻿using SlimShader.DX9Shader.Decompiler;
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
		Dictionary<object, string> ShaderNames = new Dictionary<object, string>();
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
		void BuildNameLookup()
		{
			int shaderCount = 0;
			foreach(var blob in EffectChunk.VariableBlobs)
			{
				if (blob.IsShader)
				{
					ShaderNames[blob] = $"Shader{shaderCount++}";
				}
			}
			foreach (var blob in EffectChunk.StateBlobs)
			{
				if (blob.BlobType == StateBlobType.Shader || 
					blob.BlobType == StateBlobType.IndexShader)
				{
					ShaderNames[blob] = $"Shader{shaderCount++}";
				}
			}

		}
		public void Write(Stream stream)
		{
			writer = new StreamWriter(stream);
			BuildNameLookup();
			foreach (var variable in EffectChunk.Variables)
			{
				WriteVariable(variable);
			}
			foreach(var blob in EffectChunk.StateBlobs)
			{
				if(blob.BlobType == StateBlobType.Shader ||
					blob.BlobType == StateBlobType.IndexShader)
				{
					WriteShader(blob);
				}
			}
			foreach (var technique in EffectChunk.Techniques)
			{
				WriteTechnique(technique);
			}
			WriteLine("//Entries Binary {0}({1}) Inline {2}({3})", 
				EffectChunk.VariableBlobCount,
				EffectChunk.VariableBlobs.Count,
				EffectChunk.StateBlobCount,
				EffectChunk.StateBlobs.Count);
			WriteLine("");
			for (int i = 0; i < EffectChunk.VariableBlobs.Count; i++)
			{
				var data = EffectChunk.VariableBlobs[i];
				WriteLine("//VariableBlob {0}: Index {1} Size: {2} DataLength {3} Version: {4}",
					i, data.Index, data.Size, data.Data.Length, data.Version);
			}
			for (int i = 0; i < EffectChunk.StateBlobs.Count; i++)
			{
				var data = EffectChunk.StateBlobs[i];
				WriteLine("//StateBlob {0}: TechniqueIndex {1} StateIndex {2} SamplerIndex {3}",
					i, 
					data.TechniqueIndex.ToString("X4"),
					data.PassIndex.ToString("X4"),
					data.SamplerStateIndex.ToString("X4"));
				WriteLine("//             AssignmentIndex {0} BlobType: {1} Size {2} Name: {3} Version: {4}",
							data.AssignmentIndex.ToString("X4"), data.BlobType, data.BlobSize, data.VariableName, data.VersionString);
			}

			for (int i = 0; i < EffectChunk.StateBlobs.Count; i++)
			{
				var data = EffectChunk.StateBlobs[i];
				if (data.BlobType == StateBlobType.Variable) continue;
				if (data.Shader.Type != ShaderType.Tx) continue;
				WriteLine("// Expression, StateBlob {0}: ", i);
				try
				{
					var text = HlslWriter.Decompile(data.Shader);
					WriteLine(text);
				} catch(Exception ex)
				{
					WriteLine(ex.ToString());
				}
				WriteLine("Fin");
			}
		}
		void WriteShader(StateBlob blob)
		{
			WriteLine($"// {ShaderNames[blob]}");
			var funcName = ShaderNames[blob];
			var text = "";
			if (blob.Shader.Type == ShaderType.Tx)
			{
				text = ExpressionHLSLWriter.Decompile(blob.Shader);
			}
			else
			{
				text = HlslWriter.Decompile(blob.Shader);
				text = text.Replace("main()", $"{funcName}()");
			}
			WriteLine(text);
		}
		public string StateBlobToString(Assignment key)
		{
			if (!EffectChunk.StateBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = EffectChunk.StateBlobLookup[key];
			if(data == null)
			{
				return "Blob is NULL";
			}
			if (data.BlobType == StateBlobType.Shader)
			{
				if(data.ShaderType == ShaderType.Tx)
				{
					return $"ExpressionShader";
				}
				if (string.IsNullOrEmpty(data.VersionString))
				{
					return "NULL";
				}
				else
				{
					var funcName = ShaderNames[data];
					return $"compile {data.VersionString} {funcName}";
				}
			}
			if (data.BlobType == StateBlobType.Variable)
			{
				if (string.IsNullOrEmpty(data.VariableName))
				{
					return "NULL";
				} else
				{
					return $"<{data.VariableName}>";
				}
			}
			if (data.BlobType == StateBlobType.IndexShader)
			{
				return $"{data.VariableName}[TODO]";
			}
			throw new ArgumentException();
		}
		public string VariableBlobToString(FX9.Parameter key)
		{
			if (!EffectChunk.VariableBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = EffectChunk.VariableBlobLookup[key];
			if (data == null)
			{
				return "Blob is NULL";
			}
			if (data.Data.Length == 0)
			{
				return "";
			}
			else if (data.IsShader)
			{
				return $"compile {data.Version}";
			}
			else if(key.ParameterType == ParameterType.String)
			{
				return $"\"{data.Value}\"";
			} else
			{
				return $"<{data.Value}>";
			}
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
				if(variable.SamplerStates.Count > 1)
				{
					WriteLine("{");
					indent++;
				}
				for(int i = 0; i < variable.SamplerStates.Count; i++) {
					var state = variable.SamplerStates[i];
					WriteIndent();
					WriteLine("sampler_state");
					WriteIndent();
					WriteLine("{");
					indent++;
					foreach (var assignment in state.Assignments)
					{
						WriteIndent();
						if (assignment.Type == StateType.Texture)
						{
							var data = StateBlobToString(assignment);
							WriteLine("{0} = <{1}>; // {2}", assignment.Type, data, assignment.Value[0].UInt);
						}
						else
						{
							WriteLine("{0} = {1};", assignment.Type, assignment.Value[0].UInt);
						}
					}
					indent--;
					WriteIndent();
					Write("}");
					if (variable.SamplerStates.Count == 1)
					{
						WriteLine(";");
					} else if(i < variable.SamplerStates.Count - 1)
					{
						WriteLine(",");
					} else
					{
						WriteLine();
					}
				}
				if (variable.SamplerStates.Count > 1)
				{
					indent--;
					WriteIndent();
					WriteLine("};");
				}
			} else
			{
				if (param.ParameterType.HasVariableBlob())
				{
					var data = VariableBlobToString(variable.Parameter);
					if (string.IsNullOrEmpty(data))
					{
						WriteLine("; // {0}", variable.DefaultValue[0].UInt);
					}
					else
					{
						WriteLine(" = {0}; // {1}", data, variable.DefaultValue[0].UInt);
					}
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
				if (annotation.Parameter.ParameterType.HasVariableBlob())
				{
					Write("{0} {1} = {2};", 
						annotation.Parameter.GetTypeName(), 
						annotation.Parameter.Name, 
						VariableBlobToString(annotation.Parameter));
				} else
				{
					Write("{0} {1} = {2};", annotation.Parameter.GetTypeName(), annotation.Parameter.Name, value);
				}
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
			} else if(EffectChunk.StateBlobLookup.ContainsKey(assignment))
			{
				value = StateBlobToString(assignment) + $" /* {assignment.Value[0].UInt} */";
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
