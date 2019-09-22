using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase {

    [SerializeField]
    Countdown countDown_;

    [SerializeField]
    WaraDollSystem waraDollSysPrefab_;


    private void Awake() {
        countDown_.gameObject.SetActive( false );
        waraDollSys_ = PrefabUtil.createInstance( waraDollSysPrefab_, transform, Vector3.zero );
    }

    void Start() {
        state_ = new FadeIn( this );
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
    }

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
            GlobalState.time( 1.0f, (sec, t) => {
                return true;
            } ).finish( () => {
                setNextState( new FadeOut( parent_ ) );
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
