using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBase {

    // 状態を戻す
    virtual public void resetAll()
    {

    }

    public void setSinkAcc( float acc )
    {
        sinkAcc_ = acc;
    }

    public void setRiseUpAcc( float acc )
    {
        riseUpAcc_ = acc;
    }

    public void setRiseWaitTime( float sec )
    {
        riseWaitTime_ = sec;
    }

    // 橋の現在位置を取得
    virtual public Vector3 getCurPos()
    {
        return Vector3.zero;
    }

    virtual public void switchOn()
    {

    }

    virtual public Vector3 update( Vector3 pos )
    {
        return pos;
    }

    protected float sinkAcc_ = 1.0f;
    protected float riseUpAcc_ = 1.0f;
    protected float riseWaitTime_ = 3.0f;
}

// ワンショットブリッヂ
//  スイッチを入れると上に自動上昇
public class OneShotBridge : BridgeBase
{
    public OneShotBridge()
    {
        curState_ = riseWait;
    }

    // 状態を戻す
    override public void resetAll()
    {
        curState_ = riseWait;
        Vector3 curPos_ = Vector3.zero;
        bRise_ = false;
        riseVec_ = 0.0f;
        curRiseWaitTime_ = 0.0f;
        sinkVec_ = 0.0f;
    }

    override public void switchOn()
    {
        bRise_ = true;
    }

    override public Vector3 update( Vector3 pos )
    {
        curPos_ = curState_( pos );
        return curPos_;
    }

    // 現在の橋の位置を取得
    override public Vector3 getCurPos()
    {
        return curPos_;
    }

    Vector3 riseWait( Vector3 pos )
    {
        curRiseWaitTime_ += Time.deltaTime;
        if ( curRiseWaitTime_ >= riseWaitTime_ ) {
            // 沈み込みへ
            bRise_ = false;
            curRiseWaitTime_ = 0.0f;
            curState_ = sink;
        }
        return pos;
    }

    Vector3 sink( Vector3 pos )
    {
        sinkVec_ -= sinkAcc_ * Time.deltaTime;
        pos.y += sinkVec_;
        if ( pos.y <= sinkDist_ ) {
            pos.y = sinkDist_;
            // 浮上待ちへ
            sinkVec_ = 0.0f;
            curState_ = waitRise;
        } else if ( bRise_ == true ) {
            // 浮上へ
            bRise_ = false;
            sinkVec_ = 0.0f;
            curState_ = rise;
        }
        return pos;
    }

    Vector3 waitRise( Vector3 pos )
    {
        if ( bRise_ == true ) {
            bRise_ = false;
            // 浮上開始
            curState_ = rise;
        }
        return pos;
    }

    Vector3 rise( Vector3 pos )
    {
        riseVec_ += riseUpAcc_ * Time.deltaTime;
        pos.y += riseVec_;
        if ( pos.y >= 0.0f ) {
            // 上昇待ちへ
            pos.y = 0.0f;
            riseVec_ = 0.0f;
            curState_ = riseWait;
        }
        return pos;
    }

    System.Func<Vector3, Vector3> curState_;
    Vector3 curPos_ = Vector3.zero;
    bool bRise_ = false;
    float riseVec_ = 0.0f;
    float curRiseWaitTime_ = 0.0f;
    float sinkVec_ = 0.0f;
    float sinkDist_ = -15.0f;
}

// ホールドブリッジ
//  スイッチを入れている間だけ上昇
public class HoldBridge : BridgeBase
{
    public HoldBridge()
    {
        curState_ = riseWait;
    }

    // 状態を戻す
    override public void resetAll()
    {
        curState_ = riseWait;
        Vector3 curPos_ = Vector3.zero;
        bRise_ = false;
        riseVec_ = 0.0f;
        curRiseWaitTime_ = 0.0f;
        sinkVec_ = 0.0f;
    }

    override public void switchOn()
    {
        bRise_ = true;
    }

    override public Vector3 update(Vector3 pos)
    {
        curPos_ = curState_( pos );
        bRise_ = false; // 常に落ちる方向へ
        return curPos_;
    }

    // 現在の橋の位置を取得
    override public Vector3 getCurPos()
    {
        return curPos_;
    }

    Vector3 riseWait(Vector3 pos)
    {
        curRiseWaitTime_ += Time.deltaTime;
        if ( curRiseWaitTime_ >= riseWaitTime_ ) {
            // 沈み込みへ
            curRiseWaitTime_ = 0.0f;
            curState_ = sink;
        }
        return pos;
    }

    Vector3 sink(Vector3 pos)
    {
        sinkVec_ -= sinkAcc_ * Time.deltaTime;
        pos.y += sinkVec_;
        if ( pos.y <= sinkDist_ ) {
            pos.y = sinkDist_;
            // 浮上待ちへ
            sinkVec_ = 0.0f;
            curState_ = waitRise;
        } else if ( bRise_ == true ) {
            // 浮上へ
            bRise_ = false;
            sinkVec_ = 0.0f;
            curState_ = rise;
        }
        return pos;
    }

    Vector3 waitRise(Vector3 pos)
    {
        if ( bRise_ == true ) {
            bRise_ = false;
            // 浮上開始
            curState_ = rise;
        }
        return pos;
    }

    Vector3 rise(Vector3 pos)
    {
        if ( bRise_ == false ) {
            // 沈み込みへ
            riseVec_ = 0.0f;
            curState_ = sink;
        } else {
            riseVec_ += riseUpAcc_ * Time.deltaTime;
            pos.y += riseVec_;
            if ( pos.y >= 0.0f ) {
                // 上昇待ちへ
                pos.y = 0.0f;
                riseVec_ = 0.0f;
                curState_ = riseWait;
            }
        }
        return pos;
    }

    System.Func<Vector3, Vector3> curState_;
    Vector3 curPos_ = Vector3.zero;
    bool bRise_ = false;
    float riseVec_ = 0.0f;
    float curRiseWaitTime_ = 0.0f;
    float sinkVec_ = 0.0f;
    float sinkDist_ = -15.0f;
}