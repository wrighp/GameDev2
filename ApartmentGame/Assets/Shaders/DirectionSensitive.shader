Shader "Custom/DirectionSensitive" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_SecondTex ("Texture", 2D) = "white" {}
		_Tween("Tween", Range(0,1)) = 0
	}
	SubShader {
		Tags
		{
			"Queue" = "Transparent"
		}
		pass{
			
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			
			
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
			};
			
			vertexOutput vert (vertexInput v){
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				o.tex = v.texcoord;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Color;
			
			float4 frag(vertexOutput i) : COLOR
			{
				//get the view direction
				float3 viewD = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				
				//unwrap the texture
				//float4 color = tex2D(_MainTex, i.tex);
				float4 sweep = float4(i.tex.x, i.tex.y, 0.6, 1);
				//return color * _Color;
				return /*color  _Color */ float4(viewD*max(0.5, (sin(_Time.y))), 1.0);
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
