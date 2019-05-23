using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラピッカー
//
//  カメラ空間前にある仮想平面をピックした位置を保ったまま
//  ドラッグした時にカメラの位置を調整する

public class CameraPicker {

    // スクリーン座標を指定平面座標へ変換
    Vector3 calcPlanePos( Vector2Int screenPos ) {

        Vector3 p = cameraRot_ * new Vector3( screenPos.x, screenPos.y, D_ ) + C_;
        var p_c = p - C_;
        float a = Vector3.Dot( P0_ - C_, N_ ) / Vector3.Dot( p_c, N_ );
        return C_ + p_c * a;
    }

    // ピッキング開始
    //  targetCamera     : ピッキング移動を行うカメラ
    //  screenPos        : ピッキング位置（スクリーン座標）
    //  screenHeight     : 画面の高さピクセル数
    //  planeN           : 対象平面の法線
    //  planeP0          : 対象平面上の一点
    public void startPicking( Camera targetCamera, Vector2Int screenPos, float screenHeight, Vector3 planeN, Vector3 planeP0 ) {
        targetCamera_ = targetCamera;
        C_ = targetCamera.transform.position;
        cameraRot_ = targetCamera.transform.rotation;
        float fov = targetCamera.fieldOfView * Mathf.Deg2Rad / 2.0f;
        D_ = screenHeight / ( 2.0f * Mathf.Tan( fov ) );
        N_ = planeN;
        P0_ = planeP0;
        Pd_ = calcPlanePos( screenPos );
    }

    // ピッキング開始
    //  targetCamera     : ピッキング移動を行うカメラ
    //  screenPosLB00    : ピッキング位置（Input.mousePositionが返す座標）
    //  screenHeight     : 画面の高さピクセル数
    //  planeN           : 対象平面の法線
    //  planeP0          : 対象平面上の一点
    public void startPicking(Camera targetCamera, Vector3 screenPosLB00, Vector3 planeN, Vector3 planeP0) {
        startPicking( targetCamera, new Vector2Int( ( int )screenPosLB00.x - Screen.width / 2, ( int )screenPosLB00.y - Screen.height / 2 ), Screen.height, planeN, planeP0 );
    }

    // カメラの位置を更新
    //  screenPos : 更新時のピッキング位置
    public void updateCameraPos( Vector2Int screenPos ) {
        var Qd = calcPlanePos( screenPos );
        targetCamera_.transform.position = C_ + ( Pd_ - Qd );
    }

    // カメラの位置を更新
    //  screenPosLB00: 更新時のピッキング位置（Input.mousePositionが返す座標）
    public void updateCameraPos( Vector3 screenPosLB00 ) {
        updateCameraPos( new Vector2Int( ( int )screenPosLB00.x - Screen.width / 2, ( int )screenPosLB00.y - Screen.height / 2 ) );
    }

    Camera targetCamera_;       // ターゲットカメラ
    Quaternion cameraRot_;      // カメラ回転
    Vector3 C_;                 // ピッキング開始時カメラ位置
    float D_;                   // 仮想スクリーン平面までの距離
    Vector3 Pd_;                // 指定平面上のフィット対象位置
    Vector3 N_;                 // 指定平面の法線
    Vector3 P0_;                // 指定平面上の一点
}
