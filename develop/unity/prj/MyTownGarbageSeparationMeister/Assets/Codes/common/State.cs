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
            if ( nextState_ != null )
                return nextState_;
        }
        State res = innerUpdate();
        return ( nextState_ == null ? res : nextState_ );
    }

    protected void setNextState( State nextState )
    {
        nextState_ = nextState;
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
}

// 親保持サブステート
public class State<T> : State {
	public State(T parent) {
		parent_ = parent;
	}
	protected T parent_;
}