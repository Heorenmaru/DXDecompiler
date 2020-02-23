﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugBytecodeReader : IDumpable
	{
		private readonly byte[] _buffer;
		private readonly int _offset;
		public int Indent;
		private readonly BinaryReader _reader;
		private int _parentOffset;
		private List<IDumpable> Members = new List<IDumpable>();
		DebugBytecodeReader Root = null;
		public static bool DumpOffsets = true;
		string _name;
		public bool EndOfBuffer
		{
			get { return _reader.BaseStream.Position >= _reader.BaseStream.Length; }
		}
		public DebugBytecodeReader(byte[] buffer, int index, int count)
		{
			_buffer = buffer;
			_offset = index;
			Indent = 0;
			_parentOffset = 0;
			Root = this;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
		}
		public DebugBytecodeReader(byte[] buffer, int index, int count, int parentIndex, int indent, string name, 
			DebugBytecodeReader root)
		{
			_buffer = buffer;
			_offset = index;
			_parentOffset = parentIndex;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
			_name = name;
			Root = root;
			Indent = indent;
		}
		DebugEntry AddEntry(string name, uint size)
		{
			var result = new DebugEntry()
			{
				Name = name,
				RelativeIndex = (uint)_offset - (uint)_parentOffset + (uint)_reader.BaseStream.Position - size,
				AbsoluteIndex = (uint)_offset + (uint)_reader.BaseStream.Position - size,
				Size = size,
				Indent = Indent
			};
			Root.Members.Add(result);
			return result;
		}
		public uint PeakUint32()
		{
			var result = _reader.ReadUInt32();
			_reader.BaseStream.Position -= 4;
			return result;
		}
		public uint PeakUInt32At(int offset)
		{
			var oldPos = _reader.BaseStream.Position;
			_reader.BaseStream.Position += offset;
			var result = _reader.ReadUInt32();
			_reader.BaseStream.Position = oldPos;
			return result;
		}
		public uint ReadUInt32(string name)
		{
			var result = _reader.ReadUInt32();
			var entry = AddEntry(name, 4);
			entry.Value = result.ToString();
			return result;
		}
		public int ReadInt32(string name)
		{
			var result = _reader.ReadInt32();
			var entry = AddEntry(name, 4);
			entry.Value = result.ToString();
			return result;
		}
		public ushort ReadUInt16(string name)
		{
			var result = _reader.ReadUInt16();
			var entry = AddEntry(name, 2);
			entry.Value = result.ToString();
			return result;
		}
		public byte ReadByte(string name)
		{
			var result = _reader.ReadByte();
			var entry = AddEntry(name, 1);
			entry.Value = result.ToString();
			return result;
		}
		public T ReadEnum32<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadUInt32());
			var entry = AddEntry(name, 4);
			entry.Value = result.ToString();
			return (T)result;
		}

		public T ReadEnum16<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadUInt16());
			var entry = AddEntry(name, 2);
			entry.Value = result.ToString();
			return (T)result;
		}

		public T ReadEnum8<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadByte());
			var entry = AddEntry(name, 1);
			entry.Value = result.ToString();
			return (T)result;
		}

		public string TryReadString(string name)
		{
			var length = _reader.ReadUInt32();
			string result = "";
			if(length > 0)
			{
				var bytes = _reader.ReadBytes((int)length);
				result = Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
			}
			var entry = AddEntry(name, length + 4);
			entry.Value = result;
			return result;
		}

		public string ReadString(string name)
		{
			var sb = new StringBuilder();
			char nextCharacter;
			while (!EndOfBuffer && (nextCharacter = _reader.ReadChar()) != 0)
			{
				sb.Append(nextCharacter);
			}
			var result = sb.ToString();
			var entry = AddEntry(name, (uint)result.Length + 1);
			entry.Value = result;
			return result;
		}
		public byte[] ReadBytes(string name, int count)
		{
			var result = _reader.ReadBytes(count);
			var entry = AddEntry(name, (uint)result.Length + 1);
			entry.Value = $"byte[{result.Length}] {FormatBytes(result)}";
			return result;
		}
		private string FormatBytes(byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("hex(");
			for (int i = 0; i < data.Length; i++)
			{
				var hex = data[i].ToString("X2");
				sb.Append(hex);
				if ((i + 1) % 4 == 0 && i != 0 && i < data.Length - 1) sb.Append(" ");
			}
			sb.Append("), chr(");
			for (int i = 0; i < data.Length; i++)
			{
				var c = (char)data[i];
				if (char.IsControl(c) || char.IsWhiteSpace(c))
				{
					sb.Append(".");
				}
				else
				{
					sb.Append(c);
				}
			}
			sb.Append(")");
			return sb.ToString();
		}
		public string DumpStructure()
		{
			var sb = new StringBuilder();
			foreach(var member in Members)
			{
				sb.Append(member.Dump());
			}
			sb.AppendLine();
			var entries = Members.OfType<DebugEntry>();
			var used = new bool[_buffer.Length];
			foreach(var entry in entries)
			{
				for(var i = entry.AbsoluteIndex; i < entry.AbsoluteIndex + entry.Size; i++)
				{
					used[i] = true;
				}
			}
			var sorted = entries
				.OrderBy(e => e.AbsoluteIndex)
				.ToList();
			for(int i = 0; i < used.Length; i++)
			{
				if (!used[i])
				{
					int next = 1;
					int fillerCount = _buffer[i] == 0xAB ? 1 : 0;
					while(i + next < used.Length && used[i + next] == false)
					{
						if (_buffer[i + next] == 0xAB) fillerCount++;
						next++;
					}
					//Strings are padded to 4 byte boundary with 0xAB
					if (fillerCount != next)
					{
						var closest = sorted
							.Last(e => e.AbsoluteIndex < i);
						var rel = i - (closest.AbsoluteIndex - closest.RelativeIndex);
						sb.Append($"Unread Memory {i}:{i + next - 1}[{rel}:{rel + next - 1}] (See {closest.AbsoluteIndex}:{closest.AbsoluteIndex + closest.Size} - {closest.Name})");
						var subset = _buffer.Skip(i).Take(next).ToArray();
						sb.Append(" ");
						sb.AppendLine(FormatBytes(subset));
					}
					i += next;
				}
			}
			return sb.ToString();
		}
		public string Dump()
		{
			var indent = new string(' ', (int)Indent * 2);
			string next = _offset + _buffer.Length == _buffer.Length ?
					"*" :
					(_offset + _buffer.Length - 1).ToString();
			var sb = new StringBuilder();
			sb.Append($"{indent}Container: {_name}");
			if (DumpOffsets)
			{
				sb.Append($"[{_offset}:{next}]");
			}
			sb.AppendLine();
			return sb.ToString();
		}
		public DebugBytecodeReader CopyAtCurrentPosition(string name, DebugBytecodeReader parent, int? count = null)
		{
			return CopyAtOffset(name, parent, (int)_reader.BaseStream.Position, count);
		}

		public DebugBytecodeReader CopyAtOffset(string name, DebugBytecodeReader parent, int offset, int? count = null)
		{
			count = count ?? (int)(_reader.BaseStream.Length - offset);
			var result = new DebugBytecodeReader(_buffer, _offset + offset, count.Value, _offset, parent.Indent + 1, name, Root);
			Members.Add(result);
			return result;
		}
	}
}