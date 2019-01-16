using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    TitleManager titleManager_;

    [SerializeField]
    SunManager sunManager_;

    [SerializeField]
    Transform passengerRoot_;

    [SerializeField]
    UnityStandardAssets.ImageEffects.Blur blur_;

    [SerializeField]
    RectTransform uiCanvas_;

    [ SerializeField]
    HumanRule humanWalkRule_;

    [SerializeField]
    HumanRule humanRunRule_;

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
    UnityEngine.UI.Text resultText_;

    [SerializeField]
    AudioSource gameBgm_;

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

    // ゲームを初期状態に戻す
    void resetAll()
    {
        sunManager_.resetAll();
        uiCanvas_.gameObject.SetActive( false );
        shipWarningUpBrinker_.reset();
        shipWarningUpBrinker_.setActive( false );
        shipWarningDownBrinker_.reset();
        shipWarningDownBrinker_.setActive( false );
        foreach ( var rule in passengerRule_ ) {
            rule.resetAll();
        }
        foreach ( var b in bridges_ ) {
            b.resetAll();
        }
        bGameOver_ = false;
        bUpdateGame_ = false;

        foreach( Transform passanger in passengerRoot_.transform ) {
            Destroy( passanger.gameObject );
        }
        gameOverImage_.gameObject.SetActive( false );
        foreach ( var hb in holdButtons_ ) {
            hb.resetAll();
        }
    }

    // ゲーム開始
    void start()
    {
        bUpdateGame_ = true;
        sunManager_.resetAll();
        sunManager_.setActive();
        uiCanvas_.gameObject.SetActive( true );
        foreach ( var rule in passengerRule_ ) {
            rule.resetAll();
            rule.setActive();
        }
        foreach ( var b in bridges_ ) {
            b.resetAll();
            b.setActive();
        }
        bridgeButtons_.SetActive( true );

        gameBgm_.Play();
    }

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
            shipWarningUpBrinker_.switchOn();
            shipWarningUpBrinker_.setActive( true );
        } else {
            shipWarningDownBrinker_.switchOn();
            shipWarningDownBrinker_.setActive( true );
        }
    }

    private void Awake()
    {
        titleManager_.initialize();

        humanWalkRule_.setup( this );
        humanRunRule_.setup( this );
        shipRule_.setup( this );

        passengerRule_[ 0 ] = humanWalkRule_;
        passengerRule_[ 1 ] = humanRunRule_;
        passengerRule_[ 2 ] = shipRule_;

        uiCanvas_.gameObject.SetActive( false );

        state_ = new State_Title( this );
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

        // 船接近警告イメージ初期化
        shipWarningUpBrinker_.setup( shipWarningUpImage_, 0.25f, 0.10f, 35, true, false );
        shipWarningDownBrinker_.setup( shipWarningDownImage_, 0.25f, 0.10f, 35, true, false );

        // 発生ルール関連付け
        humanWalkRule_.EmmitCallback = () => {
            if ( debugEmitActive_ == false || bGameOver_ == true )
                return;
            var human = passengerFactory_.create( Passenger.Type.Human_Walk );
            emmitHuman( human as Human );
        };
        humanRunRule_.EmmitCallback = () => {
            if ( debugEmitActive_ == false || bGameOver_ == true )
                return;
            var human = passengerFactory_.create( Passenger.Type.Human_Run );
            emmitHuman( human as Human );
        };
        shipRule_.EmmitCallback = () => {
            if ( debugEmitActive_ == false || bGameOver_ == true )
                return;
            var ship = passengerFactory_.create( Passenger.Type.Ship );
            ship.setup( this );
            ship.transform.parent = passengerRoot_;
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
        human.transform.parent = passengerRoot_;

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

        if ( state_ != null )
            state_ = state_.update();

        debugUpdate();

        // 船接近警告ブリンカー
        shipWarningUpBrinker_.update();
        shipWarningDownBrinker_.update();

        if ( bUpdateGame_ == true ) {
            // 難易度調整
            int day = sunManager_.getDay();
            int hour = sunManager_.getHour();
            float intensity = 0.2f + ( day * 24 + hour - 6 ) * 0.05f;   // 難易度
            foreach ( var ps in passengerRule_ ) {
                ps.setNumPerHourIntensity( intensity );
            }
        }
    }

    // ゲームオーバー処理
    void toGameOver( int bridgeIndex )
    {
        if ( bGameOver_ == false ) {
            bGameOver_ = true;
            int day = sunManager_.getDay() + 1;
            int hour = sunManager_.getHour();
            int min = sunManager_.getMin();
            if ( day == 1 )
                resultText_.text = string.Format( "{0}Day {1:00}:{2:00}", day, hour, min );
            else
                resultText_.text = string.Format( "{0}Days {1:00}:{2:00}", day, hour, min );
            state_ = new State_CameraZoomIn( this, bridgeIndex );

            gameBgm_.Stop();
            shipWarningUpBrinker_.reset();
            shipWarningUpBrinker_.setActive( false );
            shipWarningDownBrinker_.reset();
            shipWarningDownBrinker_.setActive( false );
        }
    }

    // タイトルへ戻る
    void toTitle()
    {
        resetAll();
    }

    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    // タイトル画面中
    class State_Title : StateBase
    {
        public State_Title(GameManager parent) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            parent_.titleManager_.gameObject.SetActive( true );
            parent_.titleManager_.OnFinish = () => {
                bFinish_ = true;
            };
        }
        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                return new State_MainGame( parent_ );
            }
            return this;
        }
        bool bFinish_ = false;
    }

    // ゲーム再生中
    class State_MainGame : StateBase
    {
        public State_MainGame(GameManager parent) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            parent_.start();
        }
        // 内部状態
        override protected State innerUpdate()
        {
            return null;    // ゲームオーバーになったら遷移復帰
        }
    }

    // ゲームオーバー前カメラズーム
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
            if ( t_ >= 5.0f ) {
                // タイトルへ
                parent_.toTitle();
                parent_.titleManager_.toTitle();
                return new State_Title( parent_ );
            }
            return this;
        }
        float t_ = 0.0f;
    }

    ImageBrinker shipWarningUpBrinker_ = new ImageBrinker();
    ImageBrinker shipWarningDownBrinker_ = new ImageBrinker();
    State state_;
    bool bGameOver_ = false;
    bool bUpdateGame_ = false;
    PassengerRule[] passengerRule_ = new PassengerRule[ 3 ];
}
