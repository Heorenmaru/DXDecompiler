using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugIndent : IDumpable
	{
		public string Name;
		public int Indent;
		public string Dump()
		{
			var member = this;
			var indent = new string(' ', (int)member.Indent * 2);
			var sb = new StringBuilder();
			sb.Append(indent);
			sb.Append($"Indent: {Name}\n");
			return sb.ToString();
		}
	}
}
