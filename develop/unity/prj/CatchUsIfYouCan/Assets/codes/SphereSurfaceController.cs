using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球表面コントローラ
// 指定の半径の球面上を動く

// 極座標系を用いる

public class SphereSurfaceController {

    // 半径を設定
    public void setRadius( float r )
    {
        if ( r <= 0.1f )
            return;
        r_ = r;
    }

    // 位置を極座標指定
    public void setPos( float latDeg, float longDeg )
    {
        curPos_ = SphereSurfUtil.convPolerToPos( latDeg, longDeg );
    }

    // 位置を直接指定
    public void setPosDirect( Vector3 pos )
    {
        curPos_ = pos.normalized;
    }

    // 移動速度(dist/sec)を設定
    public void setSpeed( float speed )
    {
        speed_ = speed;
    }

    // 移動方向を変更
    public void setDir(Vector3 dir)
    {
        // 移動方向をtangent化
        var b = Vector3.Cross( curPos_, dir );
        tangent_ = Vector3.Cross( b, curPos_ ).normalized;
    }

    // 位置を変更
    public void update()
    {
        float delta = speed_ * Time.deltaTime;
        float deltaN = delta / r_;
        var nextPos = SphereSurfUtil.calcMovePos( curPos_, tangent_, deltaN ).normalized;
        setDir( nextPos - curPos_ ); // tangent更新
        curPos_ = nextPos;
    }

    // 位置を取得
    public Vector3 getPos()
    {
        return curPos_ * r_;
    }

    // 向いている方向を取得
    public Vector3 getForward()
    {
        return tangent_;
    }

    // 上方向を取得
    public Vector3 getUp()
    {
        return curPos_;
    }

    float r_ = 1.0f;
    float speed_ = 0.0f;
    Vector3 tangent_ = new Vector3( 0.0f, 1.0f, 0.0f ); // 向き（球面の接線）
    Vector3 curPos_ = new Vector3( 0.0f, 0.0f, -1.0f ); // 位置(半径1に正規化）
}
