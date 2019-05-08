using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サイトエミッター

public class SiteEmitter {
    System.Action<SiteManager> emitCallback_;
    bool bActive_ = false;
    float aveSec_ = 10.0f;  // 平均発生秒
    float maxSec_ = 15.0f;  // 最大待ち時間
    float aveHalfUnitSec_ = 30.0f; // 平均発生秒が半減する秒数
    float fallObjPowerMin_ = 100.0f;
    float fallObjPowerMax_ = 400.0f;

    public System.Action<SiteManager> EmitCallback { set { emitCallback_ = value; } }
    public float AveSec { set { aveSec_ = value; } }
    public float MaxSec { set { maxSec_ = value; } }
    public float AveHalfUnitSec { set { aveHalfUnitSec_ = value; } }
    public float FallObjPowerMin { set { fallObjPowerMin_ = value; } }
    public float FallObjPowerMax { set { fallObjPowerMax_ = value; } }

    public SiteEmitter() {
        state_ = new Emit( this );
        siteManagerPrefab_ = ResourceLoader.getInstance().loadSync<SiteManager>( "Prefabs/SiteManager" );
        orbitLinePrefab_ = ResourceLoader.getInstance().loadSync<OrbitLine>( "Prefabs/OrbitLine" );
    }

    public void update() {
        elapsedSec_ += Time.deltaTime;
        if ( state_ != null )
            state_ = state_.update();
    }

    public void setActive( bool isActive ) {
        bActive_ = isActive;
    }

    public bool isActive() {
        return bActive_;
    }

    // 次のエミットまでの待機時間を決めて待つ
    class Wait : State< SiteEmitter > {
        public Wait(SiteEmitter parent ) : base( parent ) {
        }
        protected override State innerInit() {
            float rate = Mathf.Pow( 2.0f, -parent_.elapsedSec_ / parent_.aveHalfUnitSec_ );
            float curAveSec = parent_.aveSec_ * rate;
            float curMaxSec = parent_.maxSec_ * rate;
            sec_ = Randoms.Float.expWait( curAveSec, curMaxSec );
            return null;
        }
        protected override State innerUpdate() {
            if ( parent_.isActive() == true )
                sec_ -= Time.deltaTime;
            if ( sec_ <= 0.0f )
                return new Emit( parent_ );
            if ( parent_.bActive_ == false )
                return null;
            return this;
        }
        float sec_ = 0.0f;
    }

    // エミット
    class Emit : State< SiteEmitter > {
        public Emit(SiteEmitter parent) : base( parent ) { }
        protected override State innerInit() {
            if ( parent_.bActive_ == false )
                return null;
            var siteManager = GameObject.Instantiate<SiteManager>( parent_.siteManagerPrefab_ );
            siteManager.transform.position = Vector3.zero;

            var param = new SiteManager.Parameter();
            param.orbitPointHeightRange_ = 1.5f;
            float rate = Random.value;
            param.fallObjPower_ = parent_.fallObjPowerMin_ + rate * ( parent_.fallObjPowerMax_ - parent_.fallObjPowerMin_ );
            param.fallObjRadius_ = 0.04f + rate * 0.04f;
            siteManager.setup( parent_.orbitLinePrefab_, param );

            // 発生コールバック
            if ( parent_.emitCallback_ != null )
                parent_.emitCallback_( siteManager );

            return new Wait( parent_ );
        }
    }

    State state_;
    SiteManager siteManagerPrefab_;
    OrbitLine orbitLinePrefab_;
    float elapsedSec_ = 0.0f;
}
