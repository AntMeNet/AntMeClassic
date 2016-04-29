
float4x4 WVP;
TextureCube tex;

sampler cubeSampler = sampler_state {
    texture = <tex>;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float3 PosTex   : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, WVP).xyww;
    output.PosTex = input.Position;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return texCUBE(cubeSampler,input.PosTex);
}


technique Technique1
{
    pass Pass1
    {
        CullMode = None;
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
