using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 人

public class Human : Passenger {

    [SerializeField]
    AnimationClip walk_;

    [SerializeField]
    AnimationClip run_;

    [SerializeField]
    Animator animator_;

    [SerializeField]
    int actionState_;

    public enum ActionState : int
    {
        State_Idle = 0,
        State_Walk = 1,
        State_Run = 2,
    }

    // 状態を変更
    public void setState(ActionState actionState )
    {
        actionState_ = ( int )actionState;
    }

    // 移動方向を設定
    public void setMoveDir( bool bLeftToRight )
    {
        if ( bLeftToRight ) {
            dir_ = 1.0f;
            transform.localRotation = Quaternion.identity;
        } else {
            dir_ = -1.0f;
            transform.localRotation = Quaternion.Euler( 0.0f, 180.0f, 0.0f );
        }
    }

	// Update is called once per frame
	void Update () {
		if ( preState_ != actionState_ ) {
            animator_.SetInteger( "state", actionState_ );
            speed_ = speeds_[ actionState_ ];
        }
        preState_ = actionState_;

        var p = transform.localPosition;
        p.x += speed_ * dir_;
        transform.localPosition = p;

        if ( p.x >= removePosX_ )
            Destroy( this.gameObject );

        // 橋チェック
        updateOnBridge();
	}

    // Use this for initialization
    void Start()
    {
        initialize();
        speeds_ = new float[] {
            0.0f,
            0.04f,
            0.08f
        };

    }

    protected virtual void initialize()
    {
        state_ = new State_Normal( this );
    }

    protected void updateOnBridge()
    {
        if ( manager_ == null )
            return;

        if ( state_ != null ) {
            var preState = state_;
            state_ = state_.update();
            if ( state_ != preState ) {
                updateOnBridge();
                return;
            }
        }
    }

    class StateBase : State
    {
        public StateBase(Human parent)
        {
            parent_ = parent;
        }
        protected Human parent_;
    }

    // 通常移動
    class State_Normal : StateBase
    {
        public State_Normal(Human parent) : base( parent ) { }
        override protected State innerUpdate()
        {
            // 橋内に侵入したら「橋内移動」へ
            Bridge bridge = parent_.manager_.getCollideBridge( parent_.transform.position );
            if ( bridge != null ) {
                return new State_InBridge( parent_, bridge );
            }

            return this;
        }
    }

    // 橋内移動
    class State_InBridge : StateBase
    {
        public State_InBridge(Human parent, Bridge innerBridge) : base( parent )
        {
            innerBridge_ = innerBridge;
        }
        protected override State innerUpdate()
        {
            // 橋とのコリジョンが無くなったら脱出できるかチェック
            // 出来れば通常移動へ
            // 出来なければ待機状態へ
            Vector3 pos = parent_.transform.position;
            if ( innerBridge_.isCollide( pos ) == false ) {
                if ( innerBridge_.getYLevel() > escapeYLevel_ )
                    return new State_Normal( parent_ );
                return new State_Wait( parent_, innerBridge_, parent_.getSpeed() );
            }

            // 橋の高さに合わせてキャラの位置を変更
            pos.y = Human.calcYLevel( ref curYVec_, gravity_, pos.y, innerBridge_ );
            parent_.transform.position = pos;

            return this;
        }

        protected Bridge innerBridge_;
        protected float curYVec_ = 0.0f;
        protected float gravity_ = -1.0f;
        protected float escapeYLevel_ = -0.1f;    // 脱出可能な橋の高さ
    }

    // 橋内待機
    class State_Wait : State_InBridge
    {
        public State_Wait(Human parent, Bridge innerBridge, float moveSpeed) : base( parent, innerBridge )
        {
            moveSpeed_ = moveSpeed;
            parent_.setSpeed( 0.0f );
        }
        protected override State innerUpdate()
        {
            // 橋の高さが規定以上になれば通過
            Vector3 pos = parent_.transform.position;
            if ( innerBridge_.getYLevel() > escapeYLevel_ ) {
                pos.y = 0.0f;
                parent_.transform.position = pos;
                parent_.setSpeed( moveSpeed_ );
                return new State_Normal( parent_ );
            }

            // 橋の高さに合わせてキャラの位置を変更
            pos.y = Human.calcYLevel( ref curYVec_, gravity_, pos.y, innerBridge_ );
            parent_.transform.position = pos;

            return this;
        }

        float moveSpeed_ = 0.0f;
    }

    // 橋内のキャラの高さを計算
    // 移動速度も更新
    static float calcYLevel(ref float curYVec, float gravity, float myY, Bridge innerBridge)
    {
        float bridgeY = innerBridge.getYLevel();
        // 自由落下
        curYVec += gravity * Time.deltaTime;
        myY += curYVec;
        if ( myY > bridgeY )
            return myY;

        curYVec = 0.0f;
        return bridgeY;
    }

    int preState_ = -1;
    float[] speeds_;
    float removePosX_ = 85.0f;
    State state_ = null;
    float dir_ = 1.0f;
}
