using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugPass
	{
		public string Name { get; private set; }
		public uint NameOffset;
		public uint AnnotationCount;
		public uint UnknownCount;
		List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		List<DebugUnknownObject> UnknownObjects = new List<DebugUnknownObject>();
		public static DebugPass Parse(DebugBytecodeReader reader, DebugBytecodeReader passReader)
		{
			var result = new DebugPass();
			result.NameOffset = passReader.ReadUInt32("NameOffset");
			result.AnnotationCount = passReader.ReadUInt32("AnnoationCount");
			result.UnknownCount = passReader.ReadUInt32("UnknownCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				result.Annotations.Add(DebugAnnotation.Parse(reader, passReader));
			}
			for (int i = 0; i < result.UnknownCount; i++)
			{
				result.UnknownObjects.Add(DebugUnknownObject.Parse(passReader, 4));
			}
			var nameReader = reader.CopyAtOffset("NameReader", passReader, (int)result.NameOffset);
			result.Name = nameReader.TryReadString("Name");
			return result;
		}
	}
}