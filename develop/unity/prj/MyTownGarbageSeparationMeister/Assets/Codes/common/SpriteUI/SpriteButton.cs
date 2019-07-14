using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトボタン
//  Spriteベースのボタンです。
//  

public class SpriteButton : SpriteUI
{
    [SerializeField]
    SpriteRenderer buttonSprite_;

    [SerializeField]
    float downScale_ = 0.95f;

    [SerializeField]
    float downSec_ = 0.1f;

    [SerializeField]
    Color disableColor_ = Color.gray;


    public System.Action< string > OnDecide { set { onDecide_ = value; } }
    System.Action<string> onDecide_ = null;

    // 押し下げられた
    override protected void innerOnDown() {
        if ( buttonSprite_ != null ) {
            var e = scale_ * downScale_;
            GlobalState.time( downSec_, (sec, t) => {
                buttonSprite_.transform.localScale = Lerps.Vec3.easeIn( scale_, e, t );
                return true;
            } );
        }
        Debug.Log( "押し下げ" );
    }

    // 元に戻った
    override protected void innerOnUp() {
        if ( buttonSprite_ != null ) {
            var s = buttonSprite_.transform.localScale;
            var e = scale_;
            GlobalState.time( downSec_, (sec, t) => {
                buttonSprite_.transform.localScale = Lerps.Vec3.easeIn( s, e, t );
                return true;
            } );

            if ( onDecide_ != null ) {
                onDecide_( name );
            }
        }
        Debug.Log( "戻した" );
    }

    // キャンセル
    override protected void innerOnCancel() {
        if ( buttonSprite_ != null ) {
            var s = buttonSprite_.transform.localScale;
            var e = s / downScale_;
            GlobalState.time( 0.2f, (sec, t) => {
                buttonSprite_.transform.localScale = Lerps.Vec3.easeIn( s, e, t );
                return true;
            } );
        }
        Debug.Log( "キャンセル" );
    }

    // UIアクティブ切り替え
    override protected void innerOnEnable( bool isEnable ) {
        if ( isEnable == false ) {
            buttonSprite_.color = color_ * disableColor_;
        } else {
            buttonSprite_.color = color_;
        }
    }

    private void Awake() {
        if ( buttonSprite_ ) {
            scale_ = buttonSprite_.transform.localScale;
            color_ = buttonSprite_.color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 scale_ = Vector3.one;
    Color color_ = Color.white;
}
