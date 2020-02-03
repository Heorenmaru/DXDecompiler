using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC
	/// </summary>
	public class EffectType
	{
		public uint TypeNameOffset;
		public string TypeName { get; private set; }
		public uint Unknown2;
		public uint ElementCount { get; private set; }
		public uint GuessPackedSize { get; private set; }
		public uint GuessUnpackedSize { get; private set; }
		public uint GuessStride { get; private set; }
		public uint PackedType { get; private set; }
		public uint MemberCount { get; private set; }
		public EffectVariableType EffectVariableType { get; private set; }
		public ShaderVariableType VariableType => EffectVariableType.ToShaderVariableType();
		public ShaderVariableClass VariableClass { get; private set; }
		public uint Rows { get; private set; }
		public uint Columns { get; private set; }
		public List<EffectMember> Members { get; private set; }
		public EffectType()
		{
			Members = new List<EffectMember>();
		}
		public static EffectType Parse(BytecodeReader reader, BytecodeReader typeReader)
		{
			var result = new EffectType();
			var typeNameOffset = result.TypeNameOffset = typeReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)typeNameOffset);
			result.TypeName = nameReader.ReadString();
			//I suspect this is object type, 1 for numeric, 2 for object, 3 for struct
			result.Unknown2 = typeReader.ReadUInt32();
			result.ElementCount = typeReader.ReadUInt32();
			result.GuessUnpackedSize = typeReader.ReadUInt32();
			result.GuessStride = typeReader.ReadUInt32();
			result.GuessPackedSize = typeReader.ReadUInt32();
			var type = result.PackedType = typeReader.ReadUInt32();
			if (result.Unknown2 == 1) //numeric
			{
				var variableClass = type.DecodeValue(0, 1);
				switch (variableClass)
				{
					case 1:
						result.VariableClass = ShaderVariableClass.Scalar;
						break;
					case 2:
						result.VariableClass = ShaderVariableClass.Vector;
						break;
					case 3:
						result.VariableClass = type.DecodeValue(14, 14) == 1 ?
							ShaderVariableClass.MatrixColumns :
							ShaderVariableClass.MatrixRows;
						break;
				}
				var scalarType = type.DecodeValue(3, 5);
				switch (scalarType)
				{
					case 1:
						result.EffectVariableType = EffectVariableType.Float;
						break;
					case 2:
						result.EffectVariableType = EffectVariableType.Int;
						break;
					case 3:
						result.EffectVariableType = EffectVariableType.UInt;
						break;
					case 4:
						result.EffectVariableType = EffectVariableType.Bool;
						break;
				}
				result.Rows = type.DecodeValue(8, 10);
				result.Columns = type.DecodeValue(11, 13);
			} else if (result.Unknown2 == 2)
			{
				result.VariableClass = ShaderVariableClass.Object;
				result.EffectVariableType = (EffectVariableType)type;
			}
			else if (result.Unknown2 == 3)
			{
				result.EffectVariableType = EffectVariableType.Void;
				result.VariableClass = ShaderVariableClass.Struct;
				result.MemberCount = result.PackedType;
				for(int i = 0; i < result.MemberCount; i++)
				{
					result.Members.Add(EffectMember.Parse(reader, typeReader));
				}
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"  EffectType");
			sb.AppendLine($"    TypeNameOffset: {TypeName} ({TypeNameOffset.ToString("X4")})");
			sb.AppendLine($"    Type: {Unknown2}");
			sb.AppendLine($"    ElementCount: {ElementCount}");
			sb.AppendLine($"    GuessPackedSize: {GuessPackedSize}");
			sb.AppendLine($"    GuessUnpackedSize: {GuessUnpackedSize}");
			sb.AppendLine($"    GuessStride: {GuessStride}");
			sb.AppendLine($"    VariableTypeAndClass: {VariableClass}, {VariableType}");
			sb.AppendLine($"    DebugType: {TypeName,-16}{PackedType, 4}\t({Convert.ToString(PackedType, 2),15})");
			sb.AppendLine($"    MemberCount {MemberCount}");
			foreach(var member in Members)
			{
				sb.Append(member.ToString());
			}
			return sb.ToString();
		}
	}
}
