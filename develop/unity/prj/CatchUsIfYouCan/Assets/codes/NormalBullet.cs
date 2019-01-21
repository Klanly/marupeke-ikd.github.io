using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet {

    // 衝突報告
    protected override void onCollide(CollideType colType)
    {
        if ( colType == CollideType.CT_Human ) {
            // TODO:
            //  爆発演出。ちょっと派手で。

            // オブジェクトは無くす
            Destroy( this.gameObject );
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkCollide();
        innerUpdate();
    }
}
