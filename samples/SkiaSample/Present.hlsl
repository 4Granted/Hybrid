struct PSInput
{
    float4 Position : SV_Position;
};

PSInput VSMain(uint vertexId : SV_VertexId)
{
    PSInput output = (PSInput) 0;
    
    float2 xy = float2((vertexId << 2) & 4, (vertexId << 1) & 4) - 1;
    
    output.Position = float4(xy, 0.5, 1);
    
    return output;
}

Texture2D<float3> Source : register(t0);

float3 PSMain(PSInput input) : SV_Target
{
    return Source[input.Position.xy];
}