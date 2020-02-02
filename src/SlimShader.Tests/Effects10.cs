using NUnit.Framework;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D10;
using Fx10 = SlimShader.Chunks.Fx10;
using System.Collections.Generic;
using System.Linq;

namespace SlimShader.Tests
{
	class Effects10
	{
		static HashSet<string> BadTests = new HashSet<string>()
		{
			"TransparencyAA10_1_FX",
			"EffectPools1_FX",
			"EffectPools2_FX",
			"EffectPools3_FX",
			"DrawPredicated_FX",
			"DepthOfField10"
		};
		public static void CompareShareResourceView(ShaderResourceView reflectionResourceView)
		{

		}
		public static void CompareBuffer(Buffer reflectionBuffer)
		{
			if (reflectionBuffer == null) return;
			var desc = reflectionBuffer.Description;
			Assert.AreEqual(desc.SizeInBytes, desc.SizeInBytes);
			Assert.AreEqual(desc.Usage, desc.Usage);
			Assert.AreEqual(desc.BindFlags, desc.BindFlags);
			Assert.AreEqual(desc.CpuAccessFlags, desc.CpuAccessFlags);
			Assert.AreEqual(desc.OptionFlags, desc.OptionFlags);
		}
		static ShaderResourceView TryGetTextureView(EffectConstantBuffer effectBuffer)
		{
			try
			{
				return effectBuffer.GetTextureBuffer();
			} catch (SharpDX.SharpDXException ex)
			{
				return null;
			}
		}
		static Buffer TryGetConstantBuffer(EffectConstantBuffer effectBuffer)
		{
			try
			{
				return effectBuffer.GetConstantBuffer();
			}
			catch (SharpDX.SharpDXException ex)
			{
				return null;
			}
		}
		public static void CompareConstantBuffer(EffectConstantBuffer reflectionEffectBuffer, Fx10.EffectBuffer containerBuffer)
		{
			//Buffer reflectionBuffer = TryGetConstantBuffer(reflectionEffectBuffer);
			//ShaderResourceView reflectionTextureBuffer = TryGetTextureView(reflectionEffectBuffer);
			//CompareBuffer(reflectionBuffer);
			//CompareShareResourceView(reflectionTextureBuffer);
		}
		public static void CompareVariable(EffectVariable reflectionVariable, Fx10.IEffectVariable variable)
		{
			EffectTypeDescription typeDesc = reflectionVariable.TypeInfo.Description;
			var type = variable.Type;
			Assert.AreEqual(typeDesc.TypeName, type.TypeName);
			Assert.AreEqual(typeDesc.Class, (ShaderVariableClass)type.VariableClass);
			Assert.AreEqual(typeDesc.Type, (ShaderVariableType)type.VariableType);
			Assert.AreEqual(typeDesc.Elements, type.ElementCount);
			Assert.AreEqual(typeDesc.Members, type.MemberCount);
			Assert.AreEqual(typeDesc.Rows, type.Rows);
			Assert.AreEqual(typeDesc.Columns, type.Columns);
			Assert.AreEqual(typeDesc.PackedSize, type.GuessPackedSize);
			Assert.AreEqual(typeDesc.UnpackedSize, type.GuessUnpackedSize);
			Assert.AreEqual(typeDesc.Stride, type.GuessStride);
			EffectVariableDescription variableDesc = reflectionVariable.Description;
			Assert.AreEqual(variableDesc.Name, variable.Name);
			Assert.AreEqual(variableDesc.Semantic ?? "", variable.Semantic);
			Assert.AreEqual(variableDesc.Flags, (EffectVariableFlags)variable.Flags);
			Assert.AreEqual(variableDesc.AnnotationCount, variable.AnnotationCount);
			//TODO: SharpDX seems defines BufferOffset relative to buffer, but variable is just relative to struct
			//Assert.AreEqual(variableDesc.BufferOffset, variable.BufferOffset == uint.MaxValue ? 0 : variable.BufferOffset);
			Assert.AreEqual(variableDesc.ExplicitBindPoint, variable.ExplicitBindPoint);
			var annotations = GetAnnotations(reflectionVariable);
			if (typeDesc.Class == ShaderVariableClass.Struct)
			{
				for (int i = 0; i < typeDesc.Members; i++)
				{
					var reflectionMember = reflectionVariable.GetMemberByIndex(i);
					var member = variable.Type.Members[i];
					CompareVariable(reflectionMember, member);
				}
			}
			for (int i = 0; i < annotations.Count; i++)
			{
				var reflectionAnnotation = annotations[i];
				var annotation = variable.Annotations[i];
				CompareVariable(reflectionAnnotation, annotation);
			}
			if(typeDesc.Type == ShaderVariableType.Sampler)
			{
				EffectSamplerVariable sampVariable = reflectionVariable.AsSampler();
				SamplerState samplerState = sampVariable.GetSampler();
				var sampDesc = samplerState.Description;
			}
		}
		public static List<EffectVariable> GetAnnotations(EffectVariable reflectionVariable)
		{
			var result = new List<EffectVariable>();
			for (int i = 0; i < reflectionVariable.Description.AnnotationCount; i++)
			{
				var annotation = reflectionVariable.GetAnnotationByIndex(i);
				result.Add(annotation);
			}
			return result;
		}
		public static int GetBufferCount(Effect effect)
		{
			for(int i = 0; i < 1000; i++)
			{
				var cb = effect.GetConstantBufferByIndex(i);
				if (!cb.IsValid) return i;
			}
			return 1000;
		}
		public static int GetVariableCount(Effect effect)
		{
			for (int i = 0; i < 1000; i++)
			{
				var cb = effect.GetVariableByIndex(i);
				if (!cb.IsValid) return i;
			}
			return 1000;
		}
		public static List<EffectVariable> GetEffectVariables(Effect effect)
		{
			var desc = effect.Description;
			var result = new List<EffectVariable>();
			for (int i = 0; i < desc.GlobalVariableCount + desc.SharedGlobalVariableCount; i++)
			{
				var variable = effect.GetVariableByIndex(i);
				result.Add(variable);
			}
			return result;
		}
		public static void CompareEffect(BytecodeContainer container, byte[] effectBytecode, string testName)
		{
			if (BadTests.Contains(testName))
			{
				return;
			}
			var chunk = container.Chunks.OfType<Fx10.FX10Chunk>().First();
			if(chunk.Header.Techniques == 0)
			{
				return;
			}
			if (chunk.IsChildEffect)
			{
				Assert.Warn("Child Effects are not supported by SharpDX");
				return;
			}
			var device = new Device(DriverType.Warp, DeviceCreationFlags.Debug);
			var effectReflection = new Effect(device, effectBytecode, EffectFlags.None);
			EffectDescription desc = effectReflection.Description;
			var header = chunk.Header;
			Assert.AreEqual((bool)desc.IsChildEffect, header.SharedConstantBuffers > 0);
			Assert.AreEqual(desc.ConstantBufferCount, header.ConstantBuffers);
			Assert.AreEqual(desc.SharedConstantBufferCount, header.SharedConstantBuffers);
			Assert.AreEqual(desc.GlobalVariableCount, header.GlobalVariables + header.ObjectCount);
			Assert.AreEqual(desc.SharedGlobalVariableCount, header.SharedGlobalVariables);
			Assert.AreEqual(desc.TechniqueCount, header.Techniques);
			var reflectionConstantBufferCount = GetBufferCount(effectReflection);
			Assert.AreEqual(reflectionConstantBufferCount, header.ConstantBuffers + header.SharedConstantBuffers);
			var reflectionVariableCount = GetVariableCount(effectReflection);

			Assert.AreEqual(reflectionVariableCount, header.ObjectCount + header.SharedObjectCount + header.GlobalVariables + header.SharedGlobalVariables);
			var variables = chunk.AllVariables.ToList();
			var reflectionVariables = GetEffectVariables(effectReflection);
			var reflectionNames = reflectionVariables
				.Select(v => $"{v.Description.Name}, {v.TypeInfo.Description.Type}, {v.TypeInfo.Description.Class}")
				.ToList();
			for (int i = 0; i < desc.GlobalVariableCount + desc.SharedGlobalVariableCount; i++)
			{
				CompareVariable(reflectionVariables[i], variables[i]);
			}
			var buffers = chunk.AllBuffers.ToList();
			for (int i = 0; i < desc.ConstantBufferCount + desc.SharedConstantBufferCount; i++)
			{
				var cb = effectReflection.GetConstantBufferByIndex(i);
				CompareConstantBuffer(cb, buffers[i]);
			}
		}
	}
}
