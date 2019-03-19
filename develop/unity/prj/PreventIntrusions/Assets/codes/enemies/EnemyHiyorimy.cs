using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ヒヨリミー
//
//  日和見的にフラフラと動く一番弱っちい敵

public class EnemyHiyorimy : Enemy {

	void Start () {
		state_ = new Wait( this );
	}
	
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	// 囲まれているかチェック
	public override bool checkStockade( int[,] floorIds, List<bool> compFlag ) {
		int id = floorIds[ Pos.x, Pos.y ];
		return compFlag[ id ];
	}

	// ボーっとしている
	class Wait : State< EnemyHiyorimy > {
		public Wait( EnemyHiyorimy parent ) : base( parent ) {
		}
		protected override State innerInit() {
			float waitSec = Random.Range( 0.5f, 1.0f );
			GlobalState.time( waitSec, (sec, t) => {
				// TODO: 何かIdleモーションを
				return true;
			} ).finish(()=> {
				setNextState( new Move( parent_ ) );
			} );
			return this ;
		}
	}

	// 動けるところに一歩動く
	class Move : State< EnemyHiyorimy > {
		public Move(EnemyHiyorimy parent) : base( parent ) {
		}
		protected override State innerInit() {
			// 周囲をチェック
			Vector2Int elem = Vector2Int.zero;
			List<KeyCode> keys = new List<KeyCode>();
			foreach( var key in KeyHelper.ArrowList ) {
				var offset = KeyHelper.offset( key );
				if (
					parent_.field_.isSpace( parent_.Pos + offset ) &&
					parent_.field_.isValidateCoord( parent_.Pos + offset ) == true &&
					parent_.field_.getBarricadeOnCell( parent_.Pos, key, ref elem ) == false
				) {
					keys.Add( key );
				}
			}
			if ( keys.Count == 0 ) {
				// どこにも行けないのでWaitへ
				return new Wait( parent_ );
			}
			KeyCode nextArrow = keys[ Random.Range( 0, keys.Count ) ];
			var dir = KeyHelper.offset( nextArrow );
			var prePos = parent_.Pos;
			var endPos = prePos + dir;
			var prePos3 = parent_.transform.localPosition;
			var endPos3 = prePos3 + new Vector3( 1.0f * dir.x, 0.0f, 1.0f * dir.y );
			parent_.setPos( endPos );  // 行先は先に確定
			GlobalState.time( 0.4f, (sec, t) => {
				if ( parent_ == null )
					return false;
				parent_.transform.localPosition = Lerps.Vec3.easeInOut( prePos3, endPos3, t );
				return true;
			} ).finish(()=> {
				if ( parent_ != null )
					setNextState( new Wait( parent_ ) );
			} );
			return this;
		}
	}

	State state_;
}
