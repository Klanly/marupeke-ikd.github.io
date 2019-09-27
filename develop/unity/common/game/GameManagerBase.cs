using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 管理人基底クラス

public class GameManagerBase : MonoBehaviour {
    public Action FinishCallbacak { set => finishCallback_ = value; }
    protected Action finishCallback_;

    // 状態更新
    protected void stateUpdate() {
        if ( state_ != null )
            state_ = state_.update();
    }

    protected State state_ = null;
}
