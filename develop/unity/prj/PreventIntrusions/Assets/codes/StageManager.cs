using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

	[SerializeField]
	Field fieldPrefab_;

	[SerializeField]
	Player playerPrefab_;

	[SerializeField]
	GameOverManager gameOverPrefab_;

	[SerializeField]
	EnemyFactory enemyFactory_;

	public class Param {
		public int stageIndex_ = 0;
	}

	public System.Action< bool > FinishCallback { set { finishCallback_ = value; } }

	// ステージ設定
	public void setup( Param param ) {
		param_ = param;

		// フィールド
		field_ = Instantiate<Field>( fieldPrefab_ );
		field_.transform.parent = transform;
		field_.transform.localPosition = Vector3.zero;

		var fieldParam = new Field.Param();
		field_.setup( fieldParam );

		// TODO: 敵を配置
		for ( int i = 0; i < 5; ++i ) {
			var enemy = enemyFactory_.create( EnemyFactory.EnemyType.Hiyorimy );
			var enemyParam = new Enemy.Param();
			Vector2Int initPos = Vector2Int.zero;
			while ( true ) {
				initPos = new Vector2Int( Random.Range( 0, fieldParam.region_.x ), Random.Range( 0, fieldParam.region_.y ) );
				if ( field_.isSpace( initPos ) == true )
					break;
			}
			enemy.setup( field_, enemyParam, initPos );
			field_.addEnemy( enemy, initPos );
		}

		// プレイヤー
		var playerParam = new Player.Param();
		playerParam.moveSec_ = 0.1f;
		player_ = Instantiate<Player>( playerPrefab_ );
		player_.transform.parent = field_.transform;
		player_.setup( field_, playerParam );
		player_.setPos( new Vector2Int( 0, 0 ) );
	}

	// ステージインデックスを取得
	public int getStageIndex() {
		return param_.stageIndex_;
	}

	// 削除
	private void OnDestroy() {
		Destroy( lookerComponet_ );	// カメラを解放
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
			// カメラ初期化
			//  プレイヤーの位置を一定の角度と距離で追従
			parent_.lookerComponet_ = Camera.main.gameObject.AddComponent<ObjectLooker>();
			parent_.lookerComponet_.setTarget( parent_.player_.gameObject );
			parent_.lookerComponet_.setOffset( new Vector3( 0.0f, 5.0f, -3.0f ) );

			FaderManager.Fader.to( 0.0f, 1.0f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State<StageManager> {
		public Idle(StageManager parent) : base( parent ) { }
		protected override State innerUpdate() {
/*
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
*/
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

	Field field_;
	Player player_;
	ObjectLooker lookerComponet_;
}
