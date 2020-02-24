
//listing of all techniques and passes with embedded asm listings 

technique RenderCubic
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
            
            // approximately 11 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       4
            //   GLight         c4       4
            //   BLight         c8       4
            //   CLinBFSampler  s0       1
            //   QuadBFSampler  s1       1
            //   CubeBFASampler s2       1
            //   CubeBFBSampler s3       1
            //
            
                ps_2_0
                def c12, 1, 0, 0, 0
                dcl t0.xyz
                dcl t2
                dcl t3
                dcl t4
                dcl_cube s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                texld r0, t0, s0
                texld r1, t0, s1
                texld r2, t0, s2
                texld r3, t0, s3
                mov r4.w, c12.x
                mul r5.yzw, r0, t2.y
                mul r5.x, r0.x, t2.x
                dp4 r5.x, r5, c0
                mul r6, r1, t2.z
                dp4 r5.y, r6, c1
                add r5.x, r5.y, r5.x
                mul r6.yzw, r2, t2.w
                mul r6.x, r2.x, t2.z
                dp4 r5.y, r6, c2
                add r5.x, r5.y, r5.x
                mul r6, r3, t2.w
                dp4 r5.y, r6, c3
                add r4.x, r5.y, r5.x
                mul r5.yzw, r0, t3.y
                mul r5.x, r0.x, t3.x
                mul r6.yzw, r0, t4.y
                mul r6.x, r0.x, t4.x
                dp4 r0.x, r6, c8
                dp4 r0.y, r5, c4
                mul r5, r1, t3.z
                mul r1, r1, t4.z
                dp4 r0.z, r1, c9
                add r0.x, r0.z, r0.x
                dp4 r0.z, r5, c5
                add r0.y, r0.z, r0.y
                mul r1.yzw, r2, t3.w
                mul r1.x, r2.x, t3.z
                mul r5.yzw, r2, t4.w
                mul r5.x, r2.x, t4.z
                dp4 r0.z, r5, c10
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c6
                add r0.y, r0.z, r0.y
                mul r1, r3, t3.w
                mul r2, r3, t4.w
                dp4 r0.z, r2, c11
                add r4.z, r0.z, r0.x
                dp4 r0.x, r1, c7
                add r4.y, r0.x, r0.y
                mov oC0, r4
            
            // approximately 45 instruction slots used (4 texture, 41 arithmetic)
            };
    }
}

