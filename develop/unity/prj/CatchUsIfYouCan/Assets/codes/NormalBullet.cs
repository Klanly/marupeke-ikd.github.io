using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet {

    [SerializeField]
    GameObject explosionParticle_;

    // 衝突報告
    protected override void onCollide(CollideType colType)
    {
        if ( colType == CollideType.CT_Human ) {
            //  爆発演出。ちょっと派手で。
            explosionParticle_.SetActive( true );
            explosionParticle_.transform.parent = null;
            Destroy( explosionParticle_, 4.0f );

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
