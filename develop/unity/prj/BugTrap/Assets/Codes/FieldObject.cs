using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールド上の「物」
public class FieldObject : MonoBehaviour
{
	// オブジェクトマネージャを登録
	public void setObjectManager( ObjectManager objectManager ) {
		objectManager_ = objectManager;
	}

	// 開始処理
	//  waitCountDown: カウントダウンフラグが下りるまで待機
	public void onStart(bool waitCountDown)
	{
		bWaitCountDown_ = waitCountDown;
		if (waitCountDown == true) {
			innerState_ = new CountDownIdle( this );
		} else {
			innerState_ = new Action( this );
		}
	}

	// Shapeグループを取得
	public ShapeGroup2D getShapeGroup() {
		return shapeGroup_;
	}

	// 更新エントリー
	// 派生クラスのUpdate関数内で呼びます
	protected void updateEntry() {
		if (bFinish_ == true)
			return;
		if ( innerState_ != null ) {
			innerState_ = innerState_.update();
		}
	}

	// 内部更新
	virtual protected void innerUpdate() {
	}

	// カウントダウンフラグを下す
	void onTriggerCountDown()
	{
		bWaitCountDown_ = true;
	}

	// カウントダウン待機中
	void countDownWait( System.Action finishCallback ) {
		GlobalState.start( () => {
			if (bWaitCountDown_ == true) {
				finishCallback();
				return false;
			}
			return true;
		} );
	}

	void Start() {
		if (innerState_ == null ) {
			innerState_ = new Action( this );
		}
	}

    void Update() {
		updateEntry();
    }

	protected ObjectManager objectManager_;
	protected ShapeGroup2D shapeGroup_ = new ShapeGroup2D();
	bool bWaitCountDown_ = false;
	bool bFinish_ = false;  // 寿命が尽きた？
	State innerState_;


	// カウントダウン終了待ち
	class CountDownIdle : State< FieldObject > {
		public CountDownIdle(FieldObject parent ) : base( parent ) {}
		protected override State innerInit() {
			parent_.countDownWait( () => {
				setNextState( new Action( parent_ ) );
			} );
			return this;
		}
	}

	// ゲーム中アクション
	class Action : State<FieldObject> {
		public Action(FieldObject parent) : base( parent ) { }
		protected override State innerUpdate()
		{
			parent_.innerUpdate();
			if ( parent_.bFinish_ == true ) {
				return null;
			}
			return this;
		}
	}
}
