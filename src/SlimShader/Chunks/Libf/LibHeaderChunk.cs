using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Libf
{
	public class LibHeaderChunk : BytecodeChunk
	{
		public byte[] Data;
		public string CreatorString { get; private set; }
		public List<FunctionDesc> FunctionDescs { get; private set; }
		/// <summary>
		/// Only guessing, library chunk never seems to flags set. 
		/// </summary>
		public uint Flags { get; private set; }
		public LibHeaderChunk()
		{
			FunctionDescs = new List<FunctionDesc>();
		}
		public static LibHeaderChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var result = new LibHeaderChunk();

			//multiple creator strings?
			var unknown1 = chunkReader.ReadUInt32();
			Debug.Assert(unknown1 == 1, $"LibraryHeader.unknown1 is {unknown1}");
			var creatorStringOffset = chunkReader.ReadUInt32();
			//flags?
			result.Flags = chunkReader.ReadUInt32();

			var functionCount = chunkReader.ReadUInt32();
			//Contains function strings and function flags
			var functionInfoOffset = chunkReader.ReadUInt32();

			var creatorStringReader = chunkReader.CopyAtOffset((int)creatorStringOffset);
			result.CreatorString = creatorStringReader.ReadString();
			var functionInfoReader = reader.CopyAtOffset((int)functionInfoOffset);
			for (int i = 0; i < functionCount; i++)
			{
				// is 0 for lib_4_0, lib_4_1, lib_5_0
				// is 1 for lib_4_0_level_9_1_vs_only, lib_4_0_level_9_3_vs_only
				// is 2 for lib_4_0_level_9_1_ps_only, lib_4_0_level_9_3_ps_only
				// is 3 for lib_4_0_level_9_1, lib_4_0_level_9_3
				var mode = (ProfileMode)functionInfoReader.ReadUInt32();
				var functionNameOffset = functionInfoReader.ReadUInt32();
				var functionNameReader = reader.CopyAtOffset((int)functionNameOffset);
				var name = functionNameReader.ReadString();
				result.FunctionDescs.Add(new FunctionDesc(name, mode));
			}
			result.Data = reader.ReadBytes((int)chunkSize);
			return result;
		}
		public static string FormatReadable(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				for (int j = i; j < i + 16; j++)
				{
					if (j < data.Length)
					{
						sb.Append(data[j].ToString("X2"));
					} else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append(" ");
					}
				}
				sb.Append("\t");
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (!char.IsControl(c))
					{
						sb.Append(c);
					} else
					{
						sb.Append('.');
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("LibhChunk");
			sb.AppendLine($"CreatorString: {CreatorString}");
			for (int i = 0; i < FunctionDescs.Count; i++)
			{
				var desc = FunctionDescs[i];
				sb.AppendLine($"FunctionName[{i}]: {desc.Name}     {desc.Mode}");
			}
			sb.Append(FormatReadable(Data));
			return sb.ToString();
		}
	}
}
