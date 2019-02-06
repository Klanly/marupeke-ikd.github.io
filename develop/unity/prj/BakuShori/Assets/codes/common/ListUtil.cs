using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リストのユーティリティ

public class ListUtil {
    // シャッフル
    public static void shuffle<T>( ref List<T> list, int seed = -1 )
    {
        if ( seed >= 0 )
            Random.InitState( seed );

        int n = list.Count;
        for ( int i = 0; i < n; ++i ) {
            int r = i + ( int )( Random.value * ( n - i ) );
            var tmp = list[ r ];
            list[ r ] = list[ i ];
            list[ i ] = tmp;
        }
    }
}
