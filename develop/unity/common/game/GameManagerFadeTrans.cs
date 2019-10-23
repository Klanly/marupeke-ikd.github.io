using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フェード処理付きゲーム管理人

public class GameManagerFadeTrans : GameManagerBase {

	// 初期化
	protected void initialize() {
		fadeState_ = new PreFadeIn( this );
	}

	// フェード前初期化処理
	virtual protected void preFadeInitialize( System.Action finishCallback ) {
		// 派生クラスで必要な初期化をしてコールバックを呼ぶとフェードインする
		finishCallback();
	}

	// メインゲームフィニッシュ通知
	protected void callMainGameFinish() {
		bMainGameFinish_ = true;
		state_ = null;
	}

	// 状態更新
	override protected void stateUpdate()
	{
		if (fadeState_ != null) {
			fadeState_ = fadeState_.update();
		}
	}

	void Start() {
	}

    void Update()
    {
        
    }

	bool bMainGameFinish_ = false;
	State fadeState_;

	// フェードイン前の初期化作業
	class PreFadeIn : State< GameManagerFadeTrans > {
		public PreFadeIn( GameManagerFadeTrans parent ) : base( parent ) { }
		protected override State innerInit() {
			FaderManager.getInstance();
			return base.innerInit();
		}
	}

	// メイン状態再生
	class PlayMain : State< GameManagerFadeTrans > {
		public PlayMain(GameManagerFadeTrans parent) : base( parent ) { }
		protected override State innerUpdate()
		{
			if ( parent_.state_ != null )
				parent_.state_ = parent_.state_.update();

			// メイン遷移が自己終了宣言をしたらフェードアウトへ
			if ( parent_.bMainGameFinish_ == true ) {
				return new FadeOut( parent_ );
			}

			return base.innerUpdate();
		}
	}

	// フェードアウト
	class FadeOut : State<GameManagerFadeTrans> {
		public FadeOut(GameManagerFadeTrans parent) : base( parent ) { }
		protected override State innerInit()
		{

			return base.innerInit();
		}
	}
}
