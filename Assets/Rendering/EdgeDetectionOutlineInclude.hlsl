#ifndef SOBEL_INCLUDED
#define SOBEL_INCLUDED

static float simpleBlur[9] = {
	1,1,1,
	1,1,1,
	1,1,1
};

static float gaussianBlur[9] = {
	1,2,1,
	2,4,2,
	1,2,1
};

static float sobelYMatrix[9] = {
	1,2,1,
	0,0,0,
	-1,-2,-1
};

static float sobelXMatrix[9] = {
	1,0,-1,
	2,0,-2,
	1,0,-1
};

static float2 sobelSamplePoints[9] = {
	float2(-1,1),float2(0,1),float2(1,1),
	float2(-1,0),float2(0,0),float2(1,1),
	float2(-1,-1),float2(0,-1),float2(1,-1),
};

void TextureSobel_float(float2 UV, float Thickness, UnityTexture2D Tex, UnitySamplerState SS, out float Out) {
	float2 sobel = 0;

	[unroll] for (int i = 0; i < 9; i++)
	{
		float depth = SAMPLE_TEXTURE2D(Tex, SS, UV + sobelSamplePoints[i] * Thickness).r;
		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
	}

	Out = length(sobel);
}

void DepthSobel_float(float2 UV, float Thickness, out float Out) {
	float2 sobel = 0;

	[unroll] for (int i = 0; i < 9; i++)
	{
		float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
	}

	Out = length(sobel);
}

void NormalSobel_float(float2 UV, float Thickness, out float Out) {
	float2 sobel = 0;

	[unroll] for (int i = 0; i < 9; i++)
	{
		float normal = mul(SHADERGRAPH_SAMPLE_SCENE_NORMAL(UV + sobelSamplePoints[i] * Thickness), (float3x3) UNITY_MATRIX_I_V);
		sobel += normal * float2(sobelXMatrix[i], sobelYMatrix[i]);
	}

	Out = length(sobel);
}

void NormalTextureSample_float(float2 UV, out float3 Out) {
	Out = mul(SHADERGRAPH_SAMPLE_SCENE_NORMAL(UV), (float3x3) UNITY_MATRIX_I_V);
	//Out = SHADERGRAPH_SAMPLE_SCENE_NORMAL(UV);
}
#endif

