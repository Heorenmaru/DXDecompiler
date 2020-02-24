
//listing of all techniques and passes with embedded asm listings 

technique tec0
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
            //   float4x4 matProj;
            //   float4x4 matView;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   matView      c0       4
            //   matProj      c4       4
            //
            
                preshader
                mov r0.x, c0.x
                mov r0.y, c1.x
                mov r0.z, c2.x
                mov r0.w, c3.x
                dot c4, r0, c4
                dot c5, r0, c5
                dot c6, r0, c6
                dot c7, r0, c7
                mov r0.x, c0.y
                mov r0.y, c1.y
                mov r0.z, c2.y
                mov r0.w, c3.y
                dot c4.yzwx, r0, c4
                dot c5.yzwx, r0, c5
                dot c6.yzwx, r0, c6
                dot c7.yzwx, r0, c7
                mov r0.x, c0.z
                mov r0.y, c1.z
                mov r0.z, c2.z
                mov r0.w, c3.z
                dot c4.zwxy, r0, c4
                dot c5.zwxy, r0, c5
                dot c6.zwxy, r0, c6
                dot c7.zwxy, r0, c7
            
            // approximately 24 instructions used
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4x4 matProj;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   matProj      c0       4
            //
            
                vs_2_0
                def c8, 1, 0, 0, 0
                dcl_position v0
                mov r0.xyz, c4
                mov r0.w, c0.w
                mad r1, v0.xyzx, c8.xxxy, c8.yyyx
                dp4 oPos.x, r1, r0
                mov r0.xyz, c5
                mov r0.w, c1.w
                dp4 oPos.y, r1, r0
                mov r0.xyz, c6
                mov r0.w, c2.w
                dp4 oPos.z, r1, r0
                mov r0.xyz, c7
                mov r0.w, c3.w
                dp4 oPos.w, r1, r0
                mov oT0.xyz, v0
            
            // approximately 14 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   samplerCUBE linear_sampler;
            //
            //
            // Registers:
            //
            //   Name           Reg   Size
            //   -------------- ----- ----
            //   linear_sampler s0       1
            //
            
                ps_2_0
                dcl t0.xyz
                dcl_cube s0
                texld r0, t0, s0
                mov oC0, r0
            
            // approximately 2 instruction slots used (1 texture, 1 arithmetic)
            };
    }
}

