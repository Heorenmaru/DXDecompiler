//Test fx expressions such as i + 2 and i * 3 + j
uint i;
uint j;
DepthStencilState DepthStencilArray[3]
{
	{
		DepthFunc = 1;
	},
	{
		DepthFunc = 2;
	},
	{
		DepthFunc = i + 2;
	}
};

technique10 RenderSceneWithTexture1Light10_1
{
	pass P0
	{
		SetDepthStencilState(DepthStencilArray[i * 3 + j], 5);
	}
}
technique11 RenderSceneWithTexture1Light11_1
{
	pass P0
	{
		SetDepthStencilState(DepthStencilArray[i * 3 + j], 5);
	}
}