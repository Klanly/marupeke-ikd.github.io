using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 虫ベース

public class Bug : FieldObject
{
	[SerializeField]
	float startWaitSec_ = 0.0f;

	[SerializeField]
	float walkSpeed_ = 1.0f;	// 1秒で進むワールド距離

	private void Awake()　{
		state_ = new StartIdle( this );
	}

    void Update() {
		updateEntry();
	}

	// 内部更新
	override protected void innerUpdate() {
		if ( state_ != null ) {
			state_ = state_.update();
		}
	}

	// 歩き中
	protected virtual State<Bug> walk( State<Bug> state ) {
		float distPerFrame = walkSpeed_ / 60.0f;

		// 歩きを妨げる物とのコリジョンをチェック
		List< FieldObject > collideObjects = null;
		if ( 
			checkWalkCollision( transform.position, transform.forward, distPerFrame, out collideObjects ) == true &&
			onCollideWalkAvoidObject( collideObjects, ref state ) == true
		) {
			// 妨げる物があったので歩みを止める
		} else {
			// ちょっと進む
			var p =transform.position;
			p += transform.forward * distPerFrame;
			transform.position = p;
		}
		return state;
	}

	// 歩いている最中に妨げる物に当たったかチェック
	protected virtual bool checkWalkCollision( Vector3 pos, Vector3 forward, float advanceDist, out List< FieldObject > collideObjcts ) {
		if (objectManager_ == null) {
			collideObjcts = null;
			return false;
		}
		collideObjcts = objectManager_.checkCollide( this );
		return collideObjcts.Count > 0;
	}

	// 歩いている時に障害物に当たった
	//  戻り値：歩みを止める必要がある場合はtrue
	protected virtual bool onCollideWalkAvoidObject( List< FieldObject > collideObjects, ref State<Bug> state ) {
		// 状況に応じてstate_を更新
		return false;
	}

	protected State state_;


	// スタート時アイドル
	protected class StartIdle : State< Bug > {
		public StartIdle(Bug parent) : base( parent ) {}
		protected override State innerInit() {
			GlobalState.wait( parent_.startWaitSec_, () => {
				setNextState( new Walk( parent_ ) );
				return false;
			} );
			return this;
		}
	}

	// 歩行中
	protected class Walk : State< Bug > {
		public Walk(Bug parent) : base( parent ) { }
		protected override State innerUpdate() {
			return parent_.walk( this );
		}
	}
}
