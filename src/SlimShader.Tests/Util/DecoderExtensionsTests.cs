﻿using NUnit.Framework;
using SlimShader.Chunks.Shex;
using SlimShader.Util;
using System;
using System.IO;

namespace SlimShader.Tests.Util
{
	[TestFixture]
	public class DecoderExtensionsTests
	{
		[Test]
		public void CanDecodeSamplerMode()
		{
			// Arrange.
			const uint codedValue = 67115096;

			// Act.
			var decodedValue = (ResourceDimension) codedValue.DecodeValue(11, 15);

			// Assert.
			Assert.That(decodedValue, Is.EqualTo(ResourceDimension.Texture2D));
		}
		[Test]
		public void TestEnums()
		{
			string text = "";
			foreach (var _enum in Enum.GetValues(typeof(DX9Shader.Opcode)))
			{
				int val = (int)_enum;
				text += $"{_enum} = 0x{val.ToString("X")}\n";
			}
			File.WriteAllText($"{TestContext.CurrentContext.TestDirectory}/Enums.txt", text);
		}
	}
}