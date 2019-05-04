using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サイトエミッター

public class SiteEmitter {
    System.Action<SiteManager> emitCallback_;
    bool bActive_ = false;
    float aveSec_ = 10.0f;  // 平均発生秒
    float maxSec_ = 20.0f;  // 最大待ち時間

    public System.Action<SiteManager> EmitCallback { set { emitCallback_ = value; } }
    public float AveSec { set { aveSec_ = value; } }
    public float maxSec { set { maxSec = value; } }

    public SiteEmitter() {
        state_ = new Wait( this );
        siteManagerPrefab_ = ResourceLoader.getInstance().loadSync<SiteManager>( "Prefabs/SiteManager" );
        orbitLinePrefab_ = ResourceLoader.getInstance().loadSync<OrbitLine>( "Prefabs/OrbitLine" );
    }

    public void update() {
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
            sec_ = Randoms.Float.expWait( parent_.aveSec_, parent_.maxSec_ );
            return null;
        }
        protected override State innerUpdate() {
            if ( parent_.isActive() == true )
                sec_ -= Time.deltaTime;
            if ( sec_ <= 0.0f )
                return new Emit( parent_ );
            return this;
        }
        float sec_ = 0.0f;
    }

    // エミット
    class Emit : State< SiteEmitter > {
        public Emit(SiteEmitter parent) : base( parent ) { }
        protected override State innerInit() {
            var siteManager = GameObject.Instantiate<SiteManager>( parent_.siteManagerPrefab_ );
            siteManager.transform.position = Vector3.zero;

            var param = new SiteManager.Parameter();
            param.orbitPointHeightRange_ = 1.5f;
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
}
