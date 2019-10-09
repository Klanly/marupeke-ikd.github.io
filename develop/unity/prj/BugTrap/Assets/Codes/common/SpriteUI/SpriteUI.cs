using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトUI
//  スプライトベースのUIの基底クラス。
//  Colliderを持ちSpriteUIControllerによるレイの判定を受ける。
//  クリック判定されたスプライトUIはonDownハンドラが呼ばれる。
//  またクリック後リリースされた場合はonUpハンドラが呼ばれる。

public class SpriteUI : MonoBehaviour
{
    // クリック等で押し下げられた
    public void onDown() {
        if ( bEnable_ == false )
            return;

        bDown_ = true;
        innerOnDown();
    }

    // ダウンされていた物が元に戻った
    public void onUp() {
        if ( bEnable_ == false )
            return;

        if ( bDown_ == false )
            return;     // クリックされていないので無視
        bDown_ = false;
        innerOnUp();
    }

    // クリック後枠外で元に戻った
    public void onCancel() {
        if ( bDown_ == false )
            return;
        innerOnCancel();
    }

    // UIアクティブ切り替え
    public void setEnable( bool isEnable ) {
        if ( bEnable_ != isEnable ) {
            bEnable_ = isEnable;
            innerOnEnable( isEnable );
        }
    }

    // 押し下げられた
    virtual protected void innerOnDown() {
        // 派生クラスで具体的な実装
    }

    // 元に戻った
    virtual protected void innerOnUp() {
        // 派生クラスで具体的な実装
    }

    // キャンセル
    virtual protected void innerOnCancel() {
        // 派生クラスで具体的な実装
    }

    // UIアクティブ切り替え
    virtual protected void innerOnEnable( bool isEnable ) {
        // 派生クラスで具体的な実装
    }

    private void Awake() {
        collider_ = GetComponent<Collider>();
        if ( collider_ = null ) {
            Debug.Log( "SpriteUI warning: " + name + " has no collider." );
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    Collider collider_;
    bool bDown_ = false;
    bool bEnable_ = true;
}
