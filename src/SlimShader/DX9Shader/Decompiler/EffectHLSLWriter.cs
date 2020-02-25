using SlimShader.DX9Shader.FX9;
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
			for (int i = 0; i < EffectChunk.BinaryDataList.Count; i++)
			{
				var data = EffectChunk.BinaryDataList[i];
				WriteLine("// BinaryData{0}: Index? {1} Size: {2} ", i, data.Index, data.Size);
			}
			for (int i = 0; i < EffectChunk.InlineShaders.Count; i++)
			{
				var data = EffectChunk.InlineShaders[i];
				WriteLine("// InlineShader{0}: Unk0 {1} Unk2 {2} Unk3 {3} Index? {1}", 
					i, data.Unknown1.ToString("X8"), data.Unknown2.ToString("X8"), data.Unknown3.ToString("X8"), data.Index);
				WriteLine("//                IsVariable: {0} Size {1} Name: {2} ", 
						 data.IsVariable, data.ShaderSize, data.VariableName);
			}
		}
		public void WriteVariable(Variable variable)
		{
			var param = variable.Parameter;
			var semantic = !string.IsNullOrEmpty(param.Semantic) ?
				string.Format(" : {0}", param.Semantic) : "";
			WriteIndent();
			Write("{0} {1}{2}", 
				param.GetTypeName(), param.Name, semantic);
			if(param.ParameterType.IsSampler())
			{
				WriteLine(" =");
				WriteLine("sampler_state");
				WriteLine("{");
				indent++;
				foreach (var state in variable.SamplerStates)
				{
					WriteIndent();
					WriteLine("{0} = {1};", state.Type, state.Value.Data[0]);
				}
				indent--;
				WriteLine("};");
			} else
			{
				if (variable.DefaultValue.All(d => d.UInt == 0))
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
		public void WriteTechnique(Technique technique)
		{
			WriteLine("technique {0}", technique.Name);
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
			WriteLine("pass {0}", pass.Name);
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
			WriteLine("{0} = {1};", assignment.Type.ToString().ToLower(), assignment.Value.Data[0]);
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
