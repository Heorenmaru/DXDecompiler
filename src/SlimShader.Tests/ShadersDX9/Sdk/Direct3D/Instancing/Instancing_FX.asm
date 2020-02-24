
//listing of all techniques and passes with embedded asm listings 

technique THW_Instancing
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
            //   float4x4 g_mProj;
            //   float4x4 g_mView;
            //   float4x4 g_mWorld;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_mWorld     c0       4
            //   g_mView      c4       4
            //   g_mProj      c8       4
            //
            
                vs_2_0
                def c12, 0.999970496, 0.5, 6.28318548, -3.14159274
                def c13, 32, -16, 0, 0
                def c14, -1.55009923e-006, -2.17013894e-005, 0.00260416674, 0.00026041668
                def c15, -0.020833334, -0.125, 1, 0.5
                dcl_position v0
                dcl_texcoord v1
                dcl_color v2
                dcl_color1 v3
                mad r0.x, v3.w, c12.x, c12.y
                frc r0.x, r0.x
                mad r0.x, r0.x, c12.z, c12.w
                sincos r1.xy, r0.x, c14, c15
                mul r0.xyz, r1.xyyw, v0.xzxw
                mad r1.z, v0.z, r1.x, -r0.z
                add r1.x, r0.y, r0.x
                mov r1.y, v0.y
                mad r0.xyz, v3, c13.x, r1
                mov r0.w, v0.w
                add r0, r0, c13.yyyz
                dp4 r1.x, r0, c0
                dp4 r1.y, r0, c1
                dp4 r1.z, r0, c2
                dp4 r1.w, r0, c3
                dp4 r0.x, r1, c4
                dp4 r0.y, r1, c5
                dp4 r0.z, r1, c6
                dp4 r0.w, r1, c7
                dp4 oPos.x, r0, c8
                dp4 oPos.y, r0, c9
                dp4 oPos.z, r0, c10
                dp4 oPos.w, r0, c11
                mov oD0, v2
                mov oT0.xy, v1
            
            // approximately 32 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D g_samScene;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_samScene   s0       1
            //
            
                ps_2_0
                dcl t0.xy
                dcl v0
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

technique TShader_Instancing
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
            //   float4x4 g_mProj;
            //   float4x4 g_mView;
            //   float4x4 g_mWorld;
            //   float4 g_vBoxInstance_Color[120];
            //   float4 g_vBoxInstance_Position[120];
            //
            //
            // Registers:
            //
            //   Name                    Reg   Size
            //   ----------------------- ----- ----
            //   g_mWorld                c0       4
            //   g_mView                 c4       4
            //   g_mProj                 c8       4
            //   g_vBoxInstance_Position c16    120
            //   g_vBoxInstance_Color    c136   120
            //
            
                vs_2_0
                def c12, 0.999970496, 0.5, 6.28318548, -3.14159274
                def c13, 32, -16, 0, 0
                def c14, -1.55009923e-006, -2.17013894e-005, 0.00260416674, 0.00026041668
                def c15, -0.020833334, -0.125, 1, 0.5
                dcl_position v0
                dcl_texcoord v1
                dcl_texcoord1 v2
                frc r0.x, v2.x
                add r0.x, -r0.x, v2.x
                mova a0.x, r0.x
                mov r0, c16[a0.x]
                mad r0.w, r0.w, c12.x, c12.y
                frc r0.w, r0.w
                mad r0.w, r0.w, c12.z, c12.w
                sincos r1.xy, r0.w, c14, c15
                mul r1.yzw, r1.xxyy, v0.xxzx
                mad r2.z, v0.z, r1.x, -r1.w
                add r2.x, r1.z, r1.y
                mov r2.y, v0.y
                mad r0.xyz, r0, c13.x, r2
                mov oD0, c136[a0.x]
                mov r0.w, v0.w
                add r0, r0, c13.yyyz
                dp4 r1.x, r0, c0
                dp4 r1.y, r0, c1
                dp4 r1.z, r0, c2
                dp4 r1.w, r0, c3
                dp4 r0.x, r1, c4
                dp4 r0.y, r1, c5
                dp4 r0.z, r1, c6
                dp4 r0.w, r1, c7
                dp4 oPos.x, r0, c8
                dp4 oPos.y, r0, c9
                dp4 oPos.z, r0, c10
                dp4 oPos.w, r0, c11
                mov oT0.xy, v1
            
            // approximately 36 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D g_samScene;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_samScene   s0       1
            //
            
                ps_2_0
                dcl t0.xy
                dcl v0
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

technique TConstants_Instancing
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
            //   float4 g_BoxInstance_Color;
            //   float4 g_BoxInstance_Position;
            //   float4x4 g_mProj;
            //   float4x4 g_mView;
            //   float4x4 g_mWorld;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   g_mWorld               c0       4
            //   g_mView                c4       4
            //   g_mProj                c8       4
            //   g_BoxInstance_Position c13      1
            //   g_BoxInstance_Color    c14      1
            //
            
                vs_2_0
                def c12, 0.999970496, 0.5, 6.28318548, -3.14159274
                def c15, 32, -16, 0, 0
                def c16, -1.55009923e-006, -2.17013894e-005, 0.00260416674, 0.00026041668
                def c17, -0.020833334, -0.125, 1, 0.5
                dcl_position v0
                dcl_texcoord v1
                mov r0, c13
                mad r0.w, r0.w, c12.x, c12.y
                frc r0.w, r0.w
                mad r0.w, r0.w, c12.z, c12.w
                sincos r1.xy, r0.w, c16, c17
                mul r1.yzw, r1.xxyy, v0.xxzx
                mad r2.z, v0.z, r1.x, -r1.w
                add r2.x, r1.z, r1.y
                mov r2.y, v0.y
                mad r0.xyz, r0, c15.x, r2
                mov r0.w, v0.w
                add r0, r0, c15.yyyz
                dp4 r1.x, r0, c0
                dp4 r1.y, r0, c1
                dp4 r1.z, r0, c2
                dp4 r1.w, r0, c3
                dp4 r0.x, r1, c4
                dp4 r0.y, r1, c5
                dp4 r0.z, r1, c6
                dp4 r0.w, r1, c7
                dp4 oPos.x, r0, c8
                dp4 oPos.y, r0, c9
                dp4 oPos.z, r0, c10
                dp4 oPos.w, r0, c11
                mov oD0, c14
                mov oT0.xy, v1
            
            // approximately 33 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D g_samScene;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_samScene   s0       1
            //
            
                ps_2_0
                dcl t0.xy
                dcl v0
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

