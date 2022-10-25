Shader "Unlit/BasicBlur"
{
	Properties{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BlurLevel("Blur Level", int) = 1
	}

		SubShader
		{
			Cull Off        // カリングは不要
			ZTest Always    // ZTestは常に通す
			ZWrite Off      // ZWriteは不要

			Tags { "RenderType" = "Opaque" }

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
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _BlurLevel; //ブラーの強度
				float2 _Resolution; //C#から渡ってくるディスプレイサイズ

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float offsetU = _BlurLevel / _Resolution.x; //U方向のオフセットを計算
					float offsetV = _BlurLevel / _Resolution.y; //V方向のオフセットを計算

					float4 color = tex2D(_MainTex, i.uv);

					color += tex2D(_MainTex, i.uv + float2(offsetU, 0.0f)); //右のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(-offsetU, 0.0f)); //左のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(0.0f, offsetV)); //下のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(0.0f, -offsetV)); //上のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(offsetU, offsetV)); //右下のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(offsetU, -offsetV)); //右上のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(-offsetU, offsetV)); //左下のテクセルのカラーをサンプリング

					color += tex2D(_MainTex, i.uv + float2(-offsetU, -offsetV)); //左上のテクセルのカラーをサンプリング

					color /= 9.0f; //8テクセル分の色が加算されているので、9で除算し平均化

					return color;
				}
				ENDCG
			}
		}
}