using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.Assignemnt
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AssignmentTypeAttribute : Attribute
	{
		private readonly Type _type;

		public Type Type
		{
			get { return _type; }
		}

		public AssignmentTypeAttribute(Type type)
		{
			_type = type;
		}
	}
}
