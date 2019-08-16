using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突判定ヘルパー

public class CollideUtil {

    // レイと平面の衝突
    //  colPos: 衝突点
    //  start : レイの開始点
    //  dir   : レイの方向
    //  p0    : 平面上の一点
    //  n     : 平面の法線
    // 戻り値 : 衝突していたらtrue
    static public bool colPosRayPlane( out Vector3 colPos, Vector3 start, Vector3 dir, Vector3 p0, Vector3 n )
    {
        float b = Vector3.Dot( dir, n );
        if ( Mathf.Abs( b ) < 0.00001f ) {
            // レイと平面が平行と判断
            colPos = Vector3.zero;
            return false;
        }
        float a = Vector3.Dot( p0 - start, n ) / Vector3.Dot( dir, n );
        colPos = start + a * dir;
        return a >= 0.0f;
    }
}
