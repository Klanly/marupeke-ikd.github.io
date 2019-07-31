using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vector2ユーティリティ

public class Vector2Util {
    // aがbよりもすべて小さければtrue
    static public bool andMin( Vector2 l, Vector2 r ) {
        return ( l.x < r.x && l.y < r.y );
    }

    // aがbよりもすべて大きければtrue
    static public bool andMax(Vector2 l, Vector2 r) {
        return ( l.x > r.x && l.y > r.y );
    }

    // aの成分のどれかがbのよりも小さければtrue
    static public bool orMin(Vector2 l, Vector2 r) {
        return ( l.x < r.x || l.y < r.y );
    }

    // aの成分のどれかがbのよりも大きければtrue
    static public bool orMax(Vector2 l, Vector2 r) {
        return ( l.x > r.x || l.y > r.y );
    }

    // リストの点の最小、最大範囲を算出
    static public bool calcRegion( List<Vector2> list, out Vector2 min, out Vector2 max ) {
        return calcRegion( list.ToArray(), out min, out max );
    }

    // リストの点の最小、最大範囲を算出
    static public bool calcRegion( Vector2[] ary, out Vector2 min, out Vector2 max) {
        if ( ary.Length == 0 ) {
            min = Vector2.zero;
            max = Vector2.zero;
            return false;
        }
        var curMin = new Vector2( float.MaxValue, float.MaxValue );
        var curMax = new Vector2( float.MinValue, float.MinValue );
        foreach ( var v in ary ) {
            curMin = Vector2.Min( curMin, v );
            curMax = Vector2.Max( curMax, v );
        }
        min = curMin;
        max = curMax;
        return true;
    }

    // 成分同士の掛け算
    static public Vector2 mul(Vector2 l, Vector2 r) {
        return new Vector2( l.x * r.x, l.y * r.y );
    }
}
