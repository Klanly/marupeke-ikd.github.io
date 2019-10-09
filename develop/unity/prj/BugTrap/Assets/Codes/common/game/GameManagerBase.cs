using UnityEngine;

// 管理人基底クラス

public class GameManagerBase : MonoBehaviour {
    public System.Action FinishCallbacak { set => finishCallback_ = value; }
    protected System.Action finishCallback_;

    // 状態更新
    virtual protected void stateUpdate() {
        if ( state_ != null )
            state_ = state_.update();
    }

    protected State state_ = null;
}
