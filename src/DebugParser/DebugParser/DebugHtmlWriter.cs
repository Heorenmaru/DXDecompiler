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
			var styleTag = new XElement
				(
					"style",
					"/*",
					new XCData("*/\n" + Resources.DebugCSS + "\n/*"),
					"*/"
				);
			var xDocument = new XDocument(
				new XDocumentType("html", null, null, null),
				new XElement("html",
					new XElement("head",
						styleTag,
						javascriptTag),
					new XElement("body",
						new XElement("div", 
							new XAttribute("class", "panel-row"),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "treeview")
							),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "hexview")
							),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "detailview"),
								""
							)
						)
					)
				)
			);
			var treeView = xDocument.Root.Descendants("div")
				.Where(e => (string)e.Attribute("id") == "treeview")
				.First();
			WriteTreeView(treeView, Members);
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
		private void WriteTreeView(XElement treeView, List<IDumpable> entries)
		{
			var stack = new Stack<XElement>();
			var ul = new XElement("ul", new XAttribute("id", "myUL"));
			treeView.Add(ul);
			stack.Push(ul);
			for(int i = 0; i < entries.Count; i++)
			{
				var entry = entries[i];
				var nextEntry = i < entries.Count - 1 ? entries[i+1] : null;
				var span = new XElement("span", new XAttribute("class", "tree-item"), "");
				XElement label = null;
				if (entry is DebugEntry de)
				{
					var valueLength = 7;
					var value = de.Value.Length > valueLength ?
						(de.Value.Substring(0, valueLength - 3) + "...") :
						de.Value;
					var text = $"{de.Name}={value}";
					label = new XElement("span", text,
						new XAttribute("class", "tree-label"),
						new XAttribute("data-start", de.AbsoluteIndex),
						new XAttribute("data-end", de.AbsoluteIndex + de.Size),
						new XAttribute("id", "member_" + entry.GetHashCode()),
						new XAttribute("name", de.Name),
						new XAttribute("value", de.Value),
						new XAttribute("size", de.Size),
						new XAttribute("rel-start", de.RelativeIndex),
						new XAttribute("rel-end", de.RelativeIndex + de.Size),
						new XAttribute("type", de.Type));
				}
				if(entry is DebugIndent di){
					label = new XElement("span", "Indent: " + di.Name);
				}
				if (entry is DebugBytecodeReader dr)
				{
					label = new XElement("span", $"Container: {dr.Name}",
						new XAttribute("class", "tree-label"),
						new XAttribute("data-start", dr.Offset),
						new XAttribute("data-end", dr.Offset + dr.Count),
						new XAttribute("id", "member_" + entry.GetHashCode()),
						new XAttribute("name", dr.Name),
						new XAttribute("value", ""),
						new XAttribute("size", dr.Count),
						new XAttribute("rel-start", dr.Offset),
						new XAttribute("rel-end", dr.Offset + dr.Count),
						new XAttribute("type", "Container")
						);
				}
				var container = stack.Peek();
				var li = new XElement("li");
				container.Add(li);
				li.Add(span);
				if (nextEntry != null && nextEntry.Indent > entry.Indent)
				{
					var caret = new XElement("span", new XAttribute("class", "caret"), "");
					span.Add(caret);
					span.Add(label);
					if (nextEntry.Indent - entry.Indent > 1) throw new Exception("Unbalanced Indents");
					var subList = new XElement("ul", new XAttribute("class", "nested"));
					li.Add(subList);
					stack.Push(subList);
				} else
				{
					span.Add(label);
				}
				if (nextEntry != null && nextEntry.Indent < entry.Indent)
				{
					for (int j = 0; j < entry.Indent - nextEntry.Indent; j++)
					{
						stack.Pop();
					}
				}
			}
		}
		private void WriteHexView(XElement element)
		{
			var used = BuildUsedLookup();
			for (int i = 0; i < buffer.Length; i += 16)
			{
				var row = new XElement("div",
					new XAttribute("row", i / 16),
					new XAttribute("class", "monospace"),
					new XElement("span", i.ToString("X4") + ":"));
				element.Add(row);
				for (int j = i; j < i + 16; j++)
				{
					var text = j < buffer.Length ? buffer[j].ToString("X2") : "\u00A0\u00A0";
					var hexElement = new XElement("span", text,
						new XAttribute("index", j.ToString()),
						new XAttribute("id", "b" + j.ToString()));
					if (j < used.Length && used[j] == null)
					{
						hexElement.Add(new XAttribute("class", "unused"));
					} else if(j < used.Length && used[j] != null)
					{
						hexElement.Add(new XAttribute("member", "member_" + used[j].GetHashCode()));
					}
					row.Add(hexElement);
				}
				var asciiText = "";
				for (int j = i; j < i + 16; j++)
				{
					asciiText += j < buffer.Length ? DebugBytecodeReader.CharToReadable((char)buffer[j]) : "\u00A0";
				}
				var asciiElement = new XElement("span", asciiText);
				row.Add(asciiElement);

			}
		}
		DebugEntry[] BuildUsedLookup()
		{
			var entries = Members.OfType<DebugEntry>();
			var used = new DebugEntry[buffer.Length];
			foreach (var entry in entries)
			{
				for (var i = entry.AbsoluteIndex; i < entry.AbsoluteIndex + entry.Size; i++)
				{
					if(used[i] == null)
					{
						used[i] = entry;
					}
					else if(entry.Indent > used[i].Indent)
					{
						used[i] = entry;
					}
				}
			}
			return used;
		}
	}
}
