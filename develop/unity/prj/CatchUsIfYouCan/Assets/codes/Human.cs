using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : SphereSurfaceObject {

    [SerializeField]
    Animator animatior_;

    [SerializeField]
    float boostRate_ = 3.0f;

    [SerializeField]
    float downRate_ = 0.5f;

    [SerializeField]
    float lrBoostRate_ = 3.0f;

    [SerializeField]
    float initHP_ = 6000.0f;

    [SerializeField]
    float curHP_ = 0.0f;

    [SerializeField]
    float normalRunStamina_ = 60.0f;    // 通常走行時に1フレームで減るスタミナ

    [SerializeField]
    float overRunStamina_ = 180.0f;     // オーバーラン走行時に1フレームで減るスタミナ

    [SerializeField]
    float normalMissileDamage_ = 1600.0f;   // 通常ミサイルに衝突した時のダメージ


    public System.Action StaminaZeroCallback { set { zeroStaminaCallback_ = value; } }
    System.Action zeroStaminaCallback_;

    private void Awake()
    {
        curHP_ = initHP_;
        state_ = new Intro( this );
    }

    public void setGameStart()
    {
        bStarted_ = true;
    }

    public void setEnableCollide( bool isEnable )
    {
        bEnableCollide_ = isEnable;
    }

    public bool isEnableCollide()
    {
        return bEnableCollide_ && ( curHP_ > 0.0f );
    }

    public bool isLimitOfStamina()
    {
        return ( curHP_ <= 0.0f );
    }

    public void setClear()
    {
        bCleared_ = true;

        float s = curSpeed_;
        float e = speed_;
        float t = 0.0f;
        GlobalState.start( () => {
            t += Time.deltaTime * 1.0f;
            t = Mathf.Clamp01( t );
            cont_.setSpeed( Mathf.Lerp( s, e, t ) );
            // モーションを変更
            animatior_.SetFloat( "speed", Mathf.Lerp( 5.0f, 3.0f, t ) );
            return ( t < 1.0f );
        } );
    }

    public enum ActionState : int
    {
        ActionState_Idle = 0,
        ActionState_Run = 1
    }

    // 速さを取得
    override public float getSpeed()
    {
        return curSpeed_;
    }

    // 行動変更
    public void setAction( ActionState state )
    {
        animatior_.SetInteger( "state", ( int )state );
    }

    // 宝を捕まえる
    public void catchMe( GameObject treasure )
    {
        // 相手から自分の所へピョーン
        treasure.transform.parent = transform;
        var initP = treasure.transform.localPosition;
        float th = Mathf.PI * 0.25f + 0.5f * Random.value * Mathf.PI;
        float dist = 30.0f;
        var ctP = new Vector3( Mathf.Cos( th ) * dist, Mathf.Sin( th ) * dist, 0.0f );
        var endP = Vector3.zero;
        float t = 0.0f;
        GlobalState.start( () => {
            treasure.transform.localPosition = Bezier3D.getPos( initP, ctP, ctP, endP, t );
            t += 0.02f;
            if ( t >= 1.0f )
                return false;
            return true;
        } ).finish( () => {
            Destroy( treasure.gameObject );
        } );
    }

    // スタミナレートを取得
    public float getStaminaRate()
    {
        return curHP_ / initHP_;
    }

    // 衝突報告
    public void onCollide( CollideType colType )
    {
        if ( bCleared_ == true )
            return;

        if ( colType == CollideType.CT_NormalMissile ) {
            // スタミナを減らす
            curHP_ -= normalMissileDamage_;
            // 爆発中状態に遷移
            bExplosioning_ = true;
            setAction( ActionState.ActionState_Idle );  // モーションを変更
            curSpeed_ = 0.0f;
            GlobalState.wait( 2.0f, () => {
                bExplosioning_ = false;
                setAction( ActionState.ActionState_Run );
                return false;
            } );
        }
    }

    // Update is called once per frame
    void Update()
    {
        innerUpdate();
    }

    override protected void innerUpdate()
    {
        if ( state_ != null )
            state_ = state_.update();
        base.innerUpdate();
    }

    class StateBase : State
    {
        public StateBase( Human parent )
        {
            parent_ = parent;
        }

        protected Human parent_;
    }

    class Intro : StateBase
    {
        public Intro(Human parent) : base( parent ) {
            parent_.animatior_.SetFloat( "speed", 3.0f );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( parent_.bStarted_ == true )
                return new NormalRun( parent_ );
            return this;
        }
    }

    class NormalRun : StateBase {
        public NormalRun(Human parent) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {

        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( parent_.bCleared_ == true ) {
                return new Clear( parent_ );
            }

            if ( parent_.bZeroStamina_ == true ) {
                return new ZeroStamina( parent_ );
            }

            if ( parent_.bExplosioning_ == true ) {
                explosioning();
                return this;
            }

            // 上キーが押されていたらスピードを上げる
            bool speedUp = Input.GetKey( KeyCode.UpArrow );
            bool speedDown = Input.GetKey( KeyCode.DownArrow );
            parent_.curSpeed_ = parent_.speed_ * (
                    speedUp
                    ? parent_.boostRate_
                    : ( speedDown ? parent_.downRate_ : 1.0f )
                );
            parent_.cont_.setSpeed( parent_.curSpeed_ );

            // [Z]が押されていたらLRブースト
            bool boost = Input.GetKey( KeyCode.Z );

            // 左右キーが押されていたら進行方向に対して左右に少し曲がる
            float lr = 0.0f;
            if ( Input.GetKey( KeyCode.LeftArrow ) == true ) {
                lr = -parent_.lrSpeed_ * Time.deltaTime * ( boost ? parent_.lrBoostRate_ : 1.0f );
            } else if ( Input.GetKey( KeyCode.RightArrow ) == true ) {
                lr = parent_.lrSpeed_ * Time.deltaTime * ( boost ? parent_.lrBoostRate_ : 1.0f );
            }
            var tangent = parent_.transform.forward * parent_.speed_ + parent_.transform.right * lr;
            parent_.cont_.setDir( tangent );

            // モーションを変更
            parent_.animatior_.SetFloat( "speed", speedUp ? 5.0f : 3.0f );

            // スタミナを減少
            if ( speedUp == true ) {
                parent_.curHP_ -= parent_.overRunStamina_;
            } else {
                parent_.curHP_ -= parent_.normalRunStamina_;
            }
            if ( parent_.bZeroStamina_ == false && parent_.curHP_ <= 0.0f ) {
                parent_.curHP_ = 0.0f;
                parent_.bZeroStamina_ = true;
                if ( parent_.zeroStaminaCallback_ != null )
                    parent_.zeroStaminaCallback_();

                // モーションを変更
                parent_.setAction( ActionState.ActionState_Idle );
            }

            return this;
        }

        void explosioning()
        {
            parent_.cont_.setSpeed( parent_.curSpeed_ );
        }
    }

    class Clear : StateBase
    {
        public Clear(Human parent) : base( parent ) { }
    }

    class ZeroStamina : StateBase
    {
        public ZeroStamina(Human parent) : base( parent ) { }
        // 内部状態
        override protected State innerUpdate()
        {
            parent_.cont_.setSpeed( 0.0f );
            return this;
        }
    }

    State state_;
    bool bStarted_ = false;
    bool bEnableCollide_ = true;
    bool bExplosioning_ = false;
    bool bZeroStamina_ = false;
    float curSpeed_ = 0.0f;
    bool bCleared_ = false;
}
