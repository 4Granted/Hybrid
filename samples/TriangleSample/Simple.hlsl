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
    
    output.Position = float4(input.Position, 1.0);
    output.Color = input.Color;
    
    return output;
}

float4 PSMain(PSInput input) : SV_Target
{
    return input.Color;
}