using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 乱数ヘルパー
public class Randoms {

    public class Vec3
    {
        // 各要素0～1の乱数
        static public Vector3 value()
        {
            return new Vector3( Random.value, Random.value, Random.value );
        }

        // XZ要素0～1の乱数
        static public Vector3 valueXZ()
        {
            return new Vector3( Random.value, 0.0f, Random.value );
        }

        // 各要素-1.0f～1.0fの乱数
        static public Vector3 valueCenter()
        {
            return new Vector3( 2.0f * ( Random.value - 0.5f ), 2.0f * ( Random.value - 0.5f ), 2.0f * ( Random.value - 0.5f ) );
        }

        // XZ要素-1.0f～1.0fの乱数
        static public Vector3 valueCenterXZ()
        {
            return new Vector3( 2.0f * ( Random.value - 0.5f ), 0.0f, 2.0f * ( Random.value - 0.5f ) );
        }
    }

    public class Float {
        // 指数分布待ち時間
        //  aveTime: 平均待ち時間(sec)
        static public float expWait( float aveTime, float maxTime ) {
            float r = Mathf.Clamp( Random.value, 0.0001f, 0.9999f );
            float t = -aveTime * Mathf.Log( 1.0f - r );
            return ( t < maxTime ? t : maxTime );
        }
    }
}
