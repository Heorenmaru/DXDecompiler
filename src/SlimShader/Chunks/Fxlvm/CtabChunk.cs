using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	/* 
	* Format
	* uint FourCC
	* uint ChunkSize
	* uint Count
	* uint[] Numbers
	*/
	public class CtabChunk : BytecodeChunk
	{
		public ConstantTable ConstantTable { get; private set; }

		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			//TODO: Merge Ctab parsing with DX9 Ctab
			var result = new CtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			result.ConstantTable = ConstantTable.Parse(chunkReader);
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
			foreach(var decl in ConstantTable.ConstantDeclarations)
			{
				sb.AppendLine($"ConstantDeclarations: {decl}");
			}
			return sb.ToString();
		}
	}
}
