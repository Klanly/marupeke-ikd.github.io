using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラオペレータ

public class CameraOperator : MonoBehaviour {

    // 視点先の距離を設定
    public void setDistance( float dist )
    {
        dist_ = dist;
        updateLookAtPos();
    }

    // オペレート
    public void setActive( bool isActive )
    {
        bActive_ = isActive;
        if ( bActive_ == true ) {
            updateLookAtPos();
            preCursorPos_ = Input.mousePosition;
        }
    }

    // 経度移動スピードを設定（角速度）
    public void setLongitudeSpeed( float speed )
    {
        longiSpeed_ = speed;
    }

    // 緯度移動スピードを設定（角速度）
    public void setLatitudeSpeed( float speed )
    {
        latSpeed_ = speed;
    }

    // 視点の位置を更新
    void updateLookAtPos()
    {
        var pos = Camera.main.transform.position;
        lookAtPos_ = pos + Camera.main.transform.forward * dist_;
        SphereSurfUtil.convPosToPoler( pos - lookAtPos_, out lat_, out longi_ );
    }

    // マウスカーソル移動量を更新
    void updateMouseMoveDef()
    {
        cursorMoveDef_ = Input.mousePosition - preCursorPos_;
        preCursorPos_ = Input.mousePosition;
        bCursorMove_ = cursorMoveDef_.magnitude > 0.00001f;
    }

    void Start () {
        state_ = idle;
	}
	
	void Update () {

        if ( bActive_ == false )
            return;
        updateMouseMoveDef();

        state_();

        updatePose();
    }

    void updatePose()
    {
        var pos = SphereSurfUtil.convPolerToPos( lat_, longi_ ) * dist_ + lookAtPos_;
        var forward = lookAtPos_ - pos;
        Camera.main.transform.position = pos;
        Camera.main.transform.rotation = Quaternion.LookRotation( forward );
    }

    void idle()
    {
        // 中ボタン押し下げで回転へ
        if ( Input.GetMouseButtonDown( 2 ) == true ) {
            state_ = rotate;
        }
    }

    // 視点を中心に回転
    void rotate()
    {
        if ( Input.GetMouseButton( 2 ) == false )
            state_ = idle;

        if ( bCursorMove_ == false )
            return;

        // X方向で経度、Y方向で緯度を変更
        lat_ += cursorMoveDef_.y * latSpeed_;
        longi_ += cursorMoveDef_.x * longiSpeed_;
    }

    System.Action state_;

    bool bActive_ = false;
    float dist_ = 1.0f;
    Vector3 lookAtPos_ = Vector3.zero;
    Vector3 cursorMoveDef_ = Vector3.zero;
    Vector3 preCursorPos_ = Vector3.zero;
    bool bCursorMove_ = false;
    float lat_ = 0.0f;
    float longi_ = 0.0f;
    float latSpeed_ = 0.5f;
    float longiSpeed_ = 0.5f;
}
