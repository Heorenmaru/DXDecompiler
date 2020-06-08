using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode.Declaration
{
	public class ConstantDeclaration
	{
		public string Name { get; private set; }
		public RegisterSet RegisterSet { get; private set; }
		public short RegisterIndex { get; private set; }
		public short RegisterCount { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public ParameterType ParameterType { get; private set; }
		public int Rows { get; private set; }
		public int Columns { get; set; }
		public int Elements { get; private set; }
		public int Members { get; private set; }
		public List<float> DefaultValue { get; set; }
		public ConstantDeclaration(string name, RegisterSet registerSet, short registerIndex, short registerCount,
			ParameterClass parameterClass, ParameterType parameterType, int rows, int columns, int elements, List<float> defaultValue)
		{
			Name = name;
			RegisterSet = registerSet;
			RegisterIndex = registerIndex;
			RegisterCount = registerCount;
			ParameterClass = parameterClass;
			ParameterType = parameterType;
			Rows = rows;
			Columns = columns;
			Elements = elements;
			DefaultValue = defaultValue;
		}
		public ConstantDeclaration()
		{
			DefaultValue = new List<float>();
		}
		public static ConstantDeclaration Parse(BytecodeReader reader, BytecodeReader decReader)
		{
			var result = new ConstantDeclaration();
			var nameOffset = decReader.ReadUInt32();
			result.RegisterSet = (RegisterSet)decReader.ReadUInt16();
			result.RegisterIndex = (short)decReader.ReadUInt16();
			result.RegisterCount = (short)decReader.ReadUInt16();
			decReader.ReadUInt16(); //Reserved
			var typeInfoOffset = decReader.ReadUInt32();
			var defaultValueOffset = decReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			var typeReader = reader.CopyAtOffset((int)typeInfoOffset);
			result.ParameterClass = (ParameterClass)typeReader.ReadUInt16();
			result.ParameterType = (ParameterType)typeReader.ReadUInt16();
			result.Rows = typeReader.ReadUInt16();
			result.Columns = typeReader.ReadUInt16();
			result.Elements = typeReader.ReadUInt16();
			result.Members = typeReader.ReadUInt16();
			var memberInfoOffset = typeReader.ReadUInt32();

			if (defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset((int)defaultValueOffset);
				for (int i = 0; i < 4; i++)
				{
					result.DefaultValue.Add(defaultValueReader.ReadSingle());
				}
			}
			return result;
		}

		public bool ContainsIndex(int index)
		{
			return (index >= RegisterIndex) && (index < RegisterIndex + RegisterCount);
		}

		public override string ToString()
		{
			return Name;
		}
		public string GetTypeName()
		{
			if (ParameterClass == ParameterClass.Vector)
			{
				if (Columns > 1)
				{
					return $"{ParameterType.GetDescription()}{Columns}";
				}
				else
				{
					return $"{ParameterType.GetDescription()}";
				}
			}
			else if (ParameterClass == ParameterClass.MatrixColumns)
			{
				return $"{ParameterType.GetDescription()}{Rows}x{Columns}";
			}
			else if (ParameterClass == ParameterClass.MatrixRows)
			{
				return $"{ParameterType.GetDescription()}{Columns}x{Rows}";
			}
			else
			{
				return ParameterType.GetDescription();
			}
		}
		public string GetRegisterName()
		{
			return $"{RegisterSet.GetDescription()}{RegisterIndex}";
		}
	}
}
