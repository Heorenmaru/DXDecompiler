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
		public ShaderVariableType VariableType { get; private set; }
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
						result.VariableType = ShaderVariableType.Float;
						break;
					case 2:
						result.VariableType = ShaderVariableType.Int;
						break;
					case 3:
						result.VariableType = ShaderVariableType.UInt;
						break;
					case 4:
						result.VariableType = ShaderVariableType.Bool;
						break;
				}
				result.Rows = type.DecodeValue(8, 10);
				result.Columns = type.DecodeValue(11, 13);
			} else if (result.Unknown2 == 2)
			{
				result.VariableClass = ShaderVariableClass.Object;
				switch (type)
				{
					case 1:
						result.VariableType = ShaderVariableType.String;
						break;
					case 2:
						result.VariableType = ShaderVariableType.Blend;
						break;
					case 3:
						result.VariableType = ShaderVariableType.DepthStencil;
						break;
					case 4:
						result.VariableType = ShaderVariableType.Rasterizer;
						break;
					case 5:
						result.VariableType = ShaderVariableType.PixelShader;
						break;
					case 6:
						result.VariableType = ShaderVariableType.VertexShader;
						break;
					case 7:
						result.VariableType = ShaderVariableType.GeometryShader;
						break;
					case 8:
						result.VariableType = ShaderVariableType.GeometryShader;
						break;
					case 10:
						result.VariableType = ShaderVariableType.Texture1D;
						break;
					case 11:
						result.VariableType = ShaderVariableType.Texture1DArray;
						break;
					case 12:
						result.VariableType = ShaderVariableType.Texture2D;
						break;
					case 13:
						result.VariableType = ShaderVariableType.Texture2DArray;
						break;
					case 14:
						result.VariableType = ShaderVariableType.Texture2DMultiSampled;
						break;
					case 15:
						result.VariableType = ShaderVariableType.Texture2DMultiSampledArray;
						break;
					case 16:
						result.VariableType = ShaderVariableType.Texture3D;
						break;
					case 17:
						result.VariableType = ShaderVariableType.TextureCube;
						break;
					case 21:
						result.VariableType = ShaderVariableType.Sampler;
						break;
					case 22:
						result.VariableType = ShaderVariableType.Buffer;
						break;
					case 23:
						result.VariableType = ShaderVariableType.TextureCubeArray;
						break;
					default:
						throw new Exception($"Unknown effect variable type {type} for {result.TypeName}");
				}
			}
			else if (result.Unknown2 == 3)
			{
				result.VariableType = ShaderVariableType.Void;
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
			sb.AppendLine($"    EffectType.Unknown2: {Unknown2}");
			sb.AppendLine($"    EffectType.ElementCount: {ElementCount}");
			sb.AppendLine($"    EffectType.GuessPackedSize: {GuessPackedSize}");
			sb.AppendLine($"    EffectType.GuessUnpackedSize: {GuessUnpackedSize}");
			sb.AppendLine($"    EffectType.GuessStride: {GuessStride}");
			sb.AppendLine($"    EffectType.Type: {VariableClass}, {VariableType}");
			sb.AppendLine($"    EffectType.Type: {TypeName,-16}{PackedType, 4}\t({Convert.ToString(PackedType, 2),15})");
			sb.AppendLine($"    EffectType.MemberCount {MemberCount}");
			foreach(var member in Members)
			{
				sb.Append(member.ToString());
			}
			return sb.ToString();
		}
	}
}