technique RenderFullA
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dcl_texcoord4 v6
                dcl_texcoord5 v7
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
                mov oT5, v6
                mov oT6.xy, v7
            
            // approximately 13 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   samplerCUBE QuarBFASampler;
            //   samplerCUBE QuarBFBSampler;
            //   samplerCUBE QuarBFCSampler;
            //   samplerCUBE QuinBFASampler;
            //   samplerCUBE QuinBFBSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       9
            //   GLight         c9       9
            //   BLight         c18      9
            //   CLinBFSampler  s0       1
            //   QuadBFSampler  s1       1
            //   CubeBFASampler s2       1
            //   CubeBFBSampler s3       1
            //   QuarBFASampler s4       1
            //   QuarBFBSampler s5       1
            //   QuarBFCSampler s6       1
            //   QuinBFASampler s7       1
            //   QuinBFBSampler s8       1
            //
            
                ps_3_0
                def c27, 1, 0, 0, 0
                dcl_texcoord v0.xyz
                dcl_texcoord2 v1
                dcl_texcoord3 v2
                dcl_texcoord4 v3
                dcl_texcoord5 v4
                dcl_texcoord6 v5.xy
                dcl_cube s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                dcl_cube s4
                dcl_cube s5
                dcl_cube s6
                dcl_cube s7
                dcl_cube s8
                texld r0, v0, s2
                mul r1, r0, v1.zwww
                mul r0, r0, v2.zwww
                dp4 r1.x, r1, c2
                texld r2, v0, s0
                mul r3, r2, v1.xyyy
                dp4 r1.y, r3, c0
                texld r3, v0, s1
                mul r4, r3, v1.z
                dp4 r1.z, r4, c1
                add r1.y, r1.z, r1.y
                add r1.x, r1.x, r1.y
                texld r4, v0, s3
                mul r5, r4, v1.w
                dp4 r1.y, r5, c3
                add r1.x, r1.y, r1.x
                texld r5, v0, s4
                mul r6, r5, v4.x
                dp4 r1.y, r6, c4
                add r1.x, r1.y, r1.x
                texld r6, v0, s5
                mul r7, r6, v4.x
                dp4 r1.y, r7, c5
                add r1.x, r1.y, r1.x
                texld r7, v0, s6
                mul r8, r7, v4.xyyy
                dp4 r1.y, r8, c6
                add r1.x, r1.y, r1.x
                texld r8, v0, s7
                mul r9, r8, v4.y
                dp4 r1.y, r9, c7
                add r1.x, r1.y, r1.x
                texld r9, v0, s8
                mul r10, r9, v4.y
                dp4 r1.y, r10, c8
                add oC0.x, r1.y, r1.x
                mul r1, r2, v2.xyyy
                mul r2, r2, v3.xyyy
                dp4 r2.x, r2, c18
                dp4 r1.x, r1, c9
                mul r10, r3, v2.z
                mul r3, r3, v3.z
                dp4 r1.y, r3, c19
                add r1.y, r1.y, r2.x
                dp4 r1.z, r10, c10
                add r1.x, r1.z, r1.x
                dp4 r1.z, r0, c11
                dp4 r0.x, r0, c20
                add r0.x, r0.x, r1.y
                add r0.y, r1.z, r1.x
                mul r1, r4, v2.w
                mul r2, r4, v3.w
                dp4 r0.z, r2, c21
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c12
                add r0.y, r0.z, r0.y
                mul r1, r5, v4.z
                mul r2, r5, v5.x
                dp4 r0.z, r2, c22
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c13
                add r0.y, r0.z, r0.y
                mul r1, r6, v4.z
                mul r2, r6, v5.x
                dp4 r0.z, r2, c23
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c23
                add r0.y, r0.z, r0.y
                mul r1, r7, v4.zwww
                mul r2, r7, v5.xyyy
                dp4 r0.z, r2, c24
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c24
                add r0.y, r0.z, r0.y
                mul r1, r8, v4.w
                mul r2, r8, v5.y
                dp4 r0.z, r2, c25
                add r0.x, r0.z, r0.x
                dp4 r0.z, r1, c25
                add r0.y, r0.z, r0.y
                mul r1, r9, v4.w
                mul r2, r9, v5.y
                dp4 r0.z, r2, c26
                add oC0.z, r0.z, r0.x
                dp4 r0.x, r1, c17
                add oC0.y, r0.x, r0.y
                mov oC0.w, c27.x
            
            // approximately 87 instruction slots used (9 texture, 78 arithmetic)
            };
    }
}

