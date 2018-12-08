using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State {
    public WaitState(float waitSec, State nextState, System.Action finishCallback )
    {
        finishCallback_ = finishCallback;
        waitSec_ = waitSec;
        nextState_ = nextState;
    }


    public WaitState( float waitSec, State nextState )
    {
        waitSec_ = waitSec;
        nextState_ = nextState;
    }

    // 内部状態
    override protected State innerUpdate()
    {
        waitSec_ -= Time.deltaTime;
        if ( waitSec_ <= 0.0f ) {
            bFinish_ = true;
            if ( finishCallback_ != null )
                finishCallback_();
            return nextState_;
        }
        return this;
    }

    bool bFinish_ = false;
    float waitSec_;
    State nextState_;
    System.Action finishCallback_;
}
