Shader "Custom/WhiteNoise" {
	Properties {
		_MainTex("Main Texture", 2D) = "white" {}
		_DisplaceTex("Whitenoise Texture", 2D) = "white" {}
		_Magnitude ("Whitenoise Magnitude", Range(0, 1)) = 1
		
		_Color("Main Color", Color) = (1,1,1,1)
		
		//_V1 ("Offset", range(0, 1)) = 0.0
		//_V2 ("value2", range(0, 1)) = 0.0
		//_V3 ("value3", range(0, 1)) = 0.0
		
		_PX ("Phase X", range(0, 100)) = 50
		_RX ("Frequency X", range(0, 100)) = 50
		_AX ("Amplitude X", range(0, 1)) = 0.1
		
		_PY ("Phase Y", range(0, 100)) = 50
		_RY ("Frequency Y", range(0, 100)) = 50
		_AY ("Amplitude Y", range(0, 1)) = 0.1
		
		_PZ ("Phase Z", range(0, 100)) = 50
		_RZ ("Frequency Z", range(0, 100)) = 50
		_AZ ("Amplitude Z", range(0, 1)) = 0.1
		
		// Cel shading bit ================================================
		
		_UnlitColor("Shadow Color", Color) = (1, 1, 1, 1)
		_DiffuseThres("Lighting Threshold", Range(0,1)) = 0.35
		
		_SpecColor("Specular Color",  Color) = (1, 1, 1, 1)
		_Shininess("Shininess", Range(0, 1)) = 1
		
		_RimCol("Rim Colour", Color) = (1, 1, 1, 1)
		_RimPow("Rim Power", Range(0.1, 10.0)) = 3.0
		
	}
	SubShader {
		
		Tags{"Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		
		pass{
		
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

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			
			/*uniform float _V1;
			uniform float _V2;
			uniform float _V3;*/
			
			uniform float _PX;
			uniform float _RX;
			uniform float _AX;
			
			uniform float _PY;
			uniform float _RY;
			uniform float _AY;
			
			uniform float _PZ;
			uniform float _RZ;
			uniform float _AZ;
			
			uniform float _Magnitude;
			
			
			uniform float4 _Color;
			
			//CEL SHADE====================================================
			
			uniform float4 _RimCol;
			uniform float4 _UnlitColor;
			
			uniform float _Shininess;
			uniform float _RimPow;
			uniform float _DiffuseThres;
			//=================================================================
			
			uniform sampler2D _MainTex;
			uniform sampler2D _DisplaceTex;
			
			uniform float4 _MainTex_ST;
			uniform float4 _DisplaceTex_ST;
			
			//input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 colour : COLOR;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};
			

			
			vertexOutput vert (vertexInput v){
				vertexOutput o;
				
				//test
				v.vertex.xyz += /*v.normal +*/ sin((v.vertex.x + _Time * _PX* _RX) * _RX) *  _AX;
				v.vertex.xyz += /*v.normal +*/ cos((v.vertex.y + _Time * _PY* _RY) * _RY) * _AY;
				v.vertex.xyz += /*v.normal +*/ sin((v.vertex.z + _Time * _PZ* _RZ) * _RZ) * _AZ;
				
				//bubbling test
				//v.vertex.xyz += v.normal * (sin((v.vertex.x+_Time*_PX)*_RX) +
					//cos((v.vertex.y + _Time * _PX)*_RX) )* _AX;
					
				//wave equation
				//Amplitude * cos( frequency * time + phase)
				/*	
				v.vertex.x+= _AX * (cos(v.vertex.y + _Time *(_RX + _PX)) + 
					sin(v.vertex.z + _Time *(_RX + _PX)));
					
				v.vertex.y+= _AY * (cos(v.vertex.x + _Time * (_RY + _PY))+
					sin(v.vertex.z + _Time * (_RY + _PY)));
					
				v.vertex.z+= _AZ * (cos(v.vertex.x + _Time * (_RZ + _PZ))+
					sin(v.vertex.y + _Time * (_RZ + _PZ)));
				*/

				float4 tex = tex2Dlod(_DisplaceTex, v.texcoord);
					
				//v.vertex.x+=sin(_Time);
				//v.vertex.y+=cos(_Time);
				//v.vertex.z+=sin(_Time);
				
				
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				
				o.colour = float4(v.normal, 1) * 0.5 + 0.25;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.tex = v.texcoord;
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
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
					
				//discard transparent pixels ======================================= TRANSPARENCY
				if(tex.a == 0)
					discard;
				
				return float4(tex.xyz * lightFinal * _Color.rgb * finalAddition, 1.0);
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
