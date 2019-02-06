using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アクション
//
//  このオブジェクトへのアクションを他者へ伝える

public class OnAction : MonoBehaviour {

    public System.Action<GameObject, string> ActionCallback { set { actionCallback_ = value; } }

    public void onAction( GameObject caller, string eventName )
    {
        if ( actionCallback_ != null )
            actionCallback_( caller, eventName );
    }
    System.Action<GameObject, string> actionCallback_;
}
