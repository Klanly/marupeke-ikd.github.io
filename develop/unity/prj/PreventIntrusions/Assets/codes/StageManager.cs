using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

	[SerializeField]
	GameOverManager gameOverPrefab_;

	public class Param {
		public int stageIndex_ = 0;
	}

	public System.Action< bool > FinishCallback { set { finishCallback_ = value; } }

	// ステージ設定
	public void setup( Param param ) {
		param_ = param;
	}

	// ステージインデックスを取得
	public int getStageIndex() {
		return param_.stageIndex_;
	}

	// Use this for initialization
	void Start () {
		state_ = new Intro( this );
	}

	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class Intro : State<StageManager> {
		public Intro(StageManager parent) : base( parent ) { }
		protected override State innerInit() {
			FaderManager.Fader.to( 0.0f, 1.0f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State<StageManager> {
		public Idle(StageManager parent) : base( parent ) { }
		protected override State innerUpdate() {
			if ( Input.GetKey( KeyCode.Z ) == true ) {
				FaderManager.Fader.to( 1.0f, 3.0f, () => {
					if ( parent_.finishCallback_ != null ) {
						parent_.finishCallback_( true );
						parent_.finishCallback_ = null;
					}
				} );
			} else if ( Input.GetKey( KeyCode.G ) == true ) {
				return new GameOver( parent_ );
			}
			return this;
		}
	}

	class GameOver : State<StageManager> {
		public GameOver(StageManager parent) : base( parent ) { }
		protected override State innerInit() {
			var gameOver = Instantiate<GameOverManager>( parent_.gameOverPrefab_ );
			gameOver.transform.parent = parent_.transform;
			gameOver.FinishCallback = () => {
				FaderManager.Fader.to( 1.0f, 3.0f, () => {
					parent_.finishCallback_( false );
				});
			};
			return this;
		}
	}
	Param param_;
	System.Action<bool> finishCallback_;
	State state_;
}
