using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.Tests
{
	public class FileUtil
	{
		public static string GetRelativePath(string filespec, string folder)
		{
			Uri pathUri = new Uri(filespec);
			// Folders must end in a slash
			if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				folder += Path.DirectorySeparatorChar;
			}
			Uri folderUri = new Uri(folder);
			return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}
		public static string FormatReadable(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				for (int j = i; j < i + 16; j++)
				{
					if (j < data.Length)
					{
						sb.Append(data[j].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append(" ");
					}
				}
				sb.Append("\t");
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (!char.IsControl(c))
					{
						sb.Append(c);
					}
					else
					{
						sb.Append('.');
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
