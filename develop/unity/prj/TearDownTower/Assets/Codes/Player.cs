using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	float radius_;

	[SerializeField]
	Camera camera_;

	[SerializeField]
	GameObject playerModel_;

	[SerializeField]
	ParticleSystem particle_L;

	[SerializeField]
	ParticleSystem particle_R;

	[SerializeField]
	GameObject detonate_;

	public float Radius { get { return radius_; } }

	public class Param {
		public float cameraRefDist_ = 4.0f;
		public float radius_ = 7.0f;
		public float transSec_ = 0.25f;
	}

	public void setup( Param param, Tower tower ) {
		tower_ = tower;
		unitDeg_ = 360.0f / tower.getParam().colNum_;
		unitRad_ = unitDeg_ * Mathf.Deg2Rad;
		param_ = param;
		radius_ = param.radius_;
		blockHeight_ = tower.getParam().blockHeight_;

		// プレイヤー位置
		playerModel_.transform.localPosition = new Vector3( 0.0f, 0.0f, -radius_ );

		// カメラ位置
		var cpos = camera_.transform.position;
		camera_.transform.position = cpos.normalized * ( radius_ + param.cameraRefDist_ );

		state_ = new Active( this );
	}

	// プレイヤー破壊
	public void destroy() {
		bDestroyed_ = true;
		state_ = new PlayerDestroy( this );
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class Active : State<Player> {
		public Active( Player parent ) : base( parent ) {
		}

		protected override State innerUpdate() {
			// 左右キーで移動
			if ( bMovingLR_ == false ) {
				if ( Input.GetKeyDown( KeyCode.LeftArrow ) == true ) {
					bMovingLR_ = true;
					curPosIdx_ = ( curPosIdx_ + parent_.tower_.getColNum() - 1 ) % parent_.tower_.getColNum();
					var init = parent_.transform.localRotation;
					var end = Quaternion.Euler( 0.0f, parent_.unitDeg_, 0.0f ) * init;
					GlobalState.time( parent_.param_.transSec_, (sec, t) => {
						parent_.transform.localRotation = Lerps.Quaternion.linear( init, end, t );
						return true;
					} ). finish(()=> {
						parent_.transform.localRotation = end;
						bMovingLR_ = false;
					} );
					parent_.particle_R.Play();
				} else if ( Input.GetKeyDown( KeyCode.RightArrow ) == true ) {
					bMovingLR_ = true;
					curPosIdx_ = ( curPosIdx_ + 1 ) % parent_.tower_.getColNum();
					var init = parent_.transform.localRotation;
					var end = Quaternion.Euler( 0.0f, -parent_.unitDeg_, 0.0f ) * init;
					GlobalState.time( parent_.param_.transSec_, (sec, t) => {
						parent_.transform.localRotation = Lerps.Quaternion.linear( init, end, t );
						return true;
					} ).finish(()=> {
						parent_.transform.localRotation = end;
						bMovingLR_ = false;
					} );
					parent_.particle_L.Play();
				}
			}
			// 上下キーでタワーの上下に移動
			if ( bMovingUD_ == false ) {
				if ( Input.GetKey( KeyCode.UpArrow ) == true ) {
					if ( curHeightPos_ + 1 < parent_.tower_.getCurMaxHeight() ) {
						curHeightPos_++;
						bMovingUD_ = true;
						var init = parent_.transform.localPosition;
						var end = new Vector3( 0.0f, parent_.blockHeight_, 0.0f ) + init;
						GlobalState.time( parent_.param_.transSec_, (sec, t) => {
							parent_.transform.localPosition = Lerps.Vec3.linear( init, end, t );
							return true;
						} ).finish( () => {
							parent_.transform.localPosition = end;
							bMovingUD_ = false;
						} );
					}
				} else if ( Input.GetKey( KeyCode.DownArrow ) == true ) {
					if ( curHeightPos_ > 0 ) {
						curHeightPos_--;
						bMovingUD_ = true;
						var init = parent_.transform.localPosition;
						var end = new Vector3( 0.0f, -parent_.blockHeight_, 0.0f ) + init;
						GlobalState.time( parent_.param_.transSec_, (sec, t) => {
							parent_.transform.localPosition = Lerps.Vec3.linear( init, end, t );
							return true;
						} ).finish( () => {
							parent_.transform.localPosition = end;
							bMovingUD_ = false;
						} );
					}
				}
			}
			// [z]で(curPosIdx_, curHeightPos_)ブロックへ弾発射
			if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
				parent_.tower_.insertBlock( curPosIdx_, curHeightPos_ );
			}
			return this;
		}

		bool bMovingLR_ = false;
		bool bMovingUD_ = false;
		int curHeightPos_ = 0;
		int curPosIdx_ = 0;
	}

	class PlayerDestroy : State<Player> {
		public PlayerDestroy( Player parent ) : base( parent ) {
		}
		protected override State innerInit() {
			parent_.detonate_.transform.parent = null;
			parent_.detonate_.SetActive( true );
			parent_.playerModel_.gameObject.SetActive( false );
			return null;
		}
	}

	State state_;
	Tower tower_;
	Param param_;
	float unitRad_;
	float unitDeg_;
	float rad_;
	float blockHeight_;
	bool bDestroyed_ = false;
}
