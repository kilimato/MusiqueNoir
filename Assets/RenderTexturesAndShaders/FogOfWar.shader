Shader "Hidden/FogOfWar"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_SecondaryTex("Secondary Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags
		{
		// now these are rendered with their sorting order instead of z-value
			"Queue" = "Transparent+1"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
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
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _SecondaryTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) + tex2D(_SecondaryTex, i.uv);
			// red = 1, blue = 1, we want alpha 0; == completely transparent
			// red = 1, blue = 0, we want 0.5 -> we want 0.3 instead
			// red = 0, blue=0, we want alpha 1 == completely solid
			// what if we want alpha 0.8 when red=0, blue=0?

		// how to count?
		// col.a = 2.0 - red value * 1.5 - blue value * 0.5 + green value
				if (col.r == 0 && col.b == 0) {
					col.a = 0.65f;
				}
				/*if (col.r == 1 && col.b == 0) {
					col.a = 0.3f;
				}*/
				else {
					col.a = 1.7f - col.r * 1.5f - col.b * 0.5f + col.g;
				}
				return fixed4(0,0,0,col.a);
			//return col;
		}
		ENDCG
	}
	}
}
