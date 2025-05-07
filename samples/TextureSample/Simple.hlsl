struct VSInput
{
    float3 Position : POSITION;
    float2 TexCoord : TEXCOORD;
};

struct PSInput
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD;
};

PSInput VSMain(in VSInput input)
{
    PSInput output = (PSInput) 0;
    
    output.Position = float4(input.Position, 1.0);
    output.TexCoord = input.TexCoord;
    
    return output;
}

Texture2D Texture : register(t0);
SamplerState Sampler : register(s0);

float4 PSMain(PSInput input) : SV_Target
{
    return Texture.Sample(Sampler, input.TexCoord);
}