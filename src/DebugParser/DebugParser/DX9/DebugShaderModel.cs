using DebugParser.DebugParser.DX9;
using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;

namespace SlimShader.DebugParser.DX9
{
	public class DebugShaderModel
	{
		private static readonly Dictionary<uint, CommentType> KnownCommentTypes =
			new Dictionary<uint, CommentType>
		{
			{ "CTAB".ToFourCc(), CommentType.CTAB },
			{ "CLIT".ToFourCc(), CommentType.CLIT },
			{ "FXLC".ToFourCc(), CommentType.FXLC },
			{ "PRES".ToFourCc(), CommentType.PRES },
			{ "PRSI".ToFourCc(), CommentType.PRSI }
		};
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public ShaderType Type { get; private set; }
		public List<DebugToken> Tokens = new List<DebugToken>();
		public DebugConstantTable ConstantTable;
		public DebugCliToken Cli;
		public DebugPresToken Pres;
		public DebugPrsiToken Prsi;
		public DebugFxlc Fxlc;
		public static DebugShaderModel Parse(DebugBytecodeReader reader)
		{
			var result = new DebugShaderModel();
			result.MinorVersion = reader.ReadByte("MinorVersion");
			result.MajorVersion = reader.ReadByte("MajorVersion");
			result.Type = reader.ReadEnum16<ShaderType>("ShaderType");
			while (true)
			{
				var token = reader.PeakUint32();
				Opcode opcode = (Opcode)(token & 0xffff);
				if (opcode == Opcode.Comment && result.ReadCommentToken(reader))
				{
					continue;
				}
				reader.AddIndent($"Token {result.Tokens.Count}");

				var indent = reader.Members.OfType<DebugIndent>().Last();
				DebugToken instruction = result.ReadInstruction(reader);
				result.Tokens.Add(instruction);
				indent.Name += $" {instruction.Opcode}";
				reader.RemoveIndent();
				if (instruction.Opcode == Opcode.End) break;
			}
			return result;
		}
		bool ReadCommentToken(DebugBytecodeReader reader)
		{
			var fourCC = reader.PeakUInt32Ahead(4);
			if(KnownCommentTypes.ContainsKey(fourCC)){
				reader.AddIndent(KnownCommentTypes[fourCC].ToString());
			} else
			{
				return false;
			}
			var instructionToken = reader.ReadUInt32("Token");
			var startPosition = reader._reader.BaseStream.Position;
			var entry = reader.Members.OfType<DebugEntry>().Last();
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			entry.AddNote("TokenOpcode", opcode.ToString());
			var size = (int)((instructionToken >> 16) & 0x7FFF);
			entry.AddNote("TokenSize", size.ToString());
			reader.ReadBytes("FourCC", 4);

			switch (KnownCommentTypes[fourCC])
			{
				case CommentType.CTAB:
					ConstantTable = DebugConstantTable.Parse(reader);
					break;
				case CommentType.CLIT:
					Cli = DebugCliToken.Parse(reader);
					break;
				case CommentType.FXLC:
					Fxlc = DebugFxlc.Parse(reader);
					break;
				case CommentType.PRES:
					Pres = DebugPresToken.Parse(reader);
					break;
				case CommentType.PRSI:
					Prsi = DebugPrsiToken.Parse(reader, (uint)size);
					break;
				default:
					return false;
			}
			reader.RemoveIndent();
			reader._reader.BaseStream.Position = startPosition + size * 4;
			return true;
		}
		DebugToken ReadInstruction(DebugBytecodeReader reader)
		{
			uint instructionToken = reader.ReadUInt32("Token");
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			int size;
			if (opcode == Opcode.Comment)
			{
				size = (int)((instructionToken >> 16) & 0x7FFF);
			}
			else
			{
				size = (int)((instructionToken >> 24) & 0x0f);
			}
			var entry = reader.Members.OfType<DebugEntry>().Last();
			entry.AddNote("TokenOpcode", opcode.ToString());
			entry.AddNote("TokenSize", size.ToString());
			DebugToken token = new DebugToken();
			token.Token = instructionToken;
			token.Opcode = opcode;
			if (opcode == Opcode.Comment)
			{
				token.Data = reader.ReadBytes("CommentData", size * 4);
			} else
			{
				token.Data = reader.ReadBytes("InstructionData", size * 4);
			}
			return token;
		}
	}
}
