using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー

public class Player : MonoBehaviour {

	public class Param {
		public Vector2Int initPos_;			// 初期位置
		public float moveSec_ = 0.30f;		// 1セル分移動する時の移動時間
		public float rotSec_ = 0.15f;		// 方向転換時間
		public float pushWaitSec_ = 0.15f;  // バリケードを押し始めるまでの待ち時間
		public float pushSec_ = 0.20f;		// バリケードを押す時間
		public float pullWaitSec_ = 0.15f;  // バリケードを引き始めるまでの待ち時間
		public float pullSec_ = 0.20f;      // バリケードを引く時間
	}

	// 初期化
	public void setup( Field field, Param param ) {
		field_ = field;
		param_ = param;
		setPos( param.initPos_ );
	}

	// プレイヤーの整数位置を変更
	public void setPos( Vector2Int pos ) {
		pos_ = pos;
		transform.localPosition = new Vector3( 0.5f + pos.x, 0.0f, 0.5f + pos.y );
	}

	// プレイヤーの整数位置を取得
	public Vector2Int getPos() {
		return pos_;
	}

	// プレイヤーの向いている方向
	KeyCode getDir() {
		return dir_;
	}

	// Use this for initialization
	void Start () {
		state_ = new Alive( this );
	}
	
	// Update is called once per frame
	void Update () {
		var preState = state_;
		if ( state_ != null ) {
			state_ = state_.update();
			if ( state_ != preState )
				Update();
		}
	}

	// 生存中のステート
	class Alive : State<Player> {
		public Alive(Player parent) : base( parent ) {
			state_ = new Idle( parent );
		}
		protected override State innerUpdate() {
			// 死亡判定
			if ( isDead() == true )
				return new Dead( parent_ );

			if ( state_ != null )
				state_ = state_.update();

			return this;
		}

		bool isDead() {
			return false;	// 後で
		}

		State state_;
	}

	// 死亡ステート
	class Dead : State<Player> {
		public Dead( Player parent ) : base( parent ) {
		}
		protected override State innerInit() {
			return this;
		}
	}

	// キーを押していない待機中
	class Idle : State<Player> {
		public Idle(Player parent) : base( parent ) { }
		protected override State innerUpdate() {
			// [Z]キー押し下げ
			//  目の前にバリケードがある場合Holdへ
			if ( Input.GetKey( KeyCode.Z ) == true ) {
				var dir = parent_.getDir();
				Vector2Int elem = Vector2Int.zero;
				var barricade = parent_.field_.getBarricadeOnCell( parent_.pos_, dir, ref elem );
				if ( barricade != null ) {
					return new BarricadeHold( parent_, dir );
				}
			}

			// 上下左右へ方向転換
			KeyCode key = KeyCode.None;
			if ( KeyHelper.getArrow( ref key ) == true ) {
				var preKey = parent_.dir_;
				parent_.dir_ = key;

				// 回転は確定
				float rotDeg = KeyHelper.arrowDeg( preKey, key );
				DeltaLerp.Float.easeInOut( rotDeg, parent_.param_.rotSec_, (sec, t, dt, delta) => {
					parent_.transform.localRotation = Quaternion.Euler( 0.0f, delta, 0.0f ) * parent_.transform.localRotation;
					return true;
				} );

				// 指定方向へ移動可能なら移動する //

				// 自分の進行方向が有効で且つバリケードが無い？
				var moveDir = KeyHelper.offset( key );
				var nextPos = parent_.pos_ + moveDir;
				Vector2Int elem = Vector2Int.zero;
				if (
					parent_.field_.getBarricadeOnCell( parent_.pos_, key, ref elem ) == null &&
					parent_.field_.isValidateCoord( nextPos ) == true )
				{
					return new Move( parent_, key );
				}
			}

			return this;
		}
	}

	// バリケードを掴む
	class BarricadeHold : State< Player > {
		public BarricadeHold(Player parent, KeyCode key ) : base( parent ) {
			key_ = key;
		}
		protected override State innerUpdate() {
			// [Z]を離したらIdleへ戻る
			if ( Input.GetKey( KeyCode.Z ) == false ) {
				return new Idle( parent_ );
			}

			// key方向へ入れている場合
			//  一つ先にスペースがあり、バリケードが無ければ押しモードへ
			var nextPushPos = parent_.pos_ + KeyHelper.offset( key_ );
			Vector2Int elem = Vector2Int.zero;
			if (
				Input.GetKey( key_ ) == true &&
				parent_.field_.isSpace( nextPushPos ) &&
				parent_.field_.getBarricadeOnCell( nextPushPos, key_, ref elem ) == null
			) {
				return new BarricadePushReady( parent_, key_ );
			}

			// keyと逆方向を入れている場合	
			//  後ろにスペースがあれば引きモードへ
			var invKey = KeyHelper.invKey( key_ );
			var nextPullPos = parent_.pos_ + KeyHelper.offset( invKey );
			if (
				Input.GetKey( invKey ) == true &&
				parent_.field_.isSpace( nextPullPos ) &&
				parent_.field_.getBarricadeOnCell( parent_.pos_, invKey, ref elem ) == null
			) {
				return new BarricadePullReady( parent_, invKey );
			}
			return this;
		}

		KeyCode key_;
	}

