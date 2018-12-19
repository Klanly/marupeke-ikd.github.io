using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理人

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    MainGame mainGame_;

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
                return null;
            }
            return this;
        }
        GameStateManager parent_;
        MainGame mainGameObj_;
    }

    // Use this for initialization
    void Start () {
        state_ = new GameStart( this );
    }
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    State state_;
}
