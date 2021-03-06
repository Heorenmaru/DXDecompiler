//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float2 g_avSampleOffsets[16];
//   sampler2D s0;
//
//
// Registers:
//
//   Name              Reg   Size
//   ----------------- ----- ----
//   g_avSampleOffsets c0       9
//   s0                s0       1
//

    ps_2_0
    def c9, 0.212500006, 0.715399981, 0.0720999986, 9.99999975e-005
    def c10, 0.693147182, 0.111111112, 1, 0
    dcl t0.xy
    dcl_2d s0
    add r0.xy, t0, c1
    add r1.xy, t0, c0
    add r2.xy, t0, c2
    add r3.xy, t0, c3
    add r4.xy, t0, c4
    add r5.xy, t0, c5
    add r6.xy, t0, c6
    add r7.xy, t0, c7
    add r8.xy, t0, c8
    texld r0, r0, s0
    texld r1, r1, s0
    texld r2, r2, s0
    texld r3, r3, s0
    texld r4, r4, s0
    texld r5, r5, s0
    texld r6, r6, s0
    texld r7, r7, s0
    texld r8, r8, s0
    dp3 r1.w, r0, c9
    add r1.w, r1.w, c9.w
    log r1.w, r1.w
    mul r1.w, r1.w, c10.x
    dp3 r2.w, r1, c9
    add r2.w, r2.w, c9.w
    log r2.w, r2.w
    mad r2.w, r2.w, c10.x, r1.w
    dp3 r3.w, r2, c9
    add r3.w, r3.w, c9.w
    log r3.w, r3.w
    mad r3.w, r3.w, c10.x, r2.w
    dp3 r4.w, r3, c9
    add r4.w, r4.w, c9.w
    log r4.w, r4.w
    mad r4.w, r4.w, c10.x, r3.w
    dp3 r5.w, r4, c9
    add r5.w, r5.w, c9.w
    log r5.w, r5.w
    mad r5.w, r5.w, c10.x, r4.w
    dp3 r6.w, r5, c9
    add r6.w, r6.w, c9.w
    log r6.w, r6.w
    mad r6.w, r6.w, c10.x, r5.w
    dp3 r7.w, r6, c9
    add r7.w, r7.w, c9.w
    log r7.w, r7.w
    mad r7.w, r7.w, c10.x, r6.w
    dp3 r8.w, r7, c9
    add r8.w, r8.w, c9.w
    log r8.w, r8.w
    mad r8.w, r8.w, c10.x, r7.w
    dp3 r0.x, r8, c9
    add r0.x, r0.x, c9.w
    log r0.x, r0.x
    mad r0.x, r0.x, c10.x, r8.w
    mul r0.xyz, r0.x, c10.y
    mov r0.w, c10.z
    mov oC0, r0

// approximately 57 instruction slots used (9 texture, 48 arithmetic)
