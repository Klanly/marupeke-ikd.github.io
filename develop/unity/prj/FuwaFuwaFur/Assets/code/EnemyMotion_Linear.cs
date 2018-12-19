using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の動き：線形
//
//  start - goal間を往復運動

public class EnemyMotion_Linear : EnemyMotionUnit {

    public EnemyMotion_Linear( Vector3 start, Vector3 goal, float secFromStartToGoal, TimeObj.TweenType tweenType = TimeObj.TweenType.TweenType_EaseInOut )
    {
        start_ = start;
        goal_ = goal;
        timeObj_ = new TimeObj( tweenType, 1.0f / secFromStartToGoal, TimeObj.LoopType.LoopType_PingPong );
    }

    // 自分の今の位置を取得
    override protected Vector3 getMyCurPos()
    {
        float t = timeObj_.getCur0_1();
        return Vector3.Lerp( start_, goal_, t );
    }

    // 線形で位置を更新
    override protected Vector3 updateMyPos( float dt )
    {
        float t = timeObj_.next0_1();
        return Vector3.Lerp( start_, goal_, t );
    }

    TimeObj timeObj_;
    Vector3 start_;
    Vector3 goal_;
}
