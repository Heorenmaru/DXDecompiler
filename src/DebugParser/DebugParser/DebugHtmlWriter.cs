using DebugParser.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SlimShader.DebugParser
{
	public class DebugHtmlWriter
	{
		DebugBytecodeReader Reader;
		byte[] buffer;
		StringBuilder stringBuilder;
		private List<IDumpable> Members = new List<IDumpable>();
		public DebugHtmlWriter(DebugBytecodeReader reader, byte[] buffer, List<IDumpable> members)
		{
			this.Reader = reader;
			this.buffer = buffer;
			this.Members = members;
		}
		public string ToHtml()
		{
			stringBuilder = new StringBuilder();
			Write();
			return stringBuilder.ToString();
		}
		private void Write()
		{
			string css = Resources.DebugCSS;
			var javascriptTag = new XElement
				(
				   "script",
				   new XAttribute("type", @"text/javascript"),
				   "//",
				   new XCData("\n" + Resources.DebugJavascript + "\n//")
				);

			var xDocument = new XDocument(
				new XDocumentType("html", null, null, null),
				new XElement("html",
					new XElement("head",
						new XElement("style", css),
						javascriptTag),
					new XElement("body",
						new XElement("div",
							new XAttribute("id", "treeview")
						),
						new XElement("div",
							new XAttribute("id", "hexview")
						)
					)
				)
			);
			var treeView = xDocument.Root.Descendants("div")
				.Where(e => (string)e.Attribute("id") == "treeview")
				.First();
			WriteTreeView(treeView, Members.OfType<DebugEntry>().ToList());
			var hexview = xDocument.Root.Descendants("div")
				.Where(e => (string)e.Attribute("id") == "hexview")
				.First();
			WriteHexView(hexview);
			var settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true,
				IndentChars = "\t"
			};

			using (var writer = XmlWriter.Create(stringBuilder, settings))
			{
				xDocument.WriteTo(writer);
			}
		}
		private void WriteTreeView(XElement treeView, List<DebugEntry> entries)
		{
			var stack = new Stack<XElement>();
			var ul = new XElement("ul", new XAttribute("id", "myUL"));
			treeView.Add(ul);
			stack.Push(ul);
			for(int i = 0; i < entries.Count; i++)
			{
				var entry = entries[i];
				var nextEntry = i < entries.Count - 1 ? entries[i+1] : null;
				var span = new XElement("span", entry.DumpInline());
				var li = new XElement("li", span);
				var container = stack.Peek();
				container.Add(li);
				if(nextEntry != null && nextEntry.Indent > entry.Indent)
				{
					var subList = new XElement("ul", new XAttribute("class", "nested"));
					li.Add(subList);
					stack.Push(subList);
					span.Add(new XAttribute("class", "caret"));
				}
				if (nextEntry != null && nextEntry.Indent < entry.Indent)
				{
					stack.Pop();
				}
			}
		}
		private void WriteHexView(XElement element)
		{
			XElement row = null;
			for(int i = 0; i < buffer.Length; i++)
			{
				if (row == null || i % 16 == 0)
				{
					row = new XElement("div",
						new XAttribute("row", i / 16),
						new XAttribute("class", "monospace"));
					element.Add(row);
				}
				var hexElement = new XElement("span", buffer[i].ToString("X2"),
					new XAttribute("index", i.ToString()));
				row.Add(hexElement);
			}
		}
	}
}
