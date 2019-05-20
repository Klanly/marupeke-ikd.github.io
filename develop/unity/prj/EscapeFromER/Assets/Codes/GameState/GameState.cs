using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム状態のひとまとまり
public class GameState : MonoBehaviour {

    public System.Action CompleteCallback { set { completeCallback_ = value; } }

    protected virtual void complete() {
        if ( completeCallback_ != null )
            completeCallback_();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected System.Action completeCallback_;  // ステート内完了コールバック
}
