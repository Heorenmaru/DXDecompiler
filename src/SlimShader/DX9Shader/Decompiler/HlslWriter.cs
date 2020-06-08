﻿using SlimShader.DX9Shader.Bytecode.Declaration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class HlslWriter
	{
		private readonly ShaderModel _shader;
		private readonly bool _doAstAnalysis;

		StreamWriter hlslWriter;
		string indent = "";

		public RegisterState _registers;

		public HlslWriter(ShaderModel shader, bool doAstAnalysis = false)
		{
			_shader = shader;
			_doAstAnalysis = doAstAnalysis;
		}

		public static string Decompile(byte[] bytecode)
		{
			var shaderModel = ShaderReader.ReadShader(bytecode);
			return Decompile(shaderModel);
		}
		public static string Decompile(ShaderModel shaderModel)
		{
			if (shaderModel.Type == ShaderType.Fx)
			{
				return EffectHLSLWriter.Decompile(shaderModel.EffectChunk);
			}
			var hlslWriter = new HlslWriter(shaderModel);
			using (var stream = new MemoryStream())
			{
				hlslWriter.Write(stream);
				stream.Position = 0;
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}

		void WriteLine()
		{
			hlslWriter.WriteLine();
		}

		void WriteLine(string value)
		{
			hlslWriter.Write(indent);
			hlslWriter.WriteLine(value);
		}

		void WriteLine(string format, params object[] args)
		{
			hlslWriter.Write(indent);
			hlslWriter.WriteLine(format, args);
		}

		private string GetDestinationName(Token instruction)
		{
			return _registers.GetDestinationName(instruction);
		}

		private string GetSourceName(Token instruction, int srcIndex)
		{
			return _registers.GetSourceName(instruction, srcIndex);
		}

		private static string GetConstantTypeName(ConstantDeclaration declaration)
		{
			switch (declaration.ParameterClass)
			{
				case ParameterClass.Scalar:
					return declaration.ParameterType.ToString().ToLower();
				case ParameterClass.Vector:
					if (declaration.ParameterType == ParameterType.Float)
					{
						return "float" + declaration.Columns;
					}
					else
					{
						throw new NotImplementedException();
					}
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					if (declaration.ParameterType == ParameterType.Float)
					{
						return $"float{declaration.Rows}x{declaration.Columns}";
					}
					else
					{
						throw new NotImplementedException();
					}
				case ParameterClass.Object:
					switch (declaration.ParameterType)
					{
						case ParameterType.Sampler1D:
						case ParameterType.Sampler2D:
						case ParameterType.Sampler3D:
						case ParameterType.SamplerCube:
							return "sampler";
						default:
							throw new NotImplementedException();
					}
			}
			throw new NotImplementedException();
		}

		private void WriteInstruction(Token instruction)
		{
			WriteLine($"// {instruction}");
			switch (instruction.Opcode)
			{
				case Opcode.Abs:
					WriteLine("{0} = abs({1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.Add:
					WriteLine("{0} = {1} + {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Cmp:
					// TODO: should be per-component
					WriteLine("{0} = ({1} >= 0) ? {2} : {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.DP2Add:
					WriteLine("{0} = dot({1}, {2}) + {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Dp3:
					WriteLine("{0} = dot({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Dp4:
					WriteLine("{0} = dot({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Else:
					indent = indent.Substring(0, indent.Length - 1);
					WriteLine("} else {");
					indent += "\t";
					break;
				case Opcode.Endif:
					indent = indent.Substring(0, indent.Length - 1);
					WriteLine("}");
					break;
				case Opcode.Exp:
					WriteLine("{0} = exp2({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Frc:
					WriteLine("{0} = frac({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.If:
					WriteLine("if ({0}) {{", GetSourceName(instruction, 0));
					indent += "\t";
					break;
				case Opcode.IfC:
					if ((IfComparison)instruction.Modifier == IfComparison.GE &&
						instruction.GetSourceModifier(0) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(1) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0) ==
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1))
					{
						WriteLine("if ({0} == 0) {{", instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0));
					}
					else if ((IfComparison)instruction.Modifier == IfComparison.LT &&
						instruction.GetSourceModifier(0) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(1) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0) ==
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1))
					{
						WriteLine("if ({0} != 0) {{", instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0));
					}
					else
					{
						string ifComparison;
						switch ((IfComparison)instruction.Modifier)
						{
							case IfComparison.GT:
								ifComparison = ">";
								break;
							case IfComparison.EQ:
								ifComparison = "==";
								break;
							case IfComparison.GE:
								ifComparison = ">=";
								break;
							case IfComparison.LE:
								ifComparison = "<=";
								break;
							case IfComparison.NE:
								ifComparison = "!=";
								break;
							case IfComparison.LT:
								ifComparison = "<";
								break;
							default:
								throw new InvalidOperationException();
						}
						WriteLine("if ({0} {2} {1}) {{", GetSourceName(instruction, 0), GetSourceName(instruction, 1), ifComparison);
					}
					indent += "\t";
					break;
				case Opcode.Log:
					WriteLine("{0} = log2({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Lrp:
					WriteLine("{0} = lerp({2}, {3}, {1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Mad:
					WriteLine("{0} = {1} * {2} + {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Max:
					WriteLine("{0} = max({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Min:
					WriteLine("{0} = min({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Mov:
					WriteLine("{0} = {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.MovA:
					WriteLine("{0} = {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Mul:
					WriteLine("{0} = {1} * {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Nrm:
					WriteLine("{0} = normalize({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Pow:
					WriteLine("{0} = pow({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Rcp:
					WriteLine("{0} = 1 / {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Rsq:
					WriteLine("{0} = 1 / sqrt({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sge:
					if (instruction.GetSourceModifier(1) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(2) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1) ==
						instruction.GetParamRegisterName(2) + instruction.GetSourceSwizzleName(2))
					{
						WriteLine("{0} = ({1} == 0) ? 1 : 0;", GetDestinationName(instruction),
							instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1));
					}
					else
					{
						WriteLine("{0} = ({1} >= {2}) ? 1 : 0;", GetDestinationName(instruction), GetSourceName(instruction, 1),
							GetSourceName(instruction, 2));
					}
					break;
				case Opcode.Slt:
					WriteLine("{0} = ({1} < {2}) ? 1 : 0;", GetDestinationName(instruction), GetSourceName(instruction, 1),
						GetSourceName(instruction, 2));
					break;
				case Opcode.SinCos:
					WriteLine("sincos({1}, {0}, {0});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sub:
					WriteLine("{0} = {1} - {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Tex:
					if ((_shader.MajorVersion == 1 && _shader.MinorVersion >= 4) || (_shader.MajorVersion > 1))
					{
						WriteLine("{0} = tex2D({2}, {1});", GetDestinationName(instruction),
							GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					}
					else
					{
						WriteLine("{0} = tex2D();", GetDestinationName(instruction));
					}
					break;
				case Opcode.TexLDL:
					WriteLine("{0} = tex2Dlod({2}, {1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Comment:
				case Opcode.End:
					break;
			}
		}
		void WriteTemps()
		{
			Dictionary<RegisterKey, int> tempRegisters = new Dictionary<RegisterKey, int>();
			foreach(var inst in _shader.Instructions)
			{
				foreach(var operand in inst.Operands)
				{
					if(operand is DestinationOperand dest)
					{
						if(dest.RegisterType == RegisterType.Temp)
						{

							var registerKey = new RegisterKey(dest.RegisterType, (int)dest.RegisterNumber);
							if (!tempRegisters.ContainsKey(registerKey))
							{
								var reg = new RegisterDeclaration(registerKey);
								_registers._registerDeclarations[registerKey] = reg;
								tempRegisters[registerKey] = (int)inst.GetDestinationWriteMask();
							}
							else
							{
								tempRegisters[registerKey] |= (int)inst.GetDestinationWriteMask();
							}
						}
					}
				}
			}
			if (tempRegisters.Count == 0) return;
			foreach(IGrouping<int, RegisterKey> group in tempRegisters.GroupBy(
				kv => kv.Value,
				kv => kv.Key))
			{
				int writeMask = group.Key;
				string writeMaskName; switch (writeMask)
				{
					case 0x1:
						writeMaskName = "float";
						break;
					case 0x3:
						writeMaskName = "float2";
						break;
					case 0x7:
						writeMaskName = "float3";
						break;
					case 0xF:
						writeMaskName = "float4";
						break;
					default:
						// TODO
						writeMaskName = "float4";
						break;
						//throw new NotImplementedException();
				}
				WriteLine("{0} {1};", writeMaskName, string.Join(", ", group));
			}
			

		}
		public void Write(Stream stream)
		{
			hlslWriter = new StreamWriter(stream);

			_registers = new RegisterState(_shader);

			WriteConstantDeclarations();

			if (_registers.MethodInputRegisters.Count > 1)
			{
				WriteInputStructureDeclaration();
			}

			if (_registers.MethodOutputRegisters.Count > 1)
			{
				WriteOutputStructureDeclaration();
			}

			string methodReturnType = GetMethodReturnType();
			string methodParameters = GetMethodParameters();
			string methodSemantic = GetMethodSemantic();
			WriteLine("{0} main({1}){2}", methodReturnType, methodParameters, methodSemantic);
			WriteLine("{");
			indent = "\t";

			if (_registers.MethodOutputRegisters.Count > 1)
			{
				var outputStructType = _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
				WriteLine($"{outputStructType} o;");
				WriteLine();
			} else
			{
				var output = _registers.MethodOutputRegisters.First().Value;
				WriteLine("{0} {1};", methodReturnType, _registers.GetRegisterName(output.RegisterKey));
				WriteLine();
			}
			WriteTemps();
			HlslAst ast = null;
			if (_doAstAnalysis)
			{
				var parser = new BytecodeParser();
				ast = parser.Parse(_shader);
				ast.ReduceTree();

				WriteAst(ast);
			}
			else
			{
				WriteInstructionList();

				if (_registers.MethodOutputRegisters.Count > 1)
				{
					WriteLine($"return o;");
				}
				else
				{
					var output = _registers.MethodOutputRegisters.First().Value;
					WriteLine($"return {_registers.GetRegisterName(output.RegisterKey)};");
				}
				
			}
			indent = "";
			WriteLine("}");
			hlslWriter.Flush();
		}

		private void WriteConstantDeclarations()
		{
			if (_registers.ConstantDeclarations.Count != 0)
			{
				foreach (ConstantDeclaration declaration in _registers.ConstantDeclarations)
				{
					try
					{
						string typeName = GetConstantTypeName(declaration);
						WriteLine("{0} {1};", typeName, declaration.Name);
					} catch(Exception ex)
					{
						WriteLine("{0} {1};", $"Error", declaration.Name);;
					}
				}

				WriteLine();
			}
		}

		private void WriteInputStructureDeclaration()
		{
			var inputStructType = _shader.Type == ShaderType.Pixel ? "PS_IN" : "VS_IN";
			WriteLine($"struct {inputStructType}");
			WriteLine("{");
			indent = "\t";
			foreach (var input in _registers.MethodInputRegisters.Values)
			{
				WriteLine($"{input.TypeName} {input.Name} : {input.Semantic};");
			}
			indent = "";
			WriteLine("};");
			WriteLine();
		}

		private void WriteOutputStructureDeclaration()
		{
			var outputStructType = _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
			WriteLine($"struct {outputStructType}");
			WriteLine("{");
			indent = "\t";
			foreach (var output in _registers.MethodOutputRegisters.Values)
			{
				WriteLine($"// {output.RegisterKey} {Operand.GetParamRegisterName(output.RegisterKey.Type, output.RegisterKey.Number)}");
				WriteLine($"{output.TypeName} {output.Name} : {output.Semantic};");
			}
			indent = "";
			WriteLine("};");
			WriteLine();
		}

		private string GetMethodReturnType()
		{
			switch (_registers.MethodOutputRegisters.Count)
			{
				case 0:
					throw new InvalidOperationException();
				case 1:
					return _registers.MethodOutputRegisters.Values.First().TypeName;
				default:
					return _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
			}
		}

		private string GetMethodSemantic()
		{
			switch (_registers.MethodOutputRegisters.Count)
			{
				case 0:
					throw new InvalidOperationException();
				case 1:
					string semantic = _registers.MethodOutputRegisters.Values.First().Semantic;
					return $" : {semantic}";
				default:
					return string.Empty;
			}
		}

		private string GetMethodParameters()
		{
			if (_registers.MethodInputRegisters.Count == 0)
			{
				return string.Empty;
			}
			else if (_registers.MethodInputRegisters.Count == 1)
			{
				var input = _registers.MethodInputRegisters.Values.First();
				return $"{input.TypeName} {input.Name} : {input.Semantic}";
			}

			return _shader.Type == ShaderType.Pixel
					? "PS_IN i"
					: "VS_IN i";
		}

		private void WriteAst(HlslAst ast)
		{
			var compiler = new NodeCompiler(_registers);

			var rootGroups = ast.Roots.GroupBy(r => r.Key.RegisterKey);
			if (_registers.MethodOutputRegisters.Count == 1)
			{
				var rootGroup = rootGroups.Single();
				var registerKey = rootGroup.Key;
				var roots = rootGroup.OrderBy(r => r.Key.ComponentIndex).Select(r => r.Value).ToList();
				string statement = compiler.Compile(roots, 4);

				WriteLine($"return {statement};");
			}
			else
			{
				foreach (var rootGroup in rootGroups)
				{
					var registerKey = rootGroup.Key;
					var roots = rootGroup.OrderBy(r => r.Key.ComponentIndex).Select(r => r.Value).ToList();
					RegisterDeclaration outputRegister = _registers.MethodOutputRegisters[registerKey];
					string statement = compiler.Compile(roots, roots.Count);

					WriteLine($"o.{outputRegister.Name} = {statement};");
				}

				WriteLine();
				WriteLine($"return o;");
			}
		}

		private void WriteInstructionList()
		{

			foreach (Token instruction in _shader.Tokens)
			{
				WriteInstruction(instruction);
			}

			WriteLine();

		}
	}
}
