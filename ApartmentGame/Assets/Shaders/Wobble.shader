Shader "Custom/Wobble" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		
		_V1 ("Size(?)", range(0, 1)) = 0.0
		_V2 ("value2", range(0, 1)) = 0.0
		_V3 ("value3", range(0, 1)) = 0.0
		_Rate ("Rate", range(0, 100)) = 50
		_Amp ("Amplitude", range(0, 1)) = 0.5
		
		_MainTex("Main Texture", 2D) = "white" {}
		
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
			
			uniform float _V1;
			uniform float _V2;
			uniform float _V3;
			uniform float _Rate;
			uniform float _Amp;
			uniform float4 _Color;
			
			uniform sampler2D _MainTex;
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
				//v.vertex.x += sin((v.vertex.y + _Time * _V3* _Rate) * _V2* _Amp ) * _V1;
				//v.vertex.y += sin((v.vertex.x + _Time * _V3* _Rate) * _V2 * _Amp) * _V1;
				
				//test2
				v.vertex.xyz += v.normal * (sin((v.vertex.x+_Time*_V3)*_V2 * _Rate) +
					cos((v.vertex.z + _Time * _V3)*_V2) + _Amp) * _V1;
				
				o.colour = float4(v.normal, 1) * 0.5 + 0.25;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.tex = v.texcoord;
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
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
