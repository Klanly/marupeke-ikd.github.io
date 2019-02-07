using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理人
public class GameManager : MonoBehaviour {

    [SerializeField]
    GimicLayoutGenerator generator_ = null;

    [SerializeField]
    HandlerOperator handler_;

    [SerializeField]
    GameObject cameraTopTimerPos_;

    [SerializeField]
    GameObject cameraCenterTimerPos_;

    [SerializeField]
    GameObject gameOverImage_;

    [SerializeField]
    UnityEngine.UI.Image missionCompleteImage_;

    [SerializeField]
    UIFader fader_;

    [SerializeField]
    bool bUseRandomSeed_ = true;

    // ゲーム終了
    public System.Action AllFinishCallback { set { allFinishCallback_ = value; } }

    void allFinish()
    {
        if ( allFinishCallback_ != null )
            allFinishCallback_();

        UnityEngine.SceneManagement.SceneManager.LoadScene( "main" );
    }

    private void Awake()
    {
        fader_.gameObject.SetActive( true );
    }

    void Start () {
        state_ = new Setup( this );
        handler_.setActive( true );
    }
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    void fader( Color color, float sec, System.Action finishCallback )
    {
        fader_.fade( color, sec, finishCallback );
    }

    class DataSet
    {
        public BombBox bombBox_;
        public LayoutSpec spec_ = new LayoutSpec();
        public GimicSpec gimicSpec_ = new GimicSpec();
    }

    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    class Setup : StateBase
    {
        enum Result
        {
            None,
            Success,
            Failure,
        }

        public Setup(GameManager parent) : base( parent ) {
        }

        protected override State innerInit()
        {
            // データ生成
            if ( parent_.bUseRandomSeed_ == true ) {
                parent_.dataSet_.spec_.seed_ = Random.Range( 0, 10000 );
                parent_.dataSet_.spec_.gimicBoxSeed_ = Random.Range( 0, 10000 );
                parent_.dataSet_.spec_.gimicSeed_ = Random.Range( 0, 10000 );
                parent_.dataSet_.spec_.trapSeed_ = Random.Range( 0, 10000 );
            }
            parent_.generator_.create( parent_.dataSet_.spec_, parent_.dataSet_.gimicSpec_, out parent_.dataSet_.bombBox_ );
            parent_.dataSet_.bombBox_.transform.parent = parent_.transform;

            // 成功を受ける
            parent_.dataSet_.bombBox_.AllDiactiveDoneCallback = () => {
                result_ = Result.Success;
            };

            // 失敗を受ける
            parent_.dataSet_.bombBox_.FailureCallback = () => {
                result_ = Result.Failure;
            };

            return null;
        }

        protected override State innerUpdate()
        {
            if ( result_ == Result.Success ) {
                return new Success( parent_ );
            } else if ( result_ == Result.Failure ) {
                return new Failure( parent_ );
            }
            return this;
        }

        Result result_ = Result.None;
    }

    class Success : StateBase
    {
        public Success(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.dataSet_.bombBox_.stopTimer();

            // 成功文字出し
            Color c = parent_.missionCompleteImage_.color;
            Color startC = c;
            startC.a = 0.0f;
            parent_.missionCompleteImage_.color = startC;
            parent_.missionCompleteImage_.gameObject.SetActive( true );
            GlobalState.time( 1.5f, (sec, t) => {
                parent_.missionCompleteImage_.color = Color.Lerp( startC, c, t );
                return true;
            } ).finish( () => {
                bFinish_ = true;
            } );
            Debug.Log( "All diactive effect on GameManager." );
            return null;
        }

        protected override State innerUpdate()
        {
            if ( bFinish_ == true ) {
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    parent_.allFinish();
                    return null;
                }
            }
            return this;
        }

        bool bFinish_ = false;
    }

    class Failure : StateBase
    {
        public Failure(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            // タイマー早回し
            parent_.dataSet_.bombBox_.advanceTimer( () => {
                // ゼロになったのを受けて爆発演出
                nextState_ = new Explosion( parent_ );
            } );

            // フロントが開いていたらカメラをフロントへ
            // 開いていなければ箱のトップへ移動
            var endPos = parent_.cameraTopTimerPos_.transform.position;
            var endQ = parent_.cameraTopTimerPos_.transform.rotation;
            if ( parent_.dataSet_.bombBox_.isOpenFrontPanel() == true ) {                
                endPos = parent_.cameraCenterTimerPos_.transform.position;
                endQ = parent_.cameraCenterTimerPos_.transform.rotation;
            }
            var startPos = Camera.main.transform.position;
            var startQ = Camera.main.transform.rotation;
            GlobalState.time( 1.0f, (sec, t) => {
                Camera.main.transform.position = Lerps.Vec3.easeInOut( startPos, endPos, t );
                Camera.main.transform.rotation = Lerps.Quaternion.easeInOut( startQ, endQ, t );
                return true;
            } );
            Debug.Log( "Failure effect on GameManager." );
            return null;
        }

        protected override State innerUpdate()
        {
            if ( nextState_ != null )
                return nextState_;
            return this;
        }

        State nextState_;
    }

    class Explosion : StateBase
    {
        public Explosion(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            // フェーダーを急激に真っ白に
            // そして黒へ -> ゲームオーバー表示
            parent_.fader( Color.white, 0.6f, () => {
                GlobalState.time( 0.6f, (_, _2) => {
                    return true;
                } ).finish( () => {
                    parent_.fader( Color.black, 0.1f, () => {
                        parent_.gameOverImage_.SetActive( true );
                    } );
                } );
                GlobalState.time( 3.0f, (sec, t) => { return true; } )
                .finish( () => {
                    bFinish_ = true;
                } );
            } );
            return null;
        }

        protected override State innerUpdate()
        {
            if ( bFinish_ == true ) {
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    parent_.allFinish();
                    return null;
                }
            }
            return this;
        }

        bool bFinish_ = false;
    }

    DataSet dataSet_ = new DataSet();
    State state_;
    System.Action allFinishCallback_;
}
