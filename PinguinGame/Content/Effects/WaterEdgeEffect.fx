#include "Macros.fxh"

DECLARE_TEXTURE(Texture0, 0);

cbuffer Parameters : register(b0) {
	float4x4 WorldViewMatrix;
	float4x4 ProjectionMatrix;

	float4 AboveWaterColor;
	float4 BelowWaterColor;
	float WaterLine;
}

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;    
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 local = mul(input.Position, WorldViewMatrix);
	
	output.Position = mul(local, ProjectionMatrix);

	output.Color = input.Color;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
	float4 textureColor = SAMPLE_TEXTURE(Texture0, input.TextureCoordinate);

	clip(textureColor.a - 0.1f);

	if(textureColor.r + 1000 > WaterLine) return AboveWaterColor;
	return BelowWaterColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};