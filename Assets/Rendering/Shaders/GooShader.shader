Shader "URP/LitStretchShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _WorldPoint ("Stretch Point", Vector) = (0,0,0)
        _MaxStretchDistance ("Max Stretch Distance", Float) = 1.0
        _SpecularColor ("Specular Color", Color) = (1,1,1,1) // Chooseable specular color
        _Smoothness ("Smoothness", Float) = 1.0 // Hard-edged specular (set to high value)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _WorldPoint;
			float _MaxStretchDistance;
			float4 _SpecularColor;
			float _Smoothness;
			float3 _LightDir;    // Manually passed light direction
			float3 _LightColor;  // Manually passed light color
			float _WobbleTime;   // Manually passed wobble time
		CBUFFER_END


        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        struct VertexInput
		{
		    float4 position : POSITION;
		    float2 uv : TEXCOORD0;
		    float3 normal : NORMAL; // Add normal data
		};

        
        struct VertexOutput
        {
            float4 position : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 worldPos : TEXCOORD1;
            float3 normal : TEXCOORD2;
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
                //o.normal = TransformObjectToWorldNormal(float3(0, 0, 1));
				//working o.normal = normalize(mul((float3x3)UNITY_MATRIX_M, i.normal));
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, i.normal));

                // Stretch effect
                float3 direction = normalize(_WorldPoint.xyz - o.worldPos);
                float distance = length(_WorldPoint.xyz - o.worldPos);
                float stretchAmount = saturate(1.0 - distance / _MaxStretchDistance);
                float3 stretchOffset = direction * stretchAmount * _MaxStretchDistance;
				stretchOffset.y = 0.0; // Keep Y axis fixed

				// Clamp the movement so it doesn't overshoot
				o.worldPos = clamp(o.worldPos + stretchOffset, min(o.worldPos, _WorldPoint.xyz), max(o.worldPos, _WorldPoint.xyz));

				float wobbleStrength = 0.5; // Adjust for how strong the wobble is
				float wobbleSpeed = 0.75; // Controls the speed of wobbling
				
				// Use sine wave for smooth oscillation
				// Use manually controlled wobble time instead of Unity's global time
				float3 wobble = float3(
					sin(_WobbleTime * wobbleSpeed + o.worldPos.x) * wobbleStrength,
					0.0, // Keep Y movement locked
					 cos(_WobbleTime * wobbleSpeed + o.worldPos.z) * wobbleStrength
				);

				
				// Apply the wobble
				o.worldPos += wobble;

                
                o.position = TransformWorldToHClip(o.worldPos);
                o.uv = i.uv;
                return o;
            }
            
            float4 frag(VertexOutput i) : SV_Target
			{
				float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				float3 normal = normalize(i.normal);

				// View direction
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

				// Specular calculation (Reflection Model)
				float3 lightDir = normalize(_LightDir); // Use manually passed light direction
				float3 lightColor = _LightColor; // Use manually passed light color
				
				float3 reflectDir = reflect(-lightDir, normal);
				float glossFactor = frac(sin(i.uv.x * 100.0 + i.uv.y * 200.0) * 43758.5453);
				float modifiedSmoothness = _Smoothness * glossFactor;
				float specFactor = pow(max(dot(viewDir, reflectDir), 0.0), modifiedSmoothness * 5012);
				float microfacetFactor = smoothstep(0.4, 0.6, dot(viewDir, reflectDir));
				float3 specular = _SpecularColor.rgb * specFactor * lightColor * microfacetFactor;


				// Final shading
				float3 finalColor = baseTex.rgb * _BaseColor.rgb * lightColor + specular;

				return float4(finalColor, 1.0);
			}

            ENDHLSL
        }
    }
}
