using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.Text timeText_;

	public System.Action TimeOverCallback { set { timeOverCallback_ = value; } }
	public void setup( int sec ) {
		sec_ = ( float )sec;
	}

	public void setActive( bool isActive ) {
		bActive_ = isActive;
	}

	public void addSec( float sec ) {
		sec_ += sec;
	}

	public float getSec() {
		return sec_;
	}

	void updatetTimeStr() {
		int minute = ( int )( sec_ / 60.0f );
		int sec = ( int )( sec_ % 60 );
		int millSec = ( int )( ( sec_ * 100.0f ) % 100 );
		timeText_.text = string.Format( "{0:00}:{1:00}:{2:00}", minute, sec, millSec );
	}

	// Use this for initialization
	void Update() {
		if ( bActive_ == false )
			return;

		if ( bTimeOver_ == true )
			return;

		sec_ -= Time.deltaTime;
		if ( sec_ <= 0.0f ) {
			sec_ = 0.0f;
			bTimeOver_ = true;
			if ( timeOverCallback_ != null ) {
				timeOverCallback_();
			}
		}
		updatetTimeStr();
	}

	float sec_;
	bool bActive_ = true;
	System.Action timeOverCallback_;
	bool bTimeOver_ = false;
}
