using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gizmoユーティリティ

public class GizmosUtil
{
	static public void drawCircle( Vector3 center, float radius, Color? color, Quaternion? q, Vector3? scale )
	{
		if ( initCircle_g == false ) {
			// 単位円作成
			for (int i = 0; i < 24; ++i) {
				circle_g[ i ].x = Mathf.Cos( 2.0f * Mathf.PI * (float)i / 24 );
				circle_g[ i ].z = Mathf.Sin( 2.0f * Mathf.PI * (float)i / 24 );
				circle_g[ i ].y = 0.0f;
			}
		}
		Color c = color ?? Color.white;
		Quaternion lq = q ?? Quaternion.identity;
		Vector3 sc = scale ?? Vector3.one;

		Gizmos.color = c;
		Vector3 s = lq * ( radius * Vector3Util.mul( circle_g[ 0 ], sc ) ) + center;
		for ( int i = 1; i <= 24; ++i ) {
			Vector3 e = lq * ( radius * Vector3Util.mul( circle_g[ i % 24 ], sc ) ) + center;
			Gizmos.DrawLine( s, e );
			s = e;
		}
	}

	// OBB描画
	static public void drawOBB2D( OBB2D obb, Color? color ) {
		if (obb == null)
			return;
		Color c = color ?? Color.white;
		Gizmos.color = c;
		tmpV3_0_.y = 0.0f;
		tmpV3_1_.y = 0.0f;
		var vs = obb.getVertices();
		for ( int i = 0; i < 4; ++i ) {
			tmpV3_0_.x = vs[ i ].x;
			tmpV3_0_.z = vs[ i ].y;
			tmpV3_1_.x = vs[ ( i + 1 ) % 4 ].x;
			tmpV3_1_.z = vs[ ( i + 1 ) % 4 ].y;
			Gizmos.DrawLine( tmpV3_0_, tmpV3_1_ );
		}
	}

	static Vector3[] circle_g = new Vector3[24];
	static bool initCircle_g = false;
	static Vector3 tmpV3_0_ = new Vector3();
	static Vector3 tmpV3_1_ = new Vector3();
}
