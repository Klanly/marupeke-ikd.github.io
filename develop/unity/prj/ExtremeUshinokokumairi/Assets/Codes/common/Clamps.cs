using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クランプヘルパー
public class Clamps
{
    // Int
    public class Int {
        public static int clamp( int r, int min, int max ) {
            return ( r < min ? min : ( r > max ? max : r ) );
        }
    }

    // Vec2Int
    public class Vec2Int {
        public static Vector2Int clamp( Vector2Int r, Vector2Int min, Vector2Int max ) {
            Swaps.minMax( ref min, ref max );
            return new Vector2Int( Int.clamp( r.x, min.x, max.x ), Int.clamp( r.y, min.y, max.y ) );
        }
    }
}
