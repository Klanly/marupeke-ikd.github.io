Shader "Custom/Fur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FurDotTex( "Fur dot texture", 2D ) = "black" {}
		_FurStep ("Fur step index (no touch)", float) = 0.0
		_ShellNum ("Total shell num (no touch)", float) = 1.0
		_FurLength ("Fur length", float) = 0.0
		_FurScale ("Fur scale", float) = 1.0
		_FurDrawThreshold ("Fur draw threshold (0 - 1)", float) = 0.0
		_FurColorDarkness ("Fur color darkness (0 - 1)", float) = 1.0
		_FurTension ("Fur tension (0 - inf)", float) = 1.0
		_MaxFurRadian("Max fur radian( 0 - pi/2 )", float) = 0.7
		_Blow ("Blow direction", Vector) = ( 0.0, 0.0, 0.0, 0.0 )
		_BlowPower( "Blow power", float) = 1.0
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// 入力セマンティクス
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 n : NORMAL;
			};

			// ピクセルシェーダ入力セマンティクス
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 n : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _FurDotTex;
			float4 _MainTex_ST;
			float _FurStep;
			float _ShellNum;
			float _FurLength;
			float _FurScale;
			float _FurDrawThreshold;
			float _FurColorDarkness;
			float _FurTension;
			float _MaxFurRadian;
			float3 _Blow;
			float _BlowPower;

			v2f vert (appdata v)
			{
				v2f o;

				float3 n_n = normalize( v.n );
				float3 p = v.vertex.xyz + ( n_n * _FurLength );
				float3 blow_n = normalize( _Blow );

				if ( length( blow_n ) >= 0.00001 ) {
					// Blow効力算出
					float blowEffect = ( 1.0 - dot( n_n, blow_n ) ) * 0.5;

					// 回転角度算出
					float th = _MaxFurRadian * pow( blowEffect, _FurTension ) * ( _FurStep / _ShellNum ) * _BlowPower;
					th = clamp( th, -1.57, 1.57 );

					// Blowの逆方向に頂点を回転
					float3 x = cross( blow_n, v.n );
					float3 z = cross( x, v.n );
					float3 move_dir = normalize( n_n * cos( th ) - normalize( z ) * sin( th ) );

					// ローカル座標の回転を考慮した方向に移動（＝拡大）
					p = v.vertex.xyz + ( move_dir * _FurLength );
				}

				// スクリーンへ
				o.vertex = UnityObjectToClipPos( float4(p, 1.0));	// MVP掛けるのと一緒
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.n = UnityObjectToViewPos( float4(v.n, 0.0) ); // MV掛けるのと一緒

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 furDotColor = tex2D( _FurDotTex, i.uv * _FurScale );
				if ( furDotColor.r <= _FurDrawThreshold )
					discard;
				fixed4 col = tex2D( _MainTex, i.uv );
				col.rgb *= _FurColorDarkness;
				return col;
			}
			ENDCG
		}
	}
}
