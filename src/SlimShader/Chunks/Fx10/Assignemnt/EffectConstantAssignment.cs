using SlimShader.Chunks.RTS0;
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
		public string FormatValue(Number value)
		{
			var type = MemberType.GetAssignmentType();
			if(type == null)
			{
				return value.UInt.ToString();
			}
			if (type.IsEnum)
			{
				return Enum.GetName(type, value.UInt);
			}
			if (type == typeof(float))
			{
				return value.ToString();
			}
			if (type == typeof(byte))
			{
				return value.UInt.ToString();
			}
			if (type == typeof(bool))
			{
				return value.ToString(Shex.NumberType.Bool);
			}
			return value.UInt.ToString();
		}
		public string FormatValue()
		{
			var type = MemberType.GetAssignmentType();
			string value = string.Join(", ", Values.Select(v => FormatValue(v)));
			string typeName = "unknown";
			if (type == null)
			{
				typeName = "null";
			}
			else if (type.IsEnum)
			{
				typeName = "uint";
			}
			else if (type == typeof(float))
			{
				typeName = "float";
			}
			else if (type == typeof(byte))
			{
				typeName = "byte";
			}
			else if (type == typeof(bool))
			{
				typeName = "bool";
			}
			if(Values.Count > 1)
			{
				typeName += Values.Count;
			}
			return string.Format("{0}({1})", typeName, value);
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append("[");
			sb.Append(MemberIndex);
			sb.Append("]");
			sb.Append(" = ");
			sb.Append(FormatValue());
			sb.Append(";");
			return sb.ToString();
		}
	}
}
