using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour {

	public System.Action FinishCallback { set { finishCallback_ = value; } }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKey( KeyCode.Z ) == true ) {
			if ( finishCallback_ != null ) {
				finishCallback_();
				finishCallback_ = null;
			}
		}
	}

	System.Action finishCallback_;
}
