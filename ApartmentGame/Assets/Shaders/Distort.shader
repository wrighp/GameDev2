Shader "Custom/Distort"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Range(0, 0.1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"
				"Queue" = "Transparent"}
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION; 
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata v){
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _DisplaceTex;
			float _Magnitude;
			
			float4 frag(v2f i) : SV_TARGET
			{
				float2 displace = tex2D(_DisplaceTex, i.uv).xy;
				displace = ((displace*2) - 1) *( _Magnitude*sin(_Time.y));
				
				float4 colour = tex2D(_MainTex, i.uv + displace);
				
				return colour;
			}
			
			ENDCG
		}
	}
}
