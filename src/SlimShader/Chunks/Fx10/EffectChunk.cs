﻿using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Looks vaguely similar to D3D10_EFFECT_DESC and D3D10_STATE_BLOCK_MASK
	/// Some Notes:
	/// Cbuffer has a stride of 24 bytes
	/// Effect Variables have a stride of 28 (32 for sampler variables) bytes without annotations
	/// Effect Variables Types has as stride of 28 bytes
	/// Effect Annotation has a stride of 3 bytes
	/// Sampler Annotations have a stride of 4 bytes
	/// Sampler Annotation types have a stride of 12 bytes?
	/// Effect Techniques with 0 pass 0 shaders have a stride of 12 bytes
	/// Effect Techniques with 1 pass 0 shaders have a stride of 24 bytes
	/// Effect Techniques with 1 pass 2 shaders(1 vs, 1 ps) have a stride of 56 bytes
	/// Effect Pass with 0 shaders have a stride of 12 bytes
	/// Effect shaders have a stride of 12 bytes
	/// 
	/// /// </summary>
	public class EffectChunk : BytecodeChunk
	{
		public byte[] HeaderData;
		public byte[] BodyData;
		public byte[] FooterData;
		uint Size;
		public EffectHeader Header;
		public IEnumerable<EffectBuffer> AllBuffers
		{
			get
			{
				foreach (var buffer in LocalBuffers)
				{
					yield return buffer;
				}
				foreach (var buffer in SharedBuffers)
				{
					yield return buffer;
				}
			}
		}
		public IEnumerable<IEffectVariable> AllVariables
		{
			get
			{
				foreach (var buffer in LocalBuffers)
				{
					foreach (var variable in buffer.Variables)
					{
						yield return variable;
					}
				}
				foreach (var variable in LocalVariables)
				{
					yield return variable;
				}
				foreach (var buffer in SharedBuffers)
				{
					foreach (var variable in buffer.Variables)
					{
						yield return variable;
					}
				}
				foreach (var variable in SharedVariables)
				{
					yield return variable;
				}
			}
		}
		public bool IsChildEffect => SharedBuffers.Count > 0 || SharedVariables.Count > 0;
		public List<EffectBuffer> LocalBuffers { get; private set; }
		public List<EffectBuffer> SharedBuffers { get; private set; }
		public List<EffectObjectVariable> LocalVariables { get; private set; }
		public List<EffectObjectVariable> SharedVariables { get; private set; }
		public List<EffectInterfaceVariable> InterfaceVariables { get; private set; }
		public List<EffectTechnique> Techniques { get; private set; }
		/// <summary>
		/// Only used in fx_5_0
		/// </summary>
		public List<EffectGroup> Groups { get; private set; }
		string Error = "";
		public EffectChunk()
		{
			LocalBuffers = new List<EffectBuffer>();
			SharedBuffers = new List<EffectBuffer>();
			LocalVariables = new List<EffectObjectVariable>();
			SharedVariables = new List<EffectObjectVariable>();
			InterfaceVariables = new List<EffectInterfaceVariable>();
			Techniques = new List<EffectTechnique>();
			Groups = new List<EffectGroup>();
		}
		public static EffectChunk Parse(BytecodeReader reader, uint size)
		{
			var headerReader = reader.CopyAtCurrentPosition();
			var result = new EffectChunk();
			result.Size = size;
			var header = result.Header = EffectHeader.Parse(headerReader);
			var bodyOffset = header.Version.MajorVersion < 5 ?
				0x4C : 0x60;
			var footerOffset = (int)(result.Header.FooterOffset + bodyOffset);
			var bodyReader = reader.CopyAtOffset((int)bodyOffset);
			var footerReader = reader.CopyAtOffset(footerOffset);
			try
			{
				for (int i = 0; i < header.ConstantBuffers; i++)
				{
					result.LocalBuffers.Add(EffectBuffer.Parse(bodyReader, footerReader, false));
				}
				for (int i = 0; i < header.ObjectCount; i++)
				{
					result.LocalVariables.Add(EffectObjectVariable.Parse(bodyReader, footerReader, false));
				}
				for (int i = 0; i < header.SharedConstantBuffers; i++)
				{
					result.SharedBuffers.Add(EffectBuffer.Parse(bodyReader, footerReader, true));
				}
				for (int i = 0; i < header.SharedObjectCount; i++)
				{
					result.SharedVariables.Add(EffectObjectVariable.Parse(bodyReader, footerReader, true));
				}
				if (header.Version.MajorVersion >= 5)
				{
					for (int i = 0; i < header.InterfaceVariableCount; i++)
					{
						result.InterfaceVariables.Add(EffectInterfaceVariable.Parse(bodyReader, footerReader));
					}
					for (int i = 0; i < header.GroupCount; i++)
					{
						result.Groups.Add(EffectGroup.Parse(bodyReader, footerReader));
					}
				}
				else
				{
					for (int i = 0; i < header.Techniques; i++)
					{
						result.Techniques.Add(EffectTechnique.Parse(bodyReader, footerReader));
					}
				}
			}
			catch (Exception ex)
			{
				result.Error = ex.ToString();
				//throw;
			}

			var headerDataReader = reader.CopyAtCurrentPosition();
			result.HeaderData = headerDataReader.ReadBytes((int)bodyOffset);

			var bodyDataReader = reader.CopyAtOffset(bodyOffset);
			result.BodyData = bodyDataReader.ReadBytes((int)footerOffset - bodyOffset);

			var footerDataReader = reader.CopyAtOffset(footerOffset);
			result.FooterData = footerDataReader.ReadBytes((int)(size - footerOffset));

			return result;
		}
		public static string FormatReadable(byte[] data, bool endian = false)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
				for (int j = i; j < i + 16; j++)
				{
					var index = endian ? j : j + (3 - (j % 4) * 2);
					if (index < data.Length)
					{
						sb.Append(data[index].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append("  ");
					}
				}
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (char.IsControl(c))
					{
						sb.Append("_");
					}
					else if (c > 0x7E)
					{
						sb.Append('.');
					}
					else if (char.IsWhiteSpace(c))
					{
						sb.Append('.');
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public string Dump()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"Size: {Size}");
			sb.AppendLine("Header:");
			sb.AppendLine(Header.ToString());
			sb.AppendLine("Foo:");
			foreach (var buffer in LocalBuffers)
			{
				sb.AppendLine(buffer.Dump());
			}
			foreach (var variable in LocalVariables)
			{
				sb.AppendLine(variable.Dump());
			}
			foreach (var variable in SharedBuffers)
			{
				sb.AppendLine(variable.Dump());
			}
			foreach (var variable in SharedVariables)
			{
				sb.AppendLine(variable.Dump());
			}
			foreach (var variable in InterfaceVariables)
			{
				sb.AppendLine(variable.Dump());
			}
			foreach (var technique in Techniques)
			{
				sb.AppendLine(technique.Dump());
			}
			foreach (var group in Groups)
			{
				sb.AppendLine(group.Dump());
			}

			sb.AppendLine("FooterData:");
			sb.AppendLine(FormatReadable(FooterData));

			sb.AppendLine("HeaderData:");
			sb.AppendLine(FormatReadable(HeaderData));

			sb.AppendLine("BodyData:");
			sb.AppendLine(FormatReadable(BodyData));

			if (!string.IsNullOrEmpty(Error))
			{
				sb.AppendLine(Error);
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("//");
			sb.AppendLine(string.Format("// FX Version: {0}", Header.Version));
			if (Header.Version.MajorVersion == 4)
			{
				sb.AppendLine(string.Format("// Child effect (requires effect pool): {0}", 
						IsChildEffect.ToString().ToLowerInvariant()));
			}
			sb.AppendLine("//");
			if(LocalBuffers.Count > 0)
			{
				sb.AppendLine(string.Format("// {0} local buffer(s)", LocalBuffers.Count));
				sb.AppendLine("//");
				foreach(var buffer in LocalBuffers)
				{
					sb.AppendLine(buffer.ToString());
				}
			}
			if (LocalVariables.Count > 0)
			{
				sb.AppendLine("//");
				sb.AppendLine(string.Format("// {0} local object(s)", LocalVariables.Count));
				sb.AppendLine("//");
				foreach (var variable in LocalVariables)
				{
					sb.AppendLine(variable.ToString());
				}
			}
			if (SharedBuffers.Count > 0)
			{
				sb.AppendLine(string.Format("// {0} shared buffer(s)", SharedBuffers.Count));
				sb.AppendLine("//");
				foreach (var buffer in SharedBuffers)
				{
					sb.AppendLine(buffer.ToString());
				}
			}
			if (SharedVariables.Count > 0)
			{
				sb.AppendLine("//");
				sb.AppendLine(string.Format("// {0} shared object(s)", SharedVariables.Count));
				sb.AppendLine("//");
				foreach (var variable in SharedVariables)
				{
					sb.AppendLine(variable.ToString());
				}
			}
			if (InterfaceVariables.Count > 0)
			{
				sb.AppendLine("//");
				sb.AppendLine(string.Format("// {0} local interface(s)", InterfaceVariables.Count));
				sb.AppendLine("//");
				foreach (var variable in InterfaceVariables)
				{
					sb.AppendLine(variable.ToString());
				}
				sb.AppendLine();
			}
			if (Techniques.Count > 0)
			{
				sb.AppendLine("//");
				sb.AppendLine(string.Format("// {0} technique(s)", Techniques.Count));
				sb.AppendLine("//");
				foreach (var technique in Techniques)
				{
					sb.AppendLine(technique.ToString());
				}
			}
			if (Groups.Count > 0)
			{
				sb.AppendLine("//");
				sb.AppendLine(string.Format("// {0} groups(s)", Groups.Count));
				sb.AppendLine("//");
				foreach (var group in Groups)
				{
					sb.AppendLine(group.ToString());
				}
			}
			return sb.ToString();
		}
	}
}
