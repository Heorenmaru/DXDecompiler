//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 g_LightDiffuse;
//   float3 g_LightDir;
//   float4 g_MaterialAmbientColor;
//   float4 g_MaterialDiffuseColor;
//   float4x4 g_mWorld;
//   float4x4 g_mWorldViewProjection;
//
//
// Registers:
//
//   Name                   Reg   Size
//   ---------------------- ----- ----
//   g_mWorldViewProjection c0       4
//   g_mWorld               c4       3
//   g_MaterialAmbientColor c7       1
//   g_MaterialDiffuseColor c8       1
//   g_LightDir             c9       1
//   g_LightDiffuse         c10      1
//

    vs_3_sw
    def c11, 0, 1, 0, 0
    dcl_position v0
    dcl_normal v1
    dcl_texcoord v2
    dcl_position o0
    dcl_color o1
    dcl_texcoord o2.xy
    dp4 o0.x, v0, c0
    dp4 o0.y, v0, c1
    dp4 o0.z, v0, c2
    dp4 o0.w, v0, c3
    dp3 r0.x, v1, c4
    dp3 r0.y, v1, c5
    dp3 r0.z, v1, c6
    nrm r1.xyz, r0
    dp3 r0.x, r1, c9
    max r0.x, r0.x, c11.x
    mov r1.xyz, c8
    mul r0.yzw, r1.xxyz, c10.xxyz
    mad o1.xyz, r0.yzww, r0.x, c7
    mov o1.w, c11.y
    mov o2.xy, v2

// approximately 17 instruction slots used
