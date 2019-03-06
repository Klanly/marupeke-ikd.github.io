using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameCore corePrefab_;

	[SerializeField]
	TitleManager titlePrefab_;

	[SerializeField]
	Fader fader_;
	
	[SerializeField]
	int initLevel_ = 1;


	// Use this for initialization
	void Start () {
		state_ = new Title( this );
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null ) {
			state_ = state_.update();
		}
	}

	class Title : State< GameManager > {
		public Title(GameManager parent) : base( parent ) { }
		protected override State innerInit() {
			SoundAccessor.getInstance().stopBGM();
			title_ = Instantiate<TitleManager>( parent_.titlePrefab_ );
			title_.transform.position = Vector3.zero;
			title_.setup( parent_.fader_, parent_.initLevel_ );
			title_.FinishCallback = ( selectStageIdx ) => {
				parent_.initLevel_ = selectStageIdx;
				setNextState( new GameIntro( parent_ ) );
				Destroy( title_.gameObject );
			};
			return this;
		}
		TitleManager title_;
	}

	class GameIntro : State< GameManager > {
		public GameIntro(GameManager parent) : base( parent ) { }
		protected override State innerInit() {
			var coreParam = new GameCore.Param();
			coreParam.playerParam_.transSec_ = 0.1f;
			core_ = Instantiate<GameCore>( parent_.corePrefab_ );
			core_.setup( coreParam, parent_.initLevel_, parent_.fader_ );

			parent_.fader_.to( 0.0f, 0.75f );

			return new GameIdle( parent_, core_ );
		}
		GameCore core_;
	}

	class GameIdle : State<GameManager> {
		public GameIdle(GameManager parent, GameCore core ) : base( parent ) {
			core_ = core;
		}
		protected override State innerInit() {
			core_.AllFinishCallback = () => {
				setNextState( new Title( parent_ ) );
				Destroy( core_.gameObject );
			};
			return this;
		}
		GameCore core_;
	}

	State state_;
}
