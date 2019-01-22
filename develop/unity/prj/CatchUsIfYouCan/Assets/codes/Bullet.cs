using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SphereSurfaceObject {

    [SerializeField]
    float collisionRadius_ = 1.0f;

    public Human Human { set { human_ = value; } }

    // コリジョンチェック
    protected bool checkCollide()
    {
        if ( bCollided_ == true )
            return false;

        // 人のHPがもうなくなっていたら無視
        if ( human_.isLimitOfStamina() == true )
            return false;

        if ( ( transform.position - human_.transform.position ).magnitude <= collisionRadius_ ) {
            onCollide( CollideType.CT_Human );
            human_.onCollide( CollideType.CT_NormalMissile );
            return true;
        }

        return false;
    }

    // 衝突報告
    protected virtual void onCollide( CollideType colType )
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected Human human_;
    bool bCollided_ = false;
}