technique RenderFullB
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dcl_texcoord4 v6
                dcl_texcoord5 v7
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
                mov oT5, v6
                mov oT6.xy, v7
            
            // approximately 13 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   samplerCUBE QuarBFASampler;
            //   samplerCUBE QuarBFBSampler;
            //   samplerCUBE QuarBFCSampler;
            //   samplerCUBE QuinBFASampler;
            //   samplerCUBE QuinBFBSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       9
            //   GLight         c9       9
            //   BLight         c18      9
            //   CLinBFSampler  s0       1
            //   QuadBFSampler  s1       1
            //   CubeBFASampler s2       1
            //   CubeBFBSampler s3       1
            //   QuarBFASampler s4       1
            //   QuarBFBSampler s5       1
            //   QuarBFCSampler s6       1
            //   QuinBFASampler s7       1
            //   QuinBFBSampler s8       1
            //
            
                ps_2_x
                def c27, 1, 0, 0, 0
                dcl t0.xyz
                dcl t2
                dcl t3
                dcl t4
                dcl t5
                dcl t6.xy
                dcl_cube s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                dcl_cube s4
                dcl_cube s5
                dcl_cube s6
                dcl_cube s7
                dcl_cube s8
                texld r0, t0, s2
                texld r1, t0, s0
                texld r2, t0, s1
                texld r3, t0, s3
                texld r4, t0, s4
                texld r5, t0, s5
                texld r6, t0, s6
                texld r7, t0, s7
                texld r8, t0, s8
                mul r9.yzw, r0, t2.w
                mul r9.x, r0.x, t2.z
                mul r10.yzw, r0, t3.w
                mul r10.x, r0.x, t3.z
                dp4 r0.x, r9, c2
                mul r9.yzw, r1, t2.y
                mul r9.x, r1.x, t2.x
                dp4 r0.y, r9, c0
                mul r9, r2, t2.z
                dp4 r0.z, r9, c1
                add r0.y, r0.z, r0.y
                add r0.x, r0.x, r0.y
                mul r9, r3, t2.w
                dp4 r0.y, r9, c3
                add r0.x, r0.y, r0.x
                mul r9, r4, t5.x
                dp4 r0.y, r9, c4
                add r0.x, r0.y, r0.x
                mul r9, r5, t5.x
                dp4 r0.y, r9, c5
                add r0.x, r0.y, r0.x
                mul r9.yzw, r6, t5.y
                mul r9.x, r6.x, t5.x
                dp4 r0.y, r9, c6
                add r0.x, r0.y, r0.x
                mul r9, r7, t5.y
                dp4 r0.y, r9, c7
                add r0.x, r0.y, r0.x
                mul r9, r8, t5.y
                dp4 r0.y, r9, c8
                add r0.x, r0.y, r0.x
                mov r0.w, c27.x
                mul r9.yzw, r1, t3.y
                mul r9.x, r1.x, t3.x
                mul r11.yzw, r1, t4.y
                mul r11.x, r1.x, t4.x
                dp4 r1.x, r11, c18
                dp4 r1.y, r9, c9
                mul r9, r2, t3.z
                mul r2, r2, t4.z
                dp4 r1.z, r2, c19
                add r1.x, r1.z, r1.x
                dp4 r1.z, r9, c10
                add r1.y, r1.z, r1.y
                dp4 r1.z, r10, c11
                dp4 r1.w, r10, c20
                add r1.x, r1.w, r1.x
                add r1.y, r1.z, r1.y
                mul r2, r3, t3.w
                mul r3, r3, t4.w
                dp4 r1.z, r3, c21
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c12
                add r1.y, r1.z, r1.y
                mul r2, r4, t5.z
                mul r3, r4, t6.x
                dp4 r1.z, r3, c22
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c13
                add r1.y, r1.z, r1.y
                mul r2, r5, t5.z
                mul r3, r5, t6.x
                dp4 r1.z, r3, c23
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c23
                add r1.y, r1.z, r1.y
                mul r2.yzw, r6, t5.w
                mul r2.x, r6.x, t5.z
                mul r3.yzw, r6, t6.y
                mul r3.x, r6.x, t6.x
                dp4 r1.z, r3, c24
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c24
                add r1.y, r1.z, r1.y
                mul r2, r7, t5.w
                mul r3, r7, t6.y
                dp4 r1.z, r3, c25
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c25
                add r1.y, r1.z, r1.y
                mul r2, r8, t5.w
                mul r3, r8, t6.y
                dp4 r1.z, r3, c26
                add r0.z, r1.z, r1.x
                dp4 r1.x, r2, c17
                add r0.y, r1.x, r1.y
                mov oC0, r0
            
            // approximately 96 instruction slots used (9 texture, 87 arithmetic)
            };
    }
}

