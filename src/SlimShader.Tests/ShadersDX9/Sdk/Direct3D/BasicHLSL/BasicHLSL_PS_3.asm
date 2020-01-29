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
//   $bTexture          b0       1
//   MeshTextureSampler s0       1
//

    ps_3_0
    dcl_color v0
    dcl_texcoord v1.xy
    dcl_2d s0
    if b0
      texld r0, v1, s0
      mul oC0, r0, v0
    else
      mov oC0, v0
    endif

// approximately 8 instruction slots used (1 texture, 7 arithmetic)
