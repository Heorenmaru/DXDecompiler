using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugIndent : IDumpable
	{
		public string Name;
		public int Indent { get; set; }
		public List<IDumpable> Members = new List<IDumpable>();
		public uint AbsoluteIndex => Members.Count == 0 ? 0 : Members.First().AbsoluteIndex;
		public uint RelativeIndex => Members.Count == 0 ? 0 : Members.First().RelativeIndex;
		public string Value => "";
		public string Type => "Indent";
		public uint Size
		{
			get
			{
				if(Members.Count == 0)
				{
					return 0;
				}
				if(Members.Count == 1)
				{
					return Members.First().Size;
				}
				return Members.Max(m => m.AbsoluteIndex) - Members.Min(m => m.AbsoluteIndex);
			}
		}
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
