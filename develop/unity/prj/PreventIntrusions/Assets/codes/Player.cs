using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー

public class Player : MonoBehaviour {

	// 初期化
	public void setup( Field field ) {
		field_ = field;
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
			// 上下左右のキー押し下げチェック
			KeyCode key = KeyCode.None;
			if ( Input.GetKey( KeyCode.LeftArrow ) == true && parent_.pos_.x > 0 ) {
				key = KeyCode.LeftArrow;
			} else if ( Input.GetKey( KeyCode.RightArrow ) == true && parent_.pos_.x + 1 < parent_.field_.getWidth() ) {
				key = KeyCode.RightArrow;
			} else if ( Input.GetKey( KeyCode.UpArrow ) == true && parent_.pos_.y + 1 < parent_.field_.getHeight() ) {
				key = KeyCode.UpArrow;
			} else if ( Input.GetKey( KeyCode.DownArrow ) == true && parent_.pos_.y > 0 ) {
				key = KeyCode.DownArrow;
			}
			if ( key != KeyCode.None ) {
				// フィールド上でのインタラクティブチェック
				// 自分の進行方向にバリケードがある？
				Barricade baricade = parent_.field_.getBarricadeOnCell( parent_.pos_, key );

				// バリケードが無ければ単純にそちらの方向へ移動する遷移へ
				if ( baricade == null ) {
					return new Move( parent_, key );
				}
			}
			return this;
		}
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
			GlobalState.time( 0.15f, (sec, t) => {
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
	Vector2Int pos_ = Vector2Int.zero;  // プレイヤーの整数座標
	Field field_;
}
