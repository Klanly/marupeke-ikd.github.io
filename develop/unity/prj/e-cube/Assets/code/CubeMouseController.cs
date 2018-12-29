using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMouseController : CubeController
{
    struct RotDir
    {
        public RotDir(AxisType axis, CubeRotationType rotType, int colIdx) {
            axis_ = axis;
            rotType_ = rotType;
            colIdx_ = new int[ 1 ] { colIdx };
        }
        public AxisType axis_;
        public CubeRotationType rotType_;
        public int[] colIdx_;
    }

    public CubeMouseController( Cube cube, CubeCamera camera )
    {
        cube_ = cube;
        camera_ = camera;
    }

    // コントローラのイベントを取得
    override public void getInputEvents(ref List<CubeEvent> events)
    {
        // 左クリックマウスアクションをチェック
        // 左ボタン押し下げ
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            // カーソル位置にキューブのピースがあればつまむ
            var piece = cube_.ray( Camera.main.ScreenPointToRay( Input.mousePosition ), out pickUpFace_ );
            if ( piece != null ) {
                pickUpPiece_ = piece;
                pickUpPos_ = Input.mousePosition;
            }
        }

        // ピックアップ中。ドラッグ方向を監視
        if ( pickUpPiece_ != null && Input.GetMouseButton( 0 ) == true ) {
            // 側面（LRFB)面を摘まんでいる状態で上下ドラッグはX軸、
            // 左右ドラッグはY軸回転。
            // UD面を摘まんでいる時に上下ドラッグはX軸、
            // 左右ドラッグはZ軸回転とする。
            var curMouseVec = Input.mousePosition - pickUpPos_;     // ドラッグ方向
            if ( curMouseVec.magnitude >= dragDist_ ) {
                // 回転確定
                var faceTargets = cube_.getFaceNormalsTargetPosInWorld();
                var bodyScreenPos = Camera.main.WorldToScreenPoint( cube_.getBodyPos() );
                // ドラッグ方向基準
                Vector3[] dirs = new Vector3[ 6 ] {
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 0 ] ) - bodyScreenPos ).normalized, // L
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 1 ] ) - bodyScreenPos ).normalized, // R
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 2 ] ) - bodyScreenPos ).normalized, // D
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 3 ] ) - bodyScreenPos ).normalized, // U
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 4 ] ) - bodyScreenPos ).normalized, // F
                    ( Camera.main.WorldToScreenPoint( faceTargets[ 5 ] ) - bodyScreenPos ).normalized, // B
                };
                float maxDot = -2.0f;
                int idx = -1;
                int[] focusIndices = new int[ 4 ];
                if ( pickUpFace_ == FaceType.FaceType_Left || pickUpFace_ == FaceType.FaceType_Right ) {
                    focusIndices[ 0 ] = 2;
                    focusIndices[ 1 ] = 3;
                    focusIndices[ 2 ] = 4;
                    focusIndices[ 3 ] = 5;
                } else if ( pickUpFace_ == FaceType.FaceType_Down || pickUpFace_ == FaceType.FaceType_Up ) {
                    focusIndices[ 0 ] = 0;
                    focusIndices[ 1 ] = 1;
                    focusIndices[ 2 ] = 4;
                    focusIndices[ 3 ] = 5;
                } else {
                    focusIndices[ 0 ] = 0;
                    focusIndices[ 1 ] = 1;
                    focusIndices[ 2 ] = 2;
                    focusIndices[ 3 ] = 3;
                }
                foreach( int i in focusIndices ) {
                    float dot = Vector3.Dot( curMouseVec.normalized, dirs[ i ] );
                    if ( dot > maxDot ) {
                        maxDot = dot;
                        idx = i;
                    }
                }
                // 摘まんだフェイスとスクラッチした方向のフェイスから回転軸と回転方向を算出
                var pc = pickUpPiece_.getCoord();
                Dictionary<int, RotDir> rotDirs = new Dictionary<int, RotDir>() {
                    // Left
                    { (int)FaceType.FaceType_Left | ((int)FaceType.FaceType_Down) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Minus_90, pc.z ) },
                    { (int)FaceType.FaceType_Left | ((int)FaceType.FaceType_Up) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Plus_90, pc.z ) },
                    { (int)FaceType.FaceType_Left | ((int)FaceType.FaceType_Front) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Plus_90, pc.y ) },
                    { (int)FaceType.FaceType_Left | ((int)FaceType.FaceType_Back) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Minus_90, pc.y ) },
                    // Right
                    { (int)FaceType.FaceType_Right | ((int)FaceType.FaceType_Down) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Plus_90, pc.z ) },
                    { (int)FaceType.FaceType_Right | ((int)FaceType.FaceType_Up) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Minus_90, pc.z ) },
                    { (int)FaceType.FaceType_Right | ((int)FaceType.FaceType_Front) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Minus_90, pc.y ) },
                    { (int)FaceType.FaceType_Right | ((int)FaceType.FaceType_Back) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Plus_90, pc.y ) },
                    // Down
                    { (int)FaceType.FaceType_Down | ((int)FaceType.FaceType_Left)  << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Plus_90, pc.z ) },
                    { (int)FaceType.FaceType_Down | ((int)FaceType.FaceType_Right) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Minus_90, pc.z ) },
                    { (int)FaceType.FaceType_Down | ((int)FaceType.FaceType_Front) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Minus_90, pc.x ) },
                    { (int)FaceType.FaceType_Down | ((int)FaceType.FaceType_Back) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Plus_90, pc.x ) },
                    // Up
                    { (int)FaceType.FaceType_Up | ((int)FaceType.FaceType_Left)  << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Minus_90, pc.z ) },
                    { (int)FaceType.FaceType_Up | ((int)FaceType.FaceType_Right) << 4, new RotDir(AxisType.AxisType_Z, CubeRotationType.CRT_Plus_90, pc.z ) },
                    { (int)FaceType.FaceType_Up | ((int)FaceType.FaceType_Front) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Plus_90, pc.x ) },
                    { (int)FaceType.FaceType_Up | ((int)FaceType.FaceType_Back) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Minus_90, pc.x ) },
                    // Front
                    { (int)FaceType.FaceType_Front | ((int)FaceType.FaceType_Left) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Minus_90, pc.y ) },
                    { (int)FaceType.FaceType_Front | ((int)FaceType.FaceType_Right) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Plus_90, pc.y ) },
                    { (int)FaceType.FaceType_Front | ((int)FaceType.FaceType_Down) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Plus_90, pc.x ) },
                    { (int)FaceType.FaceType_Front | ((int)FaceType.FaceType_Up) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Minus_90, pc.x ) },
                    // Back
                    { (int)FaceType.FaceType_Back | ((int)FaceType.FaceType_Left) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Plus_90, pc.y ) },
                    { (int)FaceType.FaceType_Back | ((int)FaceType.FaceType_Right) << 4, new RotDir(AxisType.AxisType_Y, CubeRotationType.CRT_Minus_90, pc.y ) },
                    { (int)FaceType.FaceType_Back | ((int)FaceType.FaceType_Down) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Minus_90, pc.x ) },
                    { (int)FaceType.FaceType_Back | ((int)FaceType.FaceType_Up) << 4, new RotDir(AxisType.AxisType_X, CubeRotationType.CRT_Plus_90, pc.x ) },
                };
                int hash = ( int )pickUpFace_ | ( idx << 4 );
                RotDir res = rotDirs[ hash ];
                events.Add( new CubeEvent_Rotate( res.axis_, res.rotType_, res.colIdx_ ) );

                // リセット
                pickUpPiece_ = null;
                pickUpPos_ = Vector3.zero;
                pickUpFace_ = FaceType.FaceType_None;
            }
        }

        // 右クリックマウスアクションをチェック
        // 右ボタン押し下げ
        if ( Input.GetMouseButtonDown( 1 ) == true ) {
            // カメラ回転開始（ホールド）
            cameraRotOrigin_ = Input.mousePosition;
            camera_.setHold( true );
            rButtonDraging_ = true;
        } else if ( Input.GetMouseButtonUp( 1 ) == true ) {
            rButtonDraging_ = false;
            preLatDeg_ = Mathf.Clamp( preLatDeg_, -camera_.getLatitudeLimit(), camera_.getLatitudeLimit() );
            preLongDeg_ = Mathf.Clamp( preLongDeg_, -camera_.getLongitudeLimit(), camera_.getLongitudeLimit() );
        } else if ( Input.GetMouseButtonDown( 2 ) == true ) {
            camera_.setHold( false );
            preLatDeg_ = preLongDeg_ = 0.0f;
        }

        // カメラ動かし中。ドラッグ方向を監視
        if ( rButtonDraging_ == true && Input.GetMouseButton( 1 ) == true ) {
            // ピクセル差分だけ回転
            var curMouseVec = cameraRotOrigin_ - Input.mousePosition;
            cameraRotOrigin_ = Input.mousePosition;
            preLatDeg_ = preLatDeg_ + curMouseVec.y * rotLatDegPerPixel_;
            preLongDeg_ = preLongDeg_ + curMouseVec.x * rotLongDegPerPixel_;
            camera_.setLatitude( preLatDeg_ );
            camera_.setLongitude( preLongDeg_ );
            // Debug.Log( "latDeg = " + latDeg + ", longDeg = " + longDeg + ", " + curMouseVec.ToString() );
        }
    }

    float rotLatDegPerPixel_ = 1.0f;
    float rotLongDegPerPixel_ = 0.5f;
    float preLatDeg_ = 0.0f;
    float preLongDeg_ = 0.0f;
    Cube cube_;
    CubeCamera camera_;
    Vector3 pickUpPos_ = Vector3.zero;
    NormalPiece pickUpPiece_ = null;
    FaceType pickUpFace_ = FaceType.FaceType_None;
    float dragDist_ = 10.0f;
    Vector3 cameraRotOrigin_ = Vector3.zero;
    bool rButtonDraging_ = false;
    float cameraRotRadius_ = 150.0f;    // カメラ回転時のマウス移動距離
}
