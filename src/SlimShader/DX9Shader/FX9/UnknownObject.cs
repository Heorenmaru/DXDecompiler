using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class UnknownObject
	{
		public List<uint> Data = new List<uint>();
		public static UnknownObject Parse(BytecodeReader variableReader, uint count)
		{
			var result = new UnknownObject();
			for(int i = 0; i < count; i++)
			{
				result.Data.Add(variableReader.ReadUInt32());
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			for(int i = 0; i < Data.Count; i++)
			{
				sb.AppendLine($"    UnknownObject.Unk{i}: {Data[i]} {Data[i].ToString("X4")}");
			}
			return sb.ToString();
		}
	}
}