technique RenderCubicWithTexture
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
            
            // approximately 11 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D AlbedoSampler;
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       4
            //   GLight         c4       4
            //   BLight         c8       4
            //   AlbedoSampler  s0       1
            //   CLinBFSampler  s1       1
            //   QuadBFSampler  s2       1
            //   CubeBFASampler s3       1
            //   CubeBFBSampler s4       1
            //
            
                ps_2_0
                def c12, 1, 0, 0, 0
                dcl t0.xyz
                dcl t1.xy
                dcl t2
                dcl t3
                dcl t4
                dcl_2d s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                dcl_cube s4
                texld r0, t1, s0
                texld r1, t0, s1
                texld r2, t0, s2
                texld r3, t0, s3
                texld r4, t0, s4
                mov r5.w, c12.x
                mul r6.yzw, r1, t2.y
                mul r6.x, r1.x, t2.x
                dp4 r6.x, r6, c0
                mul r7, r2, t2.z
                dp4 r6.y, r7, c1
                add r6.x, r6.y, r6.x
                mul r7.yzw, r3, t2.w
                mul r7.x, r3.x, t2.z
                dp4 r6.y, r7, c2
                add r6.x, r6.y, r6.x
                mul r7, r4, t2.w
                dp4 r6.y, r7, c3
                add r5.x, r6.y, r6.x
                mul r6.yzw, r1, t3.y
                mul r6.x, r1.x, t3.x
                mul r7.yzw, r1, t4.y
                mul r7.x, r1.x, t4.x
                dp4 r1.x, r7, c8
                dp4 r1.y, r6, c4
                mul r6, r2, t3.z
                mul r2, r2, t4.z
                dp4 r1.z, r2, c9
                add r1.x, r1.z, r1.x
                dp4 r1.z, r6, c5
                add r1.y, r1.z, r1.y
                mul r2.yzw, r3, t3.w
                mul r2.x, r3.x, t3.z
                mul r6.yzw, r3, t4.w
                mul r6.x, r3.x, t4.z
                dp4 r1.z, r6, c10
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c6
                add r1.y, r1.z, r1.y
                mul r2, r4, t3.w
                mul r3, r4, t4.w
                dp4 r1.z, r3, c11
                add r5.z, r1.z, r1.x
                dp4 r1.x, r2, c7
                add r5.y, r1.x, r1.y
                mul r0, r0, r5
                mov oC0, r0
            
            // approximately 47 instruction slots used (5 texture, 42 arithmetic)
            };
    }
}

technique RenderFullAWithTexture
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dcl_texcoord4 v6
                dcl_texcoord5 v7
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
                mov oT5, v6
                mov oT6.xy, v7
            
            // approximately 13 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D AlbedoSampler;
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   samplerCUBE QuarBFASampler;
            //   samplerCUBE QuarBFBSampler;
            //   samplerCUBE QuarBFCSampler;
            //   samplerCUBE QuinBFASampler;
            //   samplerCUBE QuinBFBSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       9
            //   GLight         c9       9
            //   BLight         c18      9
            //   AlbedoSampler  s0       1
            //   CLinBFSampler  s1       1
            //   QuadBFSampler  s2       1
            //   CubeBFASampler s3       1
            //   CubeBFBSampler s4       1
            //   QuarBFASampler s5       1
            //   QuarBFBSampler s6       1
            //   QuarBFCSampler s7       1
            //   QuinBFASampler s8       1
            //   QuinBFBSampler s9       1
            //
            
                ps_3_0
                def c27, 1, 0, 0, 0
                dcl_texcoord v0.xyz
                dcl_texcoord1 v1.xy
                dcl_texcoord2 v2
                dcl_texcoord3 v3
                dcl_texcoord4 v4
                dcl_texcoord5 v5
                dcl_texcoord6 v6.xy
                dcl_2d s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                dcl_cube s4
                dcl_cube s5
                dcl_cube s6
                dcl_cube s7
                dcl_cube s8
                dcl_cube s9
                texld r0, v0, s3
                mul r1, r0, v2.zwww
                mul r0, r0, v3.zwww
                dp4 r1.x, r1, c2
                texld r2, v0, s1
                mul r3, r2, v2.xyyy
                dp4 r1.y, r3, c0
                texld r3, v0, s2
                mul r4, r3, v2.z
                dp4 r1.z, r4, c1
                add r1.y, r1.z, r1.y
                add r1.x, r1.x, r1.y
                texld r4, v0, s4
                mul r5, r4, v2.w
                dp4 r1.y, r5, c3
                add r1.x, r1.y, r1.x
                texld r5, v0, s5
                mul r6, r5, v5.x
                dp4 r1.y, r6, c4
                add r1.x, r1.y, r1.x
                texld r6, v0, s6
                mul r7, r6, v5.x
                dp4 r1.y, r7, c5
                add r1.x, r1.y, r1.x
                texld r7, v0, s7
                mul r8, r7, v5.xyyy
                dp4 r1.y, r8, c6
                add r1.x, r1.y, r1.x
                texld r8, v0, s8
                mul r9, r8, v5.y
                dp4 r1.y, r9, c7
                add r1.x, r1.y, r1.x
                texld r9, v0, s9
                mul r10, r9, v5.y
                dp4 r1.y, r10, c8
                add r1.x, r1.y, r1.x
                texld r10, v1, s0
                mov r1.w, c27.x
                mul r11, r2, v3.xyyy
                mul r2, r2, v4.xyyy
                dp4 r2.x, r2, c18
                dp4 r2.y, r11, c9
                mul r11, r3, v3.z
                mul r3, r3, v4.z
                dp4 r2.z, r3, c19
                add r2.x, r2.z, r2.x
                dp4 r2.z, r11, c10
                add r2.y, r2.z, r2.y
                dp4 r2.z, r0, c11
                dp4 r0.x, r0, c20
                add r0.x, r0.x, r2.x
                add r0.y, r2.z, r2.y
                mul r2, r4, v3.w
                mul r3, r4, v4.w
                dp4 r0.z, r3, c21
                add r0.x, r0.z, r0.x
                dp4 r0.z, r2, c12
                add r0.y, r0.z, r0.y
                mul r2, r5, v5.z
                mul r3, r5, v6.x
                dp4 r0.z, r3, c22
                add r0.x, r0.z, r0.x
                dp4 r0.z, r2, c13
                add r0.y, r0.z, r0.y
                mul r2, r6, v5.z
                mul r3, r6, v6.x
                dp4 r0.z, r3, c23
                add r0.x, r0.z, r0.x
                dp4 r0.z, r2, c23
                add r0.y, r0.z, r0.y
                mul r2, r7, v5.zwww
                mul r3, r7, v6.xyyy
                dp4 r0.z, r3, c24
                add r0.x, r0.z, r0.x
                dp4 r0.z, r2, c24
                add r0.y, r0.z, r0.y
                mul r2, r8, v5.w
                mul r3, r8, v6.y
                dp4 r0.z, r3, c25
                add r0.x, r0.z, r0.x
                dp4 r0.z, r2, c25
                add r0.y, r0.z, r0.y
                mul r2, r9, v5.w
                mul r3, r9, v6.y
                dp4 r0.z, r3, c26
                add r1.z, r0.z, r0.x
                dp4 r0.x, r2, c17
                add r1.y, r0.x, r0.y
                mul oC0, r1, r10
            
            // approximately 89 instruction slots used (10 texture, 79 arithmetic)
            };
    }
}

