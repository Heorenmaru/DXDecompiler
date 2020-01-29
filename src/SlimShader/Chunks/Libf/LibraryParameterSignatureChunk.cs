using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Libf
{
	public class LibraryParameterSignatureChunk : BytecodeChunk
	{
		public List<LibraryParameterDescription> Parameters { get; private set; }
		public LibraryParameterSignatureChunk()
		{
			Parameters = new List<LibraryParameterDescription>();
		}
		public static LibraryParameterSignatureChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new LibraryParameterSignatureChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var parameterCount = chunkReader.ReadUInt32();
			var paramterOffset = chunkReader.ReadUInt32();
			for(int i = 0; i < parameterCount; i++)
			{
				var parameterReader = chunkReader.CopyAtOffset((int)paramterOffset + 12*4*i);
				result.Parameters.Add(LibraryParameterDescription.Parse(reader, parameterReader));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("// Function parameter signature (return: {0}, parameters: {1}):",
				"yes", Parameters.Count));
			sb.Append("//");
			sb.AppendLine("// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           ");
			sb.AppendLine("// -------------------- -------------------- --------------- ---------------- ------------------------------ ");
			foreach(var param in Parameters)
			{
				sb.AppendLine(param.ToString());
			}
			return sb.ToString();
		}
	}
}
