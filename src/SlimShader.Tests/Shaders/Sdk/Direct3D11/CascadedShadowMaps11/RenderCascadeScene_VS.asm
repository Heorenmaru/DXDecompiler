//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer cbAllShadowData
// {
//
//   float4x4 m_mWorldViewProjection;   // Offset:    0 Size:    64
//   float4x4 m_mWorld;                 // Offset:   64 Size:    64
//   float4x4 m_mWorldView;             // Offset:  128 Size:    64
//   float4x4 m_mShadow;                // Offset:  192 Size:    64
//   float4 m_vCascadeOffset[8];        // Offset:  256 Size:   128 [unused]
//   float4 m_vCascadeScale[8];         // Offset:  384 Size:   128 [unused]
//   int m_nCascadeLevels;              // Offset:  512 Size:     4 [unused]
//   int m_iVisualizeCascades;          // Offset:  516 Size:     4 [unused]
//   int m_iPCFBlurForLoopStart;        // Offset:  520 Size:     4 [unused]
//   int m_iPCFBlurForLoopEnd;          // Offset:  524 Size:     4 [unused]
//   float m_fMinBorderPadding;         // Offset:  528 Size:     4 [unused]
//   float m_fMaxBorderPadding;         // Offset:  532 Size:     4 [unused]
//   float m_fShadowBiasFromGUI;        // Offset:  536 Size:     4 [unused]
//   float m_fShadowPartitionSize;      // Offset:  540 Size:     4 [unused]
//   float m_fCascadeBlendArea;         // Offset:  544 Size:     4 [unused]
//   float m_fTexelSize;                // Offset:  548 Size:     4 [unused]
//   float m_fNativeTexelSizeInX;       // Offset:  552 Size:     4 [unused]
//   float m_fPaddingForCB3;            // Offset:  556 Size:     4 [unused]
//   float4 m_fCascadeFrustumsEyeSpaceDepthsFloat[2];// Offset:  560 Size:    32 [unused]
//   float4 m_fCascadeFrustumsEyeSpaceDepthsFloat4[8];// Offset:  592 Size:   128 [unused]
//   float3 m_vLightDir;                // Offset:  720 Size:    12 [unused]
//   float m_fPaddingCB4;               // Offset:  732 Size:     4 [unused]
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// cbAllShadowData                   cbuffer      NA          NA            cb0      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyzw
// NORMAL                   0   xyz         1     NONE   float   xyz 
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// NORMAL                   0   xyz         0     NONE   float   xyz 
// TEXCOORD                 0   xy          1     NONE   float   xy  
// TEXCOORD                 3     z         1     NONE   float     z 
// TEXCOORD                 1   xyzw        2     NONE   float   xyzw
// SV_POSITION              0   xyzw        3      POS   float   xyzw
// TEXCOORD                 2   xyzw        4     NONE   float   xyzw
//
vs_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[16], immediateIndexed
dcl_input v0.xyzw
dcl_input v1.xyz
dcl_input v2.xy
dcl_output o0.xyz
dcl_output o1.xy
dcl_output o1.z
dcl_output o2.xyzw
dcl_output_siv o3.xyzw, position
dcl_output o4.xyzw
dp3 o0.x, v1.xyzx, cb0[4].xyzx
dp3 o0.y, v1.xyzx, cb0[5].xyzx
dp3 o0.z, v1.xyzx, cb0[6].xyzx
dp4 o1.z, v0.xyzw, cb0[10].xyzw
mov o1.xy, v2.xyxx
dp4 o2.x, v0.xyzw, cb0[12].xyzw
dp4 o2.y, v0.xyzw, cb0[13].xyzw
dp4 o2.z, v0.xyzw, cb0[14].xyzw
dp4 o2.w, v0.xyzw, cb0[15].xyzw
dp4 o3.x, v0.xyzw, cb0[0].xyzw
dp4 o3.y, v0.xyzw, cb0[1].xyzw
dp4 o3.z, v0.xyzw, cb0[2].xyzw
dp4 o3.w, v0.xyzw, cb0[3].xyzw
mov o4.xyzw, v0.xyzw
ret 
// Approximately 15 instruction slots used
