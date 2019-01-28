using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    CanvasGroup group_;

    [SerializeField]
    UnityEngine.UI.Button startBtn_;

    [SerializeField]
    UnityEngine.UI.Button continueBtn_;

    [SerializeField]
    UnityEngine.UI.Button autoBtn_;

    [SerializeField]
    UnityEngine.UI.Button endingBtn_;



    public enum Mode
    {
        None,
        Start,
        Continue,
        Auto,
        Ending
    }

    public System.Action<Mode> FinishCallback { set { finishCallback_ = value; } }

    // Use this for initialization
    void Start () {
        state_ = new FadeIn( this );
        startBtn_.onClick.AddListener( () => {
            selectMode_ = Mode.Start;
        } );
        continueBtn_.onClick.AddListener( () => {
            selectMode_ = Mode.Continue;
        } );
        autoBtn_.onClick.AddListener( () => {
            selectMode_ = Mode.Auto;
        } );
        endingBtn_.onClick.AddListener( () => {
            selectMode_ = Mode.Ending;
        } );
    }

    private void Awake()
    {
        group_.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();

        cameraMove();
	}

    void cameraMove()
    {
        rotY_ += 0.1f;
        rotY_ %= 360;
        Camera.main.transform.rotation = Quaternion.Euler( 30.0f, rotY_, 0.0f );
    }

    class FadeIn : State
    {
        public FadeIn(TitleManager manager)
        {
            manager_ = manager;
        }

        protected override void innerInit()
        {
            manager_.startBtn_.enabled = false;
            manager_.continueBtn_.enabled = false;
            manager_.autoBtn_.enabled = false;
            GlobalState.time( 2.0f, (sec, t) => {
                manager_.group_.alpha = t;
                return true;
            } ).finish( () => {
                bFinish_ = true;
            } );
        }

        protected override State innerUpdate()
        {
            if ( bFinish_ == true ) {
                return  new Wait( manager_ );
            }
            return this;
        }

        TitleManager manager_;
        bool bFinish_ = false;
    }

    class Wait : State
    {
        public Wait( TitleManager manager )
        {
            manager_ = manager;
        }

        protected override void innerInit()
        {
            manager_.startBtn_.enabled = true;
            manager_.continueBtn_.enabled = true;
            manager_.autoBtn_.enabled = true;
        }

        protected override State innerUpdate()
        {
            if ( manager_.selectMode_ != Mode.None ) {
                return new FadeOut( manager_ );
            }
            return this;
        }

        TitleManager manager_;
    }

    class FadeOut : State
    {
        public FadeOut( TitleManager manager )
        {
            manager_ = manager;
        }

        protected override void innerInit()
        {
            GlobalState.time( 3.0f, ( sec, t ) => {
                manager_.group_.alpha = 1.0f - t;
                return true;
            } ).finish( () => {
                manager_.finishCallback_( manager_.selectMode_ );
                bFinish_ = true;
            } );
        }

        protected override State innerUpdate()
        {
            return bFinish_ == false ? this : null;
        }

        TitleManager manager_;
        bool bFinish_ = false;
    }

    State state_;
    System.Action<Mode> finishCallback_;
    Mode selectMode_ = Mode.None;
    float rotY_ = 0.0f;
}
