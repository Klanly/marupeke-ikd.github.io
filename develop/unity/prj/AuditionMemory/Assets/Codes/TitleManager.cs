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

    [SerializeField]
    UnityEngine.UI.Button creditBtn_;

    [SerializeField]
    UnityEngine.UI.Button allBtn_;

    [SerializeField]
    AudioSource bgm_;

    [SerializeField]
    SpriteRenderer notify_;

    [SerializeField]
    Camera phoneCamera_;

    [SerializeField]
    GameObject credit_;


    public System.Action< string > FinishCallback { set { finishCallback_ = value; } }

    private void Awake() {
        credit_.SetActive( false );
    }

    // Use this for initialization
    void Start () {
        state_ = new Notify( this );
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();
    }

    class Notify : State< TitleManager > {
        public Notify( TitleManager parent ) : base( parent ) { }
        protected override State innerInit() {
            var color = parent_.notify_.color;
            GlobalState.time( 0.75f, (sec, t) => {
                color.a = t;
                parent_.notify_.color = color;
                return true;
            } ).nextTime( 2.75f, (sec, t) => {
                return true;
            } ).nextTime( 0.75f, (sec, t) => {
                color.a = 1.0f - t;
                parent_.notify_.color = color;
                return true;
            } ).finish( () => {
                Destroy( parent_.phoneCamera_.gameObject );
                setNextState( new FadeIn( parent_ ) );
            } );
            return this;
        }
    }

    class FadeIn : State< TitleManager > {
        public FadeIn(TitleManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 1.0f, () => {
                parent_.bgm_.Play();
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
            parent_.allBtn_.onClick.AddListener( () => {
                level = "all";
            } );
            parent_.creditBtn_.onClick.AddListener( () => {
                parent_.credit_.SetActive( true );
                GlobalState.start( () => {
                    if ( Input.GetMouseButton( 0 ) == true ) {
                        parent_.credit_.SetActive( false );
                        return false;
                    }
                    return true;
                } );
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
            GlobalState.time( 0.9f, (sec, t) => {
                parent_.bgm_.volume = 1.0f - t;
                return true;
            } );
            FaderManager.Fader.to( 1.0f, 1.0f, () => {
                parent_.finishCallback_( level_ );
            } );
            return this;
        }
        string level_;
    }

    State state_;
    System.Action<string> finishCallback_;
}
