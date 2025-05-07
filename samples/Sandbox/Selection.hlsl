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

cbuffer PerDraw : register(b1)
{
    row_major float4x4 World;
    row_major float4x4 WorldInverse;
};

struct VSInput
{
    float3 Position : POSITION;
    float3 Normal : NORMAL;
    float2 TexCoord : TEXCOORD;
};

struct PSInput
{
    float4 Position : SV_Position;
    float3 NormalVS : NORMAL0;
    float3 NormalWS : NORMAL1;
    float2 TexCoord : TEXCOORD0;
};

PSInput VSMain(in VSInput input)
{
    PSInput output = (PSInput) 0;
    
    output.Position = mul(float4(input.Position, 1.0), mul(World, ViewProjection));
    
    float3 wsn = mul(input.Normal, (float3x3) transpose(WorldInverse));
    
    output.NormalVS = mul(wsn, (float3x3) transpose(ViewInverse));
    output.NormalWS = wsn;
    
    return output;
}

float4 PSMain(PSInput input) : SV_Target0
{
    return float4(1.0, 0.0, 0.0, 1.0);
}