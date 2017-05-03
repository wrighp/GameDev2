Shader "Custom/Wobble" {
	Properties {
		_MainTex("Main Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude ("Distortion Magnitude", Range(0, 0.1)) = 1
		
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
		
	}
	SubShader {
		
		Tags{"Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		
		pass{
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
			
			uniform sampler2D _MainTex;
			sampler2D _DisplaceTex;
			
			uniform float4 _MainTex_ST;
			
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
				//float4 posWorld : TEXCOORD1;
				//float3 normalDir : TEXCOORD2;
			};
			

			
			vertexOutput vert (vertexInput v){
				vertexOutput o;
				
				//test
				//v.vertex.x += sin((v.vertex.y + _Time * _V3* _RX) * _V2* _AX ) * _V1;
				//v.vertex.y += sin((v.vertex.x + _Time * _V3* _RX) * _V2 * _AX) * _V1;
				
				//test2
				//v.vertex.xyz += v.normal * (sin((v.vertex.x+_Time*_V3)*_V2 * _RX) +
					//cos((v.vertex.z + _Time * _V3)*_V2) + _AX) * _V1;
					
				//wave equation
				//Amplitude * cos( frequency * time + phase)
					
				v.vertex.x+=v.normal.x * _AX * (cos(v.vertex.y + _Time *(_RX + _PX)) + 
					sin(v.vertex.z + _Time *(_RX + _PX)));
					
				v.vertex.y+=v.normal.y * _AY * (cos(v.vertex.x + _Time * (_RY + _PY))+
					sin(v.vertex.z + _Time * (_RY + _PY)));
					
				v.vertex.z+=v.normal.z * _AZ * (cos(v.vertex.x + _Time * (_RZ + _PZ))+
					sin(v.vertex.y + _Time * (_RZ + _PZ)));
				
				o.colour = float4(v.normal, 1) * 0.5 + 0.25;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.tex = v.texcoord;
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				float2 displace = tex2D(_DisplaceTex, i.tex.xy).xy;
				displace = ((displace*2) - 1) *( _Magnitude*sin(_Time.y));
				
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw + displace);
				if(tex.a == 0)
					discard;
				return float4(tex.xyz * _Color, 1);
				//return i.colour;
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
