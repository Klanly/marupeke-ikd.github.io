using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 蜂生成器

public class BeeFactory {

    public Bee beePrefab { set { beePrefab_ = value; } }
    Bee beePrefab_;

    public Bee create( Vector3 center, float minTime, float maxTime, float maxLinearLength, float minCircleRadius, float maxCircleRadius )
    {
        var bee = GameObject.Instantiate<Bee>( beePrefab_ );
        bee.transform.localPosition = center;

        var motion = new EnemyMotionUnit();

        // 最大2ライン移動
        int linearNum = Random.Range( 0, 3 );
        for ( int i = 0; i < linearNum; ++i ) {
            float rad = Random.value * Mathf.PI * 2.0f;
            Vector3 s = new Vector3( maxLinearLength * Mathf.Cos( rad ), maxLinearLength * Mathf.Sin( rad ), 0.0f );
            var m = new EnemyMotion_Linear( s, -s, Random.Range( minTime, maxTime ) );
            motion.setChildMotion( m );
        }

        // ライン移動が無いかランダムに選択された時に円運動を追加
        if ( linearNum == 0 || Random.Range( 0, 2 ) == 1 ) {
            var m = new EnemyMotion_Circle(
                Random.Range( minCircleRadius, maxCircleRadius ),
                Vector3.back,
                Vector3.up,
                Random.Range( minTime, maxTime ),
                Random.value * 360.0f
            );
            motion.setChildMotion( m );
        }

        bee.setMotion( motion );

        return bee;
    }
}
