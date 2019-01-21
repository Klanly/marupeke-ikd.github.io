using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エネミーマーカー
//  敵のいる方向を指し示す

public class EnemyMarker : MonoBehaviour {

    [SerializeField]
    SpriteRenderer marker_;

    public Human Human { set { human_ = value; } }
    public Transform Target { set { target_ = value; } }
    public float Radius { set { radius_ = value; } }

    // Use this for initialization
	void Start () {
        color_ = marker_.color;
    }
	
	// Update is called once per frame
	void Update () {
        // humanのいる位置からtargetの位置までの方向を算出
        Vector3 hp = human_.transform.position;
        Vector3 tp = target_.transform.position;
        Vector3 dir = SphereSurfUtil.calcTangent( hp, tp );

        // 方向が人の後ろだったら反転する
        Vector3 hforward = human_.transform.forward;
        color_.a = 1.0f;
        if ( Vector3.Dot( dir, hforward ) < 0.0f ) {
            dir *= -1.0f;
            color_.a = 0.1f;
        }

        // 指定距離だけ離れた所にマーカー表示
        float rad = 30.0f * Mathf.Deg2Rad;
        var pos = SphereSurfUtil.calcMovePos( hp, dir, rad ) * radius_;
        transform.position = pos;

        // マーカーカラーを設定
        marker_.color = color_;

    }

    Transform target_;
    Human human_;
    float radius_ = 0.0f;
    Color color_;
}
