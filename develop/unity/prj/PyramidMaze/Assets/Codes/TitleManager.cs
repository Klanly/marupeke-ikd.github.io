using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイトル
public class TitleManager : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Image fader_;

    [SerializeField]
    float fadeInTime_ = 2.0f;

    [SerializeField]
    float fadeOutTime_ = 2.0f;

    [SerializeField]
    int level_ = 3;

    [SerializeField]
    CanvasGroup levelGroup_;

    [SerializeField]
    UnityEngine.UI.Button level3Btn_;

    [SerializeField]
    UnityEngine.UI.Button level5Btn_;

    [SerializeField]
    UnityEngine.UI.Button level7Btn_;


    public System.Action<int> FinishCallback { set { finishCallback_ = value; } }

    private void Awake() {
        fader_.color = new Color( 0.0f, 0.0f, 0.0f, 1.0f );    
    }

    // Use this for initialization
    void Start () {
        state_ = new FadeIn( this );
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null ) {
            state_ = state_.update();
        }
	}

    class FadeIn : State< TitleManager > {
        public FadeIn( TitleManager parent ) : base( parent ) {
        }
        protected override State innerInit() {
            float a = 0.0f;
            Color c = Color.black;
            GlobalState.time( parent_.fadeInTime_, (sec, t) => {
                c.a = 1.0f - t;
                parent_.fader_.color = c;
                return true;
            } ).finish(()=> {
                setNextState( new Idle( parent_ ) );
            } );
            return this;
        }
    }

    class Idle : State< TitleManager > {
        public Idle( TitleManager parent ) : base( parent ) {
            GlobalState.time( 2.0f, (sec, t) => {
                parent_.levelGroup_.alpha = t;
                return true;
            } ).finish(()=> {
                parent_.level3Btn_.onClick.AddListener( () => { level_ = 3; } );
                parent_.level5Btn_.onClick.AddListener( () => { level_ = 5; } );
                parent_.level7Btn_.onClick.AddListener( () => { level_ = 7; } );
            } );
        }
        protected override State innerUpdate() {
            if ( level_ > 0 ) {
                parent_.level_ = level_;
                parent_.level3Btn_.enabled = true;
                parent_.level5Btn_.enabled = true;
                parent_.level7Btn_.enabled = true;
                return new FadeOut( parent_ );
            }
            return this;
        }
        int level_ = 0;
    }

    class FadeOut : State< TitleManager > {
        public FadeOut( TitleManager parent ) : base( parent ) {
        }
        protected override State innerInit() {
            float a = 0.0f;
            Color c = parent_.fader_.color;
            GlobalState.time( parent_.fadeOutTime_, (sec, t) => {
                c.a = t;
                parent_.fader_.color = c;
                return true;
            } ).finish( () => {
                parent_.finishCallback_( parent_.level_ );
            } );
            return this;
        }
    }

    State state_;
    System.Action<int> finishCallback_;
}
