using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	TitleManager titlePrefab_;

	[SerializeField]
	StageManager stagePrefab_;

	[SerializeField]
	EndingManager endingPrefab_;

	[SerializeField]
	int stageIndex_ = 1;

	void Start () {
		FaderManager.Fader.setColor( Color.black, 1.0f );
		state_ = new Title( this );
	}
	
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class Title : State< GameManager > {
		public Title( GameManager parent ) : base( parent ) {
		}

		protected override State innerInit() {
			title_ = Instantiate<TitleManager>( parent_.titlePrefab_ );
			title_.FinishCallback = () => {
				setNextState( new Stage( parent_, parent_.stageIndex_ ) );
				Destroy( title_.gameObject );
			};
			return this;
		}
		TitleManager title_;
	}

	class Stage : State<GameManager> {
		public Stage(GameManager parent, int stageIndex ) : base( parent ) {
			stageIndex_ = stageIndex;
			parent_.stageIndex_ = stageIndex;
		}
		protected override State innerInit() {
			stage_ = Instantiate<StageManager>( parent_.stagePrefab_ );
			var param = new StageManager.Param();   // TODO: テーブルから取得
			param.stageIndex_ = stageIndex_;
			stage_.setup( param );
			stage_.FinishCallback = ( isNext )=> {
				var finalStage = 2;	// TODO: テーブルから取得
				if ( isNext == true ) {
					if ( stage_.getStageIndex() == finalStage ) {
						// エンディングへ
						setNextState( new Ending( parent_ ) );
					} else {
						// 次のステージへ
						setNextState( new Stage( parent_, stage_.getStageIndex() + 1 ) );
					}
				} else {
					// ゲームオーバー後なのでタイトルへ
					setNextState( new Title( parent_ ) );
				}
				Destroy( stage_.gameObject );
			};
			return this;
		}
		StageManager stage_;
		int stageIndex_ = 0;
	}

	class Ending : State<GameManager> {
		public Ending(GameManager parent) : base( parent ) {
		}
		protected override State innerInit() {
			ending_ = Instantiate< EndingManager >( parent_.endingPrefab_ );
			ending_.FinishCallback = () => {
				setNextState( new Title( parent_ ) );
				Destroy( ending_.gameObject );
			};
			return this;
		}
		EndingManager ending_;
	}

	State state_;
}
