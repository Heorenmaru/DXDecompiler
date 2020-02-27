using SlimShader.DX9Shader.FX9;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class EffectAsmWriter
	{
		FX9.Fx9Chunk EffectChunk;
		StreamWriter asmWriter;
		int indent = 0;
		public EffectAsmWriter(FX9.Fx9Chunk effectChunk)
		{
			EffectChunk = effectChunk;
		}
		public static string Disassemble(FX9.Fx9Chunk effectChunk)
		{
			var asmWriter = new EffectAsmWriter(effectChunk);
			using (var stream = new MemoryStream())
			{
				asmWriter.Write(stream);
				asmWriter.asmWriter.Flush();
				stream.Position = 0;
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}
		public void Write(Stream stream)
		{
			asmWriter = new StreamWriter(stream);
			WriteLine("//listing of all techniques and passes with embedded asm listings");
			WriteLine();
			foreach(var technique in EffectChunk.Techniques)
			{
				WriteLine("technique {0}", technique.Name);
				WriteLine("{");
				indent++;
				foreach(var pass in technique.Passes)
				{
					WritePass(pass);
				}
				indent--;
				WriteLine("}");
				WriteLine("");
			}
		}
		public void WritePass(Pass pass)
		{
			WriteIndent();
			WriteLine("pass {0}", pass.Name);
			WriteIndent();
			WriteLine("{");
			indent++;
			var shaderAssignments = pass.Assignments
				.Where(a => a.Type == StateType.VertexShader || a.Type == StateType.PixelShader);
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
			WriteLine("{0} = ", assignment.Type.ToString().ToLower());
			indent++;
			WriteIndent();
			WriteLine("asm {");

			WriteIndent();
			WriteLine("};");
			indent--;
		}
		public void WriteIndent()
		{
			asmWriter.Write(new string(' ', indent * 4));
		}
		void WriteLine()
		{
			asmWriter.WriteLine();
		}

		void WriteLine(string value)
		{
			asmWriter.WriteLine(value);
		}

		void WriteLine(string format, params object[] args)
		{
			asmWriter.WriteLine(format, args);
		}
	}
}
