using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

	[SerializeField]
	Field fieldPrefab_;

	[SerializeField]
	protected Player playerPrefab_;

	[SerializeField]
	GameOverManager gameOverPrefab_;

	[SerializeField]
	protected EnemyFactory enemyFactory_;

	[SerializeField]
	UnityEngine.UI.Image clearImage_;

	[SerializeField]
	UnityEngine.UI.Image gameOverImage_;

	[SerializeField]
	protected TimeCounter timeCounter_;


	private void Awake() {
		clearImage_.gameObject.SetActive( false );
	}

	public class Param {
		public int stageIndex_ = 0;
		public Field.Param fieldParam_ = new Field.Param();
		public int enemyNum_ = 3;
		public int timeSec_ = 120;
	}

	public System.Action< bool > FinishCallback { set { finishCallback_ = value; } }

	// ステージ設定
	public void setup( Param param ) {
		param_ = param;

		// フィールド
		field_ = Instantiate<Field>( fieldPrefab_ );
		field_.transform.parent = transform;
		field_.transform.localPosition = Vector3.zero;
		field_.setup( param.fieldParam_ );

		// TODO: 敵を配置
		for ( int i = 0; i < param.enemyNum_; ++i ) {
			emitEnemy();
		}

		// プレイヤー
		var playerParam = new Player.Param();
		playerParam.moveSec_ = 0.24f;
		player_ = Instantiate<Player>( playerPrefab_ );
		player_.transform.parent = field_.transform;
		player_.setup( field_, playerParam );
		player_.setPos( new Vector2Int( 0, 0 ) );

		// タイマー
		timeCounter_.setup( param.timeSec_ );

		bInitialized_ = true;
	}

	// ステージインデックスを取得
	public int getStageIndex() {
		return param_.stageIndex_;
	}

	// 削除
	private void OnDestroy() {
		Destroy( lookerComponet_ );	// カメラを解放
	}

	// 敵を生成
	virtual protected void emitEnemy() {
		if ( field_.isAllRegionStockaded() == true ) {
			return;	// 置き場が無い
		}
		var enemy = enemyFactory_.create( EnemyFactory.EnemyType.Hiyorimy );
		var enemyParam = new Enemy.Param();
		Vector2Int initPos = Vector2Int.zero;
		Vector2Int region = field_.getRegion();
		while ( true ) {
			initPos = new Vector2Int( Random.Range( 0, region.x ), Random.Range( 0, region.y ) );
			if ( field_.isSpace( initPos ) == true && field_.isStockadePos( initPos ) == false )
				break;
		}
		enemy.setup( field_, enemyParam, initPos );
		enemy.DestroyCallback = () => {
			GlobalState.wait( 15.0f, () => {
				if ( this == null )
					return false;
				emitEnemy();
				return false;
			} );
		};

		field_.addEnemy( enemy, initPos );
	}

	// Use this for initialization
	void Start () {
		if ( bInitialized_ == true )
			state_ = new Intro( this );
	}

	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	protected class Intro : State<StageManager> {
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

	protected class Idle : State<StageManager> {
		public Idle(StageManager parent) : base( parent ) { }
		protected override State innerInit() {
			// 全囲い達成したらクリア
			parent_.field_.AllRegionStockadeCallback = () => {
				parent_.timeCounter_.setActive( false );
				parent_.clearImage_.gameObject.SetActive( true );
				GlobalState.wait( 3.0f, () => {
					FaderManager.Fader.to( 1.0f, 3.0f, () => {
						parent_.finishCallback_( true );
					} );
					return false;
				} );
			};
			// タイムアウトしたらゲームオーバー
			parent_.timeCounter_.TimeOverCallback = () => {
				parent_.gameOverImage_.gameObject.SetActive( true );
				GlobalState.wait( 3.0f, () => {
					FaderManager.Fader.to( 1.0f, 3.0f, () => {
						parent_.finishCallback_( false );
					} );
					return false;
				} );
			};
			return this;
		}
	}

	class Clear : State<StageManager> {
		public Clear(StageManager parent) : base( parent ) {
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

	protected Param param_;
	protected System.Action<bool> finishCallback_;
	protected State state_;

	protected Field field_;
	protected Player player_;
	protected ObjectLooker lookerComponet_;
	protected bool bInitialized_ = false;
}
