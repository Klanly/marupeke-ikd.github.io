using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : GameManagerBase {
    // Start is called before the first frame update
    void Start() {
        state_ = new FadeIn( this );
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
    }

    class FadeIn : State<TitleManager> {
        public FadeIn(TitleManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 2.0f, () => {
                setNextState( new Idle( parent_ ) );
            } );
            return this;
        }
    }

    class Idle : State<TitleManager> {
        public Idle(TitleManager parent) : base( parent ) { }
        protected override State innerUpdate() {
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                setNextState( new FadeOut( parent_ ) );
            }
            return this;
        }
    }

    class FadeOut : State<TitleManager> {
        public FadeOut(TitleManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 2.0f, () => {
                parent_.finishCallback_();
            } );
            return this;
        }
    }

}
