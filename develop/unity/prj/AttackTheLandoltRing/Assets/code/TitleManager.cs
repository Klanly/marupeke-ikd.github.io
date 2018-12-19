using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    SpriteRenderer logo_;

    [SerializeField]
    SpriteRenderer pressZ_;

    [SerializeField]
    UnityEngine.UI.Text credit_;

    [SerializeField]
    UnityEngine.UI.Text credit_to_C;

    [SerializeField]
    AudioSource introAudioSource_;

    [SerializeField]
    AudioSource startAudioSource_;


    public bool isFinish()
    {
        return bFinish_;
    }

    private void Awake()
    {
        credit_.CrossFadeAlpha( 0.0f, 0.0f, true );
    }

    // Use this for initialization
    void Start() {
        Color c = logo_.material.color;
        c.a = 0.0f;
        logo_.material.color = c;

        c = pressZ_.material.color;
        c.a = 0.0f;
        pressZ_.material.color = c;

        state_ = new FadeIn( this );
    }

    // Update is called once per frame
    void Update() {
        if ( state_ != null )
            state_ = state_.update();
    }

    State state_;
    bool bFinish_ = false;
    Tweener pressZTw_;


    class FadeIn : State
    {
        public FadeIn(TitleManager manager)
        {
            manager_ = manager;
        }
        // 内部初期化
        override protected void innerInit()
        {
            manager_.introAudioSource_.Play();
            manager_.logo_.material.DOFade( 1.0f, 0.75f ).OnComplete( () => {
                bReady_ = true;
                Color c = manager_.pressZ_.material.color;
                c.a = 1.0f;
                manager_.pressZ_.material.color = c;
                manager_.pressZ_.material.DOFade( 0.0f, 0.75f ).SetEase( Ease.InOutCubic ).SetLoops( -1, LoopType.Yoyo );
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bReady_ == true ) {
                return new Idle( manager_ );
            }
            return this;
        }

        bool bReady_ = false;
        TitleManager manager_;
    }

    class Idle : State
    {
        public Idle(TitleManager manager)
        {
            manager_ = manager;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
                manager_.startAudioSource_.Play();
                return new FadeOut( manager_ );
            } else if ( Input.GetKeyDown( KeyCode.C ) == true ) {
                return new Credit( manager_ );
            }
            return this;
        }

        TitleManager manager_;
    }

    class FadeOut : State
    {
        public FadeOut(TitleManager manager)
        {
            manager_ = manager;
        }
        // 内部初期化
        override protected void innerInit()
        {
            manager_.logo_.material.DOFade( 0.0f, 1.75f ).OnComplete( () => {
                bFinish_ = true;
            } );
            manager_.pressZ_.material.DOFade( 0.0f, 1.75f ).SetLoops( 0 );
            manager_.credit_to_C.CrossFadeAlpha( 0.0f, 0.75f, true );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                manager_.bFinish_ = true;
                return null;
            }
            return this;
        }

        TitleManager manager_;
        bool bFinish_ = false;
    }

    class Credit : State
    {
        public Credit(TitleManager manager)
        {
            manager_ = manager;
            manager_.pressZ_.material.DOKill();
            manager_.logo_.material.DOFade( 0.0f, 0.75f );
            manager_.pressZ_.material.DOFade( 0.0f, 0.75f );
            manager_.credit_.CrossFadeAlpha( 1.0f, 0.75f, true );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( Input.GetKeyDown( KeyCode.C ) == true ) {
                manager_.logo_.material.DOFade( 1.0f, 0.75f );
                manager_.pressZ_.material.DOFade( 1.0f, 0.75f ).SetEase( Ease.InOutCubic ).SetLoops( -1, LoopType.Yoyo );
//                manager_.credit_.material.DOFade( 0.0f, 0.75f );
                manager_.credit_.CrossFadeAlpha( 0.0f, 0.75f, true );
                return new Idle( manager_ );
            }
            return this;
        }

        TitleManager manager_;
    }
}
