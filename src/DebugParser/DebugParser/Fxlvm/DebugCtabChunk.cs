using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugCtabChunk : DebugBytecodeChunk
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

		public static DebugBytecodeChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			//TODO: Merge Ctab parsing with DX9 Ctab
			var result = new DebugCtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader, (int)chunkSize);
			var size = chunkReader.ReadUInt32("Size");
			var creatorPosition = result.creatorPosition = chunkReader.ReadUInt32("CreatorPosition");
			var minorVersion = result.MinorVersion = chunkReader.ReadByte("MinorVersion");
			var majorVersion = result.MajorVersion = chunkReader.ReadByte("MajorVersion");
			var shaderType = result.ShaderType = chunkReader.ReadUInt16("ShaderType");
			var numConstants = result.numConstants = chunkReader.ReadUInt32("NumConstants");
			var constantInfoPosition = chunkReader.ReadUInt32("ConstantInfoPosition");
			var shaderFlags = chunkReader.ReadUInt32("ShaderFlags");
			var shaderModelPosition = chunkReader.ReadUInt32("ShaderModelPosition");

			var creatorReader = reader.CopyAtOffset("CreatorReader", chunkReader, (int)creatorPosition);
			var creatorString = result.CreatorString = creatorReader.ReadString("CreatorString");

			var shaderModelReader = reader.CopyAtOffset("ShaderModelReader", chunkReader, (int)shaderModelPosition);
			var shaderModel = result.ShaderModel = shaderModelReader.ReadString("ShaderModel");

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

		private static ConstantDeclaration ReadConstantDeclaration(DebugBytecodeReader reader, DebugBytecodeReader ctabReader)
		{
			// D3DXSHADER_CONSTANTINFO
			var nameOffset = ctabReader.ReadUInt32("NameOffset");
			var registerSet = ctabReader.ReadEnum16<RegisterSet>("RegisterSet");
			var registerIndex = ctabReader.ReadUInt16("RegisterIndex");
			var registerCount = ctabReader.ReadUInt16("RegisterCount");
			ctabReader.ReadUInt16("Reserved"); //Reserved
			var typeInfoOffset = ctabReader.ReadUInt32("TypeInfoOffset");
			var defaultValueOffset = ctabReader.ReadUInt32("DefaultValueOffset");
			List<float> defaultValue = new List<float>(defaultValueOffset != 0 ? 4 : 0);
			var nameReader = reader.CopyAtOffset("NameReader", ctabReader, (int)nameOffset);
			var name = nameReader.ReadString("Name");
			if (defaultValueOffset != 0)
			{
				//Note: there are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset("DefaultValueReader", ctabReader, (int)defaultValueOffset);
				for (int i = 0; i < 4; i++)
				{
					defaultValue.Add(defaultValueReader.ReadSingle($"DefaultValue{i}"));
				}
			}
			// D3DXSHADER_TYPEINFO
			var typeInfoReader = reader.CopyAtOffset("TypeInfoReader", ctabReader, (int)typeInfoOffset);
			var cl = (ParameterClass)typeInfoReader.ReadUInt16("CL");
			var type = (ParameterType)typeInfoReader.ReadUInt16("Type");
			var rows = typeInfoReader.ReadUInt16("Rows");
			var columns = typeInfoReader.ReadUInt16("Columns");
			var numElements = typeInfoReader.ReadUInt16("NumElements");
			var numStructMembers = typeInfoReader.ReadUInt16("NumStructMembers");
			var structMemberInfoOffset = typeInfoReader.ReadUInt32("StructMemberInfoOffset");
			//System.Diagnostics.Debug.Assert(numElements == 1);
			//System.Diagnostics.Debug.Assert(structMemberInfoOffset == 0);

			return new ConstantDeclaration(name, registerSet, (short)registerIndex, (short)registerCount, cl, type, rows, columns, numElements, defaultValue);
		}

	}
}
