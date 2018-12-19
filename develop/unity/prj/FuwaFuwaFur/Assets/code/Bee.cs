using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ハチ管理

public class Bee : MonoBehaviour {

    [SerializeField]
    Transform beeLocal_;    // 蜂の行動中心点

    [SerializeField]
    ColliderCallback collider_; // 衝突コールバック

    public ColliderCallback ColliderCallback { get { return collider_; } }


    protected class NormalRoundMotiionState : State
    {
        public NormalRoundMotiionState( Bee parent, EnemyMotionUnit motion )
        {
            parent_ = parent;
            motion_ = motion;
        }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.prePos_ = motion_.getCurPos();
            parent_.beeLocal_.transform.localPosition = parent_.prePos_;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 通常動作中はcenterを中心に動く
            parent_.beeLocal_.transform.localPosition = motion_.update();
            return this;
        }

        Bee parent_;
        EnemyMotionUnit motion_ = new EnemyMotionUnit();    // 動き
    }

    public virtual void setMotion( EnemyMotionUnit motion )
    {
        state_ = new NormalRoundMotiionState( this, motion );
        bSetMotion_ = true;
    }
    
	// Use this for initialization
	void Start () {
        if ( bSetMotion_ == false ) {
            var motion = new EnemyMotionUnit();
            var mx_ = new EnemyMotion_Linear( new Vector3( -30.0f, 0.0f, 0.0f ), new Vector3( 30.0f, 0.0f, 0.0f ), 3.0f );
            var my_ = new EnemyMotion_Linear( new Vector3( 0.0f, 30.0f, 0.0f ), new Vector3( 0.0f, -30.0f, 0.0f ), 5.0f );
            var circle_ = new EnemyMotion_Circle( 30.0f, Vector3.back, Vector3.up, 6.0f, 30.0f );
            motion.setChildMotion( mx_ );
            motion.setChildMotion( my_ );
            motion.setChildMotion( circle_ );
            state_ = new NormalRoundMotiionState( this, motion );
            bSetMotion_ = true;
        }
    }

    // Update is called once per frame
    void Update () {
        myUpdate();
    }

    protected void myUpdate()
    {
        if ( state_ != null )
            state_ = state_.update();

        innerUpdate();

        // 方向転換
        updateDirection();
    }

    void updateDirection()
    {
        // 移動ベクトルの方向が変わったらY軸回転で反転する
        float curVecX = beeLocal_.transform.localPosition.x - prePos_.x;
        if ( preVecX_ * curVecX <= 0.0f ) {
            rotY_.setAim( Mathf.Sign( curVecX ) * 90.0f );
        }
        beeLocal_.transform.localRotation = Quaternion.Euler( 0.0f, rotY_.update(), 0.0f );

        preVecX_ = curVecX;
        prePos_ = beeLocal_.transform.localPosition;
    }

    virtual protected void innerUpdate()
    {
    }

    Vector3 prePos_;
    float preVecX_ = 0.0f;
    MoveValue rotY_ = new MoveValue( 90.0f, 0.05f, 0.001f );
    bool bSetMotion_ = false;
    State state_;
}
