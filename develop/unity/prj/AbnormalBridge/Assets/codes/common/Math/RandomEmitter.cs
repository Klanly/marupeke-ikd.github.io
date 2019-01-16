using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランダムエミッター
//
//  指定の時間内で物事が発生する回数等を算出

public class RandomEmitter {
    // 指数分布待ち行列による次に遭遇する時間を取得
    //  rambda : 単位時間当たり遭遇数
    //  maxTime: 返す最大時間
    //  戻り値 : 次に遭遇する時間（rambdaの時間単位）
    static public float exponentialNextEncountTime( float rambda, float maxTime = float.MaxValue )
    {
        if ( rambda <= 0.0f )
            return float.MaxValue;

        float r = Random.value;
        if ( r == 1.0f )
            return maxTime;
        float nextTime = -Mathf.Log( 1.0f - r ) / rambda;
        return ( nextTime < maxTime ? nextTime : maxTime );
    }
}
