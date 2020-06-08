using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	public class CtabChunk : BytecodeChunk
	{
		public uint MinorVersion { get; private set; }
		public uint MajorVersion { get; private set; }
		public uint ShaderType { get; private set; }

		public ShaderFlags ShaderFlags { get; private set; }
		public string CreatorString { get; private set; }
		public string ShaderModel { get; private set; }
		public ConstantTable ConstantTable { get; private set; }

		public uint numConstants;
		public uint creatorPosition;

		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			//TODO: Merge Ctab parsing with DX9 Ctab
			var result = new CtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var size = result.creatorPosition = chunkReader.ReadUInt32();
			var creatorPosition = result.creatorPosition = chunkReader.ReadUInt32();
			var minorVersion = result.MinorVersion = chunkReader.ReadByte();
			var majorVersion = result.MajorVersion = chunkReader.ReadByte();
			var shaderType = result.ShaderType = chunkReader.ReadUInt16();
			var numConstants = result.numConstants = chunkReader.ReadUInt32();
			var constantInfoPosition = chunkReader.ReadUInt32();
			var shaderFlags = chunkReader.ReadUInt32();
			var shaderModelPosition = chunkReader.ReadUInt32();

			var creatorReader = reader.CopyAtOffset((int)creatorPosition);
			var creatorString = result.CreatorString = creatorReader.ReadString();

			var shaderModelReader = reader.CopyAtOffset((int)shaderModelPosition);
			var shaderModel = result.ShaderModel = shaderModelReader.ReadString();

			var constantDeclarations = new List<ConstantDeclaration>((int)numConstants);
			for(int i = 0; i < numConstants; i++)
			{
				ConstantDeclaration declaration = ReadConstantDeclaration(reader, chunkReader);
				constantDeclarations.Add(declaration);
			}
			result.ConstantTable = new ConstantTable(creatorString, shaderModel, (int)majorVersion, (int)minorVersion, constantDeclarations);
			return result;
		}

		public string GetVariable(uint elementIndex)
		{
			var decl = ConstantTable.ConstantDeclarations
				.FirstOrDefault((v) => v.RegisterIndex == elementIndex);
			if(decl == null)
			{
				return string.Format("var{0}", elementIndex);
			}
			return decl.Name;
		}

		private static ConstantDeclaration ReadConstantDeclaration(BytecodeReader reader, BytecodeReader ctabReader)
		{
			// D3DXSHADER_CONSTANTINFO
			var nameOffset = ctabReader.ReadUInt32();
			var registerSet = (RegisterSet)ctabReader.ReadUInt16();
			var registerIndex = ctabReader.ReadUInt16();
			var registerCount = ctabReader.ReadUInt16();
			ctabReader.ReadUInt16(); //Reserved
			var typeInfoOffset = ctabReader.ReadUInt32();
			var defaultValueOffset = ctabReader.ReadUInt32();
			List<float> defaultValue = new List<float>(defaultValueOffset != 0 ? 4 : 0);
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			var name = nameReader.ReadString();
			if (defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset((int)defaultValueOffset);
				for (int i = 0; i < 4; i++)
				{
					defaultValue.Add(defaultValueReader.ReadSingle());
				}
			}
			// D3DXSHADER_TYPEINFO
			var typeInfoReader = reader.CopyAtOffset((int)typeInfoOffset);
			var cl = (ParameterClass)typeInfoReader.ReadUInt16();
			var type = (ParameterType)typeInfoReader.ReadUInt16();
			var rows = typeInfoReader.ReadUInt16();
			var columns = typeInfoReader.ReadUInt16();
			var numElements = typeInfoReader.ReadUInt16();
			var numStructMembers = typeInfoReader.ReadUInt16();
			var structMemberInfoOffset = typeInfoReader.ReadUInt32();
			//System.Diagnostics.Debug.Assert(numElements == 1);
			//System.Diagnostics.Debug.Assert(structMemberInfoOffset == 0);

			return new ConstantDeclaration(name, registerSet, (short)registerIndex, (short)registerCount, cl, type, rows, columns, numElements, defaultValue);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetType().Name);
			sb.AppendLine($"CreatorPosition: {creatorPosition} {creatorPosition.ToString("X4")}");
			sb.AppendLine($"MinorVersion: {MinorVersion} {MinorVersion.ToString("X2")}");
			sb.AppendLine($"MajorVersion: {MajorVersion} {MajorVersion.ToString("X2")}");
			sb.AppendLine($"ShaderType: {ShaderType} {ShaderType.ToString("X2")}");
			sb.AppendLine($"numConstants: {numConstants} {numConstants.ToString("X2")}");
			sb.AppendLine($"CreatorString: {CreatorString}");
			sb.AppendLine($"ShaderModel: {ShaderModel}");
			foreach(var decl in ConstantTable.ConstantDeclarations)
			{
				sb.AppendLine($"ConstantDeclarations: {decl}");
			}
			return sb.ToString();
		}
	}
}
