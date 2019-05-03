using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 処理現場管理人

public class SiteManager : MonoBehaviour {

    public class Parameter {
        public float orbitPointHeightRange_ = 1.5f;
        public OrbitLine orbitLine_;
    }

    public bool setup( Parameter param ) {
        param = param_;

        // カメラアングル算出
        // ある高さH(orbitPointHeightRange_)までに含まれる軌道点から軌道を含む面法線Nを算出。
        // それらの平均座標がカメラのLookAtポイント。
        // Nの方向にカメラを下げて対象軌道点を画角内に捉える。
        // Upベクトルはカメラの位置ベクトル（地球を真下に見る）

        var data = param.orbitLine_.getData();
        var list = new List<Vector3>();
        for ( int i = data.orbit_.Count - 1; i >= 0; --i ) {
            if ( data.orbit_[ i ].magnitude <= param.orbitPointHeightRange_ )
                list.Add( data.orbit_[ i ] );
        }
        if ( list.Count <= 2 ) {
            return false;   // 対象となる軌道が無い
        }

        var normal = Vector3.Cross( list[ 1 ] - list[ 0 ], list[ 2 ] - list[ 0 ] ).normalized;
        aabb_.set( list );
        CameraUtil.fitAABB( Camera.main, aabb_.Center - normal, aabb_.Center.normalized, aabb_, out cameraPos_, out cameraQ_ );

        return true;
    }

    // 削除時処理
    private void OnDestroy() {
        if ( param_ != null && param_.orbitLine_ != null )
            Destroy( param_.orbitLine_.gameObject );        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Parameter param_;
    AABB aabb_;
    Vector3 cameraPos_;
    Quaternion cameraQ_;
}
