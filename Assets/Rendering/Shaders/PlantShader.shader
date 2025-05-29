Shader "URP/WavingPlantShader"
{
    Properties
    {
        _WaveStrength ("Wave Strength", Float) = 0.2
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _CenterY ("Fixed Height Y", Float) = 0.0 // Y-coordinate below which no movement occurs
		_MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float _WaveStrength;
            float _WaveSpeed;
            float _CenterY;
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
        CBUFFER_END

        struct VertexInput
		{
		    float4 position : POSITION;
		    float3 normal : NORMAL;
		    float2 uv : TEXCOORD0; 
		};

        struct VertexOutput
		{
		    float4 position : SV_POSITION;
		    float3 worldPos : TEXCOORD0;
		    float2 uv : TEXCOORD1; 
		};

        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            VertexOutput vert(VertexInput i)
			{
			    VertexOutput o;
			    o.worldPos = TransformObjectToWorld(i.position.xyz); 
			   
			    // Use object-space Y instead of global world-space Y
			    float localY = i.position.y; 
			
			    if (localY > _CenterY)  
			    {
					float waveOffset = sin(_Time.x * _WaveSpeed + o.worldPos.x) * _WaveStrength;
					o.worldPos.z += cos(_Time.x * _WaveSpeed + o.worldPos.z) * _WaveStrength; // Use `_Time.x` instead of `_Time.y`
					o.worldPos.x += waveOffset;
			    }

				o.uv = i.uv; 
			
			    o.position = TransformWorldToHClip(o.worldPos);
			    return o;
			}

            float4 frag(VertexOutput i) : SV_Target
			{
			    float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			
			    // Prepare input data for lighting
			    InputData inputData;
			    inputData.positionWS = i.worldPos;
			    inputData.normalWS = normalize(i.worldPos - _WorldSpaceCameraPos);
			    inputData.viewDirectionWS = normalize(_WorldSpaceCameraPos - i.worldPos);
			

			    float3 totalLighting = baseTex.rgb * 0.1;
			
			    #ifdef _ADDITIONAL_LIGHTS
			        int additionalLightCount = min(GetAdditionalLightsCount(), 2); // 
			        for (int i = 0; i < additionalLightCount; i++)
			        {
			            LightData dynamicLight;
			            GetAdditionalLight(i, inputData, dynamicLight);
			
			            float3 lightDir = normalize(dynamicLight.direction);
			            float3 lightColor = dynamicLight.color * dynamicLight.distanceAttenuation;
			
			            float diffuse = max(dot(inputData.normalWS, lightDir), 0.0);
			            totalLighting += baseTex.rgb * diffuse * lightColor; 
			        }
			    #endif
			
			    return float4(totalLighting, baseTex.a);
			}

            ENDHLSL
        }
    }
}
