using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵弾02
//  シンプルにプレイヤーの方へ直進する弾。偏差射撃をする
//  乱数範囲で速度が揺れる
public class EnemyBullet02 : EnemyBulletBase
{
    [SerializeField]
    float initSpeed_ = 1.0f;   // 初速(m/s)

    [SerializeField]
    float speedRand_ = 0.0f;    // 初速の乱数範囲（±speedRand_)

    [SerializeField]
    float lifeTime_ = 2.5f;

    void Start() {
        initSpeed_ += ( Randoms.Float.valueCenter() * 2.0f * speedRand_ );
        target_ = GameManager.getInstance().getPlayer();
        if ( DeviationShooting.calcDirction(
            target_.transform.position,
            target_.getVelosity(),
            transform.position,
            initSpeed_,
            out colPos_,
            out dir_,
            out reachTime_
        ) == true ) {
            dir_ = dir_.normalized;
        } else {
            // 偏差射撃できないときはシンプルにプレイヤー方向へ
            dir_ = ( target_.transform.position - transform.position ).normalized;
        }
        target_.addEnemyBullet( this );
    }

    // Update is called once per frame
    void Update() {
        float d = Time.deltaTime;
        t_ += d;
        if ( t_ >= lifeTime_ ) {
            Destroy( gameObject );
            return;
        }
        transform.localPosition = transform.localPosition + initSpeed_ * dir_ * d;
    }

    Player target_ = null;
    float t_ = 0.0f;
    Vector3 dir_;
    Vector3 colPos_;
    float reachTime_ = 0.0f;
}
