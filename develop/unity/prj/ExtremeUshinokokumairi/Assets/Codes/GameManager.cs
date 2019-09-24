using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase {

    [SerializeField]
    Countdown countDown_;

    [SerializeField]
    WaraDollSystem waraDollSysPrefab_;

    [SerializeField]
    Transform uiCameraRoot_;
    public Transform UIRoot { get { return uiCameraRoot_; } }

    [SerializeField]
    ParticleEmitter particleEmitter_;
    public ParticleEmitter ParticleEmitter { get { return particleEmitter_; } }

    [SerializeField]
    HummerMotion hummer_;

    [SerializeField]
    Tree tree_;

    [SerializeField]
    NoroiGetCounter noroiGetCounter_;

    [SerializeField]
    Watch watch_;

    [SerializeField]
    ECG ecg_;

    [SerializeField]
    SpriteUIController uiController_;

    [SerializeField]
    SpriteButton failButton_;


    public static GameManager getInstance() {
        return gameManager_g;
    }

    private void Awake() {
        gameManager_g = this;
        countDown_.gameObject.SetActive( false );
        hummer_.gameObject.SetActive( false );
        waraDollSys_ = PrefabUtil.createInstance( waraDollSysPrefab_, null, Vector3.zero );
        tree_.setDoolSys( waraDollSys_ );
        waraDollSys_.setup( new WaraDollSystem.Parameter(), hummer_ );
        waraDollSys_.setActive( false );
        watch_.setActive( false );
    }

    private void OnDestroy() {
        gameManager_g = null;
    }

    void Start() {
        state_ = new FadeIn( this );
        extremeState_ = null;
        // 成功を続けたら心拍数を上げていく
        hummer_.SuccessHit = () => {
            if ( extremeState_ != null ) {
                return;
            }
            float b = ecg_.addBeat( 2.0f );
            if ( b >= 150.0f ) {
                // エクストリーム！
                // ハンマー下ろしまくり！
                extremeState_ = new Extreme( this );
            }
        };
        // 失敗したら心拍数下げる
        hummer_.FailHit = () => {
            if ( extremeState_ != null ) {
                return;
            }
            ecg_.addBeat( 1.0f );
        };
        // 空ぶり
        failButton_.OnDecide = ( name ) => {
            // ハンマーの位置をXY平面投影点へ
            Vector3 pos = Vector3.zero;
            if ( CameraUtil.calcClickPosition( Camera.main, Input.mousePosition, out pos ) == true ) {
                hummer_.hit( pos );
            }
        };
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
        if ( extremeState_ != null ) {
            extremeState_ = extremeState_.update();
        }
    }

    static GameManager gameManager_g;
    WaraDollSystem waraDollSys_;
    State extremeState_;

    // ハンマー叩きまくり
    class Extreme : State<GameManager> {
        public Extreme( GameManager parent ) : base( parent ) {}
        protected override State innerInit() {
            return null;
        }
        protected override State innerUpdate() {
            interval_ += Time.deltaTime;
            t_ += Time.deltaTime;
            float num = 8.0f;
            if ( interval_ >= 1.0f / num ) {
                parent_.uiController_.forceMouseButtonClick();  // 強制ハンマー振り下ろし
                interval_ -= 1.0f / num;
            }
            if ( t_ >= 12.0f ) {
                parent_.ecg_.setBeat( 65.0f );
                return null;
            }
            return this;
        }
        float interval_ = 0.0f;
        float t_ = 0.0f;
    }

    class FadeIn : State<GameManager> {
        public FadeIn(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 2.0f, () => {
                parent_.countDown_.gameObject.SetActive( true );
                setNextState( new CountdownState( parent_ ) );
            } );
            return this;
        }
    }

    class CountdownState : State<GameManager> {
        public CountdownState(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.countDown_.FinishCallback = () => {
                setNextState( new Idle( parent_ ) );
            };
            return this;
        }
    }

    class Idle : State<GameManager> {
        public Idle(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.hummer_.gameObject.SetActive( true );
            parent_.waraDollSys_.setActive( true );
            parent_.watch_.setActive( true );
            parent_.waraDollSys_.AllHitCallback = () => {
                setNextState( new NextDoolSet( parent_ ) );
            };
            return null;
        }
        protected override State innerUpdate() {
            // 時刻が午前3時を過ぎたら終了
            int curSec = parent_.watch_.getCurSec();
            if ( curSec >= limitSec_ ) {
                parent_.hummer_.gameObject.SetActive( false );
                parent_.waraDollSys_.setActive( false );
                parent_.watch_.setActive( false );
                parent_.waraDollSys_.AllHitCallback = null;
                return new FadeOut( parent_ );
            }
            return this;
        }
        int limitSec_ = 3 * 3600;   // 午前3時
        bool bExtreme_ = false;
    }

    class NextDoolSet : State<GameManager> {
        public NextDoolSet(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            // 呪い獲得パーティクルをカウンターに向けて飛ばす
            var noroiGetParticle = parent_.ParticleEmitter.emit( "NoroiGetMovePt" );
            noroiGetParticle.transform.localPosition = Vector3.zero;
            var p = noroiGetParticle as NoroiGetParticle;
            if ( p != null ) {
                var nextPos = parent_.noroiGetCounter_.getNextCountPosition();
                p.setEndPosition( nextPos );
                p.FinishCallback = () => {
                    parent_.noroiGetCounter_.add();
                };
            }
            // 同時に今のワラ人形を次へ
            //  エクストリーム中はチョッパヤで
            parent_.hummer_.gameObject.SetActive( false );
            parent_.waraDollSys_.setActive( false );
            var preWaraDollSys = parent_.waraDollSys_;
            float waitTime = ( parent_.extremeState_ != null ? 0.1f : 0.5f );
            float turnTime = ( parent_.extremeState_ != null ? 0.3f : 1.0f );
            GlobalState.wait( waitTime, () => {
                parent_.waraDollSys_ = PrefabUtil.createInstance( parent_.waraDollSysPrefab_, null, Vector3.zero );
                parent_.tree_.setDoolSys( parent_.waraDollSys_ );
                parent_.waraDollSys_.setup( new WaraDollSystem.Parameter(), parent_.hummer_ );
                parent_.waraDollSys_.setActive( false );

                parent_.tree_.turnNext( turnTime, () => {
                    Destroy( preWaraDollSys.gameObject );
                    setNextState( new Idle( parent_ ) );
                } );
                return false;
            } );
            return this;
        }
    }


    class FadeOut : State<GameManager> {
        public FadeOut(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 2.0f, () => {
                parent_.finishCallback_();
            } );
            return this;
        }
    }
}
