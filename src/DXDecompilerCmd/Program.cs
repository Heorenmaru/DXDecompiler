﻿using SlimShader;
using SlimShader.DebugParser;
using SlimShader.DebugParser.FX9;
using SlimShader.Decompiler;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDecompilerCmd
{
	class Program
	{
		public static ProgramType GetProgramType(byte[] data)
		{
			if(data.Length < 4)
			{
				return ProgramType.Unknown;
			}
			var dxbcHeader = BitConverter.ToUInt32(data, 0);
			if(dxbcHeader == "DXBC".ToFourCc())
			{
				return ProgramType.DXBC;
			}
			if (dxbcHeader == 0xFEFF2001)
			{
				return ProgramType.DXBC;
			}
			var dx9ShaderType = (SlimShader.DX9Shader.ShaderType)BitConverter.ToUInt16(data, 2);
			if (dx9ShaderType == SlimShader.DX9Shader.ShaderType.Vertex ||
				dx9ShaderType == SlimShader.DX9Shader.ShaderType.Pixel || 
				dx9ShaderType == SlimShader.DX9Shader.ShaderType.Fx)
			{
				return ProgramType.DX9;
			}
			return ProgramType.Unknown;
		}
		static StreamWriter GetStream(Options options)
		{
			if (string.IsNullOrEmpty(options.DestPath))
			{
				var sw = new StreamWriter(Console.OpenStandardOutput());
				sw.AutoFlush = true;
				Console.SetOut(sw);
				return sw;
			}
			try
			{
				return new StreamWriter(options.DestPath);
			} catch(Exception ex)
			{
				Console.Error.WriteLine("Error creating output file");
				Console.Error.WriteLine(ex);
				Environment.Exit(1);
				return null;
			}
		}
		static void Main(string[] args)
		{
			var options = new Options();
			for(int i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-O":
						if(args.Length <= i + 1)
						{
							Console.Error.WriteLine("No output path specified");
							return;
						}
						options.DestPath = args[i + 1];
						i += 1;
						break;
					case "-a":
						options.Mode = DecompileMode.Dissassemble;
						break;
					case "-d":
						options.Mode = DecompileMode.Debug;
						break;
					default:
						options.SourcePath = args[i];
						break;
				}
			}
			if (string.IsNullOrEmpty(options.SourcePath))
			{
				Console.Error.WriteLine("No source path specified");
				Environment.Exit(1);
			}

			byte[] data = null;
			try
			{
				data = File.ReadAllBytes(options.SourcePath);
			} catch(Exception ex) {
				Console.Error.WriteLine("Error reading source");
				Console.Error.WriteLine(ex);
				Environment.Exit(1);
			}
			var programType = GetProgramType(data);
			using (var sw = GetStream(options))
			{
				sw.WriteLine(string.Join(" ", args));
				if (programType == ProgramType.Unknown)
				{
					Console.Error.WriteLine($"Unable to identify shader object format");
					Environment.Exit(1);
				}
				else if (programType == ProgramType.DXBC)
				{
					if (options.Mode == DecompileMode.Dissassemble)
					{
						var container = new BytecodeContainer(data);
						sw.Write(container.ToString());
					}
					else if (options.Mode == DecompileMode.Decompile)
					{
						var hlsl = DXDecompiler.Decompile(data);
						sw.Write(hlsl);
					}
					else if (options.Mode == DecompileMode.Debug)
					{
						var shaderBytecode = DebugBytecodeContainer.Parse(data);
						var result = shaderBytecode.Dump();
						sw.Write(result);
					}
				}
				else if (programType == ProgramType.DX9)
				{
					if (options.Mode == DecompileMode.Dissassemble)
					{
						var disasm = SlimShader.DX9Shader.AsmWriter.Disassemble(data);
						sw.Write(disasm);
					}
					else if (options.Mode == DecompileMode.Decompile)
					{
						var hlsl = SlimShader.DX9Shader.HlslWriter.Decompile(data);
						sw.Write(hlsl);
					}
					else if (options.Mode == DecompileMode.Debug)
					{
						if (data[2] == 255 && data[3] == 254)
						{
							var reader = new DebugBytecodeReader(data, 0, data.Length);
							string error = "";
							try
							{
								reader.ReadByte("minorVersion");
								reader.ReadByte("majorVersion");
								reader.ReadUInt16("shaderType");
								DebugFx9Chunk.Parse(reader, (uint)(data.Length - 4));
							}
							catch (Exception ex)
							{
								error = ex.ToString();
							}
							var dump = reader.DumpStructure();
							if (!string.IsNullOrEmpty(error))
							{
								dump += "\n" + error;
							}
							sw.Write(dump);
						}
						else
						{
							sw.Write("Format not supported");
						}
					}
				}
			}
		}
	}
}
