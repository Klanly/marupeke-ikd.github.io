using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 交換系ヘルパー
public class Swaps
{
    // 単純交換
    static public void swap<T>( ref T a, ref T b ) {
        T tmp = a;
        a = b;
        b = a;
    }

    // 成分の最小、最大を揃える
	static public void minMax( ref float min, ref float max )
	{
		if ( min > max ) {
			float tmp = min;
			min = max;
			max = tmp;
		}
	}
    static public void minMax(ref Vector2Int min, ref Vector2Int max) {
        int minX = min.x;
        int minY = min.y;
        int maxX = max.x;
        int maxY = max.y;
        if ( min.x > max.x )
            swap( ref minX, ref maxX );
        if ( min.y > max.y )
            swap( ref minY, ref maxY );
        min.x = minX;
        min.y = minY;
        max.x = maxX;
        max.y = maxY;
    }
    static public void minMax( ref Vector2 min, ref Vector2 max ) {
        if ( min.x > max.x )
            swap( ref min.x, ref max.x );
        if ( min.y > max.y )
            swap( ref min.y, ref max.y );
    }
    static public void minMax(ref Vector3 min, ref Vector3 max) {
        if ( min.x > max.x )
            swap( ref min.x, ref max.x );
        if ( min.y > max.y )
            swap( ref min.y, ref max.y );
        if ( min.z > max.z )
            swap( ref min.z, ref max.z );
    }
}
