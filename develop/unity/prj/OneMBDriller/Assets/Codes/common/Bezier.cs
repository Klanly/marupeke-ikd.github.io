using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ベジェ曲線

public class Bezier3D {

    public Vector3 P0 { set { p0_ = value; } get { return p0_; } }
    public Vector3 C0 { set { c0_ = value; } get { return c0_; } }
    public Vector3 C1 { set { c1_ = value; } get { return c1_; } }
    public Vector3 P1 { set { p1_ = value; } get { return p1_; } }

    // ベジェ曲線上の値を直接算出
    static public Vector3 getPos( Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1, float t )
    {
        var invT = 1.0f - t;
        return invT * ( invT * ( invT * p0 + 3.0f * t * c0 ) + 3.0f * t * t * c1 ) + t * t * t * p1;
    }

    // ベジェ曲線の距離を算出（近似値）
    static public float getDist( Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1 )
    {
        var pt0 = p0;
        float dist = 0.0f;
        for ( int i = 1; i <= sepNum_; ++i ) {
            float t = (float)i / sepNum_;
            var pt = getPos( p0, c0, c1, p1, t );
            dist += ( pt - pt0 ).magnitude;
            pt0 = pt;
        }
        return dist;
    }

    // 4点設定
    public void setPoints( Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1 )
    {
        p0_ = p0;
        c0_ = c0;
        c1_ = c1;
        p1_ = p1;
        dist_ = getDist( p0, c0, c1, p1 );

        // 24分割点のtの値に対応する距離を保持
        var pt0 = p0;
        dists_[ 0 ] = 0.0f;
        for ( int i = 1; i <= sepNum_; ++i ) {
            float t = (float)i / sepNum_;
            var pt = getPos( p0, c0, c1, p1, t );
            dists_[ i ] = ( pt - pt0 ).magnitude + dists_[ i - 1 ];
            pt0 = pt;
        }
    }

    // 距離を取得
    public float getDist()
    {
        return dist_;
    }

    // 補間点を取得
    public Vector3 getPos( float t )
    {
        return getPos( p0_, c0_, c1_, p1_, t );
    }

    // 距離補間点を取得
    // d: 距離補間値 (0～1)
    public Vector3 getPosAtD( float d )
    {
        d = Mathf.Clamp01( d );
        if ( d == 1.0f )
            return p1_;
        else if ( d == 0.0f )
            return p0_;

        float targetDist = dist_ * d;
        int idx1 = sepNum_;
        for ( int i = 1; i < sepNum_; ++i ) {
            if ( dists_[ i ] >= targetDist ) {
                idx1 = i;
                break;
            }
        }
        int idx0 = idx1 - 1;
        float t = ( targetDist - dists_[ idx0 ] ) / ( dists_[ idx1 ] - dists_[ idx0 ] );
        float t2 = ( float )( idx0 + t ) / sepNum_;
  //      Debug.LogFormat("targetDist:{0}, idx0:{1}, idx1:{2}, t:{3}, t2:{4}", targetDist, idx0, idx1, t, t2 );
        return getPos( t2 );
    }

    // p0からの距離に対応した点を取得
    // 距離が終点をオーバーしている場合メソッドはfalseを返し、overDistにオーバー距離が返る
    // distがマイナスの場合もfalseが返り、マイナス距離がoverDistに返る
    public bool getPosAtDist( float dist, out Vector3 pos, out float overDist )
    {
        if ( dist < 0.0f ) {
            overDist = dist;
            pos = p0_;
            return false;
        }

        if ( dist > dist_ ) {
            overDist = dist - dist_;
            pos = p1_;
            return false;
        }

        float d = dist / dist_;
        pos = getPosAtD( d );

 //       Debug.LogFormat( "dist:{0}, dist_:{1}, d:{2}, x:{3}, y:{4}, z{5}", dist, dist_, d, pos.x, pos.y, pos.z );

        overDist = 0.0f;
        return true;
    }

    Vector3 p0_;    // 通過点P0
    Vector3 c0_;    // 制御点C0
    Vector3 c1_;    // 制御点C1
    Vector3 p1_;    // 通過点P1
    float dist_;    // 距離（近似値）
    float[] dists_ = new float[41]; // tまでの距離
    static int sepNum_ = 40;
}
