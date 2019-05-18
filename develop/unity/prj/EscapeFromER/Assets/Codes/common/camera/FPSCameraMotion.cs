using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FPSの基本操作をカメラに反映させるクラス
//
//  マウスの移動：視点を上下左右に動かす
public class FPSCameraMotion : MonoBehaviour {

    [SerializeField]
    bool bVisibleCursor_ = true;
    bool preVisibleCursor_ = true;

    [SerializeField]
    float sensitivity_ = 1.0f;      // マウス1ドットに対する回転角度

    [SerializeField]
    float maxPitchAngle_ = 80.0f;

    [SerializeField]
    float minPitchAngle_ = -80.0f;

    [SerializeField]
    float moveSpeed_ = 1.0f / 60.0f;    // 1フレームでの移動スピード

    // FPS視点移動のアクティブを切り替え
    public void setActive( bool isAcive ) {
        bVisibleCursor_ = !isAcive;
    }

    private void OnDestroy() {
        Cursor.visible = true;    
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bVisibleCursor_ != preVisibleCursor_ ) {
            // Cursor.visible = bVisibleCursor_;
            Cursor.visible = true;

            if ( bVisibleCursor_ == false ) {
                // カーソルを画面中央に固定
                Cursor.lockState = CursorLockMode.Confined;
                // 基点確定
                cursorOrg_ = Input.mousePosition;
            } else {
                // 移動範囲解放
                Cursor.lockState = CursorLockMode.None;
            }
        }
        preVisibleCursor_ = bVisibleCursor_;

        // ESCでカーソルを強制表示
        if ( Input.GetKeyDown( KeyCode.Escape ) == true ) {
            bVisibleCursor_ = true;
            return;
        }

        // FPSカメラはカーソルがOFFの時だけアクティブ
        if ( bVisibleCursor_ == true )
            return;

        // 基点からの相対値で軸回転角度を算出
        // X軸方向：Y軸差分回転量
        // Y軸方向：X軸回転量 (minPitchAngle_～maxPitchAngle_）
        var refDist = ( Input.mousePosition - cursorOrg_ ) * sensitivity_;
        refDist.x *= -1.0f;     // マウスを左右に移動したらそちらへ向く
        if ( pitchRot_ + refDist.y < minPitchAngle_ )
            pitchRot_ = minPitchAngle_;
        else if ( pitchRot_ + refDist.y > maxPitchAngle_ )
            pitchRot_ = maxPitchAngle_;
        else
            pitchRot_ += refDist.y;
        yawRot_ += refDist.x;

        var forward = SphereSurfUtil.convPolerToPos( pitchRot_, yawRot_ );
        var q = Quaternion.LookRotation( forward );
        transform.localRotation = q;

        cursorOrg_ = Input.mousePosition;

        // WASDで移動（現在の姿勢ベース）
        // AD: 左右へ平行移動
        // WS: 前後へ平行移動
        Vector3 xMove = Vector3.zero;
        Vector3 yMove = Vector3.zero;
        if ( Input.GetKey( KeyCode.A ) == true ) {
            xMove -= transform.right * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.D ) == true ) {
            xMove += transform.right * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.S ) == true ) {
            var f = transform.forward;
            f.y = 0.0f;
            yMove -= f.normalized * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.W ) == true ) {
            var f = transform.forward;
            f.y = 0.0f;
            yMove += f.normalized * moveSpeed_;
        }
        var p = transform.localPosition;
        p += xMove + yMove;
        transform.localPosition = p;
    }

    Vector3 cursorOrg_ = Vector3.zero;
    float pitchRot_ = 0.0f;
    float yawRot_ = 0.0f;
}
