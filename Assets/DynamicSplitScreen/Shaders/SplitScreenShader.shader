
Shader "Split Screen Shader" 
{
	Properties
	{
		_MainTex("Player 1 Screen", 2D) = "white" {}
		_SecondaryTex("Player 2 Screen", 2D) = "white" {}
		_BackgroundTex("BackgroundTexture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

		ZWrite Off

		ColorMask RGB

		Pass 
		{

		CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _SecondaryTex;
			sampler2D _BackgroundTex;

			struct appdata
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_BackgroundTex, float2(i.uv.x, i.uv.y));

				if (col.x > 0)
				{
					return tex2D(_MainTex, float2(i.uv.x, i.uv.y));
				}
				else
				{
					return tex2D(_SecondaryTex, float2(i.uv.x, i.uv.y));
				}
			}
			ENDCG
		}
	}
}

