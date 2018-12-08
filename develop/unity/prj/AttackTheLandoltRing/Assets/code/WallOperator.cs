using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WallOperator : MonoBehaviour {

    [SerializeField]
    Wall wall_ = null;

    [SerializeField]
    Cannon cannon_ = null;

    public Action FinishIntroCallback {
        set{ finishIntroCallback_ = value; }
    }

    public System.Action<LandoltEnemy, bool, bool> DestroyEnemyCallback {
        set { destroyEnemyCallback_ = value; }
    }

    public void setup( int wallUnitDegree, float fieldRadius, float wallTickness )
    {
        // 壁作成
        int wallNum = 360 / wallUnitDegree;
        for ( int i = 0; i < wallNum; ++i ) {
            var obj = Instantiate<Wall>( wall_ );
            obj.getMesh().setup( fieldRadius, wallUnitDegree, wallTickness );
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            obj.setPolerPos( fieldRadius * 3.0f, ( float )( i * wallUnitDegree ) );

            walls_.Add( obj );
        }

        // キャノン初期化
        var cannon = Instantiate<Cannon>( cannon_ );
        cannon.transform.parent = transform;
        cannon.transform.localPosition = new Vector3( 0.0f, -fieldRadius, 0.0f );
        cannons_.Add( cannon );
        var cannon2 = Instantiate<Cannon>( cannon_ );
        cannon2.transform.parent = transform;
        cannon2.transform.localPosition = new Vector3( 0.0f, fieldRadius, 0.0f );
        cannon2.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, 180.0f );
        cannons_.Add( cannon2 );

        state_ = new Intro( this );
    }

    // ダメージ
    public bool fieldOverDamage(Transform enemy, float radius, ref int criticalIdx )
    {
        criticalIdx = -1;

        // 範囲にある壁を検索
        bool bCritical = false;
        int idx = 0;
        foreach ( var w in walls_ ) {
            if ( w.isCollideEnemy( enemy.position, radius ) == true ) {
                w.addDamage();
                if ( w.isCriticalDamage() == true ) {
                    bCritical = true;
                    if ( criticalIdx == -1 )
                        criticalIdx = idx;
                }
            }
            idx++;
        }
        return bCritical;
    }

    // ゲームオーバー処理へ
    public void toGameOver( int criticalIdx )
    {
        if ( bDestroyed_ == true )
            return;
        bDestroyed_ = true;

        // 壁を連鎖破壊
        int n = walls_.Count;
        float delay = 0.0f;
        for ( int i = 0; i <= walls_.Count / 2; ++i ) {
            int pi = ( criticalIdx + i ) % n;
            int mi = ( criticalIdx - i + n ) % n;

            float t = ( float )i / ( walls_.Count / 2 );
            float d = Mathf.Lerp( 1.0f, 1.4f, t );
            delay = Mathf.Pow( d, 3.0f );

            walls_[ pi ].toGameOver( delay );
            walls_[ mi ].toGameOver( delay );
        }

        foreach( var c in cannons_ ) {
            c.toGameOver( delay + 1.0f );
        }
    }

    // ミサイルをエミット
    void emitMissile()
    {
        int i = 0;
        foreach( var c in cannons_ ) {
            c.emit( missileId_, i, (missile, enemy, isHit) => {
                // 同じmissileIdがすべて外した事が確認された時点でコンボ失敗
                if ( emittingMissiles_.ContainsKey( missile.getId() ) == false ) {
                    // 最初のミサイル
                    emittingMissiles_[ missile.getId() ] = ( isHit == true ? 1 : -1 );
                    if ( isHit == true ) {
                        // コンボ確定
                        destroyEnemyCallback_( enemy, true, false );
                    }
                } else {
                    // 2番目のミサイル
                    // 結果がわかる
                    if ( emittingMissiles_[ missile.getId() ] > 0 ) {
                        if ( isHit == true ) {
                            // ハイパーコンボ！
                            destroyEnemyCallback_( enemy, true, true );
                        }
                    } else if ( isHit == true ) {
                        // コンボ確定
                        destroyEnemyCallback_( enemy, true, false );
                    } else {
                        // ミス、コンボ消失
                        destroyEnemyCallback_( enemy, false, false );
                    }
                    emittingMissiles_.Remove( missile.getId() );
                }
            },
            i == 0
            );
            i++;
        }
        missileId_++;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    Action finishIntroCallback_;
    List<Wall> walls_ = new List<Wall>();
    State state_;
    float rotSpeed_ = 0.5f;
    List<Cannon> cannons_ = new List<Cannon>();
    bool bDestroyed_ = false;
    int missileId_ = 0;
    Dictionary<int, int> emittingMissiles_ = new Dictionary<int, int>();
    System.Action<LandoltEnemy, bool, bool> destroyEnemyCallback_;

    // ステート
    class Intro : State
    {
        public Intro(WallOperator manager)
        {
            manager_ = manager;
        }

        // 内部初期化
        override protected void innerInit()
        {
            // 壁を所定の位置へ
            foreach ( var wall in manager_.walls_ ) {
                float radius = 300.0f;
                Sequence seq = DOTween.Sequence();
                seq.Append( DOTween.To(
                    () => radius,
                    (x) => {
                        wall.setRadiusPos( x );
                    },
                    100.0f,
                    1.0f + UnityEngine.Random.value
                    ).OnComplete( () => {
                        finishCount_++;
                    } )
                ).SetEase( Ease.InExpo );
                seqs_.Add( seq );
            }
        }

        override protected State innerUpdate()
        {
            if ( finishCount_ >= manager_.walls_.Count ) {
                foreach ( var s in seqs_ ) {
                    s.Kill( true );
                }
                if ( manager_.finishIntroCallback_ != null ) {
                    manager_.finishIntroCallback_();
                    manager_.FinishIntroCallback = null;
                }
                return new Play( manager_ );
            }
            return this;
        }

        int finishCount_ = 0;
        WallOperator manager_;
        List<Sequence> seqs_ = new List<Sequence>();
    }

    class Play : State
    {
        public Play( WallOperator manager )
        {
            manager_ = manager;
        }

        // 内部初期化
        override protected void innerInit()
        {

        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( manager_.bDestroyed_ == true )
                return null;

            // 左キーで時計回り、右キーで反時計回り
            if ( Input.GetKey( KeyCode.LeftArrow ) == true ) {
                var rot = manager_.transform.localRotation.eulerAngles;
                rot.z += manager_.rotSpeed_ + acc_;
                manager_.transform.localRotation = Quaternion.Euler( rot );
                acc_ += accUnit_;
            } else if ( Input.GetKey( KeyCode.RightArrow ) == true ) {
                var rot = manager_.transform.localRotation.eulerAngles;
                rot.z -= manager_.rotSpeed_ + acc_;
                manager_.transform.localRotation = Quaternion.Euler( rot );
                acc_ += accUnit_;
            } else {
                acc_ = 0.0f;
            }

            // Zでキャノンから球発射
            if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
                manager_.emitMissile();
            }
            return this;
        }
        float accUnit_ = 0.03f;
        float acc_ = 0.0f;
        WallOperator manager_;
    }
}
