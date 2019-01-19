using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シンプルなパス曲線
//  程良い感じのベジェ曲線を描きます
//  先に点を登録した後にsetupメソッドを呼ぶと内部で曲線を再計算します

public class SimplePath {
    public int addPos( Vector3 pos )
    {
        poses_.Add( pos );
        return poses_.Count;
    }

    public int addPoses( List<Vector3> poses )
    {
        poses_.AddRange( poses );
        return poses_.Count;
    }

    public int addPoses( Vector3[] poses )
    {
        poses_.AddRange( poses );
        return poses_.Count;
    }

    public float getDist()
    {
        return dist_;
    }

    // 点をすべて登録した後に曲線を再計算します
    public void setup()
    {
        if ( poses_.Count == 0 ) {
            // 点が定義出来ないけど認める
            dist_ = 0.0f;
            return;
        }

        if ( poses_.Count == 1 ) {
            // 点だけどOK
            dist_ = 0.0f;
            return;
        }
        if ( poses_.Count == 2 ) {
            // 直線だけどOK
            dist_ = ( poses_[ 1 ] - poses_[ 0 ] ).magnitude;
            return;
        }

        // 最初の2点間はsを計算
        Vector3 p0 = poses_[ 0 ];
        Vector3 p1 = poses_[ 1 ];
        Vector3 p2 = poses_[ 2 ];
        Vector3 p02 = p2 - p0;
        Vector3 p01 = p1 - p0;
        Vector3 en = -p02.normalized;
        float dot = Vector3.Dot( en, p01.normalized );
        float eLen = 0.25f;
        if ( dot != 0.0f ) {
            eLen = p01.magnitude / dot * -0.25f;
        }
        Vector3 e = en * eLen;
        Vector3 s = e + 0.5f * p01;
        var bezier = new Bezier3D();
        bezier.setPoints( p0, p0 + s, p1 + e, p1 );
        beziers_.Add( bezier );

        // 次からはsを前のeから算出
        // 制御点までの長さは点間の半分
        for ( int i = 1; i < poses_.Count - 2; ++i ) {
            p0 = poses_[ i ];
            p1 = poses_[ i + 1 ];
            p2 = poses_[ i + 2 ];
            float len01 = ( p1 - p0 ).magnitude * 0.5f;
            s = -e.normalized * len01;
            e = ( p0 - p2 ).normalized * len01;
            bezier = new Bezier3D();
            bezier.setPoints( p0, p0 + s, p1 + e, p1 );
            beziers_.Add( bezier );
        }

        // 最後の2点間は特別
        p0 = poses_[ poses_.Count - 2 ];
        p1 = poses_[ poses_.Count - 1 ];
        p01 = p1 - p0;
        dot = Vector3.Dot( -en, p01.normalized );
        eLen = 0.25f;
        if ( dot != 0.0f ) {
            eLen = p01.magnitude / dot * 0.25f;
        }
        s = -en * eLen;
        e = s - 0.5f * p01;
        bezier = new Bezier3D();
        bezier.setPoints( p0, p0 + s, p1 + e, p1 );
        beziers_.Add( bezier );

        foreach ( var bz in beziers_ ) {
            dist_ += bz.getDist();
        }
    }

    // 追加距離に対応する点を取得
    public bool getPos( float addDist, out Vector3 pos, out float overDist )
    {
        if ( curIdx_ >= beziers_.Count ) {
            // オーバーにより即終了
            pos = poses_[ poses_.Count - 1 ];
            overDist = addDist;
            return false;
        }

        curDefDist_ += addDist;
        if ( beziers_[ curIdx_ ].getPosAtDist( curDefDist_, out pos, out overDist ) == true ) {
            // 今のBezierの範囲内だった
            return true;
        }

        // 今のBezierの範囲外なので次のBezierへ
        curIdx_++;
        curDefDist_ = 0.0f;
        return getPos( overDist, out pos, out overDist );
    }

    List<Vector3> poses_ = new List<Vector3>();
    List<Bezier3D> beziers_ = new List<Bezier3D>();
    float dist_ = 0.0f;
    float curDefDist_ = 0.0f;
    int curIdx_ = 0;
}
