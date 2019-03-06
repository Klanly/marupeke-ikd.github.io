using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValueTest : MonoBehaviour {

	[SerializeField]
	bool bUpdate_ = false;

	[SerializeField]
	long aim_ = 0;

	[SerializeField]
	float sec_ = 1.0f;

	// Use this for initialization
	void Start () {
		val_ = new MoveValueLong( 0, 1.0f );
	}
	
	// Update is called once per frame
	void Update () {
		if ( bUpdate_ == true ) {
			bUpdate_ = false;
			val_.setSec( sec_ );
			val_.setAim( aim_ );
		}	
	}

	private void OnGUI() {
		GUI.Label( new Rect( 0, 50, 300, 60 ), "val: " + val_.getCurVal() + " -> " + val_.getAim() );
	}

	MoveValueLong val_;
}
