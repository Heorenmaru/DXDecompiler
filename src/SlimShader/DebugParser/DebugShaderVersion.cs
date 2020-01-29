using SlimShader.Chunks.Common;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugShaderVersion
	{
		public byte MajorVersion { get; private set; }
		public byte MinorVersion { get; private set; }
		public ProgramType ProgramType { get; private set; }

		internal static DebugShaderVersion ParseRdef(DebugBytecodeReader reader)
		{
			byte minorVersion = reader.ReadByte("majorVersion");
			byte majorVersion = reader.ReadByte("minorVersion");
			ushort programTypeValue = reader.ReadUInt16("programTypeValue");
			ProgramType programType = (ProgramType)programTypeValue;
			switch (programTypeValue)
			{
				case 0xFFFF:
					programType = ProgramType.PixelShader;
					break;
				case 0xFFFE:
					programType = ProgramType.VertexShader;
					break;
				case 0x4853:
					programType = ProgramType.HullShader;
					break;
				case 0x4753:
					programType = ProgramType.GeometryShader;
					break;
				case 0x4453:
					programType = ProgramType.DomainShader;
					break;
				case 0x4353:
					programType = ProgramType.ComputeShader;
					break;
				case 0x4C46:
					programType = ProgramType.LibraryShader;
					break;
				default:
					throw new ParseException(string.Format("Unknown program type: 0x{0:X}", programTypeValue));
			}
			return new DebugShaderVersion
			{
				MajorVersion = majorVersion,
				MinorVersion = minorVersion,
				ProgramType = programType
			};
		}

		public static DebugShaderVersion FromShexToken(uint versionToken)
		{
			return new DebugShaderVersion
			{
				MinorVersion = versionToken.DecodeValue<byte>(0, 3),
				MajorVersion = versionToken.DecodeValue<byte>(4, 7),
				ProgramType = versionToken.DecodeValue<ProgramType>(16, 31)
			};
		}
		public static DebugShaderVersion ParseShex(DebugBytecodeReader reader)
		{
			uint versionToken = reader.ReadUInt32("Version");
			return FromShexToken(versionToken);
		}

		public static DebugShaderVersion ParseAon9(DebugBytecodeReader reader)
		{
			byte minor = reader.ReadByte("minorVersion");
			byte major = reader.ReadByte("majorVersion");
			ushort shaderType = reader.ReadUInt16("programType");
			return new DebugShaderVersion
			{
				MinorVersion = minor,
				MajorVersion = major,
				ProgramType = shaderType == 0xFFFF ? ProgramType.PixelShader : ProgramType.VertexShader
			};
		}
	}
}
