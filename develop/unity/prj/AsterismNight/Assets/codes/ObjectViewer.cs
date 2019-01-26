using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを見回す

public class ObjectViewer {

    // 仮想カメラとターゲットを設定
    //  カメラ位置とターゲット方向が正面となり、仮想の回転球があると仮定します
    public void setup( Transform camera, GameObject target )
    {
        target_ = target;
        up_ = camera.up;
        forward_ = target.transform.position - camera.position;
        right_ = Vector3.Cross( up_, forward_ ).normalized;
    }

    // マウス移動量に対する回転量係数を設定
    public void setRotScale( float rotScale )
    {
        rotScale_ = rotScale;
    }

    // 姿勢更新
    public void update()
    {
        if ( target_ == null )
            return;

        if ( onDragging_ == false && Input.GetMouseButtonDown( 0 ) == true ) {
            prePos_ = Input.mousePosition;
            onDragging_ = true;
        } else if ( onDragging_ == true && Input.GetMouseButtonUp( 0 ) == true ) {
            onDragging_ = false;
        }

        if ( onDragging_ ) {
            var curPos = Input.mousePosition;
            var def = curPos - prePos_;
            if ( def.magnitude <= 0.001f )
                return;

            // 回転軸算出
            //  マウス移動方向(right_-up_平面）とforwardに垂直
            var mouseVec = def.x * right_ + def.y * up_;
            var rotAxis = Vector3.Cross( mouseVec, forward_ ).normalized;
            var defQ = Quaternion.AngleAxis( def.magnitude * rotScale_, rotAxis );
            var preQ = target_.transform.rotation;
            target_.transform.rotation = defQ * preQ;

            prePos_ = curPos;
        }
    }

    GameObject target_ = null;
    Vector3 prePos_ = Vector3.zero;
    bool onDragging_ = false;
    Vector3 right_ = Vector3.right;
    Vector3 up_ = Vector3.up;
    Vector3 forward_ = Vector3.forward;
    float rotScale_ = 1.0f;
}
