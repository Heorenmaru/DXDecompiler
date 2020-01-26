using SlimShader.Chunks;
using SlimShader.DebugParser.Aon9;
using SlimShader.DebugParser.Icfe;
using SlimShader.DebugParser.Rdef;
using SlimShader.DebugParser.Sfi0;
using SlimShader.DebugParser.Stat;
using SlimShader.DebugParser.Xsgn;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugBytecodeChunk
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ "IFCE".ToFourCc(), ChunkType.Ifce },
			{ "ISGN".ToFourCc(), ChunkType.Isgn },
			{ "OSGN".ToFourCc(), ChunkType.Osgn },
			{ "OSG5".ToFourCc(), ChunkType.Osg5 },
			{ "PCSG".ToFourCc(), ChunkType.Pcsg },
			{ "RDEF".ToFourCc(), ChunkType.Rdef },
			{ "SDBG".ToFourCc(), ChunkType.Sdbg },
			{ "SFI0".ToFourCc(), ChunkType.Sfi0 },
			{ "SHDR".ToFourCc(), ChunkType.Shdr },
			{ "SHEX".ToFourCc(), ChunkType.Shex },
			{ "SPDB".ToFourCc(), ChunkType.Spdb },
			{ "STAT".ToFourCc(), ChunkType.Stat },
			{ "ISG1".ToFourCc(), ChunkType.Isg1 },
			{ "OSG1".ToFourCc(), ChunkType.Osg1 },
			{ "Aon9".ToFourCc(), ChunkType.Aon9 },
			{ "XNAS".ToFourCc(), ChunkType.Xnas },
			{ "XNAP".ToFourCc(), ChunkType.Xnap },
			{ "PRIV".ToFourCc(), ChunkType.Priv },
		};

		public DebugBytecodeContainer Container { get; private set; }
		public uint FourCc { get; private set; }
		public ChunkType ChunkType { get; private set; }
		public uint ChunkSize { get; private set; }

		public static DebugBytecodeChunk ParseChunk(DebugBytecodeReader chunkReader, DebugBytecodeContainer container)
		{
			// Type of chunk this is.
			uint fourCc = BitConverter.ToUInt32(chunkReader.ReadBytes("fourCc", 4), 0);

			// Total length of the chunk in bytes.
			uint chunkSize = chunkReader.ReadUInt32("chunkSize");

			ChunkType chunkType;
			if (KnownChunkTypes.ContainsKey(fourCc))
			{
				chunkType = KnownChunkTypes[fourCc];
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");
				System.Diagnostics.Debug.WriteLine("Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");
				return null;
			}

			var chunkContentReader = chunkReader.CopyAtCurrentPosition($"{fourCc.ToFourCcString()}", chunkReader, (int)chunkSize);
			DebugBytecodeChunk chunk = null;
			switch (chunkType)
			{
				case ChunkType.Ifce:
					chunk = DebugInterfacesChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Isgn:
				case ChunkType.Osgn:
				case ChunkType.Osg5:
				case ChunkType.Pcsg:
				case ChunkType.Isg1:
				case ChunkType.Osg1:
					chunk = DebugInputOutputSignatureChunk.Parse(chunkContentReader, chunkType,
						container.ResourceDefinition.Target.ProgramType);
					break;
				case ChunkType.Rdef:
					chunk = DebugResourceDefinitionChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Sdbg:
				case ChunkType.Spdb:
					//chunk = DebuggingChunk.Parse(chunkContentReader, chunkType, (int)chunkSize);
					break;
				case ChunkType.Sfi0:
					chunk = DebugSfi0Chunk.Parse(chunkContentReader, null, chunkSize);
					break;
				case ChunkType.Shdr:
				case ChunkType.Shex:
					//chunk = ShaderProgramChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Stat:
					chunk = DebugStatisticsChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Xnas:
				case ChunkType.Xnap:
				case ChunkType.Aon9:
					chunk = DebugLevel9ShaderChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Priv:
					break;
//				default:
//					throw new ParseException("Invalid chunk type: " + chunkType);
			}
			if (chunk == null)
			{
				chunkReader.ReadBytes("UnknownChunk", (int)chunkSize);
				return null;
			}
			chunk.Container = container;
			chunk.FourCc = fourCc;
			chunk.ChunkSize = chunkSize;
			chunk.ChunkType = chunkType;

			return chunk;
		}
	}
}
