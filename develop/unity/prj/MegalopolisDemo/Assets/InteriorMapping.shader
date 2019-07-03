// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "IKD/InteriorMapping"
{
    Properties
    {
		_CubeMap( "Skybox", CUBE ) = ""{}
        _MainTex ("Texture", 2D) = "white" {}
		_RoomSep ("Number of Room", Vector ) = ( 2, 2, 2, 0 )
		_OutWallTickness ("Tickness rate for outer wall", Vector ) = ( 0.8, 0.95, 0.8, 0.0 )
		_RefractiveRatio ("Refractive ratio", float )= 0.80
		_WindowTransRate ("Translate rate of window", float ) = 0.70
		_OutWallColor ("Out wall color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )

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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 cameraPosBL : TEXCOORD2;
				float3 cameraRayBL : TEXCOORD3;
				float3 vertexBL : TEXCOORD4;
				float4 scaleAndYOfs : TEXCOORD5;
				float3 normal : TEXCOORD6;
				float3 vertexW : TEXCOORD7;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _RoomSep;
			float3 _OutWallTickness;
			UNITY_DECLARE_TEXCUBE( _CubeMap );
			float _RefractiveRatio;
			float _WindowTransRate;
			float4 _OutWallColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.normal = UnityObjectToWorldNormal( v.normal );
				o.vertexW = mul( unity_ObjectToWorld, v.vertex ).xyz;

				// ビルローカル空間での頂点位置を算出
				//  単位立方体に対し「スケール」と「Y方向のオフセット」が施された状態がビルローカル空間。
				// カメラのモデルローカル座標を算出。
				// その位置からローカル頂点へ向かうのがローカル視線レイ
				float3 xAxis = float3( unity_ObjectToWorld._11, unity_ObjectToWorld._21, unity_ObjectToWorld._31 );
				float3 yAxis = float3( unity_ObjectToWorld._12, unity_ObjectToWorld._22, unity_ObjectToWorld._32 );
				float3 zAxis = float3( unity_ObjectToWorld._13, unity_ObjectToWorld._23, unity_ObjectToWorld._33 );
				float yOffset = unity_ObjectToWorld._24;
				o.scaleAndYOfs = float4( length(xAxis), length(yAxis), length(zAxis), length( yAxis ) * 0.5 );
				float4x4 objectToBillLocalMat = {
					float4( o.scaleAndYOfs.x, 0.0, 0.0, 0.0 ),
					float4( 0.0, o.scaleAndYOfs.y, 0.0, yOffset ),
					float4( 0.0, 0.0, o.scaleAndYOfs.z, 0.0 ),
					float4( 0.0, 0.0, 0.0, 1.0 )
				};
				o.vertexBL = mul( objectToBillLocalMat, v.vertex ).xyz;
				o.cameraPosBL = float3(
					_WorldSpaceCameraPos.x - unity_ObjectToWorld._14,
					_WorldSpaceCameraPos.y,
					_WorldSpaceCameraPos.z - unity_ObjectToWorld._34
				);
				o.cameraRayBL = o.vertexBL - o.cameraPosBL;

				return o;
            }

			// 外壁？
			bool isOutWall( float3 p, float whd, int3 sepNum, float3 roomWHD, float3 center, float3 N ) {
				float3 absN = abs( N );
				if ( absN.y > 0.1f )
					return true;

				float3 wc = center + N * roomWHD * 0.5;
				float3 ref = abs( ( p - wc ) / roomWHD * 2.0 );
				if ( ref.y >= _OutWallTickness.y )
					return true;
				if ( absN.z > 0.1f && ref.x >= _OutWallTickness.x )
					return true;
				if ( absN.x > 0.1f && ref.z >= _OutWallTickness.z )
					return true;

				return false;

				// 中心位置を外壁へ投影
				float n = 1 - N;
				float v = fmod( p - center, roomWHD );
				return v > roomWHD.x * 0.5;
			}

			float calcFresnel( float3 cameraDir, float3 n ) {
				// フレネル反射率計算
				float A = _RefractiveRatio;
				float B = dot( normalize( cameraDir ), n );
				float C = sqrt( 1.0f - A * A * ( 1 - B * B ) );
				float Rs = ( A*B - C ) * ( A*B - C ) / ( ( A*B + C ) * ( A*B + C ) );
				float Rp = ( A*C - B ) * ( A*C - B ) / ( ( A*C + B ) * ( A*C + B ) );
				return ( Rs + Rp ) / 2.0f;
			}


            fixed4 frag (v2f i) : SV_Target
            {
				// レイ
				float3 dL = normalize( i.cameraRayBL );
				if ( dL.x == 0.0 ) dL.x = 0.000001;
				if ( dL.y == 0.0 ) dL.y = 0.000001;
				if ( dL.z == 0.0 ) dL.z = 0.000001;

				// レイ基点
				float3 SL = i.cameraPosBL;

				// レイの先にある最短の壁と衝突点Q0を算出
				//  P0: レイが当たる可能性のある壁に含まれる点
				//  N0: 表面の法線
				float3 whd = i.scaleAndYOfs.xyz;
				float3 yOffset = float3( 0.0, i.scaleAndYOfs.w, 0.0 );
				float3 P0 = -sign( dL ) * 0.5 * whd + yOffset;
				float3 t = ( P0 - SL ) / dL;
				float minT = t.x;
				float3 N0 = float3( 1.0 * sign( dL.x ), 0.0, 0.0 );
				float3 rgb = float3( 0.7, 0.0, 0.0 );
				if ( t.y > minT ) {
					minT = t.y;
					N0 = float3( 0.0, 1.0 * sign( dL.y ), 0.0 );
					rgb = float3( 0.0, 0.4, 0.0 );
				}
				if ( t.z > minT ) {
					minT = t.z;
					N0 = float3( 0.0, 0.0, 1.0 * sign( dL.z ) );
					rgb = float3( 0.0, 0.0, 0.6 );
				}
				float3 Q0 = SL + minT * dL;
				Q0 = clamp( Q0, -0.5 * whd + yOffset, 0.49999 * whd + yOffset );	// これしないとfloorが揺れてしまう

				// レイが表面を突き抜けて内部の壁に当たる位置Q1を算出
				int3 sepNum = _RoomSep.xyz;
				float3 roomWHD = whd / sepNum;
				float3 halfXZ = float3( whd.x * 0.5, 0.0, whd.z * 0.5 );
				float3 Q0a = Q0 + halfXZ;
				float3 roomIdx = floor( Q0a / roomWHD );
				float3 roomCenter = ( roomIdx + 0.5 ) * roomWHD - halfXZ;

				// 外壁なら壁を描画
				fixed4 col = fixed4( 0.0, 0.0, 0.0, 1.0 );
				if ( isOutWall( Q0, whd, sepNum, roomWHD, roomCenter, N0 ) == true ) {
					col = _OutWallColor;
				}
				else {
					// 室内描画
					float3 P1 = roomCenter + sign( dL ) * roomWHD * 0.5;
					float3 t1 = ( P1 - SL ) / dL;
					float minT1 = t1.x;
					rgb = float3( 0.7, 0.7, 0.7 );
					if ( t1.y < minT1 ) {
						minT1 = t1.y;
						rgb = float3( 0.4, 0.4, 0.4 );
					}
					if ( t1.z < minT1 ) {
						minT1 = t.z;
						rgb = float3( 0.6, 0.6, 0.6 );
					}
					float3 Q1 = SL + minT1 * dL;

					// sample the texture
					// fixed4 col = tex2D( _MainTex, i.uv );
					// UNITY_APPLY_FOG( i.fogCoord, col );

					// 窓描画
					// 視点からのベクトルと法線から反射方向のベクトルを計算
					// キューブマップと反射方向のベクトルから反射先の色を取得する
					i.normal = normalize( i.normal );
					half3 viewDir = normalize( _WorldSpaceCameraPos - i.vertexW );
					half3 reflDir = reflect( viewDir, i.normal );
					reflDir.y *= -1.0f;
					half3 refColor = UNITY_SAMPLE_TEXCUBE( _CubeMap, reflDir );
					float fresnelAlpha = calcFresnel( viewDir, i.normal );
					float envRate = min( fresnelAlpha, 1.0f );
					col.rgb = refColor * envRate + ( 1.0f - envRate ) * rgb;
					col.rgb *= _WindowTransRate;
//					col.xyz = rgb + refColor;
				}
				return col;
            }
            ENDCG
        }
    }
}
