using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイトル

public class TitleManager : MonoBehaviour {

	[SerializeField]
	TextMesh text_;

	public System.Action< int > FinishCallback { set { finishCallback_ = value; } }
	public void setup( Fader fader, int initStageIdx ) {
		fader_ = fader;
		selectStageIdx_ = initStageIdx;
		text_.text = string.Format( "LR: Stage Select [{0}]/Move", selectStageIdx_ );
	}

	// Use this for initialization
	void Start () {
		state_ = new FadeIn( this );		
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class FadeIn : State< TitleManager > {
		public FadeIn(TitleManager parent) : base( parent ) {
			// parent_.fader_.to( 1.0f, 0.0f );
		}
		protected override State innerInit() {
			parent_.fader_.to( 0.0f, 1.0f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State< TitleManager > {
		public Idle(TitleManager parent) : base( parent ) {}
		protected override State innerInit() {
			GlobalState.start( () => {
				return !Input.GetKeyDown( KeyCode.Z );
			} ).finish( () => {
				setNextState( new FadeOut( parent_ ) );
			} );
			return this;
		}
		protected override State innerUpdate() {
			if ( Input.GetKeyDown( KeyCode.LeftArrow ) == true ) {
				parent_.selectStageIdx_ = ( parent_.selectStageIdx_ == 1 ? TowerParameterTable.getInstance().getParamNum() : parent_.selectStageIdx_ - 1 );
			} else if ( Input.GetKeyDown( KeyCode.RightArrow ) == true ) {
				parent_.selectStageIdx_ = ( parent_.selectStageIdx_ == TowerParameterTable.getInstance().getParamNum() ? 1 : parent_.selectStageIdx_ + 1 );
			}
			parent_.text_.text = string.Format( "LR: Stage Select [{0}]/Move", parent_.selectStageIdx_ );
			return this;
		}
	}

	class FadeOut : State< TitleManager > {
		public FadeOut(TitleManager parent) : base( parent ) { }
		protected override State innerInit() {
			parent_.fader_.to( 1.0f, 1.0f, () => {
				parent_.finishCallback_( parent_.selectStageIdx_ );
			} );
			return this;
		}
	}
	Fader fader_;
	State state_;
	System.Action< int > finishCallback_;
	int selectStageIdx_ = 1;
}
