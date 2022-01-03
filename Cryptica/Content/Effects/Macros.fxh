#define DECLARE_TEXTURE(Name, index) \
    Texture2D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index) { \
		MinFilter = Point; \
		MagFilter = Point; \
	 };
#define SAMPLE_TEXTURE(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)

#if NSWITCH
	#define VS_SHADERMODEL vs_5_0
	#define PS_SHADERMODEL ps_5_0
#elif OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0

	#define DECLARE_TEXTURE(Name, index) \
		Texture2D<float4> Name; \
		SamplerState Name##Sampler { \
			Texture = <Name>; \
			MinFilter = Point; \
			MagFilter = Point; \
		};
	#define SAMPLE_TEXTURE(Name, texCoord)  tex2D(Name##Sampler, texCoord);
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


