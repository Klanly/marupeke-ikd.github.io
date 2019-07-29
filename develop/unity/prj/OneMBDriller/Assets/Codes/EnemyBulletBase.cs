using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵弾基底
public class EnemyBulletBase : MonoBehaviour
{
    [SerializeField]
    float radius_;

    // 衝突半径を取得
    public float getRadius() {
        return radius_;
    }
}
