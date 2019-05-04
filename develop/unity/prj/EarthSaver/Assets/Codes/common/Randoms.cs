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

        // あるNに対して角度θだけ開きのあるランダムな方向
        static public Vector3 angleVariance( Vector3 N, float deg ) {
            // Nがゼロベクトルの場合は任意方向を
            if ( N.magnitude == 0.0f ) {
                return value();
            }
            var Z = N.normalized;

            // NをZ軸とした時のX軸、Y軸算出
            var R = value();
            while( Vector3.Dot( Z, R ) == 0.0f ) {  // 同じ方向はエラーになるので採用できない
                R = value();
            }
            var X = ( R - Vector3.Dot( Z, R ) * Z ).normalized;
            var Y = Vector3.Cross( X, Z ).normalized;

            // 経度をランダムに決定
            var th = Float.value() * Mathf.PI * 2.0f;
            var longi = X * Mathf.Cos( th ) + Y * Mathf.Sin( th );

            // deg - 90度な緯度になるベクトル算出
            th = ( deg - 90.0f ) * Mathf.Deg2Rad;

            return longi * Mathf.Cos( th ) + Z * Mathf.Sin( th );
        }
    }

    public class Float {
        // 0～1乱数
        static public float value() {
            return Random.value;
        }
        // -1～1乱数
        static public float valueCenter() {
            return ( Random.value - 0.5f ) * 2.0f;
        }

        // 指数分布待ち時間
        //  aveTime: 平均待ち時間(sec)
        static public float expWait( float aveTime, float maxTime ) {
            float r = Mathf.Clamp( Random.value, 0.0001f, 0.9999f );
            float t = -aveTime * Mathf.Log( 1.0f - r );
            return ( t < maxTime ? t : maxTime );
        }
    }
}
