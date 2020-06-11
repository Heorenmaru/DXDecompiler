using System.Diagnostics;

namespace SlimShader.DebugParser.DX9
{
	public class DebugPrsiToken
	{
		/*
		 * PRSI Tokens map preshader outputs to ConstantBuffer addresses
		 * 
		 */ 
		public static DebugPrsiToken Parse(DebugBytecodeReader reader, uint size)
		{
			//Size appears to be 12 for VS shaders and 14 for PS shaders
			var result = new DebugPrsiToken();
			Debug.Assert(size > 6, "PRSI size too small");
			reader.ReadUInt32("Unk1");
			reader.ReadUInt32("Unk2");
			reader.ReadUInt32("Unk3");
			reader.ReadUInt32("OutputRegisterCount?");
			reader.ReadUInt32("Unk4");
			reader.ReadUInt32("Unk5");
			var mappingCount = reader.ReadUInt32("MappingCount");
			reader.ReadUInt32("Unk6");
			reader.ReadUInt32("Unk7");
			for(int i = 0; i < mappingCount; i++)
			{
				reader.AddIndent($"Mapping{i}");
				reader.ReadUInt32("ConstOutput");
				reader.ReadUInt32("ConstInput");
				reader.RemoveIndent();
			}
			//if(size != 12) throw new System.Exception($"PRSI TOKEN Size {size}");
			//Debug.Assert(true, "PRSI Token");
			return result;
		}
	}
}
