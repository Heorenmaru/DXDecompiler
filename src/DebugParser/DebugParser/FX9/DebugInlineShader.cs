namespace SlimShader.DebugParser.FX9
{
	public class DebugInlineShader
	{
		public uint Unknown1;
		public uint Unknown2;
		public uint Unknown3;
		public uint Index;
		public uint IsVariable;
		public uint ShaderSize;
		public string VariableName { get; private set; }
		public byte[] Data = new byte[0];
		public static DebugInlineShader Parse(DebugBytecodeReader reader, DebugBytecodeReader shaderReader)
		{
			var result = new DebugInlineShader();
			result.Unknown1 = shaderReader.ReadUInt32("Unknown1");
			result.Unknown2 = shaderReader.ReadUInt32("Unknown2");
			result.Unknown3 = shaderReader.ReadUInt32("Unknown3");
			result.Index = shaderReader.ReadUInt32("Index?");
			result.IsVariable = shaderReader.ReadUInt32("IsVariable");
			var dataReader = shaderReader.CopyAtCurrentPosition("Data", shaderReader);
			result.ShaderSize = shaderReader.ReadUInt32("ShaderSize");
			var toRead = result.ShaderSize + (result.ShaderSize % 4 == 0 ? 0 : 4 - result.ShaderSize % 4);
			result.Data = shaderReader.ReadBytes("RawData", (int)toRead);
			if (result.IsVariable == 1)
			{
				result.VariableName = dataReader.TryReadString("VariableName");
			}
			else
			{
				result.VariableName = "";
			}
			return result;
		}
	}
}