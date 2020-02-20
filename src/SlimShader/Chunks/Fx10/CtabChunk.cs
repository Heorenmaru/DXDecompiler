using SlimShader.DX9Shader;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
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
		byte[] Data;
		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
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
			var ct = result.ConstantTable = new ConstantTable(creatorString, shaderModel, (int)majorVersion, (int)minorVersion, constantDeclarations);
			result.Data = reader.ReadBytes((int)chunkSize);
			return result;
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
			System.Diagnostics.Debug.Assert(structMemberInfoOffset == 0);

			return new ConstantDeclaration(name, registerSet, (short)registerIndex, (short)registerCount, cl, type, rows, columns, numElements, defaultValue);
		}
		public static string FormatReadable(byte[] data, bool endian = false)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
				for (int j = i; j < i + 16; j++)
				{
					var index = endian ? j : j + (3 - (j % 4) * 2);
					if (index < data.Length)
					{
						sb.Append(data[index].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append("  ");
					}
				}
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (char.IsControl(c))
					{
						sb.Append("_");
					}
					else if (c > 0x7E)
					{
						sb.Append('.');
					}
					else if (char.IsWhiteSpace(c))
					{
						sb.Append('.');
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
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
			sb.AppendLine(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
