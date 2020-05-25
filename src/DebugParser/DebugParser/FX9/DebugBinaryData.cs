namespace SlimShader.DebugParser.FX9
{
	public class DebugBinaryData
	{
		public uint Index;
		public uint Size;
		public byte[] Data;
		public static DebugBinaryData Parse(DebugBytecodeReader reader, DebugBytecodeReader dataReader)
		{
			var result = new DebugBinaryData();
			result.Index = dataReader.ReadUInt32("Index");
			result.Size = dataReader.ReadUInt32("Size");
			var toRead = result.Size + (result.Size % 4 == 0 ? 0 : 4 - result.Size % 4);
			result.Data = dataReader.ReadBytes("RawData", (int)toRead);
			return result;
		}
	}
}