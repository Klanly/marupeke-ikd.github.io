using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラユーティリティ

public class CameraUtil {
    // ワールド点をビュー空間座標に変換
    static public Vector3 worldToView( Camera camera, Vector3 point ) {
        var viewMat = camera.worldToCameraMatrix;
        return viewMat.MultiplyPoint( point );
    }

    // AABBが収まるカメラの位置と角度を計算
    //  camera: 対象カメラ。fovとaspectを得るために使う
    static public void fitAABB( Camera camera, Vector3 forward, Vector3 up, AABB aabb, out Vector3 pos, out Quaternion q ) {
        var forwardN = forward.normalized;
        var cameraPos = aabb.Center - forwardN;
        var viewMat = Matrix4x4.LookAt( cameraPos, aabb.Center, up ).inverse;
        var vertices = aabb.getVertices();
        float fovY = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float asp = camera.aspect;
        float tan = Mathf.Tan( fovY );
        float curZ = float.MaxValue;
        foreach ( var p in vertices ) {
            var vp = viewMat.MultiplyPoint( p );
            float ly = Mathf.Abs( vp.y ) / tan;
            float lx = Mathf.Abs( vp.x ) / ( tan * asp );
            if ( vp.z - lx < curZ ) {
                curZ = vp.z - lx;
            }
            if ( vp.z - ly < curZ ) {
                curZ = vp.z - ly;
            }
        }
        pos = cameraPos + curZ * forwardN;
        q = Quaternion.LookRotation( aabb.Center - pos, up );
    }

    // マウスクリックした座標の先にある任意平面との衝突位置取得
    static public bool calcClickPosition( Camera camera, Vector3? screenPos, out Vector3 pos ) {
        var touchPos = screenPos ?? Input.mousePosition;
        var plane = new Plane( Vector3.forward, 0 );
        var ray = camera.ScreenPointToRay( touchPos );
        if ( plane.Raycast( ray, out float enter ) ) {
            pos = ray.GetPoint( enter );
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
