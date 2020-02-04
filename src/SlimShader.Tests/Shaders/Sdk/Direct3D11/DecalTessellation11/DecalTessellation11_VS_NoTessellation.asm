//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer cbUpdate
// {
//
//   float4x4 g_mWorld;                 // Offset:    0 Size:    64
//   float4x4 g_mViewProjection;        // Offset:   64 Size:    64 [unused]
//   float4x4 g_mWorldViewProjection;   // Offset:  128 Size:    64
//   float4 g_vTessellationFactor;      // Offset:  192 Size:    16 [unused]
//   float4 g_vDisplacementScaleBias;   // Offset:  208 Size:    16 [unused]
//   float4 g_vLightPosition;           // Offset:  224 Size:    16
//   float4 g_vEyePosition;             // Offset:  240 Size:    16
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// cbUpdate                          cbuffer      NA          NA            cb1      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyz         0     NONE   float   xyz 
// NORMAL                   0   xyz         1     NONE   float   xyz 
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// TEXCOORD                 0   xy          1     NONE   float   xy  
// NORMAL                   0   xyz         2     NONE   float   xyz 
// TEXCOORD                 1   xyz         3     NONE   float   xyz 
// LIGHTVECTORTS            0   xyz         4     NONE   float   xyz 
// LIGHTVECTORWS            0   xyz         5     NONE   float   xyz 
// VIEWVECTORTS             0   xyz         6     NONE   float   xyz 
// VIEWVECTORWS             0   xyz         7     NONE   float   xyz 
//
vs_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB1[16], immediateIndexed
dcl_input v0.xyz
dcl_input v1.xyz
dcl_input v2.xy
dcl_output_siv o0.xyzw, position
dcl_output o1.xy
dcl_output o2.xyz
dcl_output o3.xyz
dcl_output o4.xyz
dcl_output o5.xyz
dcl_output o6.xyz
dcl_output o7.xyz
dcl_temps 2
mov r0.xyz, v0.xyzx
mov r0.w, l(1.000000)
dp4 o0.x, r0.xyzw, cb1[8].xyzw
dp4 o0.y, r0.xyzw, cb1[9].xyzw
dp4 o0.z, r0.xyzw, cb1[10].xyzw
dp4 o0.w, r0.xyzw, cb1[11].xyzw
mov o1.xy, v2.xyxx
mov o2.xyz, v1.xyzx
mov o3.xyz, l(0,0,0,0)
dp4 r1.x, r0.xyzw, cb1[0].xyzw
dp4 r1.y, r0.xyzw, cb1[1].xyzw
dp4 r1.z, r0.xyzw, cb1[2].xyzw
add r0.xyz, -r1.xyzx, cb1[14].xyzx
add r1.xyz, -r1.xyzx, cb1[15].xyzx
mov o4.xyz, r0.xyzx
mov o5.xyz, r0.xyzx
mov o6.xyz, r1.xyzx
mov o7.xyz, r1.xyzx
ret 
// Approximately 19 instruction slots used
