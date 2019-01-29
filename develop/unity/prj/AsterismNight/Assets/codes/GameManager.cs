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

    [SerializeField]
    Star skyStarPrefab_;

    [SerializeField]
    AstLine skyLinePrefab_;

    [SerializeField]
    AsterismDesc descPrefab_;

    [SerializeField]
    Canvas canvas_;

    [SerializeField]
    float curAngle_;

    [SerializeField]
    SkyAsterismName skyAstNamePrefab_;

    [SerializeField]
    TitleManager titleManagerPrefab_;


    // 天球
    class Sky
    {
        // 天空上の星座
        public class Asterism
        {
            public List<Star> stars_ = new List<Star>();        // 恒星
            public List<AstLine> lines_ = new List<AstLine>();  // 星座間ライン
            public SkyAsterismName skyAstName_;                 // 星座名
        }
        public List< Asterism > asterisms_ = new List<Asterism>();  // 星座IDに対応した星座
    }

    // 天球の星座のα値とカラースケールを設定
    void setSkyAsterisumAlpha( int astId, float alpha, float colorScale )
    {
        foreach( var star in sky_.asterisms_[ astId - 1 ].stars_ ) {
            star.setAlpha( alpha );
            star.setColorScale( colorScale );
        }
        foreach ( var line in sky_.asterisms_[ astId - 1 ].lines_ ) {
            line.setAlpha( alpha );
            line.setColorScale( colorScale );
        }
    }

    // 天球に星座名を刻印
    void setSkyAsterismName( int astId )
    {
        var list = new List<Vector3>();
        foreach( var star in sky_.asterisms_[ astId - 1 ].stars_ ) {
            list.Add( star.transform.position );
        }
        Vector3 min = Vector3.zero, max = Vector3.zero;
        Ranges.aabb3( list, out min, out max );
        Vector3 pos = ( min + max ) * 0.5f;

        var obj = Instantiate<SkyAsterismName>( skyAstNamePrefab_ );
        obj.transform.position = pos;
        Vector3 up = Camera.main.transform.up;
        Vector3 forward = -pos;
        obj.transform.rotation = Quaternion.LookRotation( forward, up );

        obj.setup( astId );
        sky_.asterisms_[ astId - 1 ].skyAstName_ = obj;
    }

    // 次の問題を出題
    void notifyNextQuestion( int forceAstId = -1 )
    {
        if ( curAnswerNum_ >= 89 ) {
            // 全て開けている
            nextState_ = new Ending( this );
            return;
        }

        // データセット作成
        dataSet_ = new DataSet();
        dataSet_.astId_ = forceAstId >= 1 ? forceAstId : questionAstIds_[ curAnswerNum_ ];
        dataSet_.radius_ = radius_;
        dataSet_.sky_ = sky_;
        dataSet_.selectMode_ = selectMode_;
        nextState_ = new SetQuestion( this, dataSet_ );
    }

    void Start () {
        // 天球を作成
        sky_ = new Sky();
        for ( int i = 1; i <= 89; ++i ) {
            var ast = new Sky.Asterism();
            createAsterism( i, skyRadius_, skyStarPrefab_, skyLinePrefab_, ref ast.stars_, ref ast.lines_, false, 0.0f, 0.7f, 0.0f );
            sky_.asterisms_.Add( ast );
        }

        // 天球のラインはすべて初期状態で非表示
        foreach ( var ast in sky_.asterisms_ ) {
            foreach ( var line in ast.lines_ ) {
                line.gameObject.SetActive( false );
            }
        }

        // 全89星座の出題順番
        for ( int i = 1; i <= 89; ++i )
            questionAstIds_.Add( i );
        ListUtil.shuffle<int>( ref questionAstIds_, Random.Range( 0, 10000 ) );

        state_ = new Title( this );
    }

    // Update is called once per frame
    void Update () {

        if ( nextState_ != null ) {
            state_ = nextState_;
            nextState_ = null;
        }

        if ( state_ != null )
            state_ = state_.update();

        if ( preId_ != astId_ ) {
            preId_ = astId_;
        }

        if ( dataSet_ != null )
            curAngle_ = dataSet_.curAngle_;
    }

    // 星座を並べる
    void createAsterism(
        int astId,
        float radius,
        Star starPrefab,
        AstLine linePrefab,
        ref List< Star > stars,
        ref List< AstLine > lines,
        bool useAutoScale,
        float alpha,
        float colorScale,
        float randomRange
    )
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
        float diagoLen = ( aabbMax - aabbMin ).magnitude;
        float range = diagoLen * 0.5f * radius;
        float astRangeScale = ( useAutoScale ? 0.01f + 0.1f * range : 1.0f );

        // 恒星
        var starPosDict = new Dictionary<int, Vector3>();
        for ( int i = 0; i < d.stars_.Count; ++i ) {
            var star = d.stars_[ i ];
            var obj = Instantiate<Star>( starPrefab );
            var pos = SphereSurfUtil.convPolerToPos( star.pos_.x, star.pos_.y );
            float R = radius + Random.Range( -range, range ) * randomRange; // 距離
            obj.transform.position = pos * R;
            var scale = obj.transform.localScale;
            obj.transform.localScale = scale * ( R / radius ) * astRangeScale;
            obj.setHipId( star.hipId_ );
            obj.setPolerCoord( star.pos_.x, star.pos_.y );
            obj.setColor( star.color_, alpha );

            stars.Add( obj );
            starPosDict[ star.hipId_ ] = obj.transform.position;
        }

        // ライン
        Vector3 lineColor = new Vector3( 0.2f, 0.3f, 0.5f );
        for ( int i = 0; i < d.lines_.Count; ++i ) {
            var line = d.lines_[ i ];
            var obj = Instantiate<AstLine>( linePrefab );
            var spos = starPosDict[ line.startHipId_ ];
            var epos = starPosDict[ line.endHipId_ ];
//            var spos = SphereSurfUtil.convPolerToPos( line.start_.x, line.start_.y );
//            var epos = SphereSurfUtil.convPolerToPos( line.end_.x, line.end_.y );
            obj.setLine( spos, epos, astRangeScale );
            obj.setColor( lineColor, alpha * 0.6f );

            lines.Add( obj );
        }
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
        public Quaternion answerQ_ = Quaternion.identity;
        public float curAngle_ = 0.0f;
        public float answerAngle_ = 7.0f;
        public AsterismDesc desc_ = null;
        public Sky sky_;
        public TitleManager.Mode selectMode_ = TitleManager.Mode.None;
        bool bCalcCenter_ = false;

        // ファイナライズ
        public void finalize()
        {
            foreach ( var star in stars_ ) {
                Destroy( star.gameObject );
            }
            foreach ( var line in lines_ ) {
                Destroy( line.gameObject );
            }
            desc_.fadeOut( () => {
                Destroy( desc_.gameObject );
            } );
            Destroy( root_ );
        }

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

        // rootから各恒星までの最長距離を算出
        public float calcStarDist()
        {
            float maxDist = -1.0f;
            foreach ( var star in stars_ ) {
                if ( star.transform.localPosition.magnitude > maxDist )
                    maxDist = star.transform.localPosition.magnitude;
            }
            return maxDist;
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

    class Title : StateBase
    {
        public Title(GameManager parent) : base( parent ) { }

        protected override void innerInit()
        {
            title_ = Instantiate<TitleManager>( parent_.titleManagerPrefab_ );
            title_.transform.localPosition = Vector3.zero;
            title_.FinishCallback = ( mode ) => {
                parent_.selectMode_ = mode;
            };
        }

        protected override State innerUpdate()
        {
            if ( parent_.selectMode_ != TitleManager.Mode.None ) {
                if ( parent_.selectMode_ == TitleManager.Mode.Ending ) {
                    // 全ての星座名をオープンしエンディングへ
                    for ( int i = 1; i <= 89; ++i ) {
                        parent_.setSkyAsterisumAlpha( i, 1.0f, 1.0f );
                        parent_.setSkyAsterismName( i );
                    }
                    // 天球のラインをすべてアクティブに
                    foreach ( var ast in parent_.sky_.asterisms_ ) {
                        foreach ( var line in ast.lines_ ) {
                            line.gameObject.SetActive( true );
                        }
                    }
                    parent_.nextState_ = new Ending( parent_ );
                } else {
                    parent_.notifyNextQuestion();
                }
                Destroy( title_.gameObject );
            }
            return this;
        }

        TitleManager title_;
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
            parent_.createAsterism( dataSet_.astId_, dataSet_.radius_, parent_.starUnitPrefab_, parent_.linePrefab_, ref dataSet_.stars_, ref dataSet_.lines_, true, 1.0f, 1.0f, 1.0f );

            dataSet_.root_ = new GameObject( "root" );
            dataSet_.root_.transform.parent = parent_.transform;
            dataSet_.root_.transform.position = dataSet_.getCenter();
            dataSet_.toRoot();

            // 恒星名があれば表示
            foreach ( var star in dataSet_.stars_ ) {
                int hipId = star.getHipId();
                string name = Table_asterism_star_hip.getInstance().getName( hipId );
                if ( name != "" )
                    star.setName( name );
            }

            // 星座のルートをランダムに回転
            dataSet_.answerQ_ = dataSet_.root_.transform.rotation;
            var q = Quaternion.Euler( Random.Range( -180.0f, 180.0f ), Random.Range( -180.0f, 180.0f ), Random.Range( -180.0f, 180.0f ) );
            dataSet_.root_.transform.rotation = q;

            float ang = Quaternion.Angle( q, dataSet_.answerQ_ );
            dataSet_.curAngle_ = ang;
            float t = 1.0f - ang / 180.0f;

            // 星座線をランダムに回転
            foreach ( var line in dataSet_.lines_ ) {
                line.backupRotation();
                line.transform.localRotation = Quaternion.Euler( Random.Range( -180.0f, 180.0f ), Random.Range( -180.0f, 180.0f ), Random.Range( -180.0f, 180.0f ) );
                line.backupQuestionRotation();
                var lq = line.getBackupRotation();
                var ansLQ = line.getQuestionRotation();
                line.transform.localRotation = Quaternion.Lerp( ansLQ, lq, t );
            }

            // 天球のラインを有効に
            foreach ( var line in dataSet_.sky_.asterisms_[ dataSet_.astId_ - 1 ].lines_ ) {
                line.gameObject.SetActive( true );
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
            var center = Vector3.zero;
            start_ = Camera.main.transform.rotation;
            end_ = dataSet_.lookAtAst();
            startFov_ = Camera.main.fieldOfView;
            float r = dataSet_.calcStarDist();
            endFov_ = 2.0f * Mathf.Asin( r / dataSet_.radius_ ) * Mathf.Rad2Deg;
            if ( startFov_ > 65.0f )
                startFov_ = 65.0f;
            if ( endFov_ < 25.0f )
                endFov_ = 25.0f;

            // TEST
            // var t = Instantiate<Star>( parent_.starUnitPrefab_ );
            // t.transform.localScale = Vector3.one * 2.0f;
            // t.transform.position = dataSet_.getCenter();
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t0 = t_ / moveSec_;
            var q = Lerps.Quaternion.easeInOut( start_, end_, t0 );
            var fov = Lerps.Float.easeInOut( startFov_, endFov_, t0 );
            Camera.main.transform.rotation = q;
            Camera.main.fieldOfView = fov;
            if ( t_ >= moveSec_ ) {
                if ( dataSet_.selectMode_ == TitleManager.Mode.Auto )
                    return new ViewMode( parent_, dataSet_ );
                else
                    return new Gaming( parent_, dataSet_ );
            }
            return this;
        }

        DataSet dataSet_;
        Quaternion start_, end_;
        float moveSec_ = 3.0f;
        float t_ = 0.0f;
        float startFov_ = 0.0f;
        float endFov_ = 0.0f;
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
            viewer_.setActive( true );

            // 情報を表示
            var desc = Instantiate<AsterismDesc>( parent_.descPrefab_ );
            desc.transform.SetParent( parent_.canvas_.transform, false );
            desc.transform.localPosition = Vector3.zero;
            desc.transform.localScale = Vector3.one;
            desc.setup( dataSet_.astId_ );

            dataSet_.desc_ = desc;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            viewer_.update();

            // rootの回転角度を監視
            // 角度差が一定範囲以内に入ったら解答出来たとみなす
            var q = dataSet_.root_.transform.rotation;
            float ang = Quaternion.Angle( q, dataSet_.answerQ_ );
            dataSet_.curAngle_ = ang;
            float t = 1.0f - ang / 180.0f;
            dataSet_.desc_.setConcodanceRate( 1.0f - ang / 180.0f );

            // 星座線の角度を更新
            // 星座線をランダムに回転
            foreach ( var line in dataSet_.lines_ ) {
                var lq = line.getBackupRotation();
                var ansLQ = line.getQuestionRotation();
                line.transform.localRotation = Quaternion.Lerp( ansLQ, lq, t );
            }

            if ( ang < dataSet_.answerAngle_ )
                return new AnswerMove( parent_, dataSet_ );
            return this;
        }

        ObjectViewer viewer_ = new ObjectViewer();
        DataSet dataSet_;
        Quaternion answerQ_ = Quaternion.identity;
    }

    // 閲覧モード中
    //  勝手に解答していく
    class ViewMode : StateBase
    {
        public ViewMode(GameManager parent, DataSet dataSet) : base( parent )
        {
            dataSet_ = dataSet;
        }

        // 内部初期化
        override protected void innerInit()
        {
            // 情報を表示
            var desc = Instantiate<AsterismDesc>( parent_.descPrefab_ );
            desc.transform.SetParent( parent_.canvas_.transform, false );
            desc.transform.localPosition = Vector3.zero;
            desc.transform.localScale = Vector3.one;
            desc.setup( dataSet_.astId_ );

            dataSet_.desc_ = desc;

            // 勝手に解答
            var q = dataSet_.root_.transform.rotation;
            GlobalState.time( 4.0f, (sec, t) => {
                dataSet_.root_.transform.rotation = Lerps.Quaternion.easeInOut( q, dataSet_.answerQ_, t );
                float ang = Quaternion.Angle( dataSet_.root_.transform.rotation, dataSet_.answerQ_ );
                dataSet_.curAngle_ = ang;
                float t2 = 1.0f - ang / 180.0f;
                dataSet_.desc_.setConcodanceRate( 1.0f - ang / 180.0f );

                // 星座線の角度を更新
                foreach ( var line in dataSet_.lines_ ) {
                    var lq = line.getBackupRotation();
                    var ansLQ = line.getQuestionRotation();
                    line.transform.localRotation = Quaternion.Lerp( ansLQ, lq, t2 );
                }

                return true;
            } ).finish(()=> {
                parent_.nextState_ = new AnswerMove( parent_, dataSet_ );
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return this;
        }

        DataSet dataSet_;
        Quaternion answerQ_ = Quaternion.identity;
    }

    // 解答移動
    class AnswerMove : StateBase
    {
        public AnswerMove( GameManager parent, DataSet dataSet ) :base(parent) {
            dataSet_ = dataSet;
        }

        // 内部初期化
        override protected void innerInit()
        {
            startQ_ = dataSet_.root_.transform.rotation;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t0 = t_ / moveSec_;
            var q = Lerps.Quaternion.easeInOut( startQ_, dataSet_.answerQ_, t0 );
            dataSet_.root_.transform.rotation = q;
            float ang = Quaternion.Angle( q, dataSet_.answerQ_ );
            dataSet_.curAngle_ = ang;
            float t2 = 1.0f - ang / 180.0f;
            dataSet_.desc_.setConcodanceRate( 1.0f - ang / 180.0f );

            foreach ( var line in dataSet_.lines_ ) {
                var lq = line.getBackupRotation();
                var ansLQ = line.getQuestionRotation();
                line.transform.localRotation = Quaternion.Lerp( ansLQ, lq, t2 );
            }

            if ( t_ >= moveSec_ ) {
                return new AnswerEffect( parent_, dataSet_ );
            }
            return this;
        }

        Quaternion startQ_;
        DataSet dataSet_;
        float t_ = 0.0f;
        float moveSec_ = 2.0f;
    }

    // 解答演出
    class AnswerEffect : StateBase
    {
        public AnswerEffect( GameManager parent, DataSet dataSet ) : base( parent ) {
            dataSet_ = dataSet;
        }

        // 内部初期化
        override protected void innerInit()
        {
            foreach( var star in dataSet_.stars_ ) {
                star.runParticle();
            }
            GlobalState.wait( waitSec_, () => {
                // 解答した星座を色濃く表示
                parent_.setSkyAsterisumAlpha( dataSet_.astId_, 1.0f, 1.0f );
                // 解答した星座名を天球に刻印
                parent_.setSkyAsterismName( dataSet_.astId_ );
                dataSet_.finalize();
                return false;
            } ).next( () => {
                GlobalState.wait( 1.5f, () => {
                    // マネージャに次の出題を通知
                    parent_.curAnswerNum_++;
                    parent_.notifyNextQuestion();
                    bFinish_ = true;
                    return false;
                } );
                return false;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                return null;
            }
            return this;
        }

        DataSet dataSet_;
        float waitSec_ = 3.0f;
        bool bFinish_ = false;
    }

    // エンディング
    class Ending : StateBase
    {
        public Ending(GameManager parent ) : base( parent ) { }

        protected override void innerInit()
        {
        }

        protected override State innerUpdate()
        {
            latDeg_ += Time.deltaTime * 3.0f;
            longDeg_ += Time.deltaTime * 5.0f;
            lat_ = Mathf.Sin( latDeg_ * Mathf.Deg2Rad ) * 88.0f;
            longi_ = Mathf.Cos( longDeg_ * Mathf.Deg2Rad ) * 180.0f + 180.0f;
            var pos = SphereSurfUtil.convPolerToPos( lat_, longi_ );
            Camera.main.transform.rotation = Quaternion.LookRotation( pos, Vector3.up );

            return this;
        }

        float t_ = 0.0f;
        float latDeg_ = 0.0f;
        float longDeg_ = 0.0f;
        float lat_ = 0.0f;
        float longi_ = 0.0f;
    }

    int preId_ = -1;
    State state_;
    State nextState_;
    Sky sky_ = new Sky();
    DataSet dataSet_;
    List<int> questionAstIds_ = new List<int>();
    int curAnswerNum_ = 0;
    TitleManager.Mode selectMode_ = TitleManager.Mode.None;
}
