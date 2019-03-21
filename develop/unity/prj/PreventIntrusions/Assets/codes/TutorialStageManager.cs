using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStageManager : StageManager {

	[SerializeField]
	TutorialField tutorialFieldPrefab_;

	[SerializeField]
	TutorialData data_;

	[SerializeField]
	TutorialDescs descs_;

	// ステージ設定
	public bool setup( int index ) {
		var tutorialParam = data_.getParam( index );
		if ( tutorialParam == null ) {
			bInitialized_ = false;
			return false;
		}

		int[,] hBarricade = tutorialParam.hBarricades_;
		int[,] vBarricade = tutorialParam.vBarricades_;
		param_ = tutorialParam.stageParam_;
		param_.stageIndex_ = index;

		// フィールド
		var field = Instantiate<TutorialField>( tutorialFieldPrefab_ );
		field_ = field;
		field_.transform.parent = transform;
		field_.transform.localPosition = Vector3.zero;
		field.setup( param_.fieldParam_, hBarricade, vBarricade );

		// TODO: 敵を配置
		for ( int i = 0; i < tutorialParam.enemyPoses_.Count; ++i ) {
			emitEnemy( tutorialParam.enemyPoses_[ i ] );
		}

		// プレイヤー
		var playerParam = new Player.Param();
		playerParam.moveSec_ = 0.24f;
		player_ = Instantiate<Player>( playerPrefab_ );
		player_.transform.parent = field_.transform;
		player_.setup( field_, playerParam );
		player_.setPos( new Vector2Int( 0, 0 ) );

		// タイマー
		timeCounter_.setup( 120 );

		// Desc
		descs_.setup( index );

		bInitialized_ = true;

		return true;
	}

	// 敵を生成
	protected void emitEnemy( Vector2Int pos ) {
		if ( field_.isAllRegionStockaded() == true ) {
			return; // 置き場が無い
		}
		var enemy = enemyFactory_.create( EnemyFactory.EnemyType.Hiyorimy );
		var enemyParam = new Enemy.Param();
		enemy.setup( field_, enemyParam, pos );
		enemy.DestroyCallback = () => {
			GlobalState.wait( 2.0f, () => {
				emitEnemy();
				return false;
			} );
		};
		field_.addEnemy( enemy, pos );
	}

	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	// Use this for initialization
	void Start() {
		if ( bInitialized_ == true )
			state_ = new Intro( this );
	}
}
