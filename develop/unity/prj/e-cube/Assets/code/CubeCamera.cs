using System.Collections;
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
        if ( moveLatVal_ == null )
            moveLatVal_ = new MoveValue( aimLatDeg_, 0.3f, 0.0001f );
        moveLatVal_.setAim( aimLatDeg_ );
    }

    // 緯度の限界値を取得
    public float getLatitudeLimit()
    {
        return latLimitDeg_;
    }

    // ゴールとなる経度を設定
    public void setLongitude( float deg )
    {
        if ( Mathf.Abs( deg ) > longLimitDeg_ ) {
            deg = ( deg >= 0.0f ? longLimitDeg_ : -longLimitDeg_ );
        }
        aimLongDeg_ = deg;
        if ( moveLongVal_ == null )
            moveLongVal_ = new MoveValue( aimLatDeg_, 0.3f, 0.0001f );
        moveLongVal_.setAim( aimLongDeg_ );
    }

    // 経度の限界値を取得
    public float getLongitudeLimit()
    {
        return longLimitDeg_;
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
    }

    void Start () {
        // カメラの位置を指定距離、緯度、経度に合わせる
        Vector3 pos = SphereSurfUtil.convPolerToPos( aimLatDeg_, aimLongDeg_ ) * dist_;
        camera_.gameObject.transform.localPosition = pos;
        camera_.gameObject.transform.LookAt( rotRoot_, Vector3.up );
        camera_.fieldOfView = fovYDeg_;
        setLatitude( aimLatDeg_ );
        setLongitude( aimLongDeg_ );
    }
	
	void Update () {
        // カメラ位置を更新、目標点に定める
        setLatitude( aimLatDeg_ );
        setLongitude( aimLongDeg_ );
        if ( bHold_ == false ) {
            setLatitude( 0.0f );
            setLongitude( 0.0f );
        }
        Vector3 pos = SphereSurfUtil.convPolerToPos( moveLatVal_.update(), moveLongVal_.update() ) * dist_;
        // moveSlerp_.setAim( pos );
        Vector3 up = calcUpVector( pos.normalized );
        camera_.gameObject.transform.localPosition = pos;
        camera_.gameObject.transform.LookAt( rotRoot_, up );
        camera_.fieldOfView = fovYDeg_;
    }

//    MoveSlerp moveSlerp_;
    MoveValue moveLatVal_;
    MoveValue moveLongVal_;

    [SerializeField]
    float aimLatDeg_ = 0.0f;   // 目標経度角
    [SerializeField]
    float aimLongDeg_ = 0.0f;  // 目標緯度角
    [SerializeField]
    bool bHold_ = false;
}
