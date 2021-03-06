//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Note: shader requires additional functionality:
//       64 UAV slots
//       Typed UAV Load Additional Formats
//
//
// Buffer Definitions: 
//
// cbuffer cbuffer1
// {
//
//   float4 cValue;                     // Offset:    0 Size:    16
//
// }
//
// tbuffer tbuffer1
// {
//
//   float4 tValue;                     // Offset:    0 Size:    16
//
// }
//
// cbuffer cbuffer2
// {
//
//   struct bar
//   {
//       
//       float foofoo;                  // Offset:    0
//       uint foobar;                   // Offset:    4
//       
//       struct bar::<unnamed>
//       {
//           
//           float foo;                 // Offset:   16
//           float bar;                 // Offset:   20
//
//       } inner_struct_1;              // Offset:   16
//       
//       struct bar::<unnamed>
//       {
//           
//           float baz;                 // Offset:   32
//           float bug;                 // Offset:   36
//           
//           struct inner_test
//           {
//               
//               float4 sValue1[2];     // Offset:   48
//               float3x4 sValue2[2];   // Offset:   80
//               float4x3 sValue3[2];   // Offset:  208
//               float4 sValue4[2];     // Offset:  304
//
//           } inner_struct_3[2];       // Offset:   48
//
//       } inner_struct_2[2];           // Offset:   32
//       float foobaz;                  // Offset: 1216
//       int foobuz[8];                 // Offset: 1232
//       float binary_decompiler_array_size_calculation_looks_sketchy;// Offset: 1348
//       int2 really[3];                // Offset: 1360
//       float sketchy;                 // Offset: 1400
//       float3 did[5];                 // Offset: 1408
//       float i;                       // Offset: 1484
//       float4 mention[7];             // Offset: 1488
//       float how_sketchy;             // Offset: 1600
//       float4x3 matCM43;              // Offset: 1616
//       float3x4 matCM34;              // Offset: 1664
//       row_major float4x3 matRM43;    // Offset: 1728
//       row_major float3x4 matRM34;    // Offset: 1792
//       float val;                     // Offset: 1840
//
//   } structVal1[2];                   // Offset:    0 Size:  3700
//   float dummy;                       // Offset: 3700 Size:     4 [unused]
//   
//   struct bar
//   {
//       
//       float foofoo;                  // Offset: 3712
//       uint foobar;                   // Offset: 3716
//       
//       struct bar::<unnamed>
//       {
//           
//           float foo;                 // Offset: 3728
//           float bar;                 // Offset: 3732
//
//       } inner_struct_1;              // Offset: 3728
//       
//       struct bar::<unnamed>
//       {
//           
//           float baz;                 // Offset: 3744
//           float bug;                 // Offset: 3748
//           
//           struct inner_test
//           {
//               
//               float4 sValue1[2];     // Offset: 3760
//               float3x4 sValue2[2];   // Offset: 3792
//               float4x3 sValue3[2];   // Offset: 3920
//               float4 sValue4[2];     // Offset: 4016
//
//           } inner_struct_3[2];       // Offset: 3760
//
//       } inner_struct_2[2];           // Offset: 3744
//       float foobaz;                  // Offset: 4928
//       int foobuz[8];                 // Offset: 4944
//       float binary_decompiler_array_size_calculation_looks_sketchy;// Offset: 5060
//       int2 really[3];                // Offset: 5072
//       float sketchy;                 // Offset: 5112
//       float3 did[5];                 // Offset: 5120
//       float i;                       // Offset: 5196
//       float4 mention[7];             // Offset: 5200
//       float how_sketchy;             // Offset: 5312
//       float4x3 matCM43;              // Offset: 5328
//       float3x4 matCM34;              // Offset: 5376
//       row_major float4x3 matRM43;    // Offset: 5440
//       row_major float3x4 matRM34;    // Offset: 5504
//       float val;                     // Offset: 5552
//
//   } structVal2;                      // Offset: 3712 Size:  1844
//   
//   struct <unnamed>
//   {
//       
//       float val1;                    // Offset: 5568
//       float val2;                    // Offset: 5572
//
//   } anonStruct1;                     // Offset: 5568 Size:     8
//   
//   struct <unnamed>
//   {
//       
//       float2 val1;                   // Offset: 5584
//       float2 val2;                   // Offset: 5592
//
//   } anonStruct2;                     // Offset: 5584 Size:    16
//
// }
//
// cbuffer cbuffer3
// {
//
//   float4x3 testCM43;                 // Offset:    0 Size:    48
//
// }
//
// cbuffer cbuffer4
// {
//
//   float3x4 testCM34;                 // Offset:    0 Size:    60
//
// }
//
// cbuffer cbuffer5
// {
//
//   row_major float4x3 testRM43;       // Offset:    0 Size:    60
//
// }
//
// cbuffer cbuffer6
// {
//
//   row_major float3x4 testRM34;       // Offset:    0 Size:    48
//
// }
//
// cbuffer matrices
// {
//
//   row_major float1x1 matRM11;        // Offset:    0 Size:     4
//   row_major float1x2 matRM12;        // Offset:    4 Size:     8 [unused]
//   row_major float1x3 matRM13;        // Offset:   16 Size:    12 [unused]
//   row_major float1x4 matRM14;        // Offset:   32 Size:    16 [unused]
//   row_major float2x1 matRM21;        // Offset:   48 Size:    20 [unused]
//   row_major float2x2 matRM22;        // Offset:   80 Size:    24 [unused]
//   row_major float2x3 matRM23;        // Offset:  112 Size:    28 [unused]
//   row_major float2x4 matRM24;        // Offset:  144 Size:    32 [unused]
//   row_major float3x1 matRM31;        // Offset:  176 Size:    36 [unused]
//   row_major float3x2 matRM32;        // Offset:  224 Size:    40 [unused]
//   row_major float3x3 matRM33;        // Offset:  272 Size:    44 [unused]
//   row_major float3x4 matRM34;        // Offset:  320 Size:    48 [unused]
//   row_major float4x1 matRM41;        // Offset:  368 Size:    52 [unused]
//   row_major float4x2 matRM42;        // Offset:  432 Size:    56 [unused]
//   row_major float4x3 matRM43;        // Offset:  496 Size:    60 [unused]
//   row_major float4x4 matRM44;        // Offset:  560 Size:    64 [unused]
//   float1x1 matCM11;                  // Offset:  624 Size:     4 [unused]
//   float1x2 matCM12;                  // Offset:  640 Size:    20 [unused]
//   float1x3 matCM13;                  // Offset:  672 Size:    36 [unused]
//   float1x4 matCM14;                  // Offset:  720 Size:    52 [unused]
//   float2x1 matCM21;                  // Offset:  772 Size:     8 [unused]
//   float2x2 matCM22;                  // Offset:  784 Size:    24 [unused]
//   float2x3 matCM23;                  // Offset:  816 Size:    40 [unused]
//   float2x4 matCM24;                  // Offset:  864 Size:    56 [unused]
//   float3x1 matCM31;                  // Offset:  928 Size:    12 [unused]
//   float3x2 matCM32;                  // Offset:  944 Size:    28 [unused]
//   float3x3 matCM33;                  // Offset:  976 Size:    44 [unused]
//   float3x4 matCM34;                  // Offset: 1024 Size:    60 [unused]
//   float4x1 matCM41;                  // Offset: 1088 Size:    16 [unused]
//   float4x2 matCM42;                  // Offset: 1104 Size:    32 [unused]
//   float4x3 matCM43;                  // Offset: 1136 Size:    48 [unused]
//   float4x4 matCM44;                  // Offset: 1184 Size:    64 [unused]
//
// }
//
// Resource bind info for tex9
// {
//
//   struct foo
//   {
//       
//       float4 sValue1;                // Offset:    0
//       float4 sValue2;                // Offset:   16
//
//   } $Element;                        // Offset:    0 Size:    32
//
// }
//
// Resource bind info for uav1
// {
//
//   struct foo
//   {
//       
//       float4 sValue1;                // Offset:    0
//       float4 sValue2;                // Offset:   16
//
//   } $Element;                        // Offset:    0 Size:    32
//
// }
//
// Resource bind info for uav2
// {
//
//   struct foo
//   {
//       
//       float4 sValue1;                // Offset:    0
//       float4 sValue2;                // Offset:   16
//
//   } $Element;                        // Offset:    0 Size:    32
//
// }
//
// Resource bind info for uav5
// {
//
//   struct foo
//   {
//       
//       float4 sValue1;                // Offset:    0
//       float4 sValue2;                // Offset:   16
//
//   } $Element;                        // Offset:    0 Size:    32
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// samp0                             sampler      NA          NA             s0      1 
// samp1                           sampler_c      NA          NA             s1      1 
// tex0                              texture  float4         buf             t0      1 
// tex1                              texture  float4          1d             t1      1 
// tex2                              texture  float4     1darray             t2      1 
// tex3                              texture  float4          2d             t3      1 
// tex4                              texture  float4     2darray             t4      1 
// tex5                              texture  float4          3d             t5      1 
// tex6                              texture  float4        cube             t6      1 
// tex7                              texture  float4   cubearray             t7      1 
// tex8                              texture    byte         r/o             t8      1 
// tex9                              texture  struct         r/o             t9      1 
// tex10                             texture   float       2dMS4            t10      1 
// tex11                             texture   float  2darrayMS4            t11      1 
// tbuffer1                          tbuffer      NA          NA            t12      1 
// uav1                                  UAV  struct      append             u1      1 
// uav2                                  UAV  struct     consume             u2      1 
// uav3                                  UAV  float4         buf             u3      1 
// uav4                                  UAV    byte         r/w             u4      1 
// uav5                                  UAV  struct         r/w             u5      1 
// uav6                                  UAV  float4          1d             u6      1 
// uav7                                  UAV  float4     1darray             u7      1 
// uav8                                  UAV  float4          2d             u8      1 
// uav9                                  UAV  float4     2darray             u9      1 
// uav10                                 UAV  float4          3d            u10      1 
// cbuffer1                          cbuffer      NA          NA            cb0      1 
// cbuffer2                          cbuffer      NA          NA            cb1      1 
// cbuffer3                          cbuffer      NA          NA            cb2      1 
// cbuffer4                          cbuffer      NA          NA            cb3      1 
// cbuffer5                          cbuffer      NA          NA            cb4      1 
// cbuffer6                          cbuffer      NA          NA            cb5      1 
// matrices                          cbuffer      NA          NA            cb6      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   x           0     NONE     int   x   
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
ps_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[1], immediateIndexed
dcl_constantbuffer CB1[350], immediateIndexed
dcl_constantbuffer CB2[3], immediateIndexed
dcl_constantbuffer CB3[4], immediateIndexed
dcl_constantbuffer CB4[4], immediateIndexed
dcl_constantbuffer CB5[3], immediateIndexed
dcl_constantbuffer CB6[1], immediateIndexed
dcl_sampler s0, mode_default
dcl_sampler s1, mode_comparison
dcl_resource_buffer (float,float,float,float) t0
dcl_resource_texture1d (float,float,float,float) t1
dcl_resource_texture1darray (float,float,float,float) t2
dcl_resource_texture2d (float,float,float,float) t3
dcl_resource_texture2darray (float,float,float,float) t4
dcl_resource_texture3d (float,float,float,float) t5
dcl_resource_texturecube (float,float,float,float) t6
dcl_resource_texturecubearray (float,float,float,float) t7
dcl_resource_raw t8
dcl_resource_structured t9, 32
dcl_resource_texture2dms(4) (float,float,float,float) t10
dcl_resource_texture2dmsarray(4) (float,float,float,float) t11
dcl_resource_buffer (mixed,mixed,mixed,mixed) t12
dcl_uav_structured u1, 32
dcl_uav_structured u2, 32
dcl_uav_typed_buffer (float,float,float,float) u3
dcl_uav_raw u4
dcl_uav_structured u5, 32
dcl_uav_typed_texture1d (float,float,float,float) u6
dcl_uav_typed_texture1darray (float,float,float,float) u7
dcl_uav_typed_texture2d (float,float,float,float) u8
dcl_uav_typed_texture2darray (float,float,float,float) u9
dcl_uav_typed_texture3d (float,float,float,float) u10
dcl_input_ps constant v0.x
dcl_output o0.xyzw
dcl_temps 6
imm_atomic_consume r0.x, u2
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r1.x, r0.x, l(0), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r1.y, r0.x, l(4), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r1.z, r0.x, l(8), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r1.w, r0.x, l(12), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r2.x, r0.x, l(16), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r2.y, r0.x, l(20), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r2.z, r0.x, l(24), u2.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r2.w, r0.x, l(28), u2.xxxx
imm_atomic_alloc r0.x, u1
ld_indexable(buffer)(mixed,mixed,mixed,mixed) r3.xyzw, l(0, 0, 0, 0), t12.xyzw
add r3.xyzw, r3.xyzw, cb0[0].xyzw
add r3.xyzw, r3.xyzw, cb1[19].xyzw
add r3.xyzw, r3.xyzw, cb1[191].xyzw
add r0.y, r3.x, cb1[296].z
add r4.x, r0.y, cb1[348].y
add r4.y, r3.y, cb1[297].z
add r0.yz, r4.xxyx, cb1[349].zzwz
add r0.y, r0.y, cb2[0].w
add r0.z, r0.z, cb2[1].w
add r4.y, r0.z, cb3[1].z
add r4.x, r0.y, cb3[0].z
add r0.y, r3.z, cb1[298].z
add r0.z, r3.w, cb1[299].z
add r3.w, r0.z, cb3[3].z
add r0.y, r0.y, cb2[2].w
add r4.z, r0.y, cb3[2].z
add r3.xyz, r4.xyzx, cb4[3].xyzx
add r3.xyzw, r3.xyzw, cb5[2].xyzw
add r3.xyzw, r3.xyzw, cb6[0].xxxx
itof r0.y, v0.x
sample_indexable(texture2d)(float,float,float,float) r4.xyzw, r0.yyyy, t3.xyzw, s0
add r3.xyzw, r3.xyzw, r4.xyzw
sample_c_indexable(texture2d)(float,float,float,float) r0.z, r0.yyyy, t3.xxxx, s1, l(0.500000)
add r3.xyzw, r0.zzzz, r3.xyzw
ld_indexable(buffer)(float,float,float,float) r4.xyzw, v0.xxxx, t0.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
ld_indexable(texture1d)(float,float,float,float) r4.xyzw, v0.xxxx, t1.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
ld_indexable(texture1darray)(float,float,float,float) r4.xyzw, v0.xxxx, t2.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
ld_indexable(texture2d)(float,float,float,float) r4.xyzw, v0.xxxx, t3.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
ld_indexable(texture2darray)(float,float,float,float) r4.xyzw, v0.xxxx, t4.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
ld_indexable(texture3d)(float,float,float,float) r4.xyzw, v0.xxxx, t5.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
sample_indexable(texturecube)(float,float,float,float) r4.xyzw, r0.yyyy, t6.xyzw, s0
sample_indexable(texturecubearray)(float,float,float,float) r5.xyzw, r0.yyyy, t7.xyzw, s0
add r3.xyzw, r3.xyzw, r4.xyzw
add r3.xyzw, r5.xyzw, r3.xyzw
ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.y, v0.x, t8.xxxx
utof r0.y, r0.y
add r3.xyzw, r0.yyyy, r3.xyzw
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r4.xyzw, v0.x, l(16), t9.xyzw
add r3.xyzw, r3.xyzw, r4.xyzw
mov r4.x, v0.x
mov r4.z, l(0)
ldms_indexable(texture2dms)(float,float,float,float) r0.y, r4.xxzz, t10.yxzw, v0.x
ldms_indexable(texture2dmsarray)(float,float,float,float) r0.z, r4.xxxz, t11.yzxw, v0.x
add r3.xyzw, r0.yyyy, r3.xyzw
add r3.xyzw, r0.zzzz, r3.xyzw
add r3.xyzw, r1.xyzw, r3.xyzw
add r1.xyzw, r1.xyzw, r3.xyzw
store_structured u1.xyzw, r0.x, l(0), r1.xyzw
store_structured u1.xyzw, r0.x, l(16), r2.xyzw
ld_uav_typed_indexable(buffer)(float,float,float,float) r0.xyzw, v0.xxxx, u3.xyzw
add r0.xyzw, r0.xyzw, r3.xyzw
ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r1.x, v0.x, u4.xxxx
utof r1.x, r1.x
add r0.xyzw, r0.xyzw, r1.xxxx
ld_structured_indexable(structured_buffer, stride=32)(mixed,mixed,mixed,mixed) r1.xyzw, v0.x, l(16), u5.xyzw
add r0.xyzw, r0.xyzw, r1.xyzw
ld_uav_typed_indexable(texture1d)(float,float,float,float) r1.xyzw, v0.xxxx, u6.xyzw
add r0.xyzw, r0.xyzw, r1.xyzw
ld_uav_typed_indexable(texture1darray)(float,float,float,float) r1.xyzw, v0.xxxx, u7.xyzw
add r0.xyzw, r0.xyzw, r1.xyzw
ld_uav_typed_indexable(texture2d)(float,float,float,float) r1.xyzw, v0.xxxx, u8.xyzw
add r0.xyzw, r0.xyzw, r1.xyzw
ld_uav_typed_indexable(texture2darray)(float,float,float,float) r1.xyzw, v0.xxxx, u9.xyzw
add r0.xyzw, r0.xyzw, r1.xyzw
ld_uav_typed_indexable(texture3d)(float,float,float,float) r1.xyzw, v0.xxxx, u10.xyzw
add o0.xyzw, r0.xyzw, r1.xyzw
ret 
// Approximately 84 instruction slots used
