using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾処理者操作
public class HandlerOperator : MonoBehaviour {

    [SerializeField]
    float rangeX_ = 1.0f;

    [SerializeField]
    float rangeZ_ = 1.5f;

    
    // オペレータアクティブ
    public void setActive( bool isActive )
    {
        bActive_ = isActive;
    }

    // マウスカーソル移動量を更新
    void updateMouseMoveDef()
    {
        cursorMoveDef_ = Input.mousePosition - preCursorPos_;
        preCursorPos_ = Input.mousePosition;
        bCursorMove_ = cursorMoveDef_.magnitude > 0.00001f;
    }

    void Start()
    {
        rotateState_ = rotateIdle;
        moveState_ = moveIdle;
        standUpState_ = sit;
        operateState_ = operate;
    }

    void Update()
    {

        if ( bActive_ == false )
            return;

        prePos_ = transform.position;

        updateMouseMoveDef();

        operateState_();
        standUpState_();
        moveState_();
        rotateState_();

        updatePose();
    }

    void updatePose()
    {
        var pos = transform.position;
        if ( Mathf.Abs( pos.x ) < rangeX_ && Mathf.Abs( pos.z ) < rangeZ_ ) {
            transform.position = prePos_;
        }
    }

    void moveIdle()
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        bool bRightOn = Input.GetMouseButton( 1 );

        // AWSD押し下げで平行移動
        if ( Input.GetKey( KeyCode.A ) == true ) {
            x -= moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.D ) == true ) {
            x += moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.W ) == true || Input.mouseScrollDelta.y > 0.0f ) {
            z += moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.S ) == true || Input.mouseScrollDelta.y < 0.0f ) {
            z -= moveSpeed_;
        }

        float dragSpeed = 0.5f;
        if ( bRightOn && cursorMoveDef_.x < 0.0f ) {
            x -= moveSpeed_ * dragSpeed;
        }
        if ( bRightOn && cursorMoveDef_.x > 0.0f ) {
            x += moveSpeed_ * dragSpeed;
        }
        if ( Mathf.Abs( x ) >= 0.0001f || Mathf.Abs( z ) >= 0.0001f ) {
            var pos = transform.position;
            var xDir = transform.right;
            var zDir = transform.forward;
            zDir.y = 0.0f;
            pos += xDir * x + Vector3.up * y + zDir.normalized * z;
            transform.position = pos;
        }
    }

    void rotateIdle()
    {
        // 中ボタン押し下げで回転見回しへ
        if ( Input.GetMouseButtonDown( 2 ) == true ) {
            rotateState_ = rotate;
        }
   }

    // 視点を回転（視点先を変更）
    void rotate()
    {
        if ( Input.GetMouseButton( 2 ) == false ) {
            rotateState_ = rotateIdle;
        }

        if ( bCursorMove_ == false )
            return;

        // X方向でY軸回転、Y方向でX軸回転を変更
        var rot = transform.rotation.eulerAngles;
        rotX_ = rot.x;
        rot.x += -cursorMoveDef_.y * xRotSpeed_;
        if ( rot.x > 85.0f && rot.x < 285.0f ) {
            rot.x = rotX_;
        }
        rot.y += cursorMoveDef_.x * yRotSpeed_;
        transform.rotation = Quaternion.Euler( rot );

        rotY_ = rot.y;
    }

    // しゃがみ中
    void sit()
    {
        // Zか中ボタンダブルクリックで立ち上がり
        if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
            transStandUp();
        } else if ( Input.GetMouseButtonDown( 2 ) == true ) {
            standUpState_ = wait;
            int count = 0;
            bool bClicked = false;
            GlobalState.time( 0.30f, (sec, t) => {
                if ( count > 0 && Input.GetMouseButtonDown( 2 ) == true ) {
                    bClicked = true;
                    transStandUp();
                    return false;
                }
                count++;
                return true;
            } ).finish(()=> {
                if ( bClicked == false )
                    standUpState_ = sit;
            } );
        }
    }

    // 立ち上がる
    void transStandUp()
    {
        standUpState_ = wait;
        float initY = transform.position.y;
        GlobalState.time( 0.75f, (sec, t) => {
            var pos = transform.position;
            pos.y = Lerps.Float.easeInOut( initY, standUpHeight_, t );
            transform.position = pos;
            return true;
        } ).finish( () => {
            standUpState_ = standUp;
        } );
    }

    // 立ち上がり中
    void standUp()
    {
        // Zでしゃがむ
        if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
            standUpState_ = transSit;
        } else if ( Input.GetMouseButtonDown( 2 ) == true ) {
            standUpState_ = wait;
            int count = 0;
            bool bClicked = false;
            GlobalState.time( 0.30f, (sec, t) => {
                if ( count > 0 && Input.GetMouseButtonDown( 2 ) == true ) {
                    bClicked = true;
                    transSit();
                    return false;
                }
                count++;
                return true;
            } ).finish(()=> {
                if ( bClicked == false )
                    standUpState_ = standUp;
            } );
        }
    }

    // しゃがむ
    void transSit()
    {
        standUpState_ = wait;
        float initY = transform.position.y;
        GlobalState.time( 0.75f, (sec, t) => {
            var pos = transform.position;
            pos.y = Lerps.Float.easeInOut( initY, 0.0f, t );
            transform.position = pos;
            return true;
        } ).finish( () => {
            standUpState_ = sit;
        } );
    }

    void wait()
    {

    }

    // 操作
    void operate()
    {
        // 左クリックでレイを飛ばす
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            ray();
        }
    }

    void ray()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
        if ( Physics.Raycast( mouseRay, out hit, 10.0f ) == true ) {
            // 対象が持っているOnActionをコール
            var onAction = hit.collider.GetComponent<OnAction>();
            if ( onAction == null )
                return;
            onAction.onAction( gameObject, "hit" );
        }
    }

    System.Action rotateState_;
    System.Action moveState_;
    System.Action standUpState_;
    System.Action operateState_;

    bool bActive_ = false;
    Vector3 cursorMoveDef_ = Vector3.zero;
    Vector3 preCursorPos_ = Vector3.zero;
    bool bCursorMove_ = false;
    float xRotSpeed_ = 0.2f;
    float yRotSpeed_ = 0.2f;
    float moveSpeed_ = 0.1f;
    float standUpHeight_ = 2.5f;
    float rotX_ = 0.0f;
    float rotY_ = 0.0f;

    Vector3 prePos_ = Vector3.zero;
}
