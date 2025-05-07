cbuffer PerFrame : register(b0)
{
    float InverseGamma;
};

struct PSInput
{
    float4 Position : SV_Position;
};

float2 GetVertex(uint vertexId)
{
    return float2((vertexId << 2) & 4, (vertexId << 1) & 4) - 1;
}

PSInput VSMain(uint vertexId : SV_VertexID)
{
    PSInput output = (PSInput) 0;
    
    output.Position = float4(GetVertex(vertexId), 0.5, 1);
    
    return output;
}

Texture2D<float3> Source : register(t0);

float3 Reinhard(float3 color)
{
    return color / (color + 1.0);
}

float4 PSMain(PSInput input) : SV_Target0
{
    const float ditherWidth = 1.0 / 255.0;
	
    float dither = input.Position.x * input.Position.x + input.Position.y;
    
    dither = asuint(dither) * 776531419;
    dither = asuint(dither) * 961748927;
    dither = asuint(dither) * 217645199;
    dither = (asuint(dither) & 65535) / 65535.0;

	// Either Reinhard or saturate can be used for tonemapping
    float3 adjustedColor = pow(Reinhard(Source[input.Position.xy]), InverseGamma);

    adjustedColor = saturate(adjustedColor + ditherWidth * (dither - 0.5));
    
    return float4(adjustedColor, 1);
    //return float4(Source[input.Position.xy], 1.0);
}