using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Libf
{
	public class FunctionDesc
	{
		public string Name { get; private set; }
		public ProfileMode Mode { get; private set; }
		public FunctionDesc(string name, ProfileMode mode)
		{
			Name = name;
			Mode = mode;
		}
	}
}
