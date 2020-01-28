using NUnit.Framework;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Tests
{
	//Refer https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/using-shader-linking
	[TestFixture]
	public class TestLibraryShader
	{
		[Test]
		public void DoTest()
		{
			var text = @"
export float3 TestFunction(float3 input)
{
	return input * 2.0f;
}";
			var compilationResult = ShaderBytecode.Compile(text, "lib_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None);
			var bytecode = compilationResult.Bytecode;
			var shaderLibrary = new Module(bytecode);
			// create the shader library module instance
			var shaderLibraryInstance = new ModuleInstance(shaderLibrary);

			// mark the implicit constant buffer (for single parameter) as bindable
			shaderLibraryInstance.BindConstantBuffer(0, 0, 0);

			var vertexShaderGraph = new FunctionLinkingGraph();
			/*typedef struct _D3D11_PARAMETER_DESC {
  LPCSTR                    Name;
  LPCSTR                    SemanticName;
  D3D_SHADER_VARIABLE_TYPE  Type;
  D3D_SHADER_VARIABLE_CLASS Class;
  UINT                      Rows;
  UINT                      Columns;
  D3D_INTERPOLATION_MODE    InterpolationMode;
  D3D_PARAMETER_FLAGS       Flags;
  UINT                      FirstInRegister;
  UINT                      FirstInComponent;
  UINT                      FirstOutRegister;
  UINT                      FirstOutComponent;
} D3D11_PARAMETER_DESC;*/
			/*vertexShaderGraph.SetInputSignature(
				new ParameterDescription()
				{ Name = "inputPos", SemanticName = "POSITION0", Type = ShaderVariableType.Float, Class = ShaderVariableClass.})
			var linker = new Linker();
			linker.Link(shaderLibraryInstance, "TestFunction", )*/

		}
	}
}
