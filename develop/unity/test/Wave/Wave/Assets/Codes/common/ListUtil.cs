using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リストのユーティリティ

public class ListUtil {
    // シャッフル
    public static List<T> shuffle<T>( ref List<T> list, int seed = -1 )
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
		return list;
    }

	// 数値振り
	//  start: 最初の値
	public static List<int> numbering( ref List<int> list, int num = 1, int start = 0 ) {
		list.Clear();
		for ( int i = start; i < start + num; ++i ) {
			list.Add( i );
		}
		return list;
	}
}
