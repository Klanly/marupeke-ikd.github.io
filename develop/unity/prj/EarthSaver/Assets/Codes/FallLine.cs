using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 落下軌道
// 
//  ある地点から地上まで落下する軌道を計算
//  重力の重心位置は原点とします。また落下物は
//  重心位置に十分に近く、重心距離に対する
//  重力加速度は一定とします。

public class FallLine {

    public class Data {
        public List<Vector3> orbit_ = new List<Vector3>();   // 軌道位置
        public float targetHeight_ = 0.0f;    // ターゲットとなる高さ
        public int targetHeightIdx_ = 0;   // ターゲット高さ以下になった時のインデックス
        public float stepSec_ = 1.0f;         // ステップ秒

        // ターゲット高に達するまでの秒数を取得
        public float getSecToReachTargetHeight() {
            return targetHeightIdx_ * stepSec_;
        }
    }

    // 軌道を計算
    //  targetHeight: サンプリング対象とする特定の高さ( >radius )
    static public Data calcOrbit( float radius, Vector3 initPos, Vector3 initV, float g, float stepSec, float targetHeight ) {
        var data = new Data();
        data.targetHeight_ = targetHeight;
        data.stepSec_ = stepSec;
        data.orbit_.Add( initPos );
        Vector3 curPos = initPos;
        Vector3 curV = initV;
        Vector3 nextPos = initPos;
        bool reachTargetHeight = false;
        int maxCount = 1000;
        while ( curPos.magnitude > radius && maxCount >= 0 ) {
            nextPos = curPos + ( curV * stepSec );
            var grad = -nextPos.normalized * ( g * stepSec * stepSec );
            curPos = nextPos + grad;
            curV += grad;
            data.orbit_.Add( curPos );

            if ( reachTargetHeight == false && curPos.magnitude <= targetHeight ) {
                reachTargetHeight = true;
                data.targetHeightIdx_ = data.orbit_.Count - 1;
            }

            maxCount--;
        }
        return data;
    }
}
