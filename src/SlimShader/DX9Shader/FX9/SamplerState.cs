using SlimShader.Util;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	/// <summary>
	/// TODO: Merge With assignments
	/// </summary>
	public class SamplerState
	{
		public StateType Type;
		public uint ArrayIndex;
		public uint TypeOffset;
		public uint ValueOffset;

		public Parameter Variable;
		public UnknownObject Value;
		public static SamplerState Parse(BytecodeReader reader, BytecodeReader stateReader)
		{
			var result = new SamplerState();
			result.Type = (StateType)stateReader.ReadUInt32();
			result.ArrayIndex = stateReader.ReadUInt32();
			result.TypeOffset = stateReader.ReadUInt32();
			result.ValueOffset = stateReader.ReadUInt32();

			var variableReader = reader.CopyAtOffset((int)result.TypeOffset);
			result.Variable = Parameter.Parse(reader, variableReader);

			var unknownReader = reader.CopyAtOffset((int)result.ValueOffset);
			result.Value = UnknownObject.Parse(unknownReader, 1);
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"    SamplerState.Type: {Type} {((int)Type).ToString("X4")}");
			sb.AppendLine($"    SamplerState.ArrayIndex: {ArrayIndex} {ArrayIndex.ToString("X4")}");
			sb.AppendLine($"    SamplerState.TypeOffset: {TypeOffset} {TypeOffset.ToString("X4")}");
			sb.AppendLine($"    SamplerState.ValueOffset: {ValueOffset} {ValueOffset.ToString("X4")}");
			sb.Append(Variable.Dump());
			sb.Append(Value.Dump());
			return sb.ToString();
		}
	}
}
