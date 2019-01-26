using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    float radius_ = 50.0f;

    [SerializeField]
    float skyRadius_ = 200.0f;

    [SerializeField]
    int astId_ = 1;

    [SerializeField]
    Star starUnitPrefab_;

    [SerializeField]
    AstLine linePrefab_;

    // 天球
    class Sky
    {
        public class Asterism
        {
            public List<Star> stars_ = new List<Star>();        // 恒星
            public List<AstLine> lines_ = new List<AstLine>();  // 星座間ライン
        }
        public List< Asterism > asterisms_ = new List<Asterism>();  // 星座IDに対応した星座
    }

    void Start () {
        // 天球を作成
        sky_ = new Sky();
        for ( int i = 1; i <= 89; ++i ) {
            var ast = new Sky.Asterism();
            createAsterism( i, skyRadius_, starUnitPrefab_, linePrefab_, ref ast.stars_, ref ast.lines_ );
            sky_.asterisms_.Add( ast );
        }

        // データセット作成
        DataSet dataSet = new DataSet();
        dataSet.astId_ = astId_;
        dataSet.radius_ = radius_;
        state_ = new SetQuestion( this, dataSet );
    }
	
	// Update is called once per frame
	void Update () {

        if ( state_ != null )
            state_ = state_.update();

        if ( preId_ != astId_ ) {
            preId_ = astId_;
        }
    }

    // 星座を並べる
    void createAsterism( int astId, float radius, Star starPrefab, AstLine linePrefab, ref List< Star > stars, ref List< AstLine > lines,  float randomRange = 0.0f )
    {
        var d = AsterismDataUtil.getData( astId );

        // 恒星のAABB範囲を先に計算
        List<Vector3> poses = new List<Vector3>();
        for ( int i = 0; i < d.stars_.Count; ++i ) {
            var star = d.stars_[ i ];
            var pos = SphereSurfUtil.convPolerToPos( star.pos_.x, star.pos_.y );
            poses.Add( pos );
        }
        Vector3 aabbMin = Vector3.zero;
        Vector3 aabbMax = Vector3.zero;
        Ranges.aabb3( poses, out aabbMin, out aabbMax );
        float range = ( aabbMax - aabbMin ).magnitude * 0.5f * radius;

        // 恒星
        var starPosDict = new Dictionary<int, Vector3>();
        for ( int i = 0; i < d.stars_.Count; ++i ) {
            var star = d.stars_[ i ];
            var obj = Instantiate<Star>( starPrefab );
            var pos = SphereSurfUtil.convPolerToPos( star.pos_.x, star.pos_.y );
            obj.transform.position = pos * ( radius + Random.Range( -range, range ) * randomRange );
            obj.setHipId( star.hipId_ );
            obj.setPolerCoord( star.pos_.x, star.pos_.y );

            stars.Add( obj );
            starPosDict[ star.hipId_ ] = obj.transform.position;
        }

        // ライン
        for ( int i = 0; i < d.lines_.Count; ++i ) {
            var line = d.lines_[ i ];
            var obj = Instantiate<AstLine>( linePrefab );
            var spos = starPosDict[ line.startHipId_ ];
            var epos = starPosDict[ line.endHipId_ ];
//            var spos = SphereSurfUtil.convPolerToPos( line.start_.x, line.start_.y );
//            var epos = SphereSurfUtil.convPolerToPos( line.end_.x, line.end_.y );
            obj.setLine( spos, epos );

            lines.Add( obj );
        }
    }


    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    class DataSet
    {
        public int astId_;
        public float radius_;
        public List<Star> stars_ = new List<Star>();
        public List<AstLine> lines_ = new List<AstLine>();
        public GameObject root_ = null;
        public Vector3 center_ = Vector3.zero;
        public Quaternion lookQuaternion_ = Quaternion.identity;
        public Vector3 aabbMin_ = Vector3.zero;
        public Vector3 aabbMax_ = Vector3.zero;
        bool bCalcCenter_ = false;

        // センター位置を取得
        public Vector3 getCenter()
        {
            if ( bCalcCenter_ == false ) {
                lookAtAst();
            }
            return center_;
        }

        // 星座の方向を向くQuaternionを取得
        public Quaternion lookAtAst()
        {
            if ( bCalcCenter_ == true ) {
                return lookQuaternion_;
            }

            // 恒星のAABB原点を算出
            Vector3 min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
            Vector3 max = new Vector3( float.MinValue, float.MinValue, float.MinValue );
            for ( int i = 0; i < stars_.Count; ++i ) {
                var pos = stars_[ i ].transform.position;
                min = Vector3.Min( min, pos );
                max = Vector3.Max( max, pos );
            }
            center_ = ( min + max ) * 0.5f;
            lookQuaternion_ = Quaternion.LookRotation( center_ );
            aabbMin_ = min;
            aabbMax_ = max;
            return lookQuaternion_;
        }

        // 恒星とラインの親をrootに
        public void toRoot()
        {
            if ( root_ == null )
                return;
            foreach ( var star in stars_ ) {
                star.transform.parent = root_.transform;
            }
            foreach ( var line in lines_ ) {
                line.transform.parent = root_.transform;
            }
        }
    }

    // 問題をセットする
    class SetQuestion : StateBase
    {
        public SetQuestion( GameManager manager, DataSet dataSet ) : base( manager )
        {
            dataSet_ = dataSet;
        }

        // 問題を作成する
        void createAsterism()
        {
            parent_.createAsterism( dataSet_.astId_, dataSet_.radius_, parent_.starUnitPrefab_, parent_.linePrefab_, ref dataSet_.stars_, ref dataSet_.lines_, 1.0f );

            dataSet_.root_ = new GameObject( "root" );
            dataSet_.root_.transform.parent = parent_.transform;
            dataSet_.root_.transform.position = dataSet_.getCenter();
            dataSet_.toRoot();
        }

        // 内部初期化
        override protected void innerInit()
        {
            createAsterism();
            next_ = new Intro_CameraToAst( parent_, dataSet_ );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return next_;
        }

        DataSet dataSet_;
        State next_;
    }

    //  カメラを指定の星座の中心位置へセット
    class Intro_CameraToAst : StateBase
    {
        public Intro_CameraToAst(GameManager parent, DataSet dataSet ) : base( parent ) {
            dataSet_ = dataSet;
        }

        // 内部初期化
        override protected void innerInit()
        {
            var center = Vector3.zero;
            start_ = Camera.main.transform.rotation;
            end_ = dataSet_.lookAtAst();

            // TEST
            var t = Instantiate<Star>( parent_.starUnitPrefab_ );
            t.transform.localScale = Vector3.one * 2.0f;
            t.transform.position = dataSet_.getCenter();
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t0 = t_ / moveSec_;
            var q = Lerps.Quaternion.easeInOut( start_, end_, t0 );
            Camera.main.transform.rotation = q;
            if ( t_ >= moveSec_ ) {
                return new Gaming( parent_, dataSet_ );
            }
            return this;
        }

        DataSet dataSet_;
        Quaternion start_, end_;
        float moveSec_ = 3.0f;
        float t_ = 0.0f;
    }

    // ゲーム中
    //  星座を回転可能に
    class Gaming : StateBase
    {
        public Gaming( GameManager parent, DataSet dataSet ) : base( parent ) {
            dataSet_ = dataSet;
        }

        // 内部初期化
        override protected void innerInit()
        {
            viewer_.setup( Camera.main.transform, dataSet_.root_, 130.0f );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            viewer_.update();
            return this;
        }

        ObjectViewer viewer_ = new ObjectViewer();
        DataSet dataSet_;
    }

    int preId_ = -1;
    State state_;
    Sky sky_ = new Sky();
}
