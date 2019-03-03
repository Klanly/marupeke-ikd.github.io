using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour {

	[SerializeField]
	Tower towerPrefab_;

	[SerializeField]
	GameObject root_;

	[SerializeField]
	Player player_;

	[SerializeField]
	TorusMesh playerSlideLine_;

	[SerializeField]
	ElectricNeedle electricNeedlePrefab_;

	[SerializeField]
	float curElectricRate_ = 0.0f;

	[SerializeField]
	ScoreManager scoreManager_;

	public class Param {
		public Tower.Param towerParam_ = new Tower.Param();
		public Player.Param playerParam_ = new Player.Param();
		public float electricNeedleSpeed_ = 20.0f;		// 最遠位置から最近位置に辿り着くまでの秒数
	}

	public void setup( Param param, ScoreManager scoreManager ) {
		param_ = param;
		scoreManager_ = scoreManager;

		tower_ = Instantiate<Tower>( towerPrefab_ );
		tower_.transform.parent = root_.transform;
		tower_.transform.localPosition = Vector3.zero;
		tower_.setup( param.towerParam_ );
		tower_.gameObject.SetActive( false );

		// プレイヤー
		player_.setup( param.playerParam_, tower_ );

		// プレイヤー通路パイプ
		playerSlideLine_.InnerRadius = player_.Radius - playerSlideLine_.TubeRadius;

		electricNeedles_[ 0 ] = Instantiate<ElectricNeedle>( electricNeedlePrefab_ );
		electricNeedles_[ 0 ].transform.parent = player_.transform;
		electricNeedles_[ 0 ].setup( -playerSlideLine_.InnerRadius, 0.0f );
		electricNeedles_[ 1 ] = Instantiate<ElectricNeedle>( electricNeedlePrefab_ );
		electricNeedles_[ 1 ].transform.parent = player_.transform;
		electricNeedles_[ 1 ].setup( -playerSlideLine_.InnerRadius, 180.0f );

		setElectricPos( 0.0f );

		state_ = new Intro( this );
	}

	// 電気ニードル位置を設定
	//  rate: 位置( 0～1 ), 0:一番遠い, 1: 最接近
	void setElectricPos( float rate ) {
		if ( rate < 0.0f )
			rate = 0.0f;
		else if ( rate > 1.0f )
			rate = 1.0f;
		curElectricRate_ = rate;
		electricNeedles_[ 0 ].setRot( -90.0f * ( 1.0f + rate * 0.97f ) );
		electricNeedles_[ 1 ].setRot( 90.0f * ( 1.0f + rate * 0.97f ) );
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
		// setElectricPos( curElectricRate_ );
	}

	class Intro : State< GameCore > {
		public Intro( GameCore parent ) : base( parent ) {
		}
		protected override State innerUpdate() {
			return new Gaming( parent_ );
		}
	}

	class Gaming : State< GameCore > {
		public Gaming( GameCore parent ) : base( parent ) {

		}
		protected override State innerInit() {
			parent_.tower_.gameObject.SetActive( true );
			parent_.tower_.start();

			// ブロックグループを破壊した時の各種処理
			parent_.tower_.BreakBlocksCallback = ( colNum, rowNum, chainCount ) => {
				breakBlocks( colNum, rowNum, chainCount );
			};
			parent_.tower_.AllBlockDeletedCallback = () => {
				setNextState( new TowerClear( parent_ ) );
			};

			// すべて破壊したらタワークリアへ
			parent_.tower_.AllBlockDeletedCallback = () => {
				setNextState( new TowerClear( parent_ ) );
			};
			return null;
		}
		protected override State innerUpdate() {
			// 秒数進行
			curNeedleSec_ += Time.deltaTime;

			// 電気ニードル進行
			float rate = curNeedleSec_ / parent_.param_.electricNeedleSpeed_;
			parent_.setElectricPos( rate );

			// rate >= 1.0fでゲームオーバー
			if ( rate >= 1.0f )
				return new GameOver( parent_ );

			return this;
		}

		void breakBlocks( int colNum, int rowNum, int chainCount ) {
			Debug.Log( "break num: " + rowNum + ", chain: " + chainCount );
			parent_.scoreManager_.breakBlocks( colNum, rowNum, chainCount );

			// rateを戻す
			DeltaLerp.Float.easeOut( -curNeedleSec_, 1.0f, (_sec, _t, _dt, _delta) => {
				curNeedleSec_ += _delta;
				if ( curNeedleSec_ < 0.0f )
					curNeedleSec_ = 0.0f;
				return true;
			} );
		}

		float curNeedleSec_ = 0.0f;
	}

	class TowerClear : State< GameCore > {
		public TowerClear( GameCore parent ) : base( parent ) {
		}
		protected override State innerInit() {
			Debug.Log( "Tower clear!" );
			return null;
		}
		protected override State innerUpdate() {
			return this;
		}
	}

	class GameOver : State< GameCore > {
		public GameOver(GameCore parent) : base( parent ) {
		}
		protected override State innerInit() {
			parent_.player_.destroy();
			Debug.Log( "Game Over..." );
			return null;
		}
		protected override State innerUpdate() {
			return this;
		}
	}

	State state_;
	Param param_;
	Tower tower_;
	ElectricNeedle[] electricNeedles_ = new ElectricNeedle[2];
}
