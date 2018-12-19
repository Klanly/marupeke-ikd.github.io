using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0 - 1時間オブジェクト

public class TimeObj {
    public enum LoopType
    {
        LoopType_Normal,    //  0 -> 1で0戻る
        LoopType_PingPong,  // 0 <-> 1繰り返し
    }

    public enum TweenType
    {
        TweenType_Linear,       // リニア
        TweenType_EaseInOut,    // EaseIn-Out
        TweenType_EaseIn,       // EaseIn
        TweenType_EaseOut,      // EaseOut
        TweenType_FirstInOut,   // FirstInOut
    }

    // intervalScale : 時間tの変化速度。tはデフォルトで0-1まで1秒で変化。intervalScaleを2にすると速度が倍になる（最大で10倍）。
    public TimeObj( TweenType tweenType, float intervalScale, LoopType loopType )
    {
        loopType_ = loopType;
        tweenType_ = tweenType;
        if ( intervalScale < 0.0f )
            intervalScale = 1.0f;
        else if ( intervalScale > 10.0f )
            intervalScale = 10.0f;
        intervalScale_ = intervalScale;
    }

    // 今の0 - 1値を取得
    public float getCur0_1()
    {
        return curOutT_;
    }

    // 次の0 - 1値を取得
    public float next0_1()
    {
        float t = getNextT( Time.deltaTime * intervalScale_ );
        curOutT_ = innerNext0_1( t );
        return curOutT_;
    }

    virtual protected float innerNext0_1( float t )
    {
        if ( tweenType_ == TweenType.TweenType_Linear ) {
            return t;
        }
        else if ( tweenType_ == TweenType.TweenType_EaseInOut ) {
            return t * t * ( -2.0f * t + 3.0f );
        }
        else if ( tweenType_ == TweenType.TweenType_EaseIn ) {
            return t * t;
        }
        else if ( tweenType_ == TweenType.TweenType_EaseOut ) {
            return t * ( 2.0f - t );
        }
        else if ( tweenType_ == TweenType.TweenType_FirstInOut ) {
            return t * ( 3.0f + t * ( -6.0f + 4.0f * t ) );
        }
        return 0.0f;
    }

    float getNextT( float t )
    {
        if ( loopType_ == LoopType.LoopType_Normal ) {
            curT_ += t;
            if ( curT_ > 1.0f ) {
                curT_ -= -1.0f;
            }
        } else if ( loopType_ == LoopType.LoopType_PingPong ) {
            curT_ += t * pingPongDir_;
            if ( curT_ > 1.0f ) {
                pingPongDir_ = -1.0f;
                curT_ = 2.0f - curT_;
            } else if ( curT_ < 0.0f ) {
                pingPongDir_ = 1.0f;
                curT_ = -curT_;
            }
        }

        return curT_;
    }

    LoopType loopType_;
    TweenType tweenType_;
    float intervalScale_;
    float curT_ = 0.0f;
    float pingPongDir_ = 1.0f;
    float curOutT_ = 0.0f;
}
