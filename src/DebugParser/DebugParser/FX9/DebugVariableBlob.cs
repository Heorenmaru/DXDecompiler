using SlimShader.DebugParser.DX9;
using SlimShader.DX9Shader;

namespace SlimShader.DebugParser.FX9
{
	public class DebugBinaryData
	{
		public uint Index;
		public uint Size;
		public byte[] Data;
		DebugShaderModel Shader;
		public static DebugBinaryData Parse(DebugBytecodeReader reader, DebugBytecodeReader blobReader)
		{
			var result = new DebugBinaryData();
			result.Index = blobReader.ReadUInt32("Index");
			result.Size = blobReader.ReadUInt32("Size");
			var startPosition = blobReader._reader.BaseStream.Position;
			var header = blobReader.PeakUint32();
			var shaderType = (ShaderType)(header >> 16);
			if(shaderType == ShaderType.Pixel || shaderType == ShaderType.Vertex)
			{
				var shaderReader = blobReader.CopyAtCurrentPosition("ShaderReader", blobReader);
				result.Shader = DebugShaderModel.Parse(shaderReader);
			}
			blobReader._reader.BaseStream.Position = startPosition + result.Size;
			return result;
		}
	}
}