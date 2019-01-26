using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを見回す

public class ObjectViewer {

    // 仮想カメラとターゲットを設定
    //  カメラ位置とターゲット方向が正面となり、仮想の回転球があると仮定します
    public void setup( Transform camera, GameObject target, float virtualSphereRadius )
    {
        target_ = target;
        up_ = camera.up;
        forward_ = target.transform.position - camera.position;
        right_ = Vector3.Cross( up_, forward_ ).normalized;
        radius_ = virtualSphereRadius;

        screenCenter_ = new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f );
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
            clickPos_ = prePos_ = Input.mousePosition;
            clickDir_ = clickPos_ - screenCenter_;
            bOutOfRadius_ = ( clickDir_.magnitude >= radius_ );
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
            if ( bOutOfRadius_ == false ) {
                //  マウス移動方向(right_-up_平面）とforwardに垂直
                var mouseVec = def.x * right_ + def.y * up_;
                var rotAxis = Vector3.Cross( mouseVec, forward_ ).normalized;
                var defQ = Quaternion.AngleAxis( def.magnitude * rotScale_, rotAxis );
                var preQ = target_.transform.rotation;
                target_.transform.rotation = defQ * preQ;
            } else {
                //  クリック点が仮想半径の外側の場合はForward水平回転（マウスの位置に対応した角度で回転）
                var rotAxis = forward_;
                var cursorDir = ( curPos - screenCenter_ ).normalized;
                var ang = Mathf.Acos( Vector3.Dot( clickDir_.normalized, cursorDir ) ) * Mathf.Rad2Deg;
                var sign = Vector3.Dot( Vector3.Cross( clickDir_, cursorDir ), Vector3.forward ) < 0.0f ? -1.0f : 1.0f;
                var defQ = Quaternion.AngleAxis( ang * sign, rotAxis );
                var preQ = target_.transform.rotation;
                target_.transform.rotation = defQ * preQ;
                clickDir_ = cursorDir;
            }

            prePos_ = curPos;
        }
    }

    GameObject target_ = null;
    Vector3 screenCenter_;
    Vector3 clickDir_ = Vector3.zero;
    Vector3 clickPos_ = Vector3.zero;
    Vector3 prePos_ = Vector3.zero;
    bool onDragging_ = false;
    bool bOutOfRadius_ = false;
    Vector3 right_ = Vector3.right;
    Vector3 up_ = Vector3.up;
    Vector3 forward_ = Vector3.forward;
    float rotScale_ = 0.3f;
    float radius_ = 100.0f;
}
