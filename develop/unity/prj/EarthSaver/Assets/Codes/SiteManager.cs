using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 処理現場管理人

public class SiteManager : MonoBehaviour {

    [SerializeField]
    Transform orbitRoot_;

    [SerializeField]
    Shild cursorObj_;

    [SerializeField]
    Transform shildRoot_;


    public class Parameter {
        public float orbitPointHeightRange_ = 1.5f;
        public OrbitLine orbitLine_;
    }

    public bool setup(OrbitLine orbitLinePrefab, Parameter param ) {

        param_ = param;
        param_.orbitLine_ = Instantiate<OrbitLine>( orbitLinePrefab );
        param_.orbitLine_.transform.position = Vector3.zero;
        param_.orbitLine_.transform.SetParent( orbitRoot_ );

        var oparam = new OrbitLine.Parameter();
        oparam.gravity_ = 9.81f;
        oparam.initPos_ = SphereSurfUtil.convPolerToPos( Randoms.Float.valueCenter() * 90.0f, Randoms.Float.valueCenter() * 180.0f ) * Random.Range( 1.8f, 3.0f );
        oparam.initVec_ = Randoms.Vec3.angleVariance( oparam.initPos_, 90.0f + Random.Range( 0.0f, 45.0f ) ) * Random.Range( 0.5f, 1.5f );  // 地球側にちょっと向く
        oparam.planetaryRadius_ = 1.0f;
        oparam.stepSec_ = 0.05f;
        oparam.targetHeight_ = 1.5f;

        param_.orbitLine_.setup( oparam );

        // カメラアングル算出
        // ある高さH(orbitPointHeightRange_)までに含まれる軌道点から軌道を含む面法線Nを算出。
        // それらの平均座標がカメラのLookAtポイント。
        // Nの方向にカメラを下げて対象軌道点を画角内に捉える。
        // Upベクトルはカメラの位置ベクトル（地球を真下に見る）

        var data = param.orbitLine_.getData();
        var list = new List<Vector3>();
        for ( int i = data.orbit_.Count - 1; i >= 0; --i ) {
            if ( data.orbit_[ i ].magnitude <= param.orbitPointHeightRange_ ) {
                list.Add( data.orbit_[ i ] );
            }
        }
        if ( list.Count <= 2 ) {
            return false;   // 対象となる軌道が無い
        }
        for ( int i = 1; i < data.orbit_.Count; ++i ) {
            segments_.Add( new Segment( data.orbit_[ i - 1 ], data.orbit_[ i ] ) );
        }
        var normal = Vector3.Cross( list[ 1 ] - list[ 0 ], list[ 2 ] - list[ 0 ] ).normalized;
        aabb_.set( list );
        CameraUtil.fitAABB( Camera.main, normal, aabb_.Center.normalized, aabb_, out cameraPos_, out cameraQ_ );

        Camera.main.transform.position = cameraPos_;
        Camera.main.transform.rotation = cameraQ_;

        debugLine_ =DebugArrowLine.create( aabb_.Center, aabb_.Center + normal * 10.0f );
        return true;
    }

    // アクティブ設定
    public void setActive( bool isActive ) {
        bActive_ = isActive;
    }

    // アクティブ？
    public bool isActive() {
        return bActive_;
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
        if ( bActive_ == false )
            return;

        // ラインとのコリジョン
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        cursorSeg_.Start = ray.origin;
        cursorSeg_.End = cursorSeg_.Start + ray.direction * 10.0f;

        debugLine_.Start = cursorSeg_.Start;
        debugLine_.End = cursorSeg_.End;

        Vector3 cp = Vector3.zero;
        Vector3 rayCp = Vector3.zero;
        float width = 0.03f;
        float minDist = float.MaxValue;
        Segment colSeg = null;
        foreach( var seg in segments_ ) {
            float s = 0.0f;
            float t = 0.0f;
            Vector3 tmpCp = Vector3.zero;
            float dist = seg.calcDistSegment( cursorSeg_, out s, out t, out tmpCp, out rayCp );
            if ( dist <= width && dist < minDist ) {
                minDist = dist;
                cp = tmpCp;
                colSeg = seg;
            }
        }
        if ( colSeg != null ) {
            cursorObj_.transform.position = cp;
            cursorObj_.transform.rotation = Quaternion.LookRotation( colSeg.Ray );
        }

        // クリックしたらシールド設置
        if ( Input.GetMouseButton( 0 ) == true && colSeg != null ) {
            var shild = Instantiate<Shild>( cursorObj_ );
            shild.transform.SetParent( shildRoot_ );
        }
    }

    Parameter param_;
    AABB aabb_ = new AABB();
    Vector3 cameraPos_ = Vector3.zero;
    Quaternion cameraQ_ = Quaternion.identity;
    List<Segment> segments_ = new List<Segment>();
    Segment cursorSeg_ = new Segment();
    DebugArrowLine debugLine_;
    bool bActive_ = true;
}
