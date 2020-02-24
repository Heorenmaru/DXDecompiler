
//listing of all techniques and passes with embedded asm listings 

technique RenderWithNdotL
{
    pass P0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 LightsDiffuse[10];
            //   float4 LightsDir[10];
            //   float4 MaterialDiffuseColor;
            //   float4x4 g_mWorldInv;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mWorldInv            c4       3
            //   MaterialDiffuseColor   c7       1
            //   LightsDir              c10     10
            //   LightsDiffuse          c20     10
            //
            
                vs_2_0
                def c8, 0, 0, 0, 0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                mov r0, c4
                dp4 r1.x, c11, r0
                mov r2, c5
                dp4 r1.y, c11, r2
                mov r3, c6
                dp4 r1.z, c11, r3
                dp3 r1.x, v1, r1
                max r1.x, r1.x, c8.x
                mul r1, r1.x, c21
                dp4 r4.x, c10, r0
                dp4 r4.y, c10, r2
                dp4 r4.z, c10, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c20, r4.x, r1
                dp4 r4.x, c12, r0
                dp4 r4.y, c12, r2
                dp4 r4.z, c12, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c22, r4.x, r1
                dp4 r4.x, c13, r0
                dp4 r4.y, c13, r2
                dp4 r4.z, c13, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c23, r4.x, r1
                dp4 r4.x, c14, r0
                dp4 r4.y, c14, r2
                dp4 r4.z, c14, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c24, r4.x, r1
                dp4 r4.x, c15, r0
                dp4 r4.y, c15, r2
                dp4 r4.z, c15, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c25, r4.x, r1
                dp4 r4.x, c16, r0
                dp4 r4.y, c16, r2
                dp4 r4.z, c16, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c26, r4.x, r1
                dp4 r4.x, c17, r0
                dp4 r4.y, c17, r2
                dp4 r4.z, c17, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c27, r4.x, r1
                dp4 r4.x, c18, r0
                dp4 r4.y, c18, r2
                dp4 r4.z, c18, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c28, r4.x, r1
                dp4 r0.x, c19, r0
                dp4 r0.y, c19, r2
                dp4 r0.z, c19, r3
                dp3 r0.x, v1, r0
                max r0.x, r0.x, c8.x
                mad r0, c29, r0.x, r1
                mul oD0, r0, c7
                mov oT0.xy, v2
            
            // approximately 69 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D AlbedoSampler;
            //
            //
            // Registers:
            //
            //   Name          Reg   Size
            //   ------------- ----- ----
            //   AlbedoSampler s0       1
            //
            
                ps_2_0
                dcl v0
                dcl t0.xy
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

technique RenderWithNdotLNoAlbedo
{
    pass P0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 LightsDiffuse[10];
            //   float4 LightsDir[10];
            //   float4 MaterialDiffuseColor;
            //   float4x4 g_mWorldInv;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mWorldInv            c4       3
            //   MaterialDiffuseColor   c7       1
            //   LightsDir              c10     10
            //   LightsDiffuse          c20     10
            //
            
                vs_2_0
                def c8, 0, 0, 0, 0
                dcl_position v0
                dcl_normal v1
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                mov r0, c4
                dp4 r1.x, c11, r0
                mov r2, c5
                dp4 r1.y, c11, r2
                mov r3, c6
                dp4 r1.z, c11, r3
                dp3 r1.x, v1, r1
                max r1.x, r1.x, c8.x
                mul r1, r1.x, c21
                dp4 r4.x, c10, r0
                dp4 r4.y, c10, r2
                dp4 r4.z, c10, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c20, r4.x, r1
                dp4 r4.x, c12, r0
                dp4 r4.y, c12, r2
                dp4 r4.z, c12, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c22, r4.x, r1
                dp4 r4.x, c13, r0
                dp4 r4.y, c13, r2
                dp4 r4.z, c13, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c23, r4.x, r1
                dp4 r4.x, c14, r0
                dp4 r4.y, c14, r2
                dp4 r4.z, c14, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c24, r4.x, r1
                dp4 r4.x, c15, r0
                dp4 r4.y, c15, r2
                dp4 r4.z, c15, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c25, r4.x, r1
                dp4 r4.x, c16, r0
                dp4 r4.y, c16, r2
                dp4 r4.z, c16, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c26, r4.x, r1
                dp4 r4.x, c17, r0
                dp4 r4.y, c17, r2
                dp4 r4.z, c17, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c27, r4.x, r1
                dp4 r4.x, c18, r0
                dp4 r4.y, c18, r2
                dp4 r4.z, c18, r3
                dp3 r4.x, v1, r4
                max r4.x, r4.x, c8.x
                mad r1, c28, r4.x, r1
                dp4 r0.x, c19, r0
                dp4 r0.y, c19, r2
                dp4 r0.z, c19, r3
                dp3 r0.x, v1, r0
                max r0.x, r0.x, c8.x
                mad r0, c29, r0.x, r1
                mul oD0, r0, c7
                mov oT0.xy, c8.x
            
            // approximately 69 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                ps_2_0
                dcl v0
                mov oC0, v0
            
            // approximately 1 instruction slot used
            };
    }
}

