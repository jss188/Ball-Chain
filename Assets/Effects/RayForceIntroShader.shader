Shader "Custom/RayForceIntroShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FadeTex ("Fade Texture", 2D) = "white" {}
		_FadeAmount("Fade Amount", Range(0,1)) = 0
		_FadeColor("Fade Color", Color) = (0,0,0,0)
		_FadeSmoother("Fade Smoother", Float) = 10
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

			float4 _MainTex_TexelSize;

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


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv1 = v.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv1.y = 1 - o.uv1.y;
				#endif
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _FadeTex;
			float _FadeAmount;
			float4 _FadeColor;
			float _FadeSmoother;

			//Inverse Lerp: t = (Output - Input1) / (Input2 - Input1)
			float invLerp(float a, float b, float value) {
				return (value - a) / (b - a);
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				
				float4 fade = tex2D(_FadeTex, i.uv1);
				
				if (fade.r < _FadeAmount)
					return _FadeColor;

				float t = min(invLerp(_FadeAmount, _FadeAmount + lerp(0, _FadeSmoother, _FadeAmount), fade.r), 1);
				col = lerp( _FadeColor, col, t);

				return col;
			}
		ENDCG
		}
	}
}
