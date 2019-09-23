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


    public static GameManager getInstance() {
        return gameManager_g;
    } 

    private void Awake() {
        gameManager_g = this;
        countDown_.gameObject.SetActive( false );
        hummer_.gameObject.SetActive( false );
        waraDollSys_ = PrefabUtil.createInstance( waraDollSysPrefab_, transform, Vector3.zero );
        waraDollSys_.setup( new WaraDollSystem.Parameter(), hummer_ );
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
            return null;
        }
        protected override State innerUpdate() {
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
