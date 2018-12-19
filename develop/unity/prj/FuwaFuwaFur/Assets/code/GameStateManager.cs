using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理人

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    MainGame mainGame_;

    [SerializeField]
    TitleManager titleManager_;

    class Title : State
    {
        public Title( GameStateManager parent )
        {
            parent_ = parent;
        }
        // 内部初期化
        override protected void innerInit()
        {
            title_ = GameObject.Instantiate<TitleManager>( parent_.titleManager_ );
        }
        // 内部状態
        override protected State innerUpdate()
        {
            if ( title_.isFinish() == true ) {
                Destroy( title_.gameObject );
                return new GameStart( parent_ );
            }
            return this;
        }
        GameStateManager parent_;
        TitleManager title_;
    }

    class GameStart : State
    {
        public GameStart( GameStateManager parent )
        {
            parent_ = parent;
        }
        // 内部初期化
        override protected void innerInit()
        {
            mainGameObj_ = GameObject.Instantiate<MainGame>( parent_.mainGame_ );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( mainGameObj_.isFinish() == true ) {
                DestroyObject( mainGameObj_.gameObject );
                return new Title( parent_ );
            }
            return this;
        }
        GameStateManager parent_;
        MainGame mainGameObj_;
    }

    // Use this for initialization
    void Start () {
        state_ = new Title( this );
    }
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    State state_;
}
