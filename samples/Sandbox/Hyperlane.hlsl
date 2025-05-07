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

struct VSInput
{
    float3 Position : POSITION;
    float4 Color : COLOR;
};

struct PSInput
{
    float4 Position : SV_Position;
    float4 Color : COLOR0;
};

PSInput VSMain(in VSInput input)
{
    PSInput output = (PSInput) 0;
    
    output.Position = mul(float4(input.Position, 1.0), ViewProjection);
    output.Color = input.Color;
    
    return output;
}

float4 PSMain(PSInput input) : SV_Target0
{
    return input.Color;
}