using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	public System.Action FinishCallback { set { finishCallback_ = value; } }

	void Start () {
		state_ = new FadeIn( this );		
	}
	
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class FadeIn : State< TitleManager > {
		public FadeIn(TitleManager parent) : base( parent ) { }
		protected override State innerInit() {
			FaderManager.Fader.to( 0.0f, 1.5f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State< TitleManager > {
		public Idle(TitleManager parent) : base( parent ) { }
		protected override State innerUpdate() {
			if ( Input.GetKey( KeyCode.Z ) == true ) {
				return new FadeOut( parent_ );
			}
			return this;
		}
	}

	class FadeOut : State<TitleManager> {
		public FadeOut(TitleManager parent) : base( parent ) { }
		protected override State innerInit() {
			FaderManager.Fader.to( 1.0f, 1.5f, () => {
				if ( parent_.finishCallback_ != null ) {
					parent_.finishCallback_();
					parent_.finishCallback_ = null;
				}
			} );
			return this;
		}
	}

	State state_;
	System.Action finishCallback_;
}
