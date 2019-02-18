using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject pieceRoot_;

    void Start() {
        pieces_ = pieceRoot_.gameObject.GetComponentsInChildren<Piece>();

        state_ = new Shuffle( this );
        cameraState_ = new CameraInitWait( this );
    }

    void Update() {
        if ( state_ != null )
            state_ = state_.update();
        if ( cameraState_ != null )
            cameraState_ = cameraState_.update();
    }

    class BaseState : State
    {
        public BaseState(GameManager parent)
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    class CameraInitWait : BaseState
    {
        public CameraInitWait(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.gameStartCallback_ = () => { bStart_ = true; };
            return null;
        }
        protected override State innerUpdate()
        {
            if ( bStart_ == true )
                return new AllowCameraOperation( parent_ );
            return this;
        }
        bool bStart_ = false;
    }

    class AllowCameraOperation : BaseState
    {
        public AllowCameraOperation(GameManager parent) : base( parent ) { }
        protected override State innerUpdate()
        {
            // [W][S]でカメラのズームイン/アウト
            float speedUnit = 0.3f;
            if ( Input.GetKey( KeyCode.W ) == true ) {
                // ズームイン
                var forward = Camera.main.transform.forward;
                var pos = Camera.main.transform.position;
                if ( ( forward + pos ).y >= 1.0f + speedUnit ) {
                    Camera.main.transform.position = pos + forward * speedUnit;
                }
            } else if ( Input.GetKey( KeyCode.S ) == true ) {
                // ズームアウト
                var forward = Camera.main.transform.forward;
                var pos = Camera.main.transform.position;
                if ( ( forward + pos ).y <= 40.0f - speedUnit ) {
                    Camera.main.transform.position = pos - forward * speedUnit;
                }
            }

            // ピックアップ中でなく左ドラッグの場合は
            // カメラを移動
            if ( parent_.bPickingUpPiece_ == false ) {
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    clickPos_ = Input.mousePosition;
                }
                if ( Input.GetMouseButton( 0 ) == true ) {
                    var cp = Input.mousePosition;
                    if ( cp != clickPos_ ) {
                        Ray preMouseRay = Camera.main.ScreenPointToRay( clickPos_ );
                        Ray curMouseRay = Camera.main.ScreenPointToRay( cp );
                        Vector3 prePos, curPos;
                        CollideUtil.colPosRayPlane( out prePos, preMouseRay.origin, preMouseRay.direction, Vector3.zero, Vector3.up );
                        CollideUtil.colPosRayPlane( out curPos, curMouseRay.origin, curMouseRay.direction, Vector3.zero, Vector3.up );
                        var cameraMoveDir = prePos - curPos;
                        var cameraPos = Camera.main.transform.position;
                        Camera.main.transform.position = cameraPos + cameraMoveDir;

                        clickPos_ = cp;
                    }
                }
            }
            return this;
        }

        Vector3 clickPos_;
    }

    class Shuffle : BaseState
    {
        public Shuffle(GameManager parent) : base( parent ) { }

        protected override State innerInit()
        {
            // ピースを適当な位置と回転でシャッフル
            foreach ( var p in parent_.pieces_ ) {
                count_++;
                float rot = ( ( int )( 180.0f - Random.value * 360.0f ) / 20.0f ) * 20.0f;
                var curQ = p.transform.localRotation;
                var initPos = p.transform.localPosition;
                var offsetPos = Randoms.Vec3.valueCenterXZ() * 20.0f;
                GlobalState.time( Random.Range( 1.0f, 1.3f ), (sec, t) => {
                    p.transform.localPosition = Lerps.Vec3.easeOutStrong( initPos, offsetPos, t );
                    p.transform.localRotation = Quaternion.Euler( 0.0f, rot * t, 0.0f ) * curQ;
                    return true;
                } ).finish( () => {
                    count_--;
                } );
            }
            return null;
        }

        protected override State innerUpdate()
        {
            if ( count_ == 0 )
                return new TimerStart( parent_ );
            return this;
        }

        int count_ = 0;
    }

    class TimerStart : BaseState
    {
        public TimerStart(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            if ( parent_.gameStartCallback_ != null )
                parent_.gameStartCallback_();
            return new Idle( parent_ );
        }
    }

    class Idle : BaseState
    {
        public Idle(GameManager parent) : base( parent ) {
            parent_.bPickingUpPiece_ = false;
        }
        protected override State innerUpdate()
        {
            // 左クリックでレイを飛ばしピース上だったら摘まむ
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hit;
                // Rayが衝突したかどうか
                if ( Physics.Raycast( mouseRay, out hit ) == true ) {
                    var onCollide = hit.collider.transform.GetComponent<OnCollideCallback>();
                    if ( onCollide != null ) {
                        // 親がpiece
                        var pickUpPiece = onCollide.transform.parent.gameObject.GetComponent<Piece>();
                        if ( pickUpPiece != null ) {
                            return new PickUpPiece( parent_, pickUpPiece );
                        }
                    }
                }
            }
            return this;
        }
    }

    class PickUpPiece : BaseState
    {
        public PickUpPiece( GameManager parent, Piece pickUpPiece ) : base( parent ) {
            // ピックアップ中を通知
            parent_.bPickingUpPiece_ = true;
            pickUpPiece_ = pickUpPiece;
        }

        protected override State innerUpdate()
        {
            // 左クリックを離した時にピースが定位置にあればハメる
            // そうでなければピースを離す
            if ( Input.GetMouseButtonUp( 0 ) == true ) {
                if ( pickUpPiece_.isStayPosition() == true ) {
                    pickUpPiece_.stay();
                }
                return new Idle( parent_ );
            }
            else {
                // ドラッグ中
                // マウスの位置の先にある平面にピースを移動
                Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                Vector3 colPos;
                CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, Vector3.zero, Vector3.up );
                pickUpPiece_.transform.localPosition = colPos;

                // [A][D]キー押し下げで左右回転
                float rotUnit = 1.0f;
                if ( Input.GetKey( KeyCode.A ) == true ) {
                    var q = Quaternion.Euler( 0.0f, -rotUnit, 0.0f );
                    var curQ = pickUpPiece_.transform.localRotation;
                    pickUpPiece_.transform.localRotation = q * curQ;
                }
                else if ( Input.GetKey( KeyCode.D ) == true ) {
                    var q = Quaternion.Euler( 0.0f, rotUnit, 0.0f );
                    var curQ = pickUpPiece_.transform.localRotation;
                    pickUpPiece_.transform.localRotation = q * curQ;
                }
            }
            return this;
        }

        Piece pickUpPiece_;
    }

    State state_;
    State cameraState_;
    Piece[] pieces_;
    System.Action gameStartCallback_;
    bool bPickingUpPiece_ = false;
}
