Shader "Custom/celShade_v4" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_UnlitColor("Shadow Color", Color) = (1, 1, 1, 1)
		_DiffuseThres("Lighting Threshold", Range(0,1)) = 0.35
		
		_SpecColor("Specular Color",  Color) = (1, 1, 1, 1)
		_Shininess("Shininess", Range(0, 1)) = 1
		
		_RimCol("Rim Colour", Color) = (1, 1, 1, 1)
		_RimPow("Rim Power", Range(0.1, 10.0)) = 3.0
		
		_MainTex("Main Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Tags { 
			"LightMode" = "ForwardBase"
			}
			
			LOD 100

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
            #include "Lighting.cginc"
			
			
			// compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"
			
			uniform float4 _Color;
			//uniform float4 _SpecColor;
			uniform float4 _RimCol;
			uniform float4 _UnlitColor;
			
			uniform float _Shininess;
			uniform float _RimPow;
			uniform float _DiffuseThres;
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			//uniform float4 _LightColor0;
			
			//input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				SHADOW_COORDS(3) // put shadows data into TEXCOORD3
			};
			
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				
				// compute shadows data
                TRANSFER_SHADOW(o)
				
				return o;
			}
			
			float4 frag(vertexOutput i) :COLOR
			{
				
				//get the normal and view directions
				float3 normalD = i.normalDir;
				float3 viewD = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				
				//for handling point lights vs directional
				//vertex to light
				float3 f2LS = float3(_WorldSpaceLightPos0.xyz - float3(i.posWorld.xyz));
				float distance = length(f2LS);
				float3 lightD = normalize(
					lerp( 
						float3(_WorldSpaceLightPos0.xyz), 
						f2LS,
						_WorldSpaceLightPos0.w
						)
					);
				
				//attenuation will be < 1 when a point light is being used
				//use this to prevent the light from
				//showing further than it should be able to
				float atten = lerp(1.0,1/distance, _WorldSpaceLightPos0.w);
				
				float testDistance = length(lightD);
				float3 L = lightD;
				L/=distance;
				float dotL = max(dot(L, i.normalDir), 0);
				
				//get the dot product of the normal to the light
				float NDotL = saturate(dot(i.normalDir, lightD));
				
				//lighting
				//get cutoff values for the diffuse and specularity to get
				//the cell shaded effect
				float diffuseCutoff = saturate((max(_DiffuseThres, NDotL) - _DiffuseThres/atten)*1000);
				float specularCutoff = saturate(max(_Shininess, 
					dot(reflect(-lightD,i.normalDir),viewD)) - _Shininess)*1000;
					
				half rim = 1 - saturate(dot(viewD,normalD));
					
				float3 diffuseReflection = (1-specularCutoff) * _Color.xyz * diffuseCutoff;
					
				float3 specularReflection = /*_SpecColor use this to control 
					the specular colour vs letting the light colour decide*/
					_LightColor0.xyz * specularCutoff;
				float3 ambientLight = (1-diffuseCutoff) * _UnlitColor.xyz;
				
				//remove the saturate and dot for circumference lighting
				float3 rimLight = saturate(dot(normalD, lightD) * _RimCol.xyz *
					pow(rim, _RimPow));
					
				// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shd = SHADOW_ATTENUATION(i);	
				int shadow = (int) (shd+0.5);
				
				float3 lightFinal = rimLight + (
					lerp(_UnlitColor.rgb, diffuseReflection, shadow)
					) + specularReflection 
					 +ambientLight * shadow;
					//UNITY_LIGHTMODEL_AMBIENT;
					
				//lightFinal = (ambientLight + diffuseReflection) * outlineStr + specularReflection
				UNITY_LIGHT_ATTENUATION(attenuation, i, float3(i.posWorld.xy, 0.0));
				
				float3 finalAddition = lerp(float3(1, 1, 1), dotL*attenuation, 
					_WorldSpaceLightPos0.w);
				//get the texture
				//textureMaps
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + 
					_MainTex_ST.zw);
				
				return float4(tex.xyz * lightFinal * _Color.rgb * finalAddition, 1.0);
				
			}
			ENDCG
		}
		Pass
		{
			Tags { 
			"LightMode" = "ForwardAdd"
			}
			
			Blend One One
			
			LOD 100

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
            #include "Lighting.cginc"
			
			
			// compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"
			
			uniform float4 _Color;
			//uniform float4 _SpecColor;
			uniform float4 _RimCol;
			uniform float4 _UnlitColor;
			
			uniform float _Shininess;
			uniform float _RimPow;
			uniform float _DiffuseThres;
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			//uniform float4 _LightColor0;
			
			//input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				SHADOW_COORDS(3) // put shadows data into TEXCOORD3
			};
			
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				
				// compute shadows data
                TRANSFER_SHADOW(o)
				
				return o;
			}
			
			float4 frag(vertexOutput i) :COLOR
			{
				
				//get the normal and view directions
				float3 normalD = i.normalDir;
				float3 viewD = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				
				//for handling point lights vs directional
				//vertex to light
				float3 f2LS = float3(_WorldSpaceLightPos0.xyz - float3(i.posWorld.xyz));
				float distance = length(f2LS);
				float3 lightD = normalize(
					lerp( 
						float3(_WorldSpaceLightPos0.xyz), 
						f2LS,
						_WorldSpaceLightPos0.w
						)
					);
					
				float testDistance = length(lightD);
				float3 L = lightD;
				L/=distance;
				float dotL = max(dot(L, i.normalDir), 0);
				
				float atten = lerp(1.0,1/distance, _WorldSpaceLightPos0.w);
				
				//get the dot product of the normal to the light
				float NDotL = saturate(dot(i.normalDir, lightD));
				
				//lighting
				float diffuseCutoff = saturate((max(_DiffuseThres, NDotL) - _DiffuseThres/atten)*1000);
				//tone down the specularity
				float specularCutoff = saturate(max(_Shininess, 
					dot(reflect(-lightD,i.normalDir),viewD)) - (_Shininess*1.05))*1000;
					
				half rim = 1 - saturate(dot(viewD,normalD));
					
				float3 diffuseReflection = (1-specularCutoff) * _Color.xyz * diffuseCutoff;
				float3 specularReflection = /*_SpecColor*/_LightColor0.xyz * specularCutoff;
				
				//remove the saturate and dot for circumference lighting
				float3 rimLight = saturate(dot(normalD, lightD) * _RimCol.xyz *
					_LightColor0.xyz * pow(rim, _RimPow));
				
				fixed shd = SHADOW_ATTENUATION(i);	
				int shadow = (int) (shd+0.5);
				
				float3 lightFinal = rimLight + (
					lerp(_UnlitColor.rgb, diffuseReflection, shadow) 
					) + specularReflection;
					
				UNITY_LIGHT_ATTENUATION(attenuation, i, float3(i.posWorld.xy, 0.0));
				
				float3 finalAddition = lerp(float3(1, 1, 1), dotL*attenuation, 
					_WorldSpaceLightPos0.w);
					
				//lightFinal = (ambientLight + diffuseReflection) * outlineStr + specularReflection
				
				return float4(lightFinal * _Color.rgb * finalAddition, 1.0);
				
			}
			ENDCG
		}
		// shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	FallBack "Diffuse"
}
