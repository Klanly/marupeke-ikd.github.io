using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		state_ = new Idle( this );
	}
	
	// Update is called once per frame
	void Update () {
		var preState = state_;
		if ( state_ != null ) {
			state_ = state_.update();
			if ( state_ != preState )
				Update();
		}
	}

	class Idle : State<Player> {
		public Idle(Player parent) : base( parent ) { }
		protected override State innerUpdate() {
			if ( Input.GetKey( KeyCode.LeftArrow ) == true ) {
				len_.x -= offset_;
			} else if ( Input.GetKey( KeyCode.RightArrow ) == true ) {
				len_.x += offset_;
			} else if ( Input.GetKey( KeyCode.UpArrow ) == true ) {
				len_.z += offset_;
			} else if ( Input.GetKey( KeyCode.DownArrow ) == true ) {
				len_.z -= offset_;
			}
			if ( len_.magnitude > 0.0001f )
				return new Move( parent_, len_ );
			return this;
		}
		Vector3 len_ = Vector3.zero;
		float offset_ = 0.5f;
	}

	class Move : State<Player> {
		public Move(Player parent, Vector3 len ) : base( parent ) {
			len_ = len;
		}
		protected override State innerInit() {
			var pos = parent_.transform.localPosition;
			GlobalState.time( 0.15f, (sec, t) => {
				parent_.transform.localPosition = pos + Lerps.Vec3.linear( Vector3.zero, len_, t );
				return true;
			} ).finish(()=> {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
		Vector3 len_;
	}

	State state_;
}
