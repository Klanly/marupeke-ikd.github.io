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

    [SerializeField]
    Canvas canvas_;

    [SerializeField]
    UnityEngine.UI.Text fallObjIdText_;

    [SerializeField]
    UnityEngine.UI.Text positionText_;

    [SerializeField]
    UnityEngine.UI.Text powerText_;

    [SerializeField]
    UnityEngine.UI.Text lookTimeText_;

    [SerializeField]
    GameObject fallObject_;


    public class Parameter {
        public float orbitPointHeightRange_ = 1.5f;
        public OrbitLine orbitLine_;
        public float fallObjPower_ = 400.0f;
        public float fallObjRadius_ = 0.04f;
    }

    // 設置完了
    public System.Action CompleteCallback { set { completeCallback_ = value; } }

    // 落下物出現時間
    public float LookTime { get { return curRemainTime_; } }

    public bool setup(OrbitLine orbitLinePrefab, Parameter param ) {

        param_ = param;
        param_.orbitLine_ = Instantiate<OrbitLine>( orbitLinePrefab );
        param_.orbitLine_.transform.position = Vector3.zero;
        param_.orbitLine_.transform.SetParent( orbitRoot_ );

        var oparam = new OrbitLine.Parameter();
        oparam.gravity_ = 9.81f;
        oparam.initPos_ = SphereSurfUtil.convPolerToPos( Randoms.Float.valueCenter() * 90.0f, Randoms.Float.valueCenter() * 180.0f ) * Random.Range( 10.0f, 15.0f );
        oparam.initVec_ = Randoms.Vec3.angleVariance( oparam.initPos_, 90.0f + Random.Range( 0.0f, 45.0f ) ) * Random.Range( 0.5f, 1.0f );  // 地球側にちょっと向く
        oparam.planetaryRadius_ = 1.0f;
        oparam.stepSec_ = 0.05f;
        oparam.targetHeight_ = 1.5f;

        param_.orbitLine_.setup( oparam );
        param_.orbitLine_.setActiveLine( false );
        curFallObjPower_ = param_.fallObjPower_;
        curRemainTime_ = param_.orbitLine_.getData().getSecToReachTargetHeight();

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

        // テキスト
        curRemainPower_ = new MoveValueFloat( param_.fallObjPower_, 0.2f );
        curRemainPower_.Value = (val) => {
            powerText_.text = string.Format( "Obj. Power: {0}", val );
            if ( val <= 0.0f ) {
                powerText_.color = ColorHelper.getColor( 0x2CEE55FF );
            } else {
                powerText_.color = ColorHelper.getColor( 0xF0FF63FF );
            }
        };
        powerText_.text = string.Format( "Obj. Power: {0}", param_.fallObjPower_ );

        float latDeg = 0.0f;
        float longDeg = 0.0f;
        SphereSurfUtil.convPosToPoler( cameraPos_, out latDeg, out longDeg );
        positionText_.text = string.Format( "Position: {0}{1} {2}{3}", ( latDeg >= 0.0f ? "N" : "S" ), Mathf.Abs( latDeg ), ( longDeg >= 0.0f ? "E" : "W" ), Mathf.Abs( longDeg ) );

        lookTimeText_.text = string.Format( "Look Time: {0}", curRemainTime_ );

        //  Camera.main.transform.position = cameraPos_;
        //  Camera.main.transform.rotation = cameraQ_;
        // debugLine_ =DebugArrowLine.create( aabb_.Center, aabb_.Center + normal * 10.0f );
        return true;
    }

    // アクティブ設定
    public void setActive( bool isActive ) {
        bActive_ = isActive;
        canvas_.gameObject.SetActive( isActive );
        param_.orbitLine_.setActiveLine( isActive );
    }

    // アクティブ？
    public bool isActive() {
        return bActive_;
    }

    // カメラ設定を取得
    public void getCameraPose( out Vector3 position, out Quaternion rotateion ) {
        position = cameraPos_;
        rotateion = cameraQ_;
    }

    // 削除時処理
    private void OnDestroy() {
        if ( param_ != null && param_.orbitLine_ != null )
            Destroy( param_.orbitLine_.gameObject );        
    }

    // 落下物破壊可能フラグをONに
    void enableToBreakFallObject() {
        if ( bEnableBreakFallObject_ == true )
            return;
        bEnableBreakFallObject_ = true;
        if ( completeCallback_ != null )
            completeCallback_();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // 落下物は落ち続ける
        if ( bFallObjectBroken_ == false ) {
            t_ += Time.deltaTime;
            curRemainTime_ -= Time.deltaTime;
            if ( curRemainTime_ < 0.0f )
                curRemainTime_ = 0.0f;

            var data = param_.orbitLine_.getData();
            int idx = ( int )( t_ / data.stepSec_ );
            float inter = t_ % data.stepSec_;
            Vector3 preFallObjPos = Vector3.zero;
            Vector3 fallObjPos = Vector3.zero;
            bool validateFallObjSeg = false;
            if ( idx + 1 < data.orbit_.Count ) {
                fallObjPos = Vector3.Lerp( data.orbit_[ idx ], data.orbit_[ idx + 1 ], inter );
                fallObject_.transform.position = fallObjPos;

                int preIdx = ( int )( ( t_ - Time.deltaTime ) / data.stepSec_ );
                float preInter = ( t_ - Time.deltaTime ) % data.stepSec_;
                preFallObjPos = Vector3.Lerp( data.orbit_[ preIdx ], data.orbit_[ preIdx + 1 ], preInter );

                curFallObjOrbit_.Start = preFallObjPos;
                curFallObjOrbit_.End = fallObjPos;
                validateFallObjSeg = true;
            }

            // シールドとの衝突チェック
            if ( validateFallObjSeg == true ) {
                for ( int i = 0; i < placeShields_.Count; ++i ) {
                    if ( placeShields_[ i ] == null )
                        continue;
                    // 落下物は高速なので線分としてチェック
                    var sp = placeShields_[ i ].transform.position;
                    float s = 0.0f;
                    Vector3 interPos = Vector3.zero;
                    float dist = curFallObjOrbit_.calcDistPoint( sp, out s, out interPos );
                    if ( dist <= param_.fallObjRadius_ ) {
                        // 衝突
                        float shieldPower = placeShields_[ i ].Power;
                        placeShields_[ i ].explosion(); // シールド爆発
                        placeShields_[ i ] = null;

                        // 落下物のパワー減少
                        curFallObjPower_ -= shieldPower;
                        if ( curFallObjPower_ <= 0.0f ) {
                            // 落下物破壊
                            bFallObjectBroken_ = true;
                            fallObject_.gameObject.SetActive( false );
                        }
                    }
                }
            }
        }

        if ( bActive_ == false )
            return;

        // ラインとのコリジョン
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        cursorSeg_.Start = ray.origin;
        cursorSeg_.End = cursorSeg_.Start + ray.direction * 10.0f;

        // debugLine_.Start = cursorSeg_.Start;
        // debugLine_.End = cursorSeg_.End;

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
        } else {
            cursorObj_.transform.position = Vector3.zero;
        }

        // クリックしたらシールド設置
        if ( Input.GetMouseButtonDown( 0 ) == true && colSeg != null ) {
            var shield = Instantiate<Shild>( cursorObj_ );
            shield.transform.SetParent( shildRoot_ );
            placeShields_.Add( shield );

            // 落下物に与えるパワーが破壊値に達したら
            // 破壊可能フラグをON
            totalBreakPower_ += shield.Power;
            if ( totalBreakPower_ >= param_.fallObjPower_ )
                enableToBreakFallObject();

            curRemainPower_.setAim( param_.fallObjPower_ - totalBreakPower_ );
        }

        // 残り時間更新
        lookTimeText_.text = string.Format( "Look Time: {0}", curRemainTime_ );
    }

    Parameter param_;
    AABB aabb_ = new AABB();
    Vector3 cameraPos_ = Vector3.zero;
    Quaternion cameraQ_ = Quaternion.identity;
    List<Segment> segments_ = new List<Segment>();
    Segment cursorSeg_ = new Segment();
    // DebugArrowLine debugLine_;
    bool bActive_ = true;
    bool bEnableBreakFallObject_ = false;
    System.Action completeCallback_ = null;
    float totalBreakPower_ = 0.0f;
    MoveValueFloat curRemainPower_ = null;
    float curFallObjPower_ = 0.0f;
    float curRemainTime_ = 0.0f;
    float t_ = 0.0f;
    List<Shild> placeShields_ = new List<Shild>();
    Segment curFallObjOrbit_ = new Segment();
    bool bFallObjectBroken_ = false;
}
