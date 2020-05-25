﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Xsgn;
using SlimShader.DebugParser.FX9;
using SlimShader.Decompiler;
using SlimShader.DX9Shader;
using SlimShader.Tests.Util;

namespace SlimShader.Tests
{
	[TestFixture]
	public class DX9Tests
	{
		private static string OutputDir => $@"{TestContext.CurrentContext.TestDirectory}/ShadersDX9";
		private static string ShaderDirectory => $"{TestContext.CurrentContext.TestDirectory}/ShadersDX9";
		[SetUp]
		public void SetUp()
		{
			Directory.CreateDirectory(OutputDir);
			AssertTraceListener.Init();
		}
		private static IEnumerable<string> TestShaders
		{
			get
			{
				string shadersDirectory = ShaderDirectory;
				return Directory.GetFiles(shadersDirectory, "*.o", SearchOption.AllDirectories)
					.Select(x => Path.GetDirectoryName(x) + @"\" + Path.GetFileNameWithoutExtension(x))
					.Select(x => FileUtil.GetRelativePath(x, ShaderDirectory));
			}
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void AsmMatchesFxc(string relPath)
		{
			string file = $"{ShaderDirectory}/{relPath}";
			// Arrange.
			var asmFileText = string.Join(Environment.NewLine,
				File.ReadAllLines(file + ".asm").Select(x => x.Trim()));
			asmFileText = TestUtils.NormalizeAssembly(asmFileText);
			// Act.
			var bytecode = File.ReadAllBytes(file + ".o");
			var shader = ShaderReader.ReadShader(bytecode);

			var decompiledAsm = AsmWriter.Disassemble(shader);

			var decompiledAsmText = string.Join(Environment.NewLine, decompiledAsm
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Select(x => x.Trim()));

			File.WriteAllText($"{file}.d.asm", decompiledAsm);

			// Assert.
			Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void Decompile(string relPath)
		{
			string file = $"{ShaderDirectory}/{relPath}";
			// Arrange.
			// Act.
			var bytecode = File.ReadAllBytes(file + ".o");

			var decompiledHlsl = HlslWriter.Decompile(bytecode);

			File.WriteAllText($"{file}.d.hlsl", decompiledHlsl);

			// Assert.
		}

		[TestCaseSource("TestShaders")]
		public void DumpStructure(string relPath)
		{
			string file = $"{ShaderDirectory}/{relPath}";
			// Arrange.
			// Act.
			var bytecode = File.ReadAllBytes(file + ".o");
			
			if(bytecode[2] == 255 && bytecode[3] == 254)
			{
				var reader = new DebugParser.DebugBytecodeReader(bytecode, 0, bytecode.Length);
				string error = "";
				try
				{
					reader.ReadByte("minorVersion");
					reader.ReadByte("majorVersion");
					reader.ReadUInt16("shaderType");
					DebugFx9Chunk.Parse(reader, (uint)(bytecode.Length - 4));
				} catch(Exception ex)
				{
					error = ex.ToString();
				}
				var dump = reader.DumpStructure();
				if (!string.IsNullOrEmpty(error))
				{
					dump += "\n" + error;
				}
				File.WriteAllText($"{file}.d.txt", dump);

				var dumpHtml = reader.DumpHtml();
				File.WriteAllText($"{file}.d.html", dumpHtml);

			} else
			{
				return;
			}

			

			// Assert.
		}

		public static string GetSourceNameFromObject(string path)
		{
			path = path.Replace(".o", "");
			var dir = Path.GetDirectoryName(path);
			var file = Path.GetFileName(path);
			var parts = file.Split('_');
			for (int i = parts.Length; i > 0; i--)
			{
				file = string.Join("_", parts.Take(i));
				if (File.Exists($"{dir}/{file}")) return file;
				if (File.Exists($"{dir}/{file}.fx")) return $"{file}.fx";
				if (File.Exists($"{dir}/{file}.hlsl")) return $"{file}.hlsl";
			}
			return Path.GetFileName(path);
		}
		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void RecompileShaders(string relPath)
		{
			string file = $"{ShaderDirectory}/{relPath}";
			// Arrange.
			var relDir = Path.GetDirectoryName(relPath);
			Directory.CreateDirectory($"{OutputDir}/{relDir}");
			var sourceName = GetSourceNameFromObject($"{ShaderDirectory}/{relPath}.o");
			if (ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relDir}/{sourceName}", $"{OutputDir}/{relDir}/{sourceName}", true);
			if (ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relPath}.asm", $"{OutputDir}/{relPath}.asm", true);

			var asmFileText = string.Join(Environment.NewLine,
				File.ReadAllLines(file + ".asm").Select(x => x.Trim()));

			// Act.
			var binaryFileBytes = File.ReadAllBytes(file + ".o");
			var shaderModel = ShaderReader.ReadShader(binaryFileBytes);
			var hlslWriter = new HlslWriter(shaderModel);
			string decompiledHLSL = "";
			using (var stream = new MemoryStream())
			{
				hlslWriter.Write(stream);
				stream.Position = 0;
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					decompiledHLSL = reader.ReadToEnd();
				}
			}
			File.WriteAllText($"{OutputDir}/{relPath}.d.hlsl", decompiledHLSL);

			using (var shaderBytecode = ShaderBytecode.FromStream(new MemoryStream(binaryFileBytes)))
			{
				var profile = shaderModel.Type == DX9Shader.ShaderType.Pixel ?
					$"ps_{shaderModel.MajorVersion}_{shaderModel.MinorVersion}" :
					$"vs_{shaderModel.MajorVersion}_{shaderModel.MinorVersion}";
				var compiledShader = ShaderBytecode.Compile(decompiledHLSL, "main", profile);
				var disassembly = shaderBytecode.Disassemble();
				var redisassembly = compiledShader.Bytecode.Disassemble();
				File.WriteAllText($"{OutputDir}/{relPath}.d1.asm", disassembly);
				File.WriteAllText($"{OutputDir}/{relPath}.d2.asm", redisassembly);

				// Assert.
				Warn.If(disassembly, Is.EqualTo(redisassembly));
			}

			// Assert.
			Assert.Pass();
		}
	}
}