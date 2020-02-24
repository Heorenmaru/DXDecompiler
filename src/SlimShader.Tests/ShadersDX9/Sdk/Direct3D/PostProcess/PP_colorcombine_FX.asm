
//listing of all techniques and passes with embedded asm listings 

technique PostProcess
{
    pass p0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D g_samSceneColor;
            //   sampler2D g_samSrcColor;
            //
            //
            // Registers:
            //
            //   Name            Reg   Size
            //   --------------- ----- ----
            //   g_samSrcColor   s0       1
            //   g_samSceneColor s1       1
            //
            
                ps_2_0
                def c0, 1, 0, 0, 0
                dcl t0.xy
                dcl t1.xy
                dcl_2d s0
                dcl_2d s1
                texld r0, t1, s1
                texld r1, t0, s0
                add r0.xyz, r0, r1
                mov r0.w, c0.x
                mov oC0, r0
            
            // approximately 5 instruction slots used (2 texture, 3 arithmetic)
            };
    }
}

