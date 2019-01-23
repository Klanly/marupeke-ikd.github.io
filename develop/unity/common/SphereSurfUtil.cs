﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球表面関連のユーティリティ

public class SphereSurfUtil {

    // 極座標を直行位置座標に変換
    //  半径は1、緯度経度(0度、0度）は-Z軸方向、経度はY軸右ネジを正とする。
    //  latDeg : 緯度( -90度～90度 ）
    //  longDeg: 経度( -180度～180度 ）
    static public Vector3 convPolerToPos( float latDeg, float longDeg )
    {
        latDeg = pingPong( -90.0f, 90.0f, latDeg ) * Mathf.Deg2Rad;
        longDeg = pingPong( -180.0f, 180.0f, longDeg ) * Mathf.Deg2Rad;
        float y = Mathf.Sin( latDeg );
        float r = Mathf.Cos( latDeg );
        float x = r * Mathf.Sin( longDeg );
        float z = -r * Mathf.Cos( longDeg );
        return new Vector3( x, y, z );
    }

    // 直行位置座標を極座標に変換
    //  直行位置座標は正規化し半径1の球面に転写
    static public void convPosToPoler( Vector3 pos, out float latDeg, out float longDeg )
    {
        pos = pos.normalized;
        if ( pos.z == 0.0f && pos.x == 0.0f ) {
            // 極上
            longDeg = 0.0f;
            latDeg = ( pos.y >= 0.0f ? 90.0f : -90.0f );
            return;
        }
        longDeg = Mathf.Atan2( pos.x, -pos.z ) * Mathf.Rad2Deg;
        latDeg = Mathf.Asin( pos.y ) * Mathf.Rad2Deg;
    }

    // 指定の間をピンポン
    static public float pingPong( float min, float max, float val )
    {
        float lv = Mathf.Abs( val - min );
        float l = max - min;
        int t = ( int )( lv / l );
        float L = lv - l * t;
        int f = ( t % 2 );
        return ( 1 - f ) * ( min + L ) + f * ( max - L );
    }

    // 球面上のAからBへ向かう接線を算出
    // a, bは正規化して半径1の球上に転写
    static public Vector3 calcTangent( Vector3 a, Vector3 b )
    {
        if ( Mathf.Abs( ( a - b ).magnitude ) < 0.0001f )
            return Vector3.zero;

        Vector3 an = a.normalized;
        Vector3 bn = b.normalized;
        Vector3 Vx = Vector3.Cross( a, b - a );
        return Vector3.Cross( Vx, a ).normalized;
    }

    // 球面上の角度（デグリー角）を算出
    //  center: 角度を計る頂点
    //  p0    : centerから伸びる線分の端点0
    //  p1    : centerから伸びる線分の端点1
    static public float calcDeg( Vector3 center, Vector3 p0, Vector3 p1 )
    {
        Vector3 t0 = calcTangent( center, p0 );
        Vector3 t1 = calcTangent( center, p1 );
        return Mathf.Acos( Vector3.Dot( t0, t1 ) ) * Mathf.Rad2Deg;
    }

    // 球面上の角度（ラジアン角）を算出
    //  center: 角度を計る頂点
    //  p0    : centerから伸びる線分の端点0
    //  p1    : centerから伸びる線分の端点1
    static public float calcRad(Vector3 center, Vector3 p0, Vector3 p1)
    {
        Vector3 t0 = calcTangent( center, p0 );
        Vector3 t1 = calcTangent( center, p1 );
        return Mathf.Acos( Vector3.Dot( t0, t1 ) );
    }

    // 球面上の球面三角形の面積を算出
    //  r         : 球の半径
    //  p0, p1, p2: 球面三角形の頂点
    static public float calcArea( float r, Vector3 p0, Vector3 p1, Vector3 p2 )
    {
        float radP0 = calcRad( p0, p1, p2 );
        float radP1 = calcRad( p1, p0, p2 );
        float radP2 = calcRad( p2, p0, p1 );
        return r * r * ( radP0 + radP1 + radP2 - Mathf.PI );
    }

    // 球面三角形補間（ベクトル）
    static public Vector3 triInterpolateV3(
        Vector3 p0, Vector3 p1, Vector3 p2, Vector3 pos,
        Vector3 v0, Vector3 v1, Vector3 v2
    ) {
        float S0 = SphereSurfUtil.calcArea( 1.0f, pos, p1, p2 );
        float S1 = SphereSurfUtil.calcArea( 1.0f, pos, p0, p2 );
        float S2 = SphereSurfUtil.calcArea( 1.0f, pos, p0, p1 );
        return ( v0 * S0 + v1 * S1 + v2 * S2 ) / ( S0 + S1 + S2 );
    }
}
