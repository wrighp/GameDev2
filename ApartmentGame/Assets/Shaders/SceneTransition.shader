Shader "Custom/SceneTransitions"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		
		_Color("Screen Color", Color) = (1,1,1,1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_MaskSpread("Smooth", Range(0, 1)) = 0
		[MaterialToggle] _Distort("Distort", Float) = 0
		_Fade("Fade", Range(0, 1)) = 0
		[Toggle(INVERT_MASK)] _INVERT_MASK ("Mask Invert", Float) = 0
	}

		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
					float4 vertex : SV_POSITION;
				};

				float4 _MainTex_TexelSize;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uv = v.uv;
					o.uv1 = v.uv;

					/*#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv1.y = 1 - o.uv1.y;
					#endif*/

					return o;
				}

			//	int _Distort;
				//float _Fade;
				sampler2D _TransitionTex;
				sampler2D _MainTex;
				float _Cutoff;
				float _MaskSpread;
				fixed4 _Color;

				fixed4 frag(v2f i) : SV_Target
				{
					float4 col = tex2D(_MainTex, i.uv);
					float4 mask = tex2D(_TransitionTex, i.uv);

					// Scale 0..255 to 0..254 range.
					float alpha = mask.x/* * (1 - 1/255.0)*/;
					
					//return float4(mask.y, 0, 0, 1);

					// If the mask value is greater than the alpha value,
					// we want to draw the mask.
					float weight = smoothstep(_Cutoff - _MaskSpread, _Cutoff, alpha);
					/*#if INVERT_MASK
						weight = 1 - weight;
					#endif*/

					// Blend in mask color depending on the weight
					//col.rgb = lerp(_MaskColor, col.rgb, weight);

					// Blend in mask color depending on the weight
					// Additionally also apply a blend between mask and scene
					col.rgb = lerp(col.rgb, lerp(_Color.rgb, col.rgb, weight), _Color.a);

					return col;
				}					
				ENDCG
			}
		}
}
