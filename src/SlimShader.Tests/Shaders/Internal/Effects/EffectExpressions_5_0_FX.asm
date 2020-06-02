//
// FX Version: fx_5_0
//
// 1 local buffer(s)
//
cbuffer $Globals
{
    uint    i;                          // Offset:    0, size:    4
    uint    j;                          // Offset:    4, size:    4
    int     k;                          // Offset:    8, size:    4
    int     l;                          // Offset:   12, size:    4
    float4  a;                          // Offset:   16, size:   16
    float4  b;                          // Offset:   32, size:   16
    float4  c;                          // Offset:   48, size:   16
    float3  f;                          // Offset:   64, size:   12
    float3  g;                          // Offset:   80, size:   12
    float4x4 m;                         // Offset:   96, size:   64
    Foo     bar;                        // Offset:  160, size:  112
}

//
// 3 local object(s)
//
DepthStencilState DepthStencilArray[3]
{
    {
        DepthFunc = uint(NEVER /* 1 */);
    },
    {
        DepthFunc = uint(LESS /* 2 */);
    },
    {
        DepthFunc = eval(iadd expr.x, i.x, (2));
    }
};
SamplerState samp
{
    BorderColor = eval(mul r0.x, a.x, m[3].z
                    mul r0.y, a.y, m[1].z
                    mul r0.z, a.z, m[2].z
                    mul r0.w, a.w, m[0].z
                    sin r1, r0
                    add expr.x, r1.x, bar[4].w
                    add expr0.y, r1.y, bar[4].y
                    add expr0.z, r1.z, bar[4].z
                    add expr0.w, r1.w, bar[4].x);
    MinLOD   = eval(dot r0, a, b
                    neg r0.y, c.x
                    add r1.x, r0.y, r0.x
                    mul r0.x, a.x, b.x
                    bige r0.y, l.x, k.x
                    movc expr.x, r0.y, r1.x, r0.x);
};
BlendState blend
{
    BlendEnable[0] = eval(mul r0.x, f.x, g.x
                    ftob expr.x, r0.x);
};

//
// 1 groups(s)
//
fxgroup
{
    //
    // 2 technique(s)
    //
    technique11 RenderSceneWithTexture1Light10_1
    {
        pass P0
        {
            DS_StencilRef = uint(5);
            DepthStencilState = DepthStencilArray[eval(imul r0.x, i.x, (3)
                            iadd expr.x, r0.x, j.x)];
        }

    }

    technique11 RenderSceneWithTexture1Light11_1
    {
        pass P0
        {
            DS_StencilRef = uint(5);
            DepthStencilState = DepthStencilArray[eval(imul r0.x, i.x, (3)
                            iadd expr.x, r0.x, j.x)];
        }

    }

}

