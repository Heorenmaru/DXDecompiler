using SlimShader.Util;
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
	/// 
	/// /// </summary>
	public class FX11Chunk : BytecodeChunk
	{
		public byte[] HeaderData;
		public byte[] BodyData;
		public byte[] FooterData;
		public byte[] Data;
		uint Size;
		public FX10Header Header;
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
		public List<EffectTexture> LocalVariables { get; private set; }
		public List<EffectTexture> SharedVariables { get; private set; }
		public List<EffectTechnique> Techniques { get; private set; }
		string Error = "";
		public FX11Chunk()
		{
			LocalBuffers = new List<EffectBuffer>();
			SharedBuffers = new List<EffectBuffer>();
			LocalVariables = new List<EffectTexture>();
			SharedVariables = new List<EffectTexture>();
			Techniques = new List<EffectTechnique>();
		}
		public static FX11Chunk Parse(BytecodeReader reader, uint size)
		{
			var headerReader = reader.CopyAtCurrentPosition();
			var result = new FX11Chunk();
			result.Size = size;
			var header = result.Header = FX10Header.Parse(headerReader);
			var footerOffset = (int)(result.Header.FooterOffset + 0x4C);
			var bodyReader = reader.CopyAtOffset((int)0x4C);
			var footerReader = reader.CopyAtOffset(footerOffset);
			try
			{
				for (int i = 0; i < header.ConstantBuffers; i++)
				{
					result.LocalBuffers.Add(EffectBuffer.Parse(bodyReader, footerReader, false));
				}
				for (int i = 0; i < header.ObjectCount; i++)
				{
					result.LocalVariables.Add(EffectTexture.Parse(bodyReader, footerReader, false));
				}
				for (int i = 0; i < header.SharedConstantBuffers; i++)
				{
					result.SharedBuffers.Add(EffectBuffer.Parse(bodyReader, footerReader, true));
				}
				for (int i = 0; i < header.SharedObjectCount; i++)
				{
					result.SharedVariables.Add(EffectTexture.Parse(bodyReader, footerReader, true));
				}
				for (int i = 0; i < header.Techniques; i++)
				{
					result.Techniques.Add(EffectTechnique.Parse(bodyReader, footerReader));
				}
			}
			catch (Exception ex)
			{
				result.Error = ex.ToString();
				//throw;
			}

			var headerDataReader = reader.CopyAtCurrentPosition();
			result.HeaderData = headerDataReader.ReadBytes((int)0x4C);

			var bodyDataReader = reader.CopyAtOffset(0x4C);
			result.BodyData = bodyDataReader.ReadBytes((int)footerOffset - 0x4C);

			var footerDataReader = reader.CopyAtOffset(footerOffset);
			result.FooterData = footerDataReader.ReadBytes((int)(size - footerOffset));

			var dataReader = reader.CopyAtCurrentPosition();
			result.Data = dataReader.ReadBytes((int)size);
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
		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"Size: {Size}");
			sb.AppendLine("Header:");
			sb.AppendLine(Header.ToString());
			sb.AppendLine("Foo:");
			foreach (var buffer in LocalBuffers)
			{
				sb.AppendLine(buffer.ToString());
			}
			foreach (var buffer in LocalVariables)
			{
				sb.AppendLine(buffer.ToString());
			}
			foreach (var buffer in SharedBuffers)
			{
				sb.AppendLine(buffer.ToString());
			}
			foreach (var buffer in SharedVariables)
			{
				sb.AppendLine(buffer.ToString());
			}
			foreach (var technique in Techniques)
			{
				sb.AppendLine(technique.ToString());
			}

			sb.AppendLine("Data:");
			sb.AppendLine(FormatReadable(Data));

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
	}
}
