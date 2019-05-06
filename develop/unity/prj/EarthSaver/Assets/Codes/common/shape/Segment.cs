using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 線分
public class Segment {
    public Vector3 Start { set { s_ = value; } get { return s_; } }
    public Vector3 End { set { e_ = value; } get { return e_; } }
    public Vector3 Center { get { return ( s_ + e_ ) * 0.5f; } }
    public Vector3 Ray { get { return e_ - s_; } }
    public float Len { get { return ( e_ - s_ ).magnitude; } }
    public float LenSq { get { return ( e_ - s_ ).sqrMagnitude; } }

    public Segment() {
    }

    public Segment( Vector3 start, Vector3 end ) {
        s_ = start;
        e_ = end;
    }

    // 縮退している？
    public bool isDegeneracy() {
        return Len < 0.00001f;
    }

    // 点との最近距離算出
    //  reference:「ゲームプログラミングの為のリアルタイム衝突判定」128p
    public float calcDistPoint( Vector3 p, out float inter, out Vector3 close ) {
        Vector3 ab = Ray;
        inter = Vector3.Dot( p - Start, ab ) / LenSq;
        if ( inter < 0.0f )
            inter = 0.0f;
        else if ( inter > 1.0f )
            inter = 1.0f;
        close = Start + inter * ab;
        return ( close - p ).magnitude;
    }

    // 線分同士の再近距離算出
    //  reference:「ゲームプログラミングの為のリアルタイム衝突判定」149p
    public float calcDistSegment( Segment s2, out float inter1, out float inter2, out Vector3 close1, out Vector3 close2 ) {
        Vector3 d1 = Ray;
        Vector3 d2 = s2.Ray;
        Vector3 r = Start - s2.Start;
        float a = LenSq;
        float e = s2.LenSq;
        float f = Vector3.Dot( d2, r );

        const float EPSILON = 0.00001f;
        if ( a <= EPSILON && e <= EPSILON ) {
            inter1 = inter2 = 0.0f;
            close1 = Start;
            close2 = s2.Start;
            return ( close1 - close2 ).magnitude;
        }

        if ( a <= EPSILON ) {
            inter1 = 0.0f;
            inter2 = Mathf.Clamp01( f / e );
        } else {
            float c = Vector3.Dot( d1, r );
            if ( e <= EPSILON ) {
                inter2 = 0.0f;
                inter1 = Mathf.Clamp01( -c / a );
            } else {
                float b = Vector3.Dot( d1, d2 );
                float denom = a * e - b * b;
                if ( denom != 0.0f ) {
                    inter1 = Mathf.Clamp01( ( b * f - c * e ) / denom );
                } else {
                    inter1 = 0.0f;
                }
                inter2 = ( b * inter1 + f ) / e;
                if ( inter2 < 0.0f ) {
                    inter2 = 0.0f;
                    inter1 = Mathf.Clamp01( -c / a );
                } else if ( inter2 > 1.0f ) {
                    inter2 = 1.0f;
                    inter1 = Mathf.Clamp01( ( b - c ) / a );
                }
            }
        }

        close1 = Start + d1 * inter1;
        close2 = s2.Start + d2 * inter2;
        return ( close1 - close2 ).magnitude;
    }

    Vector3 s_;
    Vector3 e_;
}
