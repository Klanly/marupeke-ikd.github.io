using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    float radius_ = 50.0f;

    [SerializeField]
    int astId_ = 1;

    [SerializeField]
    Star starUnitPrefab_;

    [SerializeField]
    AstLine linePrefab_;

    void Start () {
        DataSet dataSet = new DataSet();
        dataSet.astId_ = astId_;
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
        public List<Star> stars_ = new List<Star>();
        public List<AstLine> lines_ = new List<AstLine>();

        // 星座の方向を向くQuaternionを取得
        public Quaternion lookAtAst()
        {
            // 恒星のAABB原点を算出
            Vector3 min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
            Vector3 max = new Vector3( float.MinValue, float.MinValue, float.MinValue );
            for ( int i = 0; i < stars_.Count; ++i ) {
                var pos = stars_[ i ].transform.position;
                min = Vector3.Min( min, pos );
                max = Vector3.Max( max, pos );
            }
            Vector3 center = ( min + max ) * 0.5f;
            return Quaternion.LookRotation( center );
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
            var d = AsterismDataUtil.getData( dataSet_.astId_ );

            // 恒星
            for ( int i = 0; i < d.stars_.Count; ++i ) {
                var star = d.stars_[ i ];
                var obj = Instantiate<Star>( parent_.starUnitPrefab_ );
                var pos = SphereSurfUtil.convPolerToPos( star.pos_.x, star.pos_.y );
                obj.transform.position = pos * parent_.radius_;
                obj.setHipId( star.hipId_ );
                obj.setPolerCoord( star.pos_.x, star.pos_.y );

                dataSet_.stars_.Add( obj );
            }

            // ライン
            for ( int i = 0; i < d.lines_.Count; ++i ) {
                var line = d.lines_[ i ];
                var obj = Instantiate<AstLine>( parent_.linePrefab_ );
                var spos = SphereSurfUtil.convPolerToPos( line.start_.x, line.start_.y );
                var epos = SphereSurfUtil.convPolerToPos( line.end_.x, line.end_.y );
                obj.setLine( spos * parent_.radius_, epos * parent_.radius_ );

                dataSet_.lines_.Add( obj );
            }
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
            start_ = Camera.main.transform.rotation;
            end_ = dataSet_.lookAtAst();
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t0 = t_ / moveSec_;
            var q = Lerps.Quaternion.easeInOut( start_, end_, t0 );
            Camera.main.transform.rotation = q;
            if ( t_ >= moveSec_ ) {
                return null;
            }
            return this;
        }

        DataSet dataSet_;
        Quaternion start_, end_;
        float moveSec_ = 3.0f;
        float t_ = 0.0f;
    }

    int preId_ = -1;
    State state_;
}
