int CurNumBones = 2;
float light;
struct VS_OUTPUT {
    float4  Pos     : POSITION;
};
VS_OUTPUT VertSkinning( uniform int iNumBones )
{
    VS_OUTPUT   o;
    o.Pos = 5;
    return o;
}
VertexShader vsArray20[ 2 ] = { compile vs_2_0 VertSkinning( 1 ),
                                compile vs_2_0 VertSkinning( 2 ) };

technique Technique1 {
    pass {
        VertexShader = ( vsArray20[ CurNumBones ] );
        LightAttenuation0[0] = light;
    }
};