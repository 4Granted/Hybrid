#define HASHSCALE3 float3(.1031, .1030, .0973)

cbuffer PerView : register(b0)
{
    row_major float4x4 View;
    row_major float4x4 ViewInverse;
    row_major float4x4 Projection;
    row_major float4x4 ProjectionInverse;
    row_major float4x4 ViewProjection;
    row_major float4x4 ViewProjectionInverse;
    float3 CameraPosition;
    int2 Resolution;
    float DeltaTime;
};

cbuffer PerFrame : register(b1)
{
    float3 StarColor;
    float StarDensity;
    float StarIntensity;
};

struct VSInput
{
    float3 Position : POSITION;
};

struct PSInput
{
    float4 Position : SV_Position;
    float3 Direction : TEXCOORD0;
};

PSInput VSMain(in VSInput input)
{
    PSInput output = (PSInput) 0;
    
    float3 worldPosition = input.Position;
    
    worldPosition += CameraPosition;
    
    output.Position = mul(float4(worldPosition, 1.0), ViewProjection);
    output.Direction = input.Position;
    
    return output;
}

float2 Hash(float2 p)
{
    float3 p3 = frac(float3(p.xyx) * HASHSCALE3);
    
    p3 += dot(p3, p3.yzx + 19.19);
    
    return frac((p3.xx + p3.yz) * p3.zy);
}

float4 PSMain(PSInput input) : SV_Target0
{
    float3 dir = normalize(input.Direction);

    return float4(0.0, 0.0, 0.0, 1.0);
}