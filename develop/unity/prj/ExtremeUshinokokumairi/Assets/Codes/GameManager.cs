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
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
    }

    static GameManager gameManager_g;
    WaraDollSystem waraDollSys_;

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
            return this;
        }
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
            parent_.hummer_.gameObject.SetActive( false );
            parent_.waraDollSys_.setActive( false );
            var preWaraDollSys = parent_.waraDollSys_;
            GlobalState.wait( 0.5f, () => {
                parent_.waraDollSys_ = PrefabUtil.createInstance( parent_.waraDollSysPrefab_, null, Vector3.zero );
                parent_.tree_.setDoolSys( parent_.waraDollSys_ );
                parent_.waraDollSys_.setup( new WaraDollSystem.Parameter(), parent_.hummer_ );
                parent_.waraDollSys_.setActive( false );

                parent_.tree_.turnNext( () => {
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
