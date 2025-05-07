cbuffer Constants : register(b0)
{
    float4x4 Mvp;
};

struct VSInput
{
    float2 Position : POSITION;
    float2 TexCoord : TEXCOORD;
    float4 Color : COLOR;
};

struct PSInput
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};

PSInput VSMain(in VSInput input)
{
    PSInput output = (PSInput) 0;
    
    output.Position = mul(float4(input.Position, 0.0, 1.0), Mvp);
    output.TexCoord = input.TexCoord;
    output.Color = input.Color;
    
    return output;
}

Texture2D<unorm float4> Texture : register(t0);
SamplerState Sampler : register(s0);

float4 PSMain(PSInput input) : SV_Target0
{
    return Texture.Sample(Sampler, input.TexCoord) * input.Color;
}