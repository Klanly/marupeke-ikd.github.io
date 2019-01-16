using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// イメージを点滅

public class ImageBrinker {

    // 初期化
    public void setup(UnityEngine.UI.Image image, float showIntervalSec, float hideIntervalSec, int brinkCount, bool isFirstShow, bool isLastShow )
    {
        image_ = image;
        renderer_ = image_.canvasRenderer;
        showIntervalSec_ = showIntervalSec;
        hideIntervalSec_ = hideIntervalSec;
        brinkCount_ = brinkCount;
        bFirstShow_ = isFirstShow;
        bLastShow_ = isLastShow;

        image.gameObject.SetActive( false );

        state_ = null;
    }

    // スイッチON（ブリンク開始）
    public void switchOn()
    {
        image_.gameObject.SetActive( false );
        state_ = transShow;
        count_ = 0;

    }

    // リセット
    public void reset()
    {
        // 待機状態に戻す
        image_.gameObject.SetActive( false );
        state_ = null;
        count_ = 0;
    }

    // ループ設定
    public void setLoop(bool useLoop, float intervalSecBetweenOneSet)
    {
        bLoop_ = useLoop;
        intervalSecBetweenOneSet_ = intervalSecBetweenOneSet;
    }

    // ループ切り替え
    public void setLoop(bool useLoop)
    {
        bLoop_ = useLoop;
    }

    // アクティブ設定
    public void setActive( bool isActive )
    {
        bActive_ = isActive;
    }

    public void update() {
        if ( bActive_) {
            if ( state_ != null )
                state_();
        }
    }

    void transShow()
    {
        image_.gameObject.SetActive( true );
        t_ = 0.0f;
        state_ = showing;
    }

    void showing()
    {
        t_ += Time.deltaTime;
        if ( t_ >= showIntervalSec_ ) {
            count_++;
            if ( count_ >= brinkCount_ ) {
                image_.gameObject.SetActive( bLastShow_ );
                state_ = wait;
                return;
            }
            state_ = transHide;
        }
    }

    void transHide()
    {
        image_.gameObject.SetActive( false );
        t_ = 0.0f;
        state_ = hiding;
    }

    void hiding()
    {
        t_ += Time.deltaTime;
        if ( t_ >= hideIntervalSec_ ) {
            state_ = transShow;
        }
    }

    void wait()
    {
        if ( bLoop_ == false ) {
            state_ = null;
            return;
        }

        t_ += Time.deltaTime;
        if ( t_ >= intervalSecBetweenOneSet_ ) {
            state_ = transShow;
        }
    }

    UnityEngine.UI.Image image_;
    CanvasRenderer renderer_;
    bool bActive_ = true;
    bool bFirstShow_ = true;
    bool bLastShow_ = true;
    bool bLoop_ = false;
    float intervalSecBetweenOneSet_ = 1.0f;
    float showIntervalSec_ = 1.0f;
    float hideIntervalSec_ = 1.0f;
    int brinkCount_ = 5;
    System.Action state_;
    float t_ = 0.0f;
    int count_ = 0;
}
