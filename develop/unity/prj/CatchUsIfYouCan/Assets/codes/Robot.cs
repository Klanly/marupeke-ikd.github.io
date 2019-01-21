using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : SphereSurfaceObject {

    [SerializeField]
    protected float escapeSpeed_ = 90.0f;

    [SerializeField]
    float escapeRegion_ = 50.0f;

    [SerializeField]
    float minEscapeSec_ = 1.0f;

    [SerializeField]
    float escapeSecRange_ = 3.0f;

    [SerializeField]
    float catchDist_ = 1.0f;

    [SerializeField]
    GameObject[] treasures_;

    [SerializeField]
    EnemyMarker markerPrefab_;


    public Human Human { set { human_ = value; } }
    public System.Action< CollideType > CatchCallback { set { catchCallback_ = value; } }

	// Use this for initialization
	void Start () {
        initialize();
    }
	
	// Update is called once per frame
	void Update () {
        innerUpdate();
    }

    protected virtual void initialize()
    {
        marker_ = Instantiate<EnemyMarker>( markerPrefab_ );
        marker_.transform.parent = transform;
        marker_.transform.localPosition = Vector3.zero;
        marker_.Radius = radius_ + 25.0f;
        marker_.Target = transform;
        marker_.Human = human_;
        state_ = new Normal( this );
    }

    protected override void innerUpdate()
    {
        if ( state_ != null )
            state_ = state_.update();
        base.innerUpdate();
    }

    float calcDistFromHuman()
    {
        return ( human_.transform.position - transform.position ).magnitude;
    }


    class StateBase : State
    {
        public StateBase( Robot parent )
        {
            parent_ = parent;
        }
        protected Robot parent_;
    }

    // 通常
    class Normal : StateBase
    {
        public Normal(Robot parent) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.cont_.setSpeed( parent_.speed_ );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 近くにHumanがいたら逃走モードに
            if ( parent_.calcDistFromHuman() < parent_.escapeRegion_ )
                return new Escape( parent_ );
            return this;
        }
    }

    // 逃走
    class Escape : StateBase
    {
        public Escape(Robot parent) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.cont_.setSpeed( parent_.escapeSpeed_ );
            moveStates_[ 0 ] = moveLeft;
            moveStates_[ 1 ] = moveRight;
            moveStates_[ 2 ] = moveStraight;
            curState_ = moveStraight;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // Humanが離れたら通常に
            float dist = parent_.calcDistFromHuman();
            if ( dist >= parent_.escapeRegion_ )
                return new Normal( parent_ );

            // 指定時間追われ続けていると判断
            // 左右もしくはそのまま真っすぐ進むのいずれかを判断
            t_ -= Time.deltaTime;
            if ( t_ <= 0.0f ) {
                curState_ = moveStates_[ Random.Range( 0, 3 ) ];
                t_ = parent_.minEscapeSec_ + Random.Range( 0.0f, parent_.escapeSecRange_ );
            }

            curState_();

            // Humanとの距離が拿捕距離以内になったら拿捕
            if ( dist <= parent_.catchDist_ ) {
                for ( int i = 0; i < parent_.treasures_.Length; ++i ) {
                    var tr = parent_.treasures_[ i ];
                    tr.transform.parent = null;
                    GlobalState.wait( i * 0.333f, () => {
                        parent_.human_.catchMe( tr );
                        return false;
                    } );
                }
                if ( parent_.catchCallback_ != null )
                    parent_.catchCallback_( parent_.collideType_ );

                Destroy( parent_.gameObject );
            }

            return this;
        }


        void moveLeft()
        {
            var lr = -parent_.lrSpeed_ * Time.deltaTime;
            var tangent = parent_.transform.forward * parent_.speed_ + parent_.transform.right * lr;
            parent_.cont_.setDir( tangent );
        }

        void moveRight()
        {
            var lr = parent_.lrSpeed_ * Time.deltaTime;
            var tangent = parent_.transform.forward * parent_.speed_ + parent_.transform.right * lr;
            parent_.cont_.setDir( tangent );
        }

        void moveStraight()
        {
        }

        System.Action[] moveStates_ = new System.Action[ 3 ];
        System.Action curState_;
        float t_ = 0.0f;
    }

    protected Human human_;
    State state_;
    EnemyMarker marker_;
    System.Action<CollideType> catchCallback_;
    protected CollideType collideType_ = CollideType.CT_Enemy;
}
