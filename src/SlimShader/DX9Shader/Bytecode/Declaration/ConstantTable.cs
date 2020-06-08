using SlimShader.Util;
using System.Collections.Generic;

namespace SlimShader.DX9Shader.Bytecode.Declaration
{
	public class ConstantTable
	{
		public string Creator { get; private set; }
		public string ShaderModel { get; private set; }
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public List<ConstantDeclaration> ConstantDeclarations { get; private set; }
		public ConstantTable(string creator, string shaderModel, int majorVersion, int minorVersion, List<ConstantDeclaration> constantDeclarations)
		{
			Creator = creator;
			ShaderModel = shaderModel;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			ConstantDeclarations = constantDeclarations;
		}
		private ConstantTable()
		{
			ConstantDeclarations = new List<ConstantDeclaration>();
		}
		public static ConstantTable Parse(BytecodeReader ctabReader)
		{
			var result = new ConstantTable();
			var ctabSize = ctabReader.ReadUInt32();
			var creatorOffset = ctabReader.ReadInt32();
			var minorVersion = ctabReader.ReadByte();
			var majorVersion = ctabReader.ReadByte();
			var shaderType = (ShaderType)ctabReader.ReadUInt16();
			var numConstants = ctabReader.ReadInt32();
			var constantInfoOffset = ctabReader.ReadInt32();
			var shaderFlags = (ShaderFlags)ctabReader.ReadUInt32();
			var shaderModelOffset = ctabReader.ReadInt32();

			for (int i = 0; i < numConstants; i++)
			{
				var decReader = ctabReader.CopyAtOffset(constantInfoOffset + i * 20);
				ConstantDeclaration declaration = ConstantDeclaration.Parse(ctabReader, decReader);
				result.ConstantDeclarations.Add(declaration);
			}

			var shaderModelReader = ctabReader.CopyAtOffset(shaderModelOffset);
			var shaderModel = shaderModelReader.ReadString();

			var creatorReader = ctabReader.CopyAtOffset(creatorOffset);
			var creatorString = creatorReader.ReadString();
			return result;
		}
	}
}
