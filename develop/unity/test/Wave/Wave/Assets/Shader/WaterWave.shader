// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WaterWave"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EnvTex( "Environmental Texture", Cube ) = "white" {}
		_EnvRate( "Environmental Color Rate", Range( 0.0, 1.0 ) ) = 0.3

		_HeightTex0( "Height Texture0", 2D ) = "black" {}
		_HeightTexScale0( "Height Texture Scale0", float ) = 1.0
		_BumpPower0( "Bumping Power0", Range( 0.0, 100.0 ) ) = 100.0

		_HeightTex1( "Height Texture1", 2D ) = "black" {}
		_HeightTexScale1( "Height Texture Scale1", float ) = 1.0
		_BumpPower1( "Bumping Power1", Range( 0.0, 100.0 ) ) = 100.0

		_HeightTex2( "Height Texture2", 2D ) = "black" {}
		_HeightTexScale2( "Height Texture Scale2", float ) = 1.0
		_BumpPower2( "Bumping Power2", Range( 0.0, 100.0 ) ) = 100.0

		_VertexWaveLength0_3( "Vertex Wave Length No.0-3 (m)", Vector ) = ( 1.0, 1.0, 1.0, 2.0 )
		_VertexWaveLength4_7( "Vertex Wave Length No.4-7 (m)", Vector ) = ( 2.0, 4.0, 4.0, 8.0 )
		_VertexWaveDir0_1( "Vertex Wave Length No.0-1", Vector ) = ( 0.2, 0.4, -0.5, 0.3 )
		_VertexWaveDir2_3( "Vertex Wave Length No.2-3", Vector ) = ( -0.3, 0.7, 0.8, -0.2 )
		_VertexWaveDir4_5( "Vertex Wave Length No.4-5", Vector ) = ( 0.1, -0.6, 0.5, 0.1 )
		_VertexWaveDir6_7( "Vertex Wave Length No.6-7", Vector ) = ( 0.4, -0.8, 0.7, -0.2 )

		_RefractiveRatio( "Refractive Ratio", Range( 0.0, 1.0) ) = 0.8

		_UVResolutionUnit( "UV Resolution Unit", float ) = 1024
		_NearColor( "Main Color", Color ) = ( 1, 1, 1, 1 )
		_DeepColor( "Deep Color", Color ) = ( 0, 0, 0, 0 )
		_ColorTransRate( "Color Transparent Rate", Range( 0.0, 1.0 ) ) = 0.7
		_DiffuseRange( "Diffuse Range", Range( 0.0, 1.0) ) = 0.0
		_SpecularPower( "Specular Power", Range( 0.0, 50.0 ) ) = 10.0
		_FieldScale( "Field Scale(m/unit)", float ) = 1.0
		_t( "Time(sec.)", float ) = 0.0
		_AmpRate( "Amplitude Rate(0-1)", Range( 0.0, 1.0 ) ) = 0.5
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

			uniform float4 _LightColor0;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD2;
				float3 tangent : TEXCOORD3;
				float3 binormal : TEXCOORD4;
				float4 posW : TEXCOORD5;
			};

			sampler2D _MainTex;
			UNITY_DECLARE_TEXCUBE( _EnvTex );
			float _EnvRate;
			float4 _MainTex_ST;
			sampler2D _HeightTex0;
			float4 _HeightTex0_ST;
			float _HeightTexScale0;
			float _BumpPower0;
			sampler2D _HeightTex1;
			float4 _HeightTex1_ST;
			float _HeightTexScale1;
			float _BumpPower1;
			sampler2D _HeightTex2;
			float4 _HeightTex2_ST;
			float _HeightTexScale2;
			float _BumpPower2;

			float4 _VertexWaveLength0_3;
			float4 _VertexWaveLength4_7;
			float4 _VertexWaveDir0_1;
			float4 _VertexWaveDir2_3;
			float4 _VertexWaveDir4_5;
			float4 _VertexWaveDir6_7;

			float _VertexWaveLengthMin;
			float _VertexWaveLengthMax;
			float _RefractiveRatio;
			float _UVResolutionUnit;
			float4 _NearColor;
			float4 _DeepColor;
			float _ColorTransRate;
			float _DiffuseRange;
			float _SpecularPower;
			float _FieldScale;
			float _t;
			float _AmpRate;

			void addSinWave( inout float4 v, inout v2f o, float waveLen, float2 browDir ) {
				browDir = normalize( browDir );
				waveLen /= _FieldScale;
				float grav = 9.8f / _FieldScale;
				float _2pi_per_L = 2.0f * 3.14159265f / waveLen;
				float d = dot( browDir, v.xz );
				float A = _AmpRate * waveLen / 14.0f;
				float C = _2pi_per_L;
				float D = sqrt( _2pi_per_L * grav ) * _t;
				float S = C * d - D;
				float H = A * sin( S );

				// 波の高さを加算
				v.y += H;

				// 法線算出
				o.normal += UnityObjectToWorldNormal( float3( -browDir.x * A * cos( S ), 1.0f, -browDir.y * A * cos( S ) ) );

				// 接ベクトル(tangent:X軸方向の傾き）と順法線(binormal:Z軸方向の傾き)、法線を算出
				float3 t = float3( 1.0f, browDir.x * A * cos( S ), 0.0f );
				float3 b = float3( 0.0f, browDir.y * A * cos( S ), 1.0f );
				float3 n = cross( b, t );
				b = cross( t, n );	// 直行性確保
				o.tangent += t;
				o.binormal += b;
				o.normal += n;
			}

			v2f vert (appdata v)
			{
				v2f o;

				v.vertex.y = 0.0f;
				o.vertex = 0.0f;
				o.uv = 0.0f;
				o.normal = float3( 0.0f, 0.0f, 0.0f );
				o.tangent = float3( 0.0f, 0.0f, 0.0f );
				o.binormal = float3( 0.0f, 0.0f, 0.0f );
				o.posW = 0.0f;

				// 頂点波発生
				// _VertexWaveLengthMin～_VertexWaveLengthMaxの波長を持った
				// 波を8本発生
				float _waveLen0 = _VertexWaveLength0_3.x;
				float2 _browDir0 = normalize( _VertexWaveDir0_1.xy );
				float _waveLen1 = _VertexWaveLength0_3.y;
				float2 _browDir1 = normalize( _VertexWaveDir0_1.zw );
				float _waveLen2 = _VertexWaveLength0_3.z;
				float2 _browDir2 = normalize( _VertexWaveDir2_3.xy );
				float _waveLen3 = _VertexWaveLength0_3.w;
				float2 _browDir3 = normalize( _VertexWaveDir2_3.zw );
				float _waveLen4 = _VertexWaveLength4_7.x;
				float2 _browDir4 = normalize( _VertexWaveDir4_5.xy );
				float _waveLen5 = _VertexWaveLength4_7.y;
				float2 _browDir5 = normalize( _VertexWaveDir4_5.zw );
				float _waveLen6 = _VertexWaveLength4_7.z;
				float2 _browDir6 = normalize( _VertexWaveDir6_7.xy );
				float _waveLen7 = _VertexWaveLength4_7.w;
				float2 _browDir7 = normalize( _VertexWaveDir6_7.zw );

				float4 worldPos = mul( unity_ObjectToWorld, v.vertex );

				addSinWave( worldPos, o, _waveLen0, _browDir0 );
				addSinWave( worldPos, o, _waveLen1, _browDir1 );
				addSinWave( worldPos, o, _waveLen2, _browDir2 );
				addSinWave( worldPos, o, _waveLen3, _browDir3 );
				addSinWave( worldPos, o, _waveLen4, _browDir4 );
				addSinWave( worldPos, o, _waveLen5, _browDir5 );
				addSinWave( worldPos, o, _waveLen6, _browDir6 );
				addSinWave( worldPos, o, _waveLen7, _browDir7 );

				o.normal = normalize( o.normal );
				o.tangent = normalize( cross( o.normal, o.binormal ) );
				o.binormal = normalize( cross( o.tangent, o.normal ) );	// 直行性確保

				o.vertex = mul( UNITY_MATRIX_VP, worldPos );
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				o.posW = mul( unity_ObjectToWorld, v.vertex );
				return o;
			}

			float3 calcObjNormalToBump( float2 uv, float texScale, sampler2D heightTex, float bumpPower ) {
				float2 suv = uv * texScale;
				float2 uReso = float2( _UVResolutionUnit, 0.0f );
				float2 vReso = float2( 0.0f, _UVResolutionUnit );
				float4 dx = ( tex2D( heightTex, suv + uReso ) - tex2D( heightTex, suv - uReso ) ) * 0.5f;
				float4 dz = ( tex2D( heightTex, suv + vReso ) - tex2D( heightTex, suv - vReso ) ) * 0.5f;
				return normalize( float3( -dx.x * bumpPower, 1.0f, -dz.x * bumpPower ) );
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
				_UVResolutionUnit = 1.0f / _UVResolutionUnit;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 n0 = calcObjNormalToBump( i.uv + _HeightTex0_ST.zw, _HeightTexScale0, _HeightTex0, _BumpPower0 );
				float3 n1 = calcObjNormalToBump( i.uv + _HeightTex1_ST.zw, _HeightTexScale1, _HeightTex1, _BumpPower1 );
				float3 n2 = calcObjNormalToBump( i.uv + _HeightTex2_ST.zw, _HeightTexScale2, _HeightTex2, _BumpPower2 );
				float3 n = normalize(
					( n0.x + n1.x + n2.x ) * i.tangent +
					( n0.y + n1.y + n2.y ) * i.normal +
					( n0.z + n1.z + n2.z ) * i.binormal
				);

				float3 cameraDir = normalize( _WorldSpaceCameraPos - i.posW.xyz );
				float3 lightDir = -normalize( _WorldSpaceLightPos0.xyz );
				float fresnelAlpha = calcFresnel( cameraDir, n );

				// Diffuse
				float diffuseColorPower = pow( fresnelAlpha, _ColorTransRate );
				float diffusePower = max( _DiffuseRange, dot( n, lightDir ) );
				float3 diffuse = diffusePower * _LightColor0.rgb * col.xyz * ( _NearColor.rgb * diffuseColorPower + _DeepColor.rgb * ( 1.0f - diffuseColorPower ) );

				// Specular
				float2 lightRefVec = reflect( -lightDir, n );
				float specularBase = max( 0.0f, dot( lightRefVec, cameraDir ) );
				float specular = pow( specularBase, _SpecularPower ) * float3( 1.0f, 1.0f, 1.0f );

				// Env
				float3 env = UNITY_SAMPLE_TEXCUBE( _EnvTex, reflect( -cameraDir, n ) );
				float envRate = min( fresnelAlpha + _EnvRate, 1.0f );
				col.rgb = env * envRate + ( 1.0f - envRate ) * diffuse + specular;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col );
				return col;

				col.rgb = specular;
				return col;
			}
			ENDCG
		}
	}
}
