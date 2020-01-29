//
// Library:  flags 0, 6 functions:
//   0  TestFunction
//   1  TestFunction2
//   2  TestFunction3
//   3  TestFunction4
//   4  TestFunction5
//   5  TestFunction6
//
// Created by:  Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: yes, parameters: 6):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction         SV_TARGET                              o0    1   xyz  out float3
// input                                      v0    1   xyz                   in float3
// in2                  COLOR                 v1    1   x                     in float1
// val3                                       v2    3   xy     o1    3   xy   inout float2x3
// val4                                                        o4    1   x    out uint
// val5                                       v5    2   xyz                   in float3x2
// val6                                       v7    1   xyz                   in int3
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_input v0.xyz
dcl_input v2.xy
dcl_input v3.xy
dcl_input v4.xy
dcl_output o0.xyz
dcl_output o1.xy
dcl_output o2.xy
dcl_output o3.xy
dcl_output o4.x
add o0.xyz, v0.xyzx, v0.xyzx
mov o1.xy, v2.xyxx
mov o2.xy, v3.xyxx
mov o3.xy, v4.xyxx
mov o4.x, l(5)
ret 
// Approximately 6 instruction slots used
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: yes, parameters: 12):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction2                                               o0    1   xyz  out float3
// val1                                       v0    4   xyz                   in float3x4
// val2                                       v4    4   xyz                   in float3x4
// val3                                       v8    4   xyz                   in float3x4
// val4                                       v12   4   xyz                   in float3x4
// val5                                       v16   3   xyzw                  in row_major float3x4
// val6                                       v19   3   xyzw                  in row_major float3x4
// val7                                       v22   3   xy                    in float2x3
// val8                                       v25   3   xy                    in float2x3
// val9                                       v28   3   xy                    in float2x3
// val10                                      v31   3   xy                    in float2x3
// val11                                      v34   2   xyz                   in row_major float2x3
// val12                                      v36   2   xyz                   in row_major float2x3
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_output o0.xyz
mov o0.xyz, l(5.000000,5.000000,5.000000,0)
ret 
// Approximately 2 instruction slots used
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: yes, parameters: 3):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction3        SV_TARGET                              o0    1   xyz  out float3
// input                                      v0    1   xyz                   in float3
// testInterpolation                          v1    1   xyz                   in float3
// foo                                        v2    1   x                     in uint1
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_input v0.xyz
dcl_input v1.xyz
dcl_output o0.xyz
dcl_temps 1
mul r0.xyz, v0.xyzx, v1.xyzx
add o0.xyz, r0.xyzx, r0.xyzx
ret 
// Approximately 3 instruction slots used
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: no, parameters: 3):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction4                                                              void
// input                                      v0    1   x                     in float
// in2                                        v1    1   x                     in float
// result                                                      o0    1   xyz  out float3
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_input v0.x
dcl_output o0.xyz
add o0.xyz, v0.xxxx, v0.xxxx
ret 
// Approximately 2 instruction slots used
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: yes, parameters: 2):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction5                                               o0    4   xyzw out float4x4
// input                                      v0    1   x                     in float
// in2                                        v1    1   x                     in float
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_input v0.x
dcl_output o0.xyzw
dcl_output o1.xyzw
dcl_output o2.xyzw
dcl_output o3.xyzw
dcl_temps 1
add r0.xyzw, v0.xxxx, v0.xxxx
mov o0.xyzw, r0.wwww
mov o1.xyzw, r0.wwww
mov o2.xyzw, r0.wwww
mov o3.xyzw, r0.xyzw
ret 
// Approximately 6 instruction slots used
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer $Globals
// {
//
//   float4 TestGlobal;                 // Offset:    0 Size:    16
//      = 0x40a00000 0x40a00000 0x40a00000 0x40a00000 
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// $Globals                          cbuffer      NA          NA            cb0      1 
//
//
//
// Function parameter signature (return: yes, parameters: 1):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction6                                               o0    1   xyzw out float4
// input                                      v0    1   x                     in uint
//
lib_4_1
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[1], immediateIndexed
dcl_input v0.x
dcl_output o0.xyzw
dcl_temps 1
utof r0.x, v0.x
mad o0.xyzw, r0.xxxx, l(2.000000, 2.000000, 2.000000, 2.000000), cb0[0].xyzw
ret 
// Approximately 3 instruction slots used
