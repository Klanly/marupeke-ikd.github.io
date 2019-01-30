using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 色々な範囲を算出
public class Ranges {

    // Vector3群を囲むAABBを算出
    public static void aabb3( List< Vector3 > poses, out Vector3 min, out Vector3 max )
    {
        if ( poses.Count == 0 ) {
            min = Vector3.zero;
            max = Vector3.zero;
            return;
        }
        var outMin = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
        var outMax = new Vector3( float.MinValue, float.MinValue, float.MinValue );
        foreach ( var p in poses ) {
            outMin = Vector3.Min( p, outMin );
            outMax = Vector3.Max( p, outMax );
        }
        min = outMin;
        max = outMax;
    }
}
