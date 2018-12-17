using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlow : Blower
{
    [SerializeField]
    Transform target_;

    [SerializeField]
    Transform model_;

    [SerializeField]
    float blowActionRadius_ = 8.0f;

    class SpringTask
    {
        public SpringTask( Transform model, System.Action callback )
        {
            model_ = model;
            callback_ = callback;
        }

        public bool update()
        {
            if ( bFinish_ == true )
                return false;

            if ( pulling_ == true ) {
                // バネを引く
                pullLen_ += pullUnit_;
                curLen_ = -pullLen_;
                if ( pullLen_ >= initPullLen_ )
                    pulling_ = false;
            } else {
                // バネ解放中
                initPullLen_ *= dampingRate_;
                curLen_ = initPullLen_ * Mathf.Cos( curRad_ );
                curRad_ += unitRad_;
                if ( initPullLen_ <= 0.001f ) {
                    curLen_ = 0.0f;
                    callback_();
                    bFinish_ = true;
                }
            }
            scale_.y = 1.0f + curLen_ * springScale_;
            model_.localScale = scale_;
            return true;
        }

        Transform model_;
        System.Action callback_;
        float k_ = 2.0f;   // バネ係数
        bool pulling_ = true;  // バネを引いている最中
        float pullUnit_ = 0.15f;    // 1フレームで引く量
        float pullLen_ = 0.0f;      // 現在の引いている長さ
        float initPullLen_ = 1.5f;  // 引く予定の長さ
        float springScale_ = 0.25f;  // 長さをスケールに変換
        float curLen_ = 0.0f;       // 原点からの長さ
        float dampingRate_ = 0.90f;  // 減衰率
        float curRad_ = Mathf.PI;
        float unitRad_ = Mathf.PI * 2.0f / 60.0f * 3;   // 周期
        Vector3 scale_ = Vector3.one;
        bool bFinish_ = false;
    }

    public void setTarget(Transform target)
    {
        target_ = target;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( target_ == null )
            return;

        // ターゲットが指定の距離に近付いて、自分の吐き出し角度内に入っていたらブロー発生
        Vector3 targePos = target_.position;
        Vector3 myPos = transform.position;
        Vector3 toTargetV = targePos - myPos;
        Vector3 localDirect = transform.up;
        float ang = Mathf.Acos( Vector3.Dot( localDirect, toTargetV.normalized ) ) * Mathf.Rad2Deg;
        float dist = ( toTargetV ).magnitude;
        if ( dist < blowActionRadius_ && ang <= 20.0f ) {
            blowTasks_.Add( new BlowTask( toTargetV, blowPower_ * dist / blowActionRadius_, blowDecRate_ ) );
            if ( modelSpring_ == false ) {
                modelSpring_ = true;
                springTask_ = new SpringTask( model_, () => {
                    modelSpring_ = false;
                    springTask_ = null;
                } );
            }
        }
        updateTask();
        if ( springTask_ != null )
            springTask_.update();
    }

    Vector3 blowDir_ = Vector3.right;
    bool modelSpring_ = false;
    SpringTask springTask_ = null;
}
