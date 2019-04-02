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

		_RefractiveRatio( "Refractive Ratio", Range( 0.0, 1.0) ) = 0.8

		_UVResolutionUnit( "UV Resolution Unit", float ) = 1024
		_color( "Main Color", Color ) = ( 1, 1, 1, 1 )
		_DeepColor( "Deep Color", Color ) = ( 0, 0, 0, 0 )
		_ColorTransRate( "Color Transparent Rate", Range( 0.0, 1.0 ) ) = 0.7
		_DiffuseRange( "Diffuse Range", Range( 0.0, 1.0) ) = 0.0
		_specularPower( "Specular Power", Range( 0.0, 50.0 ) ) = 10.0
		_fieldScale( "Field Scale(m)", float ) = 1.0
		_t( "Time(sec.)", float ) = 0.0
		_ampRate( "Amplitude Rate(0-1)", Range( 0.0, 1.0 ) ) = 0.5
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
			float _RefractiveRatio;
			float _UVResolutionUnit;
			float4 _color;
			float4 _DeepColor;
			float _ColorTransRate;
			float _DiffuseRange;
			float _specularPower;
			float _fieldScale;
			float _t;
			float _ampRate;

			void addSinWave( inout appdata v, inout v2f o, float waveLen, float2 browDir ) {
				float grav = 9.8f;
				float _2pi_per_L = 2.0f * 3.14159265f / waveLen;
				float d = dot( browDir, v.vertex.xz * float2( _fieldScale, _fieldScale ) );
				float A = _ampRate * waveLen / 14.0f / _fieldScale;
				float C = _2pi_per_L;
				float D = sqrt( _2pi_per_L * grav ) * _t;
				float S = C * d - D;
				float H = A * sin( S );

				// 波の高さを加算
				v.vertex.y += H;

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

				// ローカル座標のY値＝波の高さを(x,z)から計算
				float _waveLen0 = 1.0f;
				float2 _browDir0 = normalize( float2( 1.0f, 0.7f ) );
				float _waveLen1 = 2.0f;
				float2 _browDir1 = normalize( float2( -0.5f, 0.4f ) );
				float _waveLen2 = 4.0f;
				float2 _browDir2 = normalize( float2( -1.54f, -0.72f ) );
				float _waveLen3 = 8.0f;
				float2 _browDir3 = normalize( float2( 0.93f, 1.60f ) );
				float _waveLen4 = 1.0f;
				float2 _browDir4 = normalize( float2( 2.4f, -0.9f ) );
				float _waveLen5 = 2.0f;
				float2 _browDir5 = normalize( float2( 0.2f, 0.8f ) );
				float _waveLen6 = 4.0f;
				float2 _browDir6 = normalize( float2( 1.1f, -0.72f ) );
				float _waveLen7 = 8.0f;
				float2 _browDir7 = normalize( float2( -0.23f, 0.40f ) );

				addSinWave( v, o, _waveLen0, _browDir0 );
				addSinWave( v, o, _waveLen1, _browDir1 );
				addSinWave( v, o, _waveLen2, _browDir2 );
				addSinWave( v, o, _waveLen3, _browDir3 );
				addSinWave( v, o, _waveLen3, _browDir4 );
				addSinWave( v, o, _waveLen3, _browDir5 );
				addSinWave( v, o, _waveLen3, _browDir6 );
				addSinWave( v, o, _waveLen3, _browDir7 );

				o.normal = normalize( o.normal );
				o.tangent = normalize( cross( o.normal, o.binormal ) );
				o.binormal = normalize( cross( o.tangent, o.normal ) );	// 直行性確保

				o.vertex = UnityObjectToClipPos(v.vertex);
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
				float3 diffuse = diffusePower * _LightColor0.rgb * col.xyz * ( _color.rgb * diffuseColorPower + _DeepColor.rgb * ( 1.0f - diffuseColorPower ) );

				// Specular
				float2 lightRefVec = reflect( -lightDir, n );
				float specularBase = max( 0.0f, dot( lightRefVec, cameraDir ) );
				float specular = pow( specularBase, _specularPower ) * float3( 1.0f, 1.0f, 1.0f );

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
