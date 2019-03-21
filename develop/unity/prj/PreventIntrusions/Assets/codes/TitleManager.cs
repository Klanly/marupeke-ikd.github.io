using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.Button startBtn_;

	[SerializeField]
	UnityEngine.UI.Button turorialBtn_;

	public enum Mode {
		Start,
		Tutorial
	}

	public System.Action<Mode> FinishCallback { set { finishCallback_ = value; } }

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
		protected override State innerInit() {
			parent_.startBtn_.onClick.AddListener( () => {
				setNextState( new FadeOut( parent_, mode_ ) );
			} );
			parent_.turorialBtn_.onClick.AddListener( () => {
				setNextState( new FadeOut( parent_, mode_ ) );
			} );
			parent_.turorialBtn_.enabled = false;
			parent_.startBtn_.enabled = true;
			parent_.startBtn_.Select();
			return null;
		}
		protected override State innerUpdate() {
			if ( Input.GetKeyDown( KeyCode.LeftArrow ) == true ) {
				parent_.startBtn_.enabled = true;
				parent_.turorialBtn_.enabled = false;
				parent_.startBtn_.Select();
				mode_ = Mode.Start;
			} else if ( Input.GetKeyDown( KeyCode.RightArrow ) == true ) {
				parent_.startBtn_.enabled = false;
				parent_.turorialBtn_.enabled = true;
				parent_.turorialBtn_.Select();
				mode_ = Mode.Tutorial;
			} else if ( Input.GetKey( KeyCode.Z ) == true ) {
				return new FadeOut( parent_, mode_ );
			}
			return this;
		}
		Mode mode_ = Mode.Start;
	}

	class FadeOut : State<TitleManager> {
		public FadeOut(TitleManager parent, Mode mode ) : base( parent ) {
			mode_ = mode;
		}
		protected override State innerInit() {
			FaderManager.Fader.to( 1.0f, 1.5f, () => {
				if ( parent_.finishCallback_ != null ) {
					parent_.finishCallback_( mode_ );
					parent_.finishCallback_ = null;
				}
			} );
			return this;
		}
		Mode mode_;
	}

	State state_;
	System.Action<Mode> finishCallback_;
}
