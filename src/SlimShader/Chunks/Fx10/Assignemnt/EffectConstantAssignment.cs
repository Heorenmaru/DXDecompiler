using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectConstantAssignment : EffectAssignment
	{
		public List<EffectScalarType> Types { get; private set; }
		public List<Number> Values { get; private set; }
		public EffectConstantAssignment()
		{
			Types = new List<EffectScalarType>();
			Values = new List<Number>();
		}
		public static EffectConstantAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectConstantAssignment();
			var assignmentCount = assignmentReader.ReadUInt32();
			for (int i = 0; i < assignmentCount; i++)
			{
				result.Types.Add((EffectScalarType)assignmentReader.ReadUInt32());
				result.Values.Add(Number.Parse(assignmentReader));
			}
			return result;
		}
		public override string Dump()
		{
			var sb = new StringBuilder();
			sb.Append(base.Dump());
			sb.AppendLine($"    EffectConstantAssignment.Count: {Types.Count}: {((uint)Types.Count).ToString("X4")}");
			for (int i = 0; i < Types.Count; i++)
			{
				var scalarType = Types[i];
				var value = Values[i];
				sb.AppendLine($"    EffectConstantAssignment.ScalarType: {scalarType}: {((uint)scalarType).ToString("X4")}");
				sb.AppendLine($"    EffectConstantAssignment.Value: {value}: {value.UInt.ToString("X4")}");
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append("[");
			sb.Append(MemberIndex);
			sb.Append("]");
			sb.Append(" = ");
			for (int i = 0; i < Values.Count; i++) {
				sb.Append(Values[i].ToString());
				if(i < Values.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(";");
			return sb.ToString();
		}
	}
}
