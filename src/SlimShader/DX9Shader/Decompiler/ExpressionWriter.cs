using SlimShader.DX9Shader.Bytecode;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Decompiler
{
	class ExpressionHLSLWriter
	{
		ShaderModel Shader;
		ConstantTable Ctab;
		CliToken Cli;
		StreamWriter writer;
		string ExpressionName;
		int indent = 0;
		public ExpressionHLSLWriter(ShaderModel shader, string expressionName)
		{
			Shader = shader;
			ExpressionName = expressionName;
			Ctab = shader.ConstantTable;
			Cli = shader.Cli;
		}
		public static string Decompile(ShaderModel shader, string expressionName = "Expression")
		{
			var asmWriter = new ExpressionHLSLWriter(shader, expressionName);
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
			WriteLine($"float4 {ExpressionName}()");
			WriteLine("{");
			indent++;
			WriteIndent();
			WriteLine("float4 expr;");
			foreach(var token in Shader.Fxlc.Tokens)
			{
				Write(token);
			}
			WriteIndent();
			WriteLine("return expr;");
			indent--;
			WriteLine("}");

			if(Shader.Preshader != null)
			{
				Write("Have Pres");
			}
		}

		void Write(FxlcToken token)
		{
			WriteIndent();
			WriteLine($"// {token.ToString(Shader.ConstantTable, Shader.Cli)}");
			switch (token.Type)
			{
				case FxlcTokenType.Mov:
					WriteIndent();
					WriteLine("{0} = {1};",
						token.Operands[0].ToString(Ctab, Cli),
						token.Operands[1].ToString(Ctab, Cli));
					break;
				case FxlcTokenType.Neg:
					WriteIndent();
					WriteLine("{0} = -{1};",
						token.Operands[0].ToString(Ctab, Cli),
						token.Operands[1].ToString(Ctab, Cli));
					break;
				case FxlcTokenType.Frc:
					WriteFunction("frac", token);
					break;
				case FxlcTokenType.Exp:
					WriteFunction("exp", token);
					break;
				case FxlcTokenType.Log:
					WriteFunction("log", token);
					break;
				case FxlcTokenType.Rsq:
					WriteFunction("rsq", token);
					break;
				case FxlcTokenType.Sin:
					WriteFunction("sin", token);
					break;
				case FxlcTokenType.Cos:
					WriteFunction("cos", token);
					break;
				case FxlcTokenType.Asin:
					WriteFunction("asin", token);
					break;
				case FxlcTokenType.Acos:
					WriteFunction("acos", token);
					break;
				case FxlcTokenType.Atan:
					WriteFunction("atam", token);
					break;
				case FxlcTokenType.Atan2:
					WriteFunction("atan2", token);
					break;
				case FxlcTokenType.Sqrt:
					WriteFunction("sqrt", token);
					break;
				case FxlcTokenType.Ineg:
					WriteFunction("~int", token);
					break;
				case FxlcTokenType.Imax:
					WriteFunction("(int)max(", token);
					break;
				case FxlcTokenType.Not:
					WriteFunction("!", token);
					break;
				case FxlcTokenType.Utof:
					WriteFunction("utof", token);
					break;
				case FxlcTokenType.Ftoi:
					WriteFunction("ftoi", token);
					break;
				case FxlcTokenType.Ftou:
					WriteFunction("ftou", token);
					break;
				case FxlcTokenType.Btoi:
					WriteFunction("btoi", token);
					break;
				case FxlcTokenType.Round:
					WriteFunction("round", token);
					break;
				case FxlcTokenType.Floor:
					WriteFunction("floor", token);
					break;
				case FxlcTokenType.Ceil:
					WriteFunction("ceil", token);
					break;
				case FxlcTokenType.Min:
					WriteFunction("min", token);
					break;
				case FxlcTokenType.Max:
					WriteFunction("max", token);
					break;
				case FxlcTokenType.Add:
					WriteInfix("+", token);
					break;
				case FxlcTokenType.Mul:
					WriteInfix("*", token);
					break;


				case FxlcTokenType.Lt:
					WriteInfix("<", token);
					break;
			}
		}
		void WriteInfix(string op, FxlcToken token)
		{
			WriteIndent();
			WriteLine("{0} = {1} {2} {3};",
				token.Operands[0].ToString(Ctab, Cli),
				token.Operands[1].ToString(Ctab, Cli),
				op,
				token.Operands[2].ToString(Ctab, Cli));
		}
		void WriteFunction(string func, FxlcToken token)
		{
			WriteIndent();
			var operands = token.Operands
				.Skip(1)
				.Select(o => o.ToString(Ctab, Cli));
			WriteLine ("{0} = {1}({2});",
				token.Operands[0].ToString(Ctab, Cli),
				func,
				string.Join(", ", operands));
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