technique RenderFullBWithTexture
{
    pass p0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float3x3 g_mNormalXform;
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorldViewProjection c0       4
            //   g_mNormalXform         c4       3
            //
            
                vs_2_0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dcl_texcoord1 v3
                dcl_texcoord2 v4
                dcl_texcoord3 v5
                dcl_texcoord4 v6
                dcl_texcoord5 v7
                dp4 oPos.x, v0, c0
                dp4 oPos.y, v0, c1
                dp4 oPos.z, v0, c2
                dp4 oPos.w, v0, c3
                dp3 oT0.x, v1, c4
                dp3 oT0.y, v1, c5
                dp3 oT0.z, v1, c6
                mov oT1.xy, v2
                mov oT2, v3
                mov oT3, v4
                mov oT4, v5
                mov oT5, v6
                mov oT6.xy, v7
            
            // approximately 13 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D AlbedoSampler;
            //   float4 BLight[9];
            //   samplerCUBE CLinBFSampler;
            //   samplerCUBE CubeBFASampler;
            //   samplerCUBE CubeBFBSampler;
            //   float4 GLight[9];
            //   samplerCUBE QuadBFSampler;
            //   samplerCUBE QuarBFASampler;
            //   samplerCUBE QuarBFBSampler;
            //   samplerCUBE QuarBFCSampler;
            //   samplerCUBE QuinBFASampler;
            //   samplerCUBE QuinBFBSampler;
            //   float4 RLight[9];
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   RLight         c0       9
            //   GLight         c9       9
            //   BLight         c18      9
            //   AlbedoSampler  s0       1
            //   CLinBFSampler  s1       1
            //   QuadBFSampler  s2       1
            //   CubeBFASampler s3       1
            //   CubeBFBSampler s4       1
            //   QuarBFASampler s5       1
            //   QuarBFBSampler s6       1
            //   QuarBFCSampler s7       1
            //   QuinBFASampler s8       1
            //   QuinBFBSampler s9       1
            //
            
                ps_2_x
                def c27, 1, 0, 0, 0
                dcl t0.xyz
                dcl t1.xy
                dcl t2
                dcl t3
                dcl t4
                dcl t5
                dcl t6.xy
                dcl_2d s0
                dcl_cube s1
                dcl_cube s2
                dcl_cube s3
                dcl_cube s4
                dcl_cube s5
                dcl_cube s6
                dcl_cube s7
                dcl_cube s8
                dcl_cube s9
                texld r0, t0, s3
                texld r1, t0, s1
                texld r2, t0, s2
                texld r3, t0, s4
                texld r4, t0, s5
                texld r5, t0, s6
                texld r6, t0, s7
                texld r7, t0, s8
                texld r8, t0, s9
                texld r9, t1, s0
                mul r10.yzw, r0, t2.w
                mul r10.x, r0.x, t2.z
                mul r11.yzw, r0, t3.w
                mul r11.x, r0.x, t3.z
                dp4 r0.x, r10, c2
                mul r10.yzw, r1, t2.y
                mul r10.x, r1.x, t2.x
                dp4 r0.y, r10, c0
                mul r10, r2, t2.z
                dp4 r0.z, r10, c1
                add r0.y, r0.z, r0.y
                add r0.x, r0.x, r0.y
                mul r10, r3, t2.w
                dp4 r0.y, r10, c3
                add r0.x, r0.y, r0.x
                mul r10, r4, t5.x
                dp4 r0.y, r10, c4
                add r0.x, r0.y, r0.x
                mul r10, r5, t5.x
                dp4 r0.y, r10, c5
                add r0.x, r0.y, r0.x
                mul r10.yzw, r6, t5.y
                mul r10.x, r6.x, t5.x
                dp4 r0.y, r10, c6
                add r0.x, r0.y, r0.x
                mul r10, r7, t5.y
                dp4 r0.y, r10, c7
                add r0.x, r0.y, r0.x
                mul r10, r8, t5.y
                dp4 r0.y, r10, c8
                add r0.x, r0.y, r0.x
                mov r0.w, c27.x
                mul r10.yzw, r1, t3.y
                mul r10.x, r1.x, t3.x
                mul r12.yzw, r1, t4.y
                mul r12.x, r1.x, t4.x
                dp4 r1.x, r12, c18
                dp4 r1.y, r10, c9
                mul r10, r2, t3.z
                mul r2, r2, t4.z
                dp4 r1.z, r2, c19
                add r1.x, r1.z, r1.x
                dp4 r1.z, r10, c10
                add r1.y, r1.z, r1.y
                dp4 r1.z, r11, c11
                dp4 r1.w, r11, c20
                add r1.x, r1.w, r1.x
                add r1.y, r1.z, r1.y
                mul r2, r3, t3.w
                mul r3, r3, t4.w
                dp4 r1.z, r3, c21
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c12
                add r1.y, r1.z, r1.y
                mul r2, r4, t5.z
                mul r3, r4, t6.x
                dp4 r1.z, r3, c22
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c13
                add r1.y, r1.z, r1.y
                mul r2, r5, t5.z
                mul r3, r5, t6.x
                dp4 r1.z, r3, c23
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c23
                add r1.y, r1.z, r1.y
                mul r2.yzw, r6, t5.w
                mul r2.x, r6.x, t5.z
                mul r3.yzw, r6, t6.y
                mul r3.x, r6.x, t6.x
                dp4 r1.z, r3, c24
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c24
                add r1.y, r1.z, r1.y
                mul r2, r7, t5.w
                mul r3, r7, t6.y
                dp4 r1.z, r3, c25
                add r1.x, r1.z, r1.x
                dp4 r1.z, r2, c25
                add r1.y, r1.z, r1.y
                mul r2, r8, t5.w
                mul r3, r8, t6.y
                dp4 r1.z, r3, c26
                add r0.z, r1.z, r1.x
                dp4 r1.x, r2, c17
                add r0.y, r1.x, r1.y
                mul r0, r0, r9
                mov oC0, r0
            
            // approximately 98 instruction slots used (10 texture, 88 arithmetic)
            };
    }
}

