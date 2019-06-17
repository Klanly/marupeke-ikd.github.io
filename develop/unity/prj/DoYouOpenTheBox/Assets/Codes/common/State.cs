using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シンプルな状態遷移

public class State {

    bool bInit_ = false;

    // 初期化
    public State init()
    {
        return innerInit();
    }

    // 状態更新
    public State update()
    {
        if ( bInit_ == false ) {
            nextState_ = init();
            bInit_ = true;
            if ( nextState_ != null ) {
                if ( bReinitialize_ == true ) {
                    bInit_ = false;
                    bReinitialize_ = false;
                }
                return nextState_;
            }
        }
        State res = innerUpdate();
        if ( nextState_ != null ) {
            if ( bReinitialize_ == true ) {
                bInit_ = false;
                bReinitialize_ = false;
            }
            return nextState_;
        }
        return res;
    }

    protected void setNextState( State nextState, bool reInit = false )
    {
        nextState_ = nextState;
        if ( reInit == true )
            bReinitialize_ = true;
    }

    // 内部初期化
    virtual protected State innerInit()
    {
        return null;
    }

    // 内部状態
    virtual protected State innerUpdate()
    {
        return null;
    }

    State nextState_ = null;
    bool bReinitialize_ = false;
}

// 親保持サブステート
public class State<T> : State {
	public State(T parent) {
		parent_ = parent;
	}
	protected T parent_;
}