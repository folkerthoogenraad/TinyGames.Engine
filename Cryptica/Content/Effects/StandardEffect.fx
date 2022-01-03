#include "Macros.fxh"

DECLARE_TEXTURE(Texture0, 0);

cbuffer Parameters : register(b0) {
	float4x4 WorldViewMatrix;
	float4x4 ProjectionMatrix;

	float ScreenWidth;
	float ScreenHeight;
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

	// float2 screenPos = output.Position.xy * float2(ScreenWidth, ScreenHeight);
	// output.Position.xy = round(screenPos) / float2(ScreenWidth, ScreenHeight);

	output.Color = input.Color;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
	float4 textureColor = SAMPLE_TEXTURE(Texture0, input.TextureCoordinate);

	// textureColor.rg += input.TextureCoordinate;
	// textureColor.a = 1;

	clip(textureColor.a - 0.1f);

	float4 output = input.Color * textureColor;
	
	return output;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};