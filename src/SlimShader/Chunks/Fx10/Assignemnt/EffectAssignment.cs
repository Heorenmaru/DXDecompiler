using SlimShader.Chunks.Fx10.Assignemnt;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectAssignment
	{
		public EffectAssignmentType MemberType;
		public uint MemberIndex;
		public EffectCompilerAssignmentType AssignmentType;
		public uint ValueOffset;

		public uint ValueSize;
		public uint GuessAnotationFormat;
		public uint GuessAnotationValue;

		public string TextureName { get; private set; }
		public static EffectAssignment Parse(BytecodeReader reader, BytecodeReader annotationReader)
		{
			//MemberType type, such as AddressV or Filter
			var memberType = (EffectAssignmentType)annotationReader.ReadUInt32();
			//Debug.Assert(Enum.IsDefined(typeof(EffectAssignmentType), memberType),
			//	$"EffectStateAnnotationType is {memberType}");
			//MemberIndex is 1 for BlendEnable[1] = TRUE;
			var memberIndex = annotationReader.ReadUInt32();
			var assignmentType = (EffectCompilerAssignmentType)annotationReader.ReadUInt32();
			var valueOffset = annotationReader.ReadUInt32();
			var typeSpecificReader = reader.CopyAtOffset((int)valueOffset);
			EffectAssignment result;
			switch (assignmentType)
			{
				case EffectCompilerAssignmentType.Constant:
					result = EffectConstantAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.Variable:
					result = EffectVariableAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.InlineShader:
					result = EffectInlineShaderAssignment.Parse(reader, typeSpecificReader);
					break;
				default:
					throw new ParseException($"Unsupported EffectCompilerAssignmentType {assignmentType}");

			}
			result.MemberType = memberType;
			result.MemberIndex = memberIndex;
			result.AssignmentType = assignmentType;
			result.ValueOffset = valueOffset;
			/*if (result.AssignmentType == EffectCompilerAssignmentType.Constant)
			{
				;
				result.ValueSize = unknownReader.ReadUInt32();

				// Could be formating flags, for example: 
				// DepthEnable = TRUE; = 2				bool(TRUE / 1 /);
				// BackFaceStencilFunc = Always; = 2
				// StencilReadMask = 255; = 2
				// StencilReadMask = 0xFF; = 3
				// DepthEnable = true; = 4				bool(TRUE / true /);

				result.GuessAnotationFormat = unknownReader.ReadUInt32();
				//Value such as Wrap in AddressU = WRAP
				result.GuessAnotationValue = unknownReader.ReadUInt32();
			} else
			{
				var textureNameReader = reader.CopyAtOffset((int)result.ValueOffset);
				result.TextureName = textureNameReader.ReadString();
			}*/
			return result;
		}
		public virtual string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("  EffectAssignment");
			sb.AppendLine($"    EffectAssignment.MemberType {MemberType} ({(uint)MemberType})");
			sb.AppendLine($"    EffectAssignment.MemberIndex {MemberIndex} ({MemberIndex.ToString("X4")})");
			sb.AppendLine($"    EffectAssignment.AssignmentType {AssignmentType} ({AssignmentType})");
			sb.AppendLine($"    EffectAssignment.ValueOffset {ValueOffset} ({ValueOffset.ToString("X4")})");
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("  EffectStateAnnotation");
			return sb.ToString();
		}
	}
}
