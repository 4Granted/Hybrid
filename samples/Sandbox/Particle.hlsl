#define DEG_TO_RAD 0.01745329251

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

cbuffer PerEffect : register(b2)
{
    int PertN;
    float StarSize;
    float ParticleSize;
    float PertAmp;
    float Time;
};

struct VSInput
{
    float Theta0 : THETAZ;
    float VelTheta : THETAV;
    float TiltAngle : TILTANGLE;
    float A : AVAL;
    float B : BVAL;
    float Temp : TEMP;
    float Mag : MAG;
    int Type : TYPE;
    float4 Color : COLOR;
};

struct GSInput
{
    float3 WorldPosition : POSITION;
    float4 Color : COLOR;
    float PointSize : PSIZE;
    int Type : TYPE;
};

struct PSInput
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD;
    float4 Color : COLOR;
    int Type : TYPE;
};

float2 GetPosition(float a, float b, float theta, float velTheta, float time, float tiltAngle)
{
    float thetaActual = theta + velTheta * time;
    float beta = -tiltAngle;
    float alpha = thetaActual * DEG_TO_RAD;
    float cosalpha = cos(alpha);
    float sinalpha = sin(alpha);
    float cosbeta = cos(beta);
    float sinbeta = sin(beta);
    float2 center = float2(0, 0);
    float2 ps = float2(center.x + (a * cosalpha * cosbeta - b * sinalpha * sinbeta),
			       center.y + (a * cosalpha * sinbeta + b * sinalpha * cosbeta));
    
    if (PertAmp > 0.0 && PertN > 0)
    {
        ps.x += (a / PertAmp) * sin(alpha * 2.0 * PertN);
        ps.y += (a / PertAmp) * cos(alpha * 2.0 * PertN);
    }
    
    return ps;
}

GSInput VSMain(in VSInput input)
{
    GSInput output = (GSInput) 0;
    
    float2 position = GetPosition(input.A, input.B, input.Theta0, input.VelTheta, Time, input.TiltAngle);
    
    float mag = input.Mag;
    float4 color = input.Color;
    
    if (input.Type == 0)
    {
        output.PointSize = mag * 4.0 * StarSize;
        output.Color = color * mag;
    }
    else if (input.Type == 1)
    {
        output.PointSize = mag * 5.0 * ParticleSize;
        output.Color = color * mag;
    }
    else if (input.Type == 2)
    {
        output.PointSize = mag * 2.0 * ParticleSize;
        output.Color = color * mag;
    }
    
    output.WorldPosition = float3(position.x, 0.0, position.y);
    output.PointSize = max(output.PointSize, 0.0);
    output.Type = input.Type;
    
    return output;
}

[maxvertexcount(6)]
void GSMain(point GSInput input[1], inout TriangleStream<PSInput> stream)
{
    float size = input[0].PointSize;

    float3 offsets[4] =
    {
        float3(-size, 0.0,  size), // Top-left
        float3( size, 0.0,  size), // Top-right
        float3( size, 0.0, -size), // Bottom-right
        float3(-size, 0.0, -size)  // Bottom-left
    };

    float2 texCoords[4] =
    {
        float2(0.0, 0.0), // Top-left
        float2(1.0, 0.0), // Top-right
        float2(1.0, 1.0), // Bottom-right
        float2(0.0, 1.0)  // Bottom-left
    };

    int indices[6] = { 0, 1, 2, 0, 2, 3 };

    for (int i = 0; i < 6; ++i)
    {
        int idx = indices[i];
        
        PSInput output = (PSInput)0;
        
        output.Position = mul(float4(input[0].WorldPosition + offsets[idx], 1.0), mul(World, ViewProjection));
        output.TexCoord = texCoords[idx];
        output.Color = input[0].Color;
        output.Type = input[0].Type;
        
        stream.Append(output);
    }
}

float4 PSMain(PSInput input) : SV_Target0
{
    float2 uv = input.TexCoord;  
    float4 color = input.Color;
    
    // Compute the normalized UV of the particle
    float2 circCoord = 2.0 * uv - 1.0;
    
    // Compute the alpha of the particle with a branchless conditional
    float alpha = input.Type == 0 ? 1 - length(circCoord)
        : input.Type == 1 ? 0.05 * (1 - length(circCoord))
        : 0.07 * (1 - length(circCoord));
    
    return float4(color.rgb, alpha);
}