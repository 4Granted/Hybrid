#define mod(x, y) (x - y * floor(x / y))

cbuffer Constants : register(b0)
{
    int2 Resolution;
    float Time;
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

// adapted from: https://www.shadertoy.com/view/33sGRM
float4 PSMain(PSInput input) : SV_Target
{
    float3 col = float3(0.0, 0.0, 0.0);
    
    float lum;
    float zoom = Time;
    
    float2 coord = input.TexCoord * Resolution;
    
    [unroll]
    for (int i = 0; i < 3; i++)
    {
        float2 uv = coord / Resolution;
        
        uv = uv * 2.0 - 1.0;
        uv.x *= Resolution.x / Resolution.y;
        
        float2 radialOffset = float2(
            sin(zoom + length(uv) * 7.0),
            cos(zoom + length(uv) * 325.0));
        
        uv += radialOffset * 0.2;
        
        lum = 0.2 / (length(mod(uv * 1.0, 2.0)) + 0.1);
        
        col[i] = lum * abs(sin(zoom + length(uv) * 11.0));
        
        zoom += 0.1;
    }
    
    return float4(col / lum, 1.0);
}