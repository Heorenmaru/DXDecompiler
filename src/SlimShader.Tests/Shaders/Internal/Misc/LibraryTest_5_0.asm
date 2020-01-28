//
// Library:  flags 0, 3 functions:
//   0  TestFunction
//   1  TestFunction2
//   2  TestFunction3
//
// Created by:  Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
//
// Function parameter signature (return: yes, parameters: 1):
//
// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
// -------------------- -------------------- --------------- ---------------- ------------------------------ 
// TestFunction                                                o0    1   xyz  out float3
// input                                      v0    1   xyz                   in float3
//
lib_5_0
dcl_globalFlags refactoringAllowed
dcl_input v0.xyz
dcl_output o0.xyz
add o0.xyz, v0.xyzx, v0.xyzx
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
// TestFunction2        SV_TARGET                              o0    1   xyz  out float3
// input                                      v0    1   xyz                   in float3
// foo                                        v1    1   x                     in uint1
//
lib_5_0
dcl_globalFlags refactoringAllowed
dcl_input v0.xyz
dcl_output o0.xyz
add o0.xyz, v0.xyzx, v0.xyzx
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
// TestFunction3                                               o0    4   xyzw out float4x4
// input                                      v0    1   x                     in uint
// foo                  COLOR                 v1    3   xyzw                  in int4x3
//
lib_5_0
dcl_globalFlags refactoringAllowed
dcl_input v0.x
dcl_output o0.xyzw
dcl_output o1.xyzw
dcl_output o2.xyzw
dcl_output o3.xyzw
dcl_temps 1
utof r0.x, v0.x
add r0.xyzw, r0.xxxx, r0.xxxx
mov o0.xyzw, r0.wwww
mov o1.xyzw, r0.wwww
mov o2.xyzw, r0.wwww
mov o3.xyzw, r0.xyzw
ret 
// Approximately 7 instruction slots used
