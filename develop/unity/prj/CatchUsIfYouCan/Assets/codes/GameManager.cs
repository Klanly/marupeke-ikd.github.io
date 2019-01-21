﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject objectRoot_;

    [SerializeField]
    BulletFactory bulletFactory_;

    [SerializeField]
    EnemyFactory enemyFactory_;

    [SerializeField]
    FieldFactory fieldFactory_;

    [SerializeField]
    SphereField fieldPrefab_;

    [SerializeField]
    Human humanPrefab_;

    [SerializeField]
    float fieldRadius_ = 300.0f;

    [SerializeField]
    Camera camera_;

    [SerializeField]
    UIGauge gauge_;

	// Use this for initialization
	void Start () {
        field_ = Instantiate<SphereField>( fieldPrefab_ );
        field_.transform.parent = transform;
        field_.transform.localPosition = Vector3.zero;
        field_.setRadius( fieldRadius_ );

        human_ = Instantiate<Human>( humanPrefab_ );
        human_.transform.parent = objectRoot_.transform;
        human_.transform.localPosition = Vector3.zero;
        human_.setup( field_.getRadius(), new Vector3( 0.0f, 0.0f, -1.0f ), new Vector3( 0.0f, 1.0f, 0.0f ) );
        human_.setAction( Human.ActionState.ActionState_Run );

        camera_.transform.parent = human_.transform;
        camera_.transform.localPosition = new Vector3( 0.0f, 25.0f, -20.0f );
        camera_.transform.localRotation = Quaternion.LookRotation( -camera_.transform.localPosition + new Vector3( 0.0f, 0.0f, 10.0f ) );

        // 弾テスト
        //  適当にあちこちに
        int num = 150;
        for ( int i = 0; i < num; ++i ) {
            var bullet = bulletFactory_.create();
            bullet.transform.parent = objectRoot_.transform;
            bullet.transform.localPosition = Vector3.zero;
            var bpos = SphereSurfUtil.randomPos( Random.value, Random.value );
            var v = SphereSurfUtil.randomPos( Random.value, Random.value );
            bullet.setup( field_.getRadius(), bpos, v );
            bullet.Human = human_;
        }

        // 敵テスト
        //  適当にあちこちに
        remainEnemyNum_ = 16;
        for ( int i = 0; i < remainEnemyNum_; ++i ) {
            var enemy = enemyFactory_.createRobot();
            enemy.transform.parent = objectRoot_.transform;
            enemy.transform.localPosition = Vector3.zero;
            var bpos = SphereSurfUtil.randomPos( Random.value, Random.value );
            var v = SphereSurfUtil.randomPos( Random.value, Random.value );
            enemy.setup( field_.getRadius(), bpos, v );
            enemy.Human = human_;
            enemy.CatchCallback = catchEnemy;
        }
    }

    void catchEnemy( CollideType type )
    {
        if ( type == CollideType.CT_Enemy ) {
            remainEnemyNum_--;
            if ( remainEnemyNum_ == 0 ) {
                // ボス出現
                var boss = enemyFactory_.createBoss();
                boss.transform.parent = objectRoot_.transform;
                boss.transform.localPosition = Vector3.zero;
                var bossPos = SphereSurfUtil.randomPos( Random.value, Random.value );
                var bossDir = SphereSurfUtil.randomPos( Random.value, Random.value );
                boss.setup( field_.getRadius(), bossPos, bossDir );
                boss.Human = human_;
                boss.CatchCallback = catchEnemy;
            }
        }
        else if ( type == CollideType.CT_Boss ) {
            // ボスを確保！
        }
    }

    // Update is called once per frame
    void Update () {
        gauge_.setLevel( human_.getStaminaRate() );

     }

    Human human_;
    SphereField field_;
    int remainEnemyNum_ = 0;
}
