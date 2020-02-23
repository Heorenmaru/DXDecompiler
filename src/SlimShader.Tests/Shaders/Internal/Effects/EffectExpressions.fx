//Test fx expressions such as i + 2 and i * 3 + j

uint i;
uint j;
int k;
int l;
float4 a;
float4 b;
float4 c;
float3 f;
float3 g;
float4x4 m;
struct Foo {
	float4 v1;
	uint3 v2;
	int2 v3;
	float3 v4;
	float4 v5;
	bool v6;
	float4 v7;
};
Foo bar;
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
float4 func() {
	if (k > l) {
		return a * b;
	}
	return dot(a, b) - c;
}
SamplerState samp
{
	BorderColor = sin(a * m[2].wyzx) + bar.v5.wyzx;
	MinLOD = func();
};

BlendState blend
{
	BlendEnable[0] = f * g;
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