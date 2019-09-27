using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトの色を変更するインターフェイスを提供

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class SpriteColor : MonoBehaviour
{
    SpriteRenderer renderer_;

    private void Awake() {
        renderer_ = GetComponent<SpriteRenderer>();       
    }

    // カラーを変更
    public void setColor( int r, int g, int b, int a ) {
        Color c = new Color( r / 255.0f, g / 255.0f, b / 255.0f / a / 255.0f );
        setColor( c );
    }
    // カラーを変更
    public void setColor( Color c ) {
        renderer_.color = c;
    }

    // α値を変更
    public void setAlpha( float a ) {
        var c = renderer_.color;
        c.a = a;
        renderer_.color = c;
    }

    // 現在のカラーから指定のカラーへ時間フェード
    public void fadeColor(int r, int g, int b, int a, float sec ) {
        Color c = new Color();
        c.r = r / 255.0f;
        c.g = g / 255.0f;
        c.b = b / 255.0f;
        c.a = a / 255.0f;
        fadeColor( c, sec );
    }

    // 現在のカラーから指定のカラーへ時間フェード
    public void fadeColor( Color endColor, float sec ) {

        //  現在フェード中に再度フェードが入った場合は
        //  前のフェード処理はやめる
        if ( fadeColorState_ != null ) {
            fadeColorState_.forceFinish();
            fadeColorState_ = null;
        }

        var sc = renderer_.color;
        fadeColorState_ = GlobalState.time( sec, (_sec, t) => {
            renderer_.color = Color.Lerp( sc, endColor, t );
            return true;
        } );
        fadeColorState_.finish(() => {
            renderer_.color = endColor;
        } );
    }

    // 現在のαから指定のαへ時間フェード
    public void fadeAlpha(float endAlpha, float sec) {

        //  現在フェード中に再度フェードが入った場合は
        //  前のフェード処理はやめる
        if ( fadeAlphaState_ != null ) {
            fadeAlphaState_.forceFinish();
            fadeAlphaState_ = null;
        }

        var sc = renderer_.color;
        var ec = renderer_.color;
        ec.a = endAlpha;
        fadeAlphaState_ = GlobalState.time( sec, (_sec, t) => {
            renderer_.color = Color.Lerp( sc, ec, t );
            return true;
        } );
        fadeAlphaState_.finish( () => {
            renderer_.color = ec;
        } );
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GlobalState fadeColorState_;
    GlobalState fadeAlphaState_;
}
