Shader "Custom/WindowPlate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_CubeMap( "CubeMap", CUBE ) = "" {}
		_TexAlpha("Texture Alpha", float ) = 1.0
		_OtherXAxis("Other X Axis", VECTOR ) = ( 1.0, 0.0, 0.0, 0.0 )
		_OtherYAxis( "Other Y Axis", VECTOR ) = ( 0.0, 1.0, 0.0, 0.0 )
		_OtherZAxis( "Other Z Axis", VECTOR ) = ( 0.0, 0.0, 1.0, 0.0 )
		_OtherPos( "Other Position", VECTOR ) = ( 0.0, 0.0, 0.0, 1.0 )
		_CameraPosInOther( "Camera Position in Other Space", VECTOR ) = ( 0.0, 0.0, 0.0, 0.0 )
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

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 ray : TEXCOORD1;	// 環境マップへの視線ベクトル
				float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TexAlpha;
			float4 _OtherXAxis;
			float4 _OtherYAxis;
			float4 _OtherZAxis;
			float4 _OtherPos;
			float3 _CameraPosInOther;
			UNITY_DECLARE_TEXCUBE( _CubeMap );

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal( v.normal );

				// 相手の行列で視線ベクトルを作成
				float4x4 otherWorldMat = float4x4( _OtherXAxis, _OtherYAxis, _OtherZAxis, _OtherPos );
				o.ray = mul( v.vertex, otherWorldMat ).xyz - _CameraPosInOther;
//				o.ray = mul( unity_ObjectToWorld, v.vertex ).xyz - _WorldSpaceCameraPos;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
				float4 envColor = UNITY_SAMPLE_TEXCUBE( _CubeMap, -normalize( i.ray ) );
				float a = _TexAlpha * texColor.a;
				float4 col = float4( envColor.rgb * ( 1.0f - a ) + texColor.rgb * a, 1.0f );
                return col;
            }
            ENDCG
        }
    }
}
