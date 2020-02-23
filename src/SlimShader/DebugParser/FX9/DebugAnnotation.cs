namespace SlimShader.DebugParser.FX9
{
	public class DebugAnnotation
	{
		public uint DataOffset;
		public uint DefaultValueOffset;
		DebugVariableData Unknown2;
		public static DebugAnnotation Parse(DebugBytecodeReader reader, DebugBytecodeReader annotationReader)
		{
			var result = new DebugAnnotation();
			result.DataOffset = annotationReader.ReadUInt32("DataOffset");
			result.DefaultValueOffset = annotationReader.ReadUInt32("DefaultValueOffset");
			var unknownReader = reader.CopyAtOffset("AnnotationType", annotationReader, (int)result.DataOffset);
			result.Unknown2 = DebugVariableData.Parse(reader, unknownReader);
			return result;
		}
	}
}