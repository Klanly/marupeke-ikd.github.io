using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterismDesc : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text nameJp_;

    [SerializeField]
    UnityEngine.UI.Text nameEn_;

    [SerializeField]
    UnityEngine.UI.Text concodanceRate_;

    // 星座情報をセット
    public void setup( int astId )
    {
        var data = Table_asterism_ast.getInstance().getData( astId - 1 );
        nameJp_.text = data.jpName_ + "座";
        nameEn_.text = data.name_;

        setAlpha( 0.0f );
    }

    // フェードアウト
    public void fadeOut(System.Action callback)
    {
        fadeOutCallback_ = callback;
        state_ = new FadeOut( this );
    }

    // 合致率を設定
    public void setConcodanceRate( float rate )
    {
        concodanceRate_.text = string.Format( "{0:0.0}%", rate * 100.0f );
    }

    // 透過度を変更
    void setAlpha( float alpha )
    {
        Color c = Color.white;
        c.a = alpha;
        nameJp_.color = c;
        nameEn_.color = c;
        concodanceRate_.color = c;
    }

    // Use this for initialization
    void Start () {
        state_ = new FadeIn( this );
    }
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class FadeIn : State
    {
        public FadeIn( AsterismDesc parent )
        {
            parent_ = parent;
        }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.setAlpha( 0.0f );
            GlobalState.time( 3.0f, ( sec, t ) => {
                parent_.setAlpha( t );
                return true;
            } ).finish( () => {
                bFinish_ = true;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true )
                return null;
            return this;
        }

        AsterismDesc parent_;
        bool bFinish_ = false;
    }

    class FadeOut : State
    {
        public FadeOut(AsterismDesc parent)
        {
            parent_ = parent;
        }
        // 内部初期化
        override protected void innerInit()
        {
            parent_.setAlpha( 1.0f );
            GlobalState.time( 2.0f, (sec, t) => {
                parent_.setAlpha( 1.0f - t );
                return true;
            } ).finish( () => {
                bFinish_ = true;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                if ( parent_.fadeOutCallback_ != null )
                    parent_.fadeOutCallback_();
                return null;
            }
            return this;
        }

        AsterismDesc parent_;
        bool bFinish_ = false;
    }

    State state_;
    System.Action fadeOutCallback_;
}
