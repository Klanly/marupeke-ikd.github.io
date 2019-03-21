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
	TutorialStageManager tutorialStagePrefeb_;

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
			title_.FinishCallback = ( mode ) => {
				if ( mode == TitleManager.Mode.Start )
					setNextState( new Stage( parent_, parent_.stageIndex_ ) );
				else
					setNextState( new TutorialStage( parent_, parent_.stageIndex_ ) );
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
			int stageNum = Stage_data.getInstance().getRowNum();
			if ( stageIndex_ >= stageNum ) {
				// タイトルへ
				parent_.stageIndex_ = 0;
				return new Title( parent_ );
			}
			var param = new StageManager.Param();
			param.stageIndex_ = stageIndex_;
			var sp = Stage_data.getInstance().getParam( "stage_" + stageIndex_ );
			param.fieldParam_.region_.x = sp.width_;
			param.fieldParam_.region_.y = sp.height_;
			param.fieldParam_.maxBarricadeNum_ = sp.maxBarricadeNum_;
			param.enemyNum_ = sp.enemyNum_;
			param.timeSec_ = sp.time_;

			stage_ = Instantiate<StageManager>( parent_.stagePrefab_ );
			stage_.setup( param );
			stage_.FinishCallback = ( isNext )=> {
				if ( isNext == true ) {
					// 次のステージへ
					setNextState( new Stage( parent_, stage_.getStageIndex() + 1 ) );
				} else {
					// ゲームオーバー後なのでタイトルへ
					parent_.stageIndex_ = 0;
					setNextState( new Title( parent_ ) );
				}
				Destroy( stage_.gameObject );
			};
			return this;
		}
		StageManager stage_;
		int stageIndex_ = 0;
	}

	class TutorialStage : State<GameManager> {
		public TutorialStage(GameManager parent, int stageIndex ) : base( parent ) {
			stageIndex_ = stageIndex;
		}
		protected override State innerInit() {
			var stage = Instantiate<TutorialStageManager>( parent_.tutorialStagePrefeb_ );
			stage_ = stage;
			if ( stage.setup( stageIndex_ ) == false ) {
				Destroy( stage_.gameObject );
				// タイトルへ戻る
				return new Title( parent_ );
			}

			stage_.FinishCallback = (isNext) => {
				if ( isNext == true ) {
					// 次のステージへ
					setNextState( new TutorialStage( parent_, stage_.getStageIndex() + 1 ) );
				} else {
					// タイトルへ戻る
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
