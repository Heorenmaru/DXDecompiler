﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Libf
{
	public class LibraryDesc
	{
		public string Name { get; private set; }
		public ProfileMode Mode { get; private set; }
		public LibraryDesc(string name, ProfileMode mode)
		{
			Name = name;
			Mode = mode;
		}
	}
}