	// バリケード押しモード発動中
	// （押し先にバリケードが無い事は確認済み）
	class BarricadePushReady : State< Player > {
		public BarricadePushReady(Player parent, KeyCode dir ) : base( parent ) {
			dir_ = dir;
		}
		protected override State innerUpdate() {
			// [Z]を離したらIdleへ戻る
			if ( Input.GetKey( KeyCode.Z ) == false ) {
				return new Idle( parent_ );
			}

			// keyを離したらHoldへ
			if ( Input.GetKey( dir_ ) == false ) {
				return new BarricadeHold( parent_, dir_ );
			}

			// keyを押し続けているので、一つ先にスペースがあれば時間を進める
			var nextPushPos = parent_.pos_ + KeyHelper.offset( dir_ );
			if ( parent_.field_.isSpace( nextPushPos ) == true ) {
				t_ += Time.deltaTime;
			}

			// 押し続けている時間が指定時間を超えたら押し成立
			if ( t_ >= parent_.param_.pushWaitSec_ ) {
				return new BarricadePush( parent_, dir_ );
			}
			return this;
		}

		float t_ = 0.0f;
		KeyCode dir_;
	}

	class BarricadePush : State< Player > {
		public BarricadePush(Player parent, KeyCode dir ) : base( parent ) {
			dir_ = dir;
		}
		protected override State innerInit() {
			// 指定時間バリケードを一マス押す
			if ( parent_.field_.moveBarricade( parent_.pos_, dir_, dir_, parent_.param_.pushSec_ ) == false ) {
				setNextState( new Idle( parent_ ) );    // ???
				return this;
			}
			// 一緒にプレイヤーも動かす
			// 指定の方向に単純移動
			var dir = KeyHelper.offset( dir_ );
			Vector3 len = new Vector3( 1.0f * dir.x, 0.0f, 1.0f * dir.y );
			var pos = parent_.transform.localPosition;
			GlobalState.time( parent_.param_.pushSec_, (sec, t) => {
				parent_.transform.localPosition = pos + Lerps.Vec3.linear( Vector3.zero, len, t );
				return true;
			} ).finish( () => {
				parent_.setPos( parent_.getPos() + dir );
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
		KeyCode dir_;
	}

	// バリケード引きモード発動中
	// （後ろに移動可能なのは確認済み）
	class BarricadePullReady : State<Player> {
		public BarricadePullReady(Player parent, KeyCode dir) : base( parent ) {
			dir_ = dir;
		}
		protected override State innerUpdate() {
			// [Z]を離したらIdleへ戻る
			if ( Input.GetKey( KeyCode.Z ) == false ) {
				return new Idle( parent_ );
			}

			// keyを離したらHoldへ
			if ( Input.GetKey( dir_ ) == false ) {
				return new BarricadeHold( parent_, dir_ );
			}

			// keyを押し続けているので、一つ先にスペースがあれば時間を進める
			var nextPullPos = parent_.pos_ + KeyHelper.offset( dir_ );
			if ( parent_.field_.isSpace( nextPullPos ) == true ) {
				t_ += Time.deltaTime;
			}

			// 引き続けている時間が指定時間を超えたら押し成立
			if ( t_ >= parent_.param_.pullWaitSec_ ) {
				return new BarricadePull( parent_, dir_ );
			}
			return this;
		}

		float t_ = 0.0f;
		KeyCode dir_;
	}

	class BarricadePull : State<Player> {
		public BarricadePull(Player parent, KeyCode dir) : base( parent ) {
			dir_ = dir;
		}
		protected override State innerInit() {
			// 指定時間バリケードを一マス引く
			if ( parent_.field_.moveBarricade( parent_.pos_, KeyHelper.invKey( dir_ ), dir_, parent_.param_.pullSec_ ) == false ) {
				setNextState( new Idle( parent_ ) );    // ???
				return this;
			}
			// 一緒にプレイヤーも動かす
			// 指定の方向に単純移動
			var dir = KeyHelper.offset( dir_ );
			Vector3 len = new Vector3( 1.0f * dir.x, 0.0f, 1.0f * dir.y );
			var pos = parent_.transform.localPosition;
			GlobalState.time( parent_.param_.pullSec_, (sec, t) => {
				parent_.transform.localPosition = pos + Lerps.Vec3.linear( Vector3.zero, len, t );
				return true;
			} ).finish( () => {
				parent_.setPos( parent_.getPos() + dir );
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
		KeyCode dir_;
	}

	class Move : State<Player> {
		public Move(Player parent, KeyCode key ) : base( parent ) {
			key_ = key;
			switch ( key_ ) {
				case KeyCode.LeftArrow: x_ = -1.0f; ix_ = -1; break;
				case KeyCode.RightArrow: x_ = 1.0f; ix_ = 1; break;
				case KeyCode.DownArrow: z_ = -1.0f; iy_ = -1; break;
				case KeyCode.UpArrow: z_ = 1.0f; iy_ = 1; break;
			}
		}
		protected override State innerInit() {
			// 指定の方向に単純移動
			Vector3 len = new Vector3( 1.0f * x_, 0.0f, 1.0f * z_ );
			var pos = parent_.transform.localPosition;
			GlobalState.time( parent_.param_.moveSec_, (sec, t) => {
				parent_.transform.localPosition = pos + Lerps.Vec3.linear( Vector3.zero, len, t );
				return true;
			} ).finish(()=> {
				parent_.setPos( parent_.getPos() + new Vector2Int( ix_, iy_ ) );
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
		KeyCode key_;
		float x_ = 0.0f;
		float z_ = 0.0f;
		int ix_ = 0;
		int iy_ = 0;
	}

	State state_;
	Param param_;
	Vector2Int pos_ = Vector2Int.zero;  // プレイヤーの整数座標
	KeyCode dir_ = KeyCode.DownArrow;	// 向いている方向
	Field field_;
}
