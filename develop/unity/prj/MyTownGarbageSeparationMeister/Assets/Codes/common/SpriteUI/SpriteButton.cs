using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトボタン
//  Spriteベースのボタンです。
//  

public class SpriteButton : SpriteUI
{
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
        var e = scale_ * downScale_;
        GlobalState.time( downSec_, (sec, t) => {
            transform.localScale = Lerps.Vec3.easeIn( scale_, e, t );
            return true;
        } );
        Debug.Log( "押し下げ" );
    }

    // 元に戻った
    override protected void innerOnUp() {
        var s = transform.localScale;
        var e = scale_;
        GlobalState.time( downSec_, (sec, t) => {
            transform.localScale = Lerps.Vec3.easeIn( s, e, t );
            return true;
        } );

        if ( onDecide_ != null ) {
            onDecide_( name );
        }
        Debug.Log( "戻した" );
    }

    // キャンセル
    override protected void innerOnCancel() {
        var s = transform.localScale;
        var e = s / downScale_;
        GlobalState.time( 0.2f, (sec, t) => {
            transform.localScale = Lerps.Vec3.easeIn( s, e, t );
            return true;
        } );
        Debug.Log( "キャンセル" );
    }

    // UIアクティブ切り替え
    override protected void innerOnEnable( bool isEnable ) {
        if ( isEnable == false ) {
            for ( int i = 0; i < renderers_.Count; ++i ) {
                renderers_[ i ].color = renderersColor_[ i ] * disableColor_;
            }
        } else {
            for ( int i = 0; i < renderers_.Count; ++i ) {
                renderers_[ i ].color = renderersColor_[ i ];
            }
        }
    }

    private void Awake() {
        // ぶら下がっているすべてのスプライトを取得
        renderers_ = GameObjectUtil.findAllComponent<SpriteRenderer>( gameObject, true, false );
        foreach ( var r in renderers_ ) {
            renderersColor_.Add( r.color );
        }
        scale_ = transform.localScale;
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
    List<SpriteRenderer> renderers_ = new List<SpriteRenderer>();
    List<Color> renderersColor_ = new List<Color>();
}
