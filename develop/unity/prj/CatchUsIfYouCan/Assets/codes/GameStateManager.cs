using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    Fader fader_;

    [SerializeField]
    GameManager gameManagerPrefab_;

    [SerializeField]
    Camera camera_;

    [SerializeField]
    UnityEngine.UI.Text keyInfo_;


    // Use this for initialization
    void Start() {
        state_ = new Title( this );
    }

    // Update is called once per frame
    void Update() {
        if ( state_ != null )
            state_ = state_.update();
    }

    // 発生させるエネミーの数を取得
    int getEmitEnemyNum()
    {
        if ( curStage_ >= emitEnemyNum_.Length )
            return emitEnemyNum_[ emitEnemyNum_.Length - 1 ];
        return emitEnemyNum_[ curStage_ ];
    }

    // ボスのLRスピードを取得
    float getBossLRSpeed() {
        if ( curStage_ >= bossLRSpeed_.Length )
            return bossLRSpeed_[ bossLRSpeed_.Length - 1 ];
        return bossLRSpeed_[ curStage_ ];
    }

    class StateBase : State
    {
        public StateBase( GameStateManager parent )
        {
            parent_ = parent;
        }
        protected GameStateManager parent_;
    }

    // タイトル
    class Title : StateBase
    {
        public Title(GameStateManager parent) : base( parent ) {
            parent_.keyInfo_.gameObject.SetActive( true );
            parent_.camera_.gameObject.SetActive( true );
        }

        // 内部初期化
        override protected void innerInit()
        {
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( Input.GetKey( KeyCode.Z ) == true ) {
                // 最初から
                parent_.keyInfo_.gameObject.SetActive( false );
                parent_.fader_.toIntro();
                return new Game( parent_ );
            } else if ( Input.GetKey( KeyCode.X ) == true ) {
                // 最後の面をリトライ
                parent_.keyInfo_.gameObject.SetActive( false );
                parent_.curStage_ = parent_.lastStage_;
                parent_.fader_.toIntro();
                return new Game( parent_ );
            }
            return this;
        }
    }

    // ゲーム
    class Game : StateBase
    {
        public Game(GameStateManager parent) : base( parent ) {
            nextState_ = this;
        }

        // 内部初期化
        override protected void innerInit()
        {
            manager_ = Instantiate<GameManager>( parent_.gameManagerPrefab_ );
            manager_.transform.localPosition = Vector3.zero;
            manager_.setEmitEnemyNum( parent_.getEmitEnemyNum() );
            manager_.setBossLRSpeed( parent_.getBossLRSpeed() );
            manager_.ResultCallback = (res) => {
                if ( res == GameManager.Result.Clear ) {
                    // 次のステージへ遷移
                    GlobalState.wait( 5.0f, () => {
                        parent_.fader_.toFadeOut( () => {
                            parent_.camera_.gameObject.SetActive( true );
                            Destroy( manager_.gameObject );
                            parent_.fader_.toIntro();
                            parent_.curStage_++;
                            nextState_ = new Game( parent_ );
                        } );
                        return false;
                    } );
                } else if ( res == GameManager.Result.GameOver ) {
                    // タイトルへ戻る
                    GlobalState.wait( 5.0f, () => {
                        parent_.fader_.toFadeOut( () => {
                            parent_.camera_.gameObject.SetActive( true );
                            Destroy( manager_.gameObject );
                            parent_.lastStage_ = parent_.curStage_;
                            parent_.curStage_ = 0;
                            nextState_ = new Title( parent_ );
                        } );
                        return false;
                    } );
                }
            };
            parent_.camera_.gameObject.SetActive( false );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return nextState_;
        }

        GameManager manager_;
        State nextState_;
    }

    State state_;
    int curStage_ = 0;
    int[] emitEnemyNum_ = new int[] { 2, 5, 12, 20, 30, 40, 50, 70, 100};
    float[] bossLRSpeed_ = new float[] { 65.0f, 95.0f, 120.0f, 150.0f, 175.0f, 195.0f, 210.0f, 230.0f, 250.0f};
    int lastStage_ = 0;
}
