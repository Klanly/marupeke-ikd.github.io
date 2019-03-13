using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	public System.Action FinishCallback { set { finishCallback_ = value; } }

	void Start () {
		GlobalState.time( 5.0f, (sec, t) => {
			return true;
		} ).finish(() => {
			if ( finishCallback_ != null ) {
				finishCallback_();
				finishCallback_ = null;
			}
		} );		
	}
	
	void Update () {
	}

	System.Action finishCallback_;
}
