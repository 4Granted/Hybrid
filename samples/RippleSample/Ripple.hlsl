#define mod(x, y) (x - y * floor(x / y))

cbuffer Constants : register(b0)
{
    int2 Resolution;
    float Time;
    float Frequency;
    float Amplitude;
    float Speed;
}

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

float4 PSMain(PSInput input) : SV_Target
{
    float2 uv = input.TexCoord;
    
    float2 center = float2(0.5, 0.5);
    
    float2 dist = uv - center;
    float d = length(dist);
    
    float wave = sin(d * Frequency - Time * Speed) * Amplitude;
    
    uv += normalize(dist) * wave;
    
    float3 color = float3(uv, 0.5);
    
    return float4(color, 1.0);
}