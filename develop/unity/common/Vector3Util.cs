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
}
