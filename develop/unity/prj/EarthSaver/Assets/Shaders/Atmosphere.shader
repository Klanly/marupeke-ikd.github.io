Shader "Custom/Atmosphere"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0 )
		_DegAve("Hight density position(0-1)", Range( 0.0, 1.0 ) ) = 0.2
		_Var("Density distribution", Range( 0.00001, 1.0 ) ) = 0.1
		_EdgeAlphaPow("Edge Alpha Power", float) = 3.0
	}
	SubShader
	{
		Tags {
			"RenderType"="Transparent"
			"Queue" = "Transparent"
		}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS( 1 )
				float3 normal: TEXCOORD2;
				float3 posW: TEXCOORD3;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _EdgeAlphaPow;
			float _DegAve;
			float _Var;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.vertex);
				o.posW = mul( unity_ObjectToWorld, v.vertex );
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 lookDirW = normalize( _WorldSpaceCameraPos - i.posW );
				float cth = dot( lookDirW, normalize( i.normal ) );

				// 正規分布値算出
				float nd_pow = ( cth - _DegAve ) / _Var;
				float nd = exp( -nd_pow * nd_pow );

				// エッジ補正値
				float edgeAlphaAdj = 1.0 - pow( 1.0 - cth, _EdgeAlphaPow );

				col.a = nd * edgeAlphaAdj;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col * _Color;
			}
			ENDCG
		}
	}
}
