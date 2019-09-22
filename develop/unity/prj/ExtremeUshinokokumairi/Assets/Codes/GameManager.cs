using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase {
    // Start is called before the first frame update
    void Start() {
        state_ = new FadeIn( this );
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
    }

    class FadeIn : State<GameManager> {
        public FadeIn(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            GlobalState.time( 1.0f, (sec, t) => {
                return true;
            } ).finish( () => {
                setNextState( new Idle( parent_ ) );
            } );
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
            GlobalState.time( 1.0f, (sec, t) => {
                return true;
            } ).finish( () => {
                parent_.finishCallback_();
            } );
            return this;
        }
    }
}
