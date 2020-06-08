using DebugParser.DebugParser.DX9;
using SlimShader.DX9Shader;
using System.Collections.Generic;
using System.Linq;

namespace SlimShader.DebugParser.DX9
{
	public class DebugShaderModel
	{
		const uint CTAB_FOUR_CC = 0x42415443;
		const uint CLIT_FOUR_CC = 0x54494C43;
		const uint FXLC_FOUR_CC = 0x434C5846;
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public ShaderType Type { get; private set; }
		public List<DebugToken> Tokens = new List<DebugToken>();
		public DebugConstantTable ConstantTable;
		public DebugClit Clit;
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
			switch (fourCC)
			{
				case CTAB_FOUR_CC:
					reader.AddIndent("CTAB");
					break;
				case CLIT_FOUR_CC:
					reader.AddIndent("CLIT");
					break;
				case FXLC_FOUR_CC:
					reader.AddIndent("FXLC");
					break;
				default:
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

			switch (fourCC)
			{
				case CTAB_FOUR_CC:
					ConstantTable = DebugConstantTable.Parse(reader);
					break;
				case CLIT_FOUR_CC:
					Clit = DebugClit.Parse(reader);
					break;
				case FXLC_FOUR_CC:
					Fxlc = DebugFxlc.Parse(reader);
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
