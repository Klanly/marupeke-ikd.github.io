using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEditController : CubeController
{
    public CubeEditController(Cube cube, CubeCamera camera)
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
            // カーソル位置にキューブのピースがあれば選択する
            var piece = cube_.ray( Camera.main.ScreenPointToRay( Input.mousePosition ), out pickUpFace_ );
            if ( piece != null ) {
                pickUpPiece_ = piece;

                // 現在セレクト中の色でピックアップ面を塗る
                pickUpPiece_.setFaceColor( pickUpFace_, pasteColor_ );
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

    // 塗りつぶし色を設定
    public void setPasteColor( FaceType color )
    {
        pasteColor_ = color;
    }

    float rotLatDegPerPixel_ = 1.0f;
    float rotLongDegPerPixel_ = 0.5f;
    float preLatDeg_ = 0.0f;
    float preLongDeg_ = 0.0f;
    Cube cube_;
    CubeCamera camera_;
    NormalPiece pickUpPiece_ = null;
    FaceType pickUpFace_ = FaceType.FaceType_None;
    Vector3 cameraRotOrigin_ = Vector3.zero;
    bool rButtonDraging_ = false;
    FaceType pasteColor_ = FaceType.FaceType_Down;
}
