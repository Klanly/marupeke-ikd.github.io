using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Button easyBtn_;

    [SerializeField]
    UnityEngine.UI.Button normalBtn_;

    [SerializeField]
    UnityEngine.UI.Button hardBtn_;

    public System.Action< string > FinishCallback { set { finishCallback_ = value; } }

    // Use this for initialization
    void Start () {
        state_ = new FadeIn( this );
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();
    }

    class FadeIn : State< TitleManager > {
        public FadeIn(TitleManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 2.0f, () => {
                setNextState( new Idle( parent_ ) );
            } );
            return this;
        }
    }

    class Idle : State< TitleManager > {
        public Idle( TitleManager parent ) : base( parent ) { }
        protected override State innerInit() {
            string level = "";
            parent_.easyBtn_.onClick.AddListener( () => {
                level = "easy";
            } );
            parent_.normalBtn_.onClick.AddListener( () => {
                level = "normal";
            } );
            parent_.hardBtn_.onClick.AddListener( () => {
                level = "hard";
            } );
            GlobalState.start( () => {
                if ( level != "" ) {
                    setNextState( new FadeOut( parent_, level ) );
                    return false;
                }
                return true;
            } );
            return this;
        }
    }

    class FadeOut : State< TitleManager > {
        public FadeOut(TitleManager parent, string level) : base( parent ) {
            level_ = level;
        }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 2.0f, () => {
                parent_.finishCallback_( level_ );
            } );
            return this;
        }
        string level_;
    }

    State state_;
    System.Action<string> finishCallback_;
}
