using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    SunManager sunManager_;

    [SerializeField]
    HumanRule humanRule_;

    [SerializeField]
    ShipRule shipRule_;

    [SerializeField]
    Bridge[] bridges_;

    [SerializeField]
    ZoomInCamera zoomInCamera_;

    [SerializeField]
    GameObject bridgeButtons_;

    [SerializeField]
    UnityEngine.UI.Button[] oneShotButtons_;

    [SerializeField]
    HoldButton[] holdButtons_;

    [SerializeField]
    PassengerFactory passengerFactory_;

    [SerializeField]
    UnityEngine.UI.Image shipWarningUpImage_;

    [SerializeField]
    UnityEngine.UI.Image shipWarningDownImage_;

    [SerializeField]
    UnityEngine.UI.Image gameOverImage_;

    [SerializeField]
    bool debugCreateHuman_ = false;

    [SerializeField]
    Human.Type debugHumanType_ = Human.Type.Human_Walk;

    [SerializeField]
    bool debugCreateShip_ = false;

    [SerializeField]
    int debugCreateLine_ = 0;

    [SerializeField]
    bool debugEmitActive_ = true;

    // 太陽マネージャを取得
    public SunManager getSunManager()
    {
        return sunManager_;
    }

    // 衝突している橋を取得
    public Bridge getCollideBridge( Vector3 pos )
    {
        foreach ( var b in bridges_ ) {
           if ( b.isCollide( pos ) == true ) {
                return b;
            }
        }
        return null;
    }

    // 船接近警告をON
    public void warningShipApproaching( bool isUpper )
    {
        if (isUpper == true) {
            shipWarningUpBrinker_.reset();
            shipWarningUpBrinker_.setActive( true );
        } else {
            shipWarningDownBrinker_.reset();
            shipWarningDownBrinker_.setActive( true );
        }
    }

    private void Awake()
    {
        humanRule_.setup( this );
        shipRule_.setup( this );
    }

    // Use this for initialization
    void Start () {
        // ボタンと橋の挙動を関連付け
        oneShotButtons_[ 0 ].onClick.AddListener( () => {
            bridges_[ 0 ].switchOn();
        } );
        holdButtons_[ 0 ].OnPush = () => {
            bridges_[ 1 ].switchOn();
        };
        oneShotButtons_[ 1 ].onClick.AddListener( () => {
            bridges_[ 2 ].switchOn();
        } );
        holdButtons_[ 1 ].OnPush = () => {
            bridges_[ 3 ].switchOn();
        };

        // 船接近警告イメージ
        shipWarningUpBrinker_.setup( shipWarningUpImage_, 0.25f, 0.10f, 10, true, false );
        shipWarningDownBrinker_.setup( shipWarningDownImage_, 0.25f, 0.10f, 10, true, false );

        // 発生ルール関連付け
        humanRule_.WalkerEmmitCallback = () => {
            if ( debugEmitActive_ == false )
                return;
            var human = passengerFactory_.create( Passenger.Type.Human_Walk );
            emmitHuman( human as Human );
        };
        humanRule_.RunnerEmmitCallback = () => {
            if ( debugEmitActive_ == false )
                return;
            var human = passengerFactory_.create( Passenger.Type.Human_Run );
            emmitHuman( human as Human );
        };
        shipRule_.EmmitCallback = () => {
            if ( debugEmitActive_ == false )
                return;
            var ship = passengerFactory_.create( Passenger.Type.Ship );
            ship.setup( this );
            ship.OnMiss = ( bridgeIdx ) => {
                // ゲームオーバーへ
                toGameOver( bridgeIdx );
            };
        };

        // 橋オーバーヒート検出
        foreach ( var br in bridges_ ) {
            br.OnMiss = ( bridgeIdx ) => {
                // ゲームオーバーへ
                toGameOver( bridgeIdx );
            };
        }
    }

    void emmitHuman( Human human )
    {
        if ( human == null )
            return;

        human.setup( this );
        float posZ = Random.Range( -2.0f, 2.0f );
        float posX = -60.0f;
        if ( Random.Range( 0, 2 ) != 0 ) {
            var h = human as Human;
            h.setMoveDir( false );
            posX = 60.0f;
            posZ -= 30.0f;
        }
        var p = human.transform.position;
        p.x = posX;
        p.z = posZ;
        human.transform.position = p;

        human.OnMiss = ( bridgeIndex ) => {
            // ゲームオーバーへ
            toGameOver( bridgeIndex );
        };
    }

    void debugUpdate()
    {
        if ( debugCreateHuman_ == true ) {
            debugCreateHuman_ = false;
            var human = passengerFactory_.create( debugHumanType_ );
            human.setup( this );
            float posZ = Random.Range( -2.0f, 2.0f );
            float posX = -60.0f;
            if ( debugCreateLine_ != 0 ) {
                var h = human as Human;
                h.setMoveDir( false );
                posX = 60.0f;
                posZ -= 30.0f;
            }
            var p = human.transform.position;
            p.x = posX;
            p.z = posZ;
            human.transform.position = p;
        }
        if ( debugCreateShip_ == true ) {
            debugCreateShip_ = false;
            var ship = passengerFactory_.create( Passenger.Type.Ship );
            ship.setup( this );
        }
    }

    // Update is called once per frame
    void Update () {

        debugUpdate();

        // 船接近警告ブリンカー
        shipWarningUpBrinker_.update();
        shipWarningDownBrinker_.update();

        if ( state_ != null )
            state_ = state_.update();
    }

    // ゲームオーバー処理
    void toGameOver( int bridgeIndex )
    {
        if ( bGameOver_ == false ) {
            bGameOver_ = true;
            state_ = new State_CameraZoomIn( this, bridgeIndex );
        }
    }

    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }
    class State_CameraZoomIn : StateBase
    {
        public State_CameraZoomIn(GameManager parent, int bridgeIndex ) : base( parent ) {
            bridgeIndex_ = bridgeIndex;
        }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.zoomInCamera_.setup( bridgeIndex_ );
            parent_.zoomInCamera_.OnFinishMove = () => {
                bFinishCameraZoom_ = true;
            };
            parent_.bridgeButtons_.SetActive( false );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinishCameraZoom_ == true )
                return new State_GameOverWait( parent_ );
            return this;
        }

        int bridgeIndex_ = 0;
        bool bFinishCameraZoom_ = false;
    }

    class State_GameOverWait : StateBase
    {
        public State_GameOverWait(GameManager parent) : base( parent ) { }
        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            if ( t_ >= 1.0f )
                return new State_GameOver( parent_ );
            return this;
        }
        float t_ = 0.0f;
    }

    class State_GameOver : StateBase
    {
        public State_GameOver( GameManager parent ) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            parent_.gameOverImage_.gameObject.SetActive( true );
        }
        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            if ( t_ >= 5.0f )
                return null;
            return this;
        }
        float t_ = 0.0f;
    }

    ImageBrinker shipWarningUpBrinker_ = new ImageBrinker();
    ImageBrinker shipWarningDownBrinker_ = new ImageBrinker();
    State state_;
    bool bGameOver_ = false;
}
