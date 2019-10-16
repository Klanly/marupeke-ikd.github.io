using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vector3ユーティリティ

public class Vector3Util {
    // aがbよりもすべて小さければtrue
    static public bool andMin( Vector3 l, Vector3 r ) {
        return ( l.x < r.x && l.y < r.y && l.z < r.z );
    }

    // aがbよりもすべて大きければtrue
    static public bool andMax(Vector3 l, Vector3 r) {
        return ( l.x > r.x && l.y > r.y && l.z > r.z );
    }

    // aの成分のどれかがbのよりも小さければtrue
    static public bool orMin(Vector3 l, Vector3 r) {
        return ( l.x < r.x || l.y < r.y || l.z < r.z );
    }

    // aの成分のどれかがbのよりも大きければtrue
    static public bool orMax(Vector3 l, Vector3 r) {
        return ( l.x > r.x || l.y > r.y || l.z > r.z );
    }

    // リストの点の最小、最大範囲を算出
    static public bool calcRegion( List<Vector3> list, out Vector3 min, out Vector3 max ) {
        return calcRegion( list.ToArray(), out min, out max );
    }

    // リストの点の最小、最大範囲を算出
    static public bool calcRegion( Vector3[] ary, out Vector3 min, out Vector3 max) {
        if ( ary.Length == 0 ) {
            min = Vector3.zero;
            max = Vector3.zero;
            return false;
        }
        var curMin = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
        var curMax = new Vector3( float.MinValue, float.MinValue, float.MinValue );
        foreach ( var v in ary ) {
            curMin = Vector3.Min( curMin, v );
            curMax = Vector3.Max( curMax, v );
        }
        min = curMin;
        max = curMax;
        return true;
    }

    // 成分同士の掛け算
    static public Vector3 mul( Vector3 l ,Vector3 r ) {
        return new Vector3( l.x * r.x, l.y * r.y, l.z * r.z );
    }

	// XZ成分でVector2化
	static public Vector2 toVector2XZ( Vector3 v3 )
	{
		tmpV2_.x = v3.x;
		tmpV2_.y = v3.z;
		return tmpV2_;
	}

	// 反射ベクトル算出
	static public Vector3 reflect( Vector3 dir, Vector3 normal, bool useDoubleSide = true ) {
		var d = dir.normalized;
		var n = normal.normalized;
		float dot = Vector3.Dot( d, n );
		if ( dot > 0.0f ) {
			if ( useDoubleSide == false ) {
				return dir;
			}
			n *= -1.0f;
		} else {
			dot *= -1.0f;
		}
		return ( 2.0f * dot * n + d ).normalized;
	}

	static Vector2 tmpV2_ = new Vector2();
}
