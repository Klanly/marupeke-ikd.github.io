using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Image logo_;

    [SerializeField]
    UnityEngine.UI.Image back_;

    [SerializeField]
    bool debugToIntro_ = false;

    [SerializeField]
    bool debugToFadeOut_ = false;


    public void toIntro()
    {
        state_ = new Intro( this );
    }

    public void toFadeOut( System.Action callback )
    {
        state_ = new FadeOut( this, callback );
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

        if ( debugToIntro_ == true ) {
            debugToIntro_ = false;
            toIntro();
        }
        if ( debugToFadeOut_ == true ) {
            debugToFadeOut_ = false;
            toFadeOut( () => { } );
        }
	}

    class Intro : State
    {
        public Intro(Fader parent)
        {
            parent_ = parent;
        }

        // 内部初期化
        override protected void innerInit()
        {
            var rt = parent_.logo_.GetComponent<RectTransform>();

            // backのαを0へ、同時にlogo_を拡大していく -> s秒後logoのαを0へ
            GlobalState.start( () => {
                parent_.back_.CrossFadeAlpha( 0.0f, 2.0f, false );
                return false;
            } );

            float t = 0.0f;
            float scaleTime = 3.5f;
            float sc = 25.0f;
            GlobalState.wait( 2.0f, () => {
                t += Time.deltaTime / scaleTime;
                float scale = 1.0f + t * sc;
                rt.localScale = Vector3.one * scale;
                if ( t >= 1.0f ) {
                    return false;
                }
                return true;
            } );

            float alphaTime = 0.5f;
            GlobalState.wait( 2.5f, () => {
                parent_.logo_.CrossFadeAlpha( 0.0f, alphaTime, false );
                return false;
            } );

            GlobalState.wait( 5.5f, () => {
                parent_.logo_.gameObject.SetActive( false );
                parent_.back_.gameObject.SetActive( false );
                bFinish_ = true;
                return false;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true )
                return null;
            return this;
        }

        Fader parent_;
        bool bFinish_ = false;
    }

    class FadeIn : State
    {
        public FadeIn( Fader parent )
        {
            parent_ = parent;
        }

        // 内部初期化
        override protected void innerInit()
        {
            // 
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return null;
        }

        Fader parent_;
    }

    class FadeOut : State
    {
        public FadeOut(Fader parent, System.Action callback )
        {
            parent_ = parent;
            callback_ = callback;
        }

        // 内部初期化
        override protected void innerInit()
        {
            var rt = parent_.logo_.GetComponent<RectTransform>();
            parent_.logo_.gameObject.SetActive( true );
            parent_.back_.gameObject.SetActive( true );

            float alphaTime = 0.5f;
            parent_.logo_.CrossFadeAlpha( 1.0f, alphaTime, false );
            parent_.back_.CrossFadeAlpha( 0.0f, 0.0f, true );

            float t = 0.0f;
            float scaleTime = 2.5f;
            float sc = 25.0f;
            GlobalState.start( () => {
                t += Time.deltaTime / scaleTime;
                t = Mathf.Clamp01( t );
                float scale = 1.0f + ( 1.0f - t ) * sc;
                rt.localScale = Vector3.one * scale;
                if ( t >= 1.0f ) {
                    return false;
                }
                return true;
            } );

            GlobalState.wait( 4.0f, () => {
                parent_.back_.CrossFadeAlpha( 1.0f, 1.0f, false );
                return false;
            } );

            GlobalState.wait( 5.5f, () => {
                bFinish_ = true;
                if ( callback_ != null )
                    callback_();
                return false;
            } );

        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true )
                return null;
            return this;
        }

        Fader parent_;
        bool bFinish_;
        System.Action callback_;
    }

    State state_;
}
