using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.FX9
{
	public class DebugFx9Chunk
	{
		const int headerLength = 4;
		public uint VariableCount;
		public uint TechniqueCount;
		public uint PassCount;
		public uint ShaderCount;
		public uint BinaryDataCount;
		public uint InlineShaderCount;

		List<DebugVariable> Variables = new List<DebugVariable>();
		List<DebugTechnique> Techniques = new List<DebugTechnique>();
		List<DebugBinaryData> BinaryDataList = new List<DebugBinaryData>();
		List<DebugInlineShader> InlineShaders = new List<DebugInlineShader>();

		public static DebugFx9Chunk Parse(DebugBytecodeReader reader, uint length)
		{
			var result = new DebugFx9Chunk();
			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader);
			var footerOffset = chunkReader.ReadUInt32("footerOffset");
			var bodyReader = chunkReader.CopyAtCurrentPosition("BodyReader", reader);
			var footerReader = reader.CopyAtOffset("footerReader", reader, (int)footerOffset + 4);
			var variableCount = result.VariableCount = footerReader.ReadUInt32("VariableCount");
			var techniqueCount = result.TechniqueCount = footerReader.ReadUInt32("TechniqueCount");
			result.PassCount = footerReader.ReadUInt32("PassCount");
			result.ShaderCount = footerReader.ReadUInt32("ShaderCount");

			for (int i = 0; i < variableCount; i++)
			{
				result.Variables.Add(DebugVariable.Parse(bodyReader, footerReader));
			}
			for (int i = 0; i < techniqueCount; i++)
			{
				result.Techniques.Add(DebugTechnique.Parse(bodyReader, footerReader));
			}

			result.BinaryDataCount = footerReader.ReadUInt32("BinaryDataCount");
			result.InlineShaderCount = footerReader.ReadUInt32("InlineShaderCount");
			for (int i = 0; i < result.BinaryDataCount; i++)
			{
				result.BinaryDataList.Add(DebugBinaryData.Parse(bodyReader, footerReader));
			}
			for (int i = 0; i < result.InlineShaderCount; i++)
			{
				result.InlineShaders.Add(DebugInlineShader.Parse(bodyReader, footerReader));
			}
			return result;

		}
	}
}
