
//listing of all techniques and passes with embedded asm listings 

technique WorldWithBlurFactor
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
            //   float4 LightAmbient;
            //   float4 LightDiffuse;
            //   float4 MaterialAmbientColor;
            //   float4 MaterialDiffuseColor;
            //
            //
            // Registers:
            //
            //   Name                 Reg   Size
            //   -------------------- ----- ----
            //   MaterialAmbientColor c0       1
            //   MaterialDiffuseColor c1       1
            //   LightAmbient         c2       1
            //   LightDiffuse         c3       1
            //
            
                preshader
                mul c10.xyz, c1.xyz, c3.xyz
                mul c11.xyz, c0.xyz, c2.xyz
            
            // approximately 2 instructions used
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3 LightDir;
            //   float MaxBlurFactor;
            //   float fHyperfocalDistance;
            //   float4x4 mWorld;
            //   float4x4 mWorldView;
            //   float4x4 mWorldViewProjection;
            //   float4 vFocalPlane;
            //
            //
            // Registers:
            //
            //   Name                 Reg   Size
            //   -------------------- ----- ----
            //   mWorldViewProjection c0       4
            //   mWorld               c4       3
            //   mWorldView           c7       3
            //   LightDir             c12      1
            //   vFocalPlane          c13      1
            //   fHyperfocalDistance  c14      1
            //   MaxBlurFactor        c15      1
            //
            
                vs_2_0
                def c16, 0, 1, 0, 0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 r0.x, v1, c4
                dp3 r0.y, v1, c5
                dp3 r0.z, v1, c6
                dp3 r0.x, r0, c12
                max r0.x, r0.x, c16.x
                mov r1.xyz, c10
                mad oD0.xyz, r1, r0.x, c11
                dp4 r0.x, v0, c7
                dp4 r0.y, v0, c8
                dp4 r0.z, v0, c9
                mov r0.w, c16.y
                dp4 r0.x, r0, c13
                mul r0.x, r0.x, c14.x
                mul r0.x, r0.x, r0.x
                min oD0.w, r0.x, c15.x
                mov oT0.xy, v2
            
            // approximately 20 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D MeshTextureSampler;
            //
            //
            // Registers:
            //
            //   Name               Reg   Size
            //   ------------------ ----- ----
            //   MeshTextureSampler s0       1
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

technique UsePS20ThirteenLookups
{
    pass P0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D RenderTargetSampler;
            //   float2 TwelveKernel[12];
            //
            //
            // Registers:
            //
            //   Name                Reg   Size
            //   ------------------- ----- ----
            //   TwelveKernel        c0      12
            //   RenderTargetSampler s0       1
            //
            
                ps_2_0
                def c12, 0.0833333358, 1, 0, 0
                dcl t0.xy
                dcl_2d s0
                add r0.xy, t0, c0
                add r1.xy, t0, c1
                add r2.xy, t0, c2
                add r3.xy, t0, c3
                add r4.xy, t0, c4
                add r5.xy, t0, c5
                add r6.xy, t0, c6
                add r7.xy, t0, c7
                add r8.xy, t0, c8
                texld r0, r0, s0
                texld r9, t0, s0
                texld r1, r1, s0
                texld r2, r2, s0
                texld r3, r3, s0
                texld r4, r4, s0
                texld r5, r5, s0
                texld r6, r6, s0
                texld r7, r7, s0
                texld r8, r8, s0
                mul_sat r0.w, r0.w, r9.w
                lrp r10.xyz, r0.w, r0, r9
                mul_sat r1.w, r1.w, r9.w
                lrp r0.xyz, r1.w, r1, r9
                add r0.xyz, r0, r10
                mul_sat r0.w, r2.w, r9.w
                lrp r1.xyz, r0.w, r2, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r3.w, r9.w
                lrp r1.xyz, r0.w, r3, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r4.w, r9.w
                lrp r1.xyz, r0.w, r4, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r5.w, r9.w
                lrp r1.xyz, r0.w, r5, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r6.w, r9.w
                lrp r1.xyz, r0.w, r6, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r7.w, r9.w
                lrp r1.xyz, r0.w, r7, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r8.w, r9.w
                lrp r1.xyz, r0.w, r8, r9
                add r0.xyz, r0, r1
                add r1.xy, t0, c9
                add r2.xy, t0, c10
                add r3.xy, t0, c11
                texld r1, r1, s0
                texld r2, r2, s0
                texld r3, r3, s0
                mul_sat r0.w, r1.w, r9.w
                lrp r4.xyz, r0.w, r1, r9
                add r0.xyz, r0, r4
                mul_sat r0.w, r2.w, r9.w
                lrp r1.xyz, r0.w, r2, r9
                add r0.xyz, r0, r1
                mul_sat r0.w, r3.w, r9.w
                lrp r1.xyz, r0.w, r3, r9
                add r0.xyz, r0, r1
                mul r0.xyz, r0, c12.x
                mov r0.w, c12.y
                mov oC0, r0
            
            // approximately 63 instruction slots used (13 texture, 50 arithmetic)
            };
    }
}

