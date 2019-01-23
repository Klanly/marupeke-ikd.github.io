using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾生成者
public class BulletFactory : MonoBehaviour {

    [SerializeField]
    NormalBullet normalBullet_;

    public Bullet create()
    {
        return Instantiate<NormalBullet>( normalBullet_ );
    }
}
