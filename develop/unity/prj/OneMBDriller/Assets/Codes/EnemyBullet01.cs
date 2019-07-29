using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵弾01
//  シンプルにプレイヤーの方へ直進する弾
//  乱数範囲で速度が揺れる
public class EnemyBullet01 : EnemyBulletBase
{
    [SerializeField]
    float initSpeed_ = 1.0f;   // 初速(m/s)

    [SerializeField]
    float speedRand_ = 0.0f;    // 初速の乱数範囲（±speedRand_)

    [SerializeField]
    float lifeTime_ = 2.5f;

    void Start()
    {
        target_ = GameManager.getInstance().getPlayer();
        dir_ = ( target_.transform.position - transform.position ).normalized;
        target_.addEnemyBullet( this );
        initSpeed_ += ( Randoms.Float.valueCenter() * 2.0f * speedRand_ );
    }

    // Update is called once per frame
    void Update()
    {
        float d = Time.deltaTime;
        t_ += d;
        if ( t_ >= lifeTime_ ) {
            Destroy( gameObject );
            return;
        }
        var pos = transform.localPosition;
        pos += dir_ * initSpeed_ * d;
        transform.localPosition = pos;
    }

    Player target_ = null;
    float t_ = 0.0f;
    Vector3 dir_;
}
