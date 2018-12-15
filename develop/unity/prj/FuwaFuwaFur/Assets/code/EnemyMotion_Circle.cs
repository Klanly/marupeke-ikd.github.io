using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の動き：円形
//
//  半径radiusの円上を周回運動

public class EnemyMotion_Circle : EnemyMotionUnit {

    // normal: 円の向き。法線の向きに対して右ネジに回転
    // secPerRound: 1周回るのにかかる時間
    // upVec: 空方向のベクトル。これと法線から回転面のX軸、Y軸が決まる。
    public EnemyMotion_Circle( float radius, Vector3 normal, Vector3 upVec, float secPerRound, float initDeg = 0.0f, TimeObj.TweenType tweenType = TimeObj.TweenType.TweenType_Linear )
    {
        timeObj_ = new TimeObj( tweenType, 1.0f / secPerRound, TimeObj.LoopType.LoopType_Normal );
        x_ = Vector3.Cross( upVec, normal ).normalized;
        y_ = Vector3.Cross( normal, x_ ).normalized;
        radius_ = radius;
        initDeg_ = initDeg;
        pos_ = calcPos( 0.0f );
    }

    // 自分の今の位置を取得
    override protected Vector3 getMyCurPos()
    {
        return pos_;
    }

    // 線形で位置を更新
    override protected Vector3 updateMyPos(float dt)
    {
        float t = timeObj_.next0_1();
        pos_ = calcPos( t );
        return pos_;
    }

    // 位置を計算
    Vector3 calcPos( float t )
    {
        float deg = initDeg_ + 360.0f * t;
        float rad = deg * Mathf.Deg2Rad;
        return radius_ * ( x_ * Mathf.Cos( rad ) + y_ * Mathf.Sin( rad ) );
    }

    TimeObj timeObj_;
    Vector3 x_, y_;
    Vector3 pos_ = Vector3.zero;
    float radius_ = 10.0f;
    float initDeg_ = 0.0f;
}
