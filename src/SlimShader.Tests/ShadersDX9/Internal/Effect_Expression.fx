int CurNumBones = 2;
float light;
struct VS_OUTPUT {
    float4  Pos : POSITION;
};
VS_OUTPUT VertSkinning( uniform int iNumBones )
{
    VS_OUTPUT   o;
    o.Pos = 5;
    return o;
}
VertexShader vsArray20[ 2 ] = { compile vs_2_0 VertSkinning( 1 ),
                                compile vs_2_0 VertSkinning( 2 ) };

void RenderSceneVS(out float4 oPos : POSITION,
	float4 vPos : POSITION,
	float3 vNormal : NORMAL,
	float2 vTexCoord0 : TEXCOORD0,
	uniform int nNumLights,
	uniform bool bTexture,
	uniform bool bAnimate)
{
	oPos = 0;
}

technique Technique1 {
    pass P0 {
        VertexShader = ( vsArray20[ CurNumBones ] );
        LightAttenuation0[0] = light;
    }
	pass P1
	{
		VertexShader = compile vs_2_0 RenderSceneVS(1, false, false);
	}
};