using SlimShader.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugBytecodeContainer
	{
		private readonly byte[] _rawBytes;

		public byte[] RawBytes
		{
			get { return _rawBytes; }
		}
		public DebugBytecodeContainerHeader Header { get; private set; }
		public List<DebugBytecodeChunk> Chunks { get; private set; }
		DebugBytecodeReader _reader;
		string Error = "";
		public DebugBytecodeContainer(byte[] rawBytes)
		{
			try
			{
				_rawBytes = rawBytes;
				Chunks = new List<DebugBytecodeChunk>();

				var reader = new DebugBytecodeReader(rawBytes, 0, rawBytes.Length);
				_reader = reader;

				Header = DebugBytecodeContainerHeader.Parse(reader);

				for (uint i = 0; i < Header.ChunkCount; i++)
				{
					uint chunkOffset = reader.ReadUInt32("chunkOffset");
					var chunkReader = reader.CopyAtOffset("BytecodeChunk", reader, (int)chunkOffset);

					var chunk = DebugBytecodeChunk.ParseChunk(chunkReader, this);
					if (chunk != null)
						Chunks.Add(chunk);
				}
			} catch (Exception ex)
			{
				Error = ex.ToString();
			}
		}
		public static DebugBytecodeContainer Parse(byte[] bytes)
		{
			return new DebugBytecodeContainer(bytes);
		}
		public string Dump()
		{
			var dump = _reader.DumpStructure();
			if (!string.IsNullOrEmpty(Error))
			{
				dump += "\n" + Error;
			}
			return dump;
		}
	}
}
