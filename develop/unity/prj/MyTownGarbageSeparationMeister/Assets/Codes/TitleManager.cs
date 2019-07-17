using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    SpriteButton start_;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_ = null;

    private void Awake() {
        state_ = new FadeIn( this );
    }

    void Start() {

    }

    void Update() {
        if ( state_ != null )
            state_ = state_.update();
    }

    class FadeIn : State<TitleManager> {
        public FadeIn(TitleManager parent) : base( parent ) {

        }
        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 2.0f );
            GlobalState.wait( 2.0f, () => {
                setNextState( new Idle( parent_ ) );
                return false;
            } );
            return this;
        }
    }

    class Idle : State<TitleManager> {
        public Idle(TitleManager parent) : base( parent ) {

        }
        protected override State innerInit() {
            parent_.start_.OnDecide = (str) => {
                setNextState( new FadeOut( parent_ ) );
            };
            return this;
        }
    }

    class FadeOut : State<TitleManager> {
        public FadeOut(TitleManager parent) : base( parent ) {

        }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 2.0f );
            GlobalState.wait( 2.0f, () => {
                parent_.finishCallback_();
                return false;
            } );
            return this;
        }
    }

    State state_;
}
