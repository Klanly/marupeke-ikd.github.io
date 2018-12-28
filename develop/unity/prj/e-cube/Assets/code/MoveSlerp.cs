using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球面線形補間による連続座標移動
//
// 指定のゴールに向かって球面線形補間していきます。
// ゴールはリアルタイムに変更可能です。
// 
public class MoveSlerp {
    // initPos : 初期位置
    // rate    : 移動更新時補間率( 0.0 <= rate <= 1.0 )。0は認める（現在位置で停止）
    // minDist : 目的位置との誤差範囲（この長さ以下になった場合現在位置＝目的位置）
    public MoveSlerp( Vector3 initPos, float rate, float minDist )
    {
        curPos_ = initPos;
        aimPos_ = initPos;
        minDist_ = minDist;
        setRate( rate );
    }

    public void setAim( Vector3 aimPos )
    {
        aimPos_ = aimPos;
    }

    public void setRate( float rate )
    {
        if ( rate < 0.0f )
            rate = 0.0f;
        else if ( rate > 1.0f )
            rate = 1.0f;
        rate_ = rate;
    }

    public void setActive( bool isActive )
    {
        bActive_ = isActive;
    }

    public Vector3 update()
    {
        if ( bActive_ == false )
            return curPos_;
        curPos_ = Vector3.Slerp( curPos_, aimPos_, rate_ );
        if ( ( curPos_ - aimPos_ ).magnitude <= minDist_ ) {
            curPos_ = aimPos_;
        }
        return curPos_;
    }

    Vector3 curPos_;
    Vector3 aimPos_;
    float rate_;
    float minDist_;
    bool bActive_ = true;
}
