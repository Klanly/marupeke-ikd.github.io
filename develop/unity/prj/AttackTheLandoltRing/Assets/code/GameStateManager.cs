using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    TitleManager titleManager_; // タイトル

    [SerializeField]
    GameManager gameManager_;   // ゲーム本体

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



    // タイトル
    class Title : State
    {
        public Title( GameStateManager manager )
        {
            manager_ = manager;
        }

        // 内部初期化
        override protected void innerInit()
        {
            titleManager_ = GameObject.Instantiate<TitleManager>( manager_.titleManager_, Vector3.zero, Quaternion.identity );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( titleManager_.isFinish() == true ) {
                GameObject.Destroy( titleManager_.gameObject );
                return new Game( manager_ );
            }
            return this;
        }

        GameStateManager manager_;
        TitleManager titleManager_;
    }


    // ゲーム
    class Game : State
    {
        public Game( GameStateManager manager )
        {
            manager_ = manager;
            gameManager_ = GameObject.Instantiate< GameManager >( manager.gameManager_, Vector3.zero, Quaternion.identity );
        }

        // 内部初期化
        override protected void innerInit()
        {

        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( gameManager_.isFinishGame() == true ) {
                GameObject.Destroy( gameManager_.gameObject );
                return new Title( manager_ );
            }
            return this;
        }

        GameStateManager manager_;
        GameManager gameManager_;
    }
}