technique UsePS20SevenLookups
{
    pass P0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D RenderTargetSampler;
            //   float2 TwelveKernel[12];
            //
            //
            // Registers:
            //
            //   Name                Reg   Size
            //   ------------------- ----- ----
            //   TwelveKernel        c0       6
            //   RenderTargetSampler s0       1
            //
            
                ps_2_0
                def c6, 0.166666672, 1, 0, 0
                dcl t0.xy
                dcl_2d s0
                add r0.xy, t0, c0
                add r1.xy, t0, c1
                add r2.xy, t0, c2
                add r3.xy, t0, c3
                add r4.xy, t0, c4
                add r5.xy, t0, c5
                texld r0, r0, s0
                texld r6, t0, s0
                texld r1, r1, s0
                texld r2, r2, s0
                texld r3, r3, s0
                texld r4, r4, s0
                texld r5, r5, s0
                mul_sat r0.w, r0.w, r6.w
                lrp r7.xyz, r0.w, r0, r6
                mul_sat r1.w, r1.w, r6.w
                lrp r0.xyz, r1.w, r1, r6
                add r0.xyz, r0, r7
                mul_sat r0.w, r2.w, r6.w
                lrp r1.xyz, r0.w, r2, r6
                add r0.xyz, r0, r1
                mul_sat r0.w, r3.w, r6.w
                lrp r1.xyz, r0.w, r3, r6
                add r0.xyz, r0, r1
                mul_sat r0.w, r4.w, r6.w
                lrp r1.xyz, r0.w, r4, r6
                add r0.xyz, r0, r1
                mul_sat r0.w, r5.w, r6.w
                lrp r1.xyz, r0.w, r5, r6
                add r0.xyz, r0, r1
                mul r0.xyz, r0, c6.x
                mov r0.w, c6.y
                mov oC0, r0
            
            // approximately 33 instruction slots used (7 texture, 26 arithmetic)
            };
    }
}

technique UsePS20SixTexcoords
{
    pass P0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D RenderTargetSampler;
            //
            //
            // Registers:
            //
            //   Name                Reg   Size
            //   ------------------- ----- ----
            //   RenderTargetSampler s0       1
            //
            
                ps_2_0
                def c0, 0.200000003, 1, 0, 0
                dcl t1.xy
                dcl t2.xy
                dcl t3.xy
                dcl t4.xy
                dcl t5.xy
                dcl t0.xy
                dcl_2d s0
                texld r0, t1, s0
                texld r1, t0, s0
                texld r2, t2, s0
                texld r3, t3, s0
                texld r4, t4, s0
                texld r5, t5, s0
                mul_sat r0.w, r0.w, r1.w
                lrp r6.xyz, r0.w, r0, r1
                mul_sat r2.w, r1.w, r2.w
                lrp r0.xyz, r2.w, r2, r1
                add r0.xyz, r0, r6
                mul_sat r0.w, r1.w, r3.w
                lrp r2.xyz, r0.w, r3, r1
                add r0.xyz, r0, r2
                mul_sat r0.w, r1.w, r4.w
                lrp r2.xyz, r0.w, r4, r1
                add r0.xyz, r0, r2
                mul_sat r0.w, r1.w, r5.w
                lrp r2.xyz, r0.w, r5, r1
                add r0.xyz, r0, r2
                mul r0.xyz, r0, c0.x
                mov r0.w, c0.y
                mov oC0, r0
            
            // approximately 23 instruction slots used (6 texture, 17 arithmetic)
            };
    }
}

technique ShowBlurFactor
{
    pass P0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D RenderTargetSampler;
            //
            //
            // Registers:
            //
            //   Name                Reg   Size
            //   ------------------- ----- ----
            //   RenderTargetSampler s0       1
            //
            
                ps_2_0
                def c0, 1, 0, 0, 0
                dcl t0.xy
                dcl_2d s0
                texld r0, t0, s0
                add r0, -r0.w, c0.x
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

technique ShowUnmodified
{
    pass P0
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D RenderTargetSampler;
            //
            //
            // Registers:
            //
            //   Name                Reg   Size
            //   ------------------- ----- ----
            //   RenderTargetSampler s0       1
            //
            
                ps_2_0
                dcl t0.xy
                dcl_2d s0
                texld r0, t0, s0
                mov oC0, r0
            
            // approximately 2 instruction slots used (1 texture, 1 arithmetic)
            };
    }
}

