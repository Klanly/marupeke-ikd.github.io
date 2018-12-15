using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シンプルな状態遷移

public class State {

    bool bInit_ = false;

    // 初期化
    public void init()
    {
        innerInit();
    }

    // 状態更新
    public State update()
    {
        if ( bInit_ == false ) {
            init();
            bInit_ = true;
        }
        return innerUpdate();
    }

    // 内部初期化
    virtual protected void innerInit()
    {

    }

    // 内部状態
    virtual protected State innerUpdate()
    {
        return null;
    }
}
