﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブカメラ
//
//  キューブの周囲を映すカメラ
//  Frontを見失わない為に未操作の場合はフロントに戻る
//  カメラの視線は基本回転中心位置を向く。緯度経度法で管理。(0度、0度)は-Z軸方向とする。
//  カメラの姿勢はキューブの各面を正面に捉えた時に面の上方向になるよう球面三角形補間を用いる。

public class CubeCamera : MonoBehaviour {

    [SerializeField]
    Transform rotRoot_; // 回転中心

    [SerializeField]
    Camera camera_;     // 撮影カメラ

    [SerializeField]
    float dist_;        // 中心点からのカメラ距離

    [SerializeField]
    float latLimitDeg_ = 50.0f;     // 緯度上限

    [SerializeField]
    float longLimitDeg_ = 130.0f;   // 経度上限

    [SerializeField]
    float fovYDeg_ = 30.0f;    // 画角


    // カメラ位置を直行軸座標でダイレクト指定
    public void setCameraPosDirect( Vector3 pos )
    {
        dist_ = pos.magnitude;
        SphereSurfUtil.convPosToPoler( pos, out aimLatDeg_, out aimLongDeg_ );
    }

    // ゴールとなる緯度を設定
    public void setLatitude( float deg )
    {
        if ( Mathf.Abs( deg ) > latLimitDeg_ ) {
            deg = ( deg >= 0.0f ? latLimitDeg_ : -latLimitDeg_ );
        }
        aimLatDeg_ = deg;
    }

    // ゴールとなる経度を設定
    public void setLongitude( float deg )
    {
        if ( Mathf.Abs( deg ) > longLimitDeg_ ) {
            deg = ( deg >= 0.0f ? longLimitDeg_ : -longLimitDeg_ );
        }
        aimLongDeg_ = deg;
    }

    // カメラ位置をホールド（緯度、経度を保持）
    public void setHold( bool isHold )
    {
        bHold_ = isHold;
    }

    // Upベクトル算出
    Vector3 calcUpVector( Vector3 pos )
    {
        return Vector3.up;

        float lat = 0.0f;
        float longi = 0.0f;
        SphereSurfUtil.convPosToPoler( pos, out lat, out longi );

        // 補間Upベクトル算出
        Vector3 left = new Vector3( -1.0f, 0.0f, 0.0f );
        Vector3 right = new Vector3( 1.0f, 0.0f, 0.0f );
        Vector3 down = new Vector3( 0.0f, -1.0f, 0.0f );
        Vector3 up = new Vector3( 0.0f, 1.0f, 0.0f );
        Vector3 front = new Vector3( 0.0f, 0.0f, -1.0f );
        Vector3 back = new Vector3( 0.0f, 0.0f, 1.0f );

        Vector3 leftUp = new Vector3( 0.0f, 1.0f, 0.0f );
        Vector3 rightUp = new Vector3( 0.0f, 1.0f, 0.0f );
        Vector3 downUp = new Vector3( 0.0f, 0.0f, -1.0f );
        Vector3 upUp = new Vector3( 0.0f, 0.0f, 1.0f );
        Vector3 frontUp = new Vector3( 0.0f, 1.0f, 0.0f );
        Vector3 backUp = new Vector3( 0.0f, 1.0f, 0.0f );

        if ( lat >= 0.0f ) {
            if ( longi <= 0.0f ) {
                if ( longi >= -90.0f ) {
                    // L,F,U領域
                    return SphereSurfUtil.triInterpolateV3( left, front, up, pos, leftUp, frontUp, upUp );
                } else {
                    // L,B,U領域
                    return SphereSurfUtil.triInterpolateV3( left, back, up, pos, leftUp, backUp, upUp );
                }
            } else {
                if ( longi <= 90.0f ) {
                    // R,F,U領域
                    return SphereSurfUtil.triInterpolateV3( right, front, up, pos, rightUp, frontUp, upUp );
                } else {
                    // R,B,U領域
                    return SphereSurfUtil.triInterpolateV3( right, back, up, pos, rightUp, backUp, upUp );
                }
            }
        } else {
            if ( longi <= 0.0f ) {
                if ( longi >= -90.0f ) {
                    // L,F,D領域
                    return SphereSurfUtil.triInterpolateV3( left, front, down, pos, leftUp, frontUp, downUp );
                } else {
                    // L,B,D領域
                    return SphereSurfUtil.triInterpolateV3( left, back, down, pos, leftUp, backUp, downUp );
                }
            } else {
                if ( longi <= 90.0f ) {
                    // R,F,D領域
                    return SphereSurfUtil.triInterpolateV3( right, front, down, pos, rightUp, frontUp, downUp );
                } else {
                    // R,B,D領域
                    return SphereSurfUtil.triInterpolateV3( right, back, down, pos, rightUp, backUp, downUp );
                }
            }
        }
    }

    void Start () {
        // カメラの位置を指定距離、緯度、経度に合わせる
        Vector3 pos = SphereSurfUtil.convPolerToPos( aimLatDeg_, aimLongDeg_ ) * dist_;
        camera_.gameObject.transform.localPosition = pos;
        camera_.gameObject.transform.LookAt( rotRoot_, Vector3.up );
        camera_.fieldOfView = fovYDeg_;
        moveSlerp_ = new MoveSlerp( pos, 0.1f, 0.01f );
    }
	
	void Update () {
        // カメラ位置を更新、目標点に定める
        setLatitude( aimLatDeg_ );
        setLongitude( aimLongDeg_ );
        if ( bHold_ == false ) {
            aimLatDeg_ = aimLongDeg_ = 0.0f;
        }
        Vector3 pos = SphereSurfUtil.convPolerToPos( aimLatDeg_, aimLongDeg_ ) * dist_;
        moveSlerp_.setAim( pos );
        pos = moveSlerp_.update();
        Vector3 up = calcUpVector( pos.normalized );
        camera_.gameObject.transform.localPosition = pos;
        camera_.gameObject.transform.LookAt( rotRoot_, up );
        camera_.fieldOfView = fovYDeg_;
    }

    MoveSlerp moveSlerp_;
    [SerializeField]
    float aimLatDeg_ = 0.0f;   // 目標経度角
    [SerializeField]
    float aimLongDeg_ = 0.0f;  // 目標緯度角
    [SerializeField]
    bool bHold_ = false;
}
