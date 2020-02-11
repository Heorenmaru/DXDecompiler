//
// FX Version: fx_5_0
//
// 1 local buffer(s)
//
cbuffer $Globals
{
    uint    i;                          // Offset:    0, size:    4
    uint    j;                          // Offset:    4, size:    4
}

//
// 1 local object(s)
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
