using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インゲーム管理人

public class GameManager : MonoBehaviour {

    class Parameter {

    }

    private void Awake() {
        siteEmitter_ = new SiteEmitter();
        siteEmitter_.EmitCallback = emitSite;
    }

    // Use this for initialization
    void Start () {
        state_ = new GameStart( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
        if ( siteEmitter_ != null )
            siteEmitter_.update();
    }

    // サイトエミット
    void emitSite() {
        Debug.Log( "Emit!!" );
    }

    class GameStart : State< GameManager > {
        public GameStart(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.siteEmitter_.setActive( true );
            return new SiteSelectWait( parent_ );
        }
    }

    //  サイト指定待ち状態
    //  サイトを指定 -> サイトのポジションへ
    class SiteSelectWait : State< GameManager > {
        public SiteSelectWait(GameManager parent) : base( parent ) { }
    }

    // サイトポジションへ移動
    //  サイト先へ到着 -> サイト処理へ
    class MoveToSite : State<GameManager> {
        public MoveToSite(GameManager parent) : base( parent ) { }
    }

    // サイト処理中
    //  別サイト選択 -> サイトポジションへ移動
    class SiteProc : State< GameManager > {
        public SiteProc(GameManager parent) : base( parent ) { }
    }

    State state_;
    SiteEmitter siteEmitter_;   // サイト発生者
}
