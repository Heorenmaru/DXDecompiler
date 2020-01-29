//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   bool $bTexture;
//   sampler2D MeshTextureSampler;
//
//
// Registers:
//
//   Name               Reg   Size
//   ------------------ ----- ----
//   $bTexture          c0       1
//   MeshTextureSampler s0       1
//

    ps_2_x
    dcl v0
    dcl t0.xy
    dcl_2d s0
    texld r0, t0, s0
    mul r0, r0, v0
    cmp r0, -c0.x, v0, r0
    mov oC0, r0

// approximately 4 instruction slots used (1 texture, 3 arithmetic)
