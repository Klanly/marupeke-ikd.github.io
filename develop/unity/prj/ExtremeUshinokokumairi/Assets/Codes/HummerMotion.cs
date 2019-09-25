using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummerMotion : MonoBehaviour {
    [SerializeField]
    Transform snapRoot_;

    [SerializeField]
    float defaultSnapDeg_ = -45.0f;

    [SerializeField]
    float hummerHeadOffset_ = 0.0f;

    // 成功ヒット時コールバック
    public System.Action SuccessHit { set { successHit_ = value; } }
    System.Action successHit_;

    // 失敗ヒット時コールバック
    public System.Action FailHit { set { failHit_ = value; } }
    System.Action failHit_;

    // 成功、失敗を通知
    public void notifySuccessHit( bool isSuccess ) {
        if ( isSuccess == true ) {
            successHit_();
        } else {
            failHit_();
        }
    }

    // 打つ
    public void hit( Vector3 pos ) {
        if ( stopMotion_ == true )
            return;
        setPosition( pos );
        state_ = new Hit( this );
    }

    // 位置を変更
    public void setPosition( Vector3 pos ) {
        if ( stopMotion_ == true )
            return;
        transform.localPosition = pos + new Vector3( 0.0f, hummerHeadOffset_, 0.0f );
    }

    // ハンマー終了
    public void finishMove() {
        stopMotion_ = true;
        state_ = null;
    }

    // Start is called before the first frame update
    void Start() {
        state_ = new Idle( this );
    }

    // Update is called once per frame
    void Update() {
        var p = snapRoot_.localPosition;
        p.y = hummerHeadOffset_;
        snapRoot_.localPosition = p;
        if ( state_ != null )
            state_ = state_.update();
    }

    State state_;
    bool stopMotion_ = false;

    class Idle : State<HummerMotion> {
        public Idle(HummerMotion parent) : base( parent ) { }
        protected override State innerUpdate() {
            // ハンマーを軽く振っておく
            t_ += Time.deltaTime;
            float refDeg = 4.0f * Mathf.Sin( 720.0f * Mathf.Deg2Rad * t_ );
            var q = Quaternion.Euler( parent_.defaultSnapDeg_ + refDeg, 0.0f, 0.0f );
            parent_.snapRoot_.localRotation = q;
            return this;
        }
        float t_ = 0.0f;
    }

    class Hit : State<HummerMotion> {
        public Hit(HummerMotion parent) : base( parent ) { }
        protected override State innerInit() {
            var q = Quaternion.Euler( 90.0f, 0.0f, 0.0f );
            parent_.snapRoot_.localRotation = q;
            action_ = swing;
            return null;
        }
        protected override State innerUpdate() {
            if ( action_ == null ) {
                return new Idle( parent_ );
            }
            action_();
            return this;
        }

        void swing() {
            // ハンマーを振る
            t_ += Time.deltaTime;
            if ( t_ >= sec_ ) {
                t_ = sec_;
            }
            float deg = 90.0f - Lerps.Float.easeIn01( Mathf.Clamp01( t_ / sec_ ) );
            var q = Quaternion.Euler( 90.0f - deg, 0.0f, 0.0f );
            parent_.snapRoot_.localRotation = q;
            if ( t_ >= sec_ ) {
                t_ = 0.0f;
                action_ = wait;
            }
        }

        void wait() {
            t_ += Time.deltaTime;
            if ( t_ >= 0.03f ) {
                action_ = null;
            }
        }
        float t_ = 0.0f;
        float sec_ = 0.05f;
        System.Action action_;
    }
}
