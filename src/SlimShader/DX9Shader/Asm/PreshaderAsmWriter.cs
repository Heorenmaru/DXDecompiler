using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Asm
{
	public class PreshaderAsmWriter : DecompileWriter
	{
		public ShaderModel Preshader;
		public PreshaderAsmWriter(ShaderModel preshader)
		{
			Preshader = preshader;
		}
		public static string Disassemble(ShaderModel preshader)
		{
			var asmWriter = new PreshaderAsmWriter(preshader);
			return asmWriter.Decompile();
		}
		protected override void Write()
		{
			WriteLine("preshader");
		}
	}
}
