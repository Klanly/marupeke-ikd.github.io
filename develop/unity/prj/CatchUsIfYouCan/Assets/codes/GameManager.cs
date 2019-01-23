using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    int emitEnemyNum_ = 16;

    [SerializeField]
    GameObject objectRoot_;

    [SerializeField]
    BulletFactory bulletFactory_;

    [SerializeField]
    EnemyFactory enemyFactory_;

    [SerializeField]
    FieldFactory fieldFactory_;

    [SerializeField]
    SphereField fieldPrefab_;

    [SerializeField]
    Human humanPrefab_;

    [SerializeField]
    float fieldRadius_ = 300.0f;

    [SerializeField]
    Camera camera_;

    [SerializeField]
    UIGauge gauge_;

    [SerializeField]
    GameObject warning_;

    [SerializeField]
    GameObject clearImage_;

    [SerializeField]
    GameObject limitStaminaImage_;

    [SerializeField]
    UnityEngine.UI.Text catachEnemies_;

    [SerializeField]
    GameObject introReady_;

    [SerializeField]
    GameObject introGo_;

    // Use this for initialization
    void initialize () {
        field_ = Instantiate<SphereField>( fieldPrefab_ );
        field_.transform.parent = transform;
        field_.transform.localPosition = Vector3.zero;
        field_.setRadius( fieldRadius_ );

        human_ = Instantiate<Human>( humanPrefab_ );
        human_.transform.parent = objectRoot_.transform;
        human_.transform.localPosition = Vector3.zero;
        human_.setup( field_.getRadius(), new Vector3( 0.0f, 0.0f, -1.0f ), new Vector3( 0.0f, 1.0f, 0.0f ) );
        human_.setAction( Human.ActionState.ActionState_Run );
        human_.StaminaZeroCallback = () => {
            limitStaminaImage_.SetActive( true );
        };

        camera_.transform.parent = human_.transform;
        camera_.transform.localPosition = new Vector3( 0.0f, 25.0f, -20.0f );
        camera_.transform.localRotation = Quaternion.LookRotation( -camera_.transform.localPosition + new Vector3( 0.0f, 0.0f, 10.0f ) );
    }

    private void Start()
    {
        state_ = new Intro( this );
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();
     }

    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            p_ = parent;
        }
        protected GameManager p_;
    }

    // イントロ
    //  「Ready Go!」表示
    class Intro : StateBase
    {
        public Intro( GameManager parent ) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {
            p_.initialize();
            p_.human_.setEnableCollide( false );

            var game = new Game( p_ );

            // 弾テスト
            //  適当にあちこちに
            int num = 150;
            for ( int i = 0; i < num; ++i ) {
                var bullet = p_.bulletFactory_.create();
                bullet.transform.parent = p_.objectRoot_.transform;
                bullet.transform.localPosition = Vector3.zero;
                var bpos = SphereSurfUtil.randomPos( Random.value, Random.value );
                var v = SphereSurfUtil.randomPos( Random.value, Random.value );
                bullet.setup( p_.field_.getRadius(), bpos, v );
                bullet.Human = p_.human_;
            }

            // 敵テスト
            //  適当にあちこちに
            for ( int i = 0; i < p_.emitEnemyNum_; ++i ) {
                var enemy = p_.enemyFactory_.createRobot();
                enemy.transform.parent = p_.objectRoot_.transform;
                enemy.transform.localPosition = Vector3.zero;
                var bpos = SphereSurfUtil.randomPos( Random.value, Random.value );
                var v = SphereSurfUtil.randomPos( Random.value, Random.value );
                enemy.setup( p_.field_.getRadius(), bpos, v );
                enemy.Human = p_.human_;
                enemy.CatchCallback = game.CatchEnemyCallback;
            }

            // [Catch <p_.emitEnemyNum_> enemies !]
            // [Ready]
            // [Go!]
            float t = 0.0f;
            GlobalState.start( () => {
                p_.catachEnemies_.text = string.Format("Catach {0} Enemies !", p_.emitEnemyNum_ );
                p_.catachEnemies_.gameObject.SetActive( true );
            }, () => {
                t += Time.deltaTime;
                return ( t <= 3.5f );
            }, () => { t = 0.0f; }

            ).next( () => {
                p_.introReady_.SetActive( true );
            }, () => {
                t += Time.deltaTime;
                return ( t <= 1.0f );
            }, () => { t = 0.0f; }

            ).next( () => {
                p_.introGo_.SetActive( true );
            }, () => {
                t += Time.deltaTime;
                return ( t <= 0.4f );
            }, () => {
                nextState_ = game;
            }
            );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( nextState_ != null )
                return nextState_;
            return this;
        }

        State nextState_;
    }

    // ゲーム中
    class Game : StateBase
    {

        public System.Action<CollideType> CatchEnemyCallback
        {
            get {
                return (type) => { catchEnemy( type ); };
            }
        }

        public Game( GameManager parent ) : base( parent ) {
            remainEnemyNum_ = p_.emitEnemyNum_;
            nextState_ = this;
        }

        void catchEnemy(CollideType type)
        {
            if ( type == CollideType.CT_Enemy ) {
                remainEnemyNum_--;
                if ( remainEnemyNum_ == 0 ) {
                    // ボス出現
                    var boss = p_.enemyFactory_.createBoss();
                    boss.transform.parent = p_.objectRoot_.transform;
                    boss.transform.localPosition = Vector3.zero;
                    var bossPos = SphereSurfUtil.randomPos( Random.value, Random.value );
                    var bossDir = SphereSurfUtil.randomPos( Random.value, Random.value );
                    boss.setup( p_.field_.getRadius(), bossPos, bossDir );
                    boss.Human = p_.human_;
                    boss.CatchCallback = catchEnemy;

                    GlobalState.wait( 0.75f, () => {
                        p_.warning_.SetActive( true );
                        return false;
                    } );
                }
            } else if ( type == CollideType.CT_Boss ) {
                // ボスを確保！
                nextState_ = new Clear( p_ );
            }
        }

        protected override void innerInit()
        {
            p_.human_.setGameStart();
            p_.human_.setEnableCollide( true );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            p_.gauge_.setLevel( p_.human_.getStaminaRate() );
            return nextState_;
        }

        int remainEnemyNum_ = 0;
        State nextState_ = null;
    }

    class Clear : StateBase
    {
        public Clear(GameManager manager) : base( manager ) { }
        
        // 内部初期化
        override protected void innerInit()
        {
            p_.human_.setClear();

            GlobalState.wait( 1.0f, () => {
                p_.clearImage_.SetActive( true );
                return false;
            } );

            var v = p_.camera_.transform.localPosition;
            float t0 = 0.0f;
            float t1 = 0.0f;
            var sl = new Vector3( 0.0f, 0.0f, 10.0f );
            var el = Vector3.zero;
            var pos = new Vector3( 0.0f, 25.0f, -20.0f );
            var posE = new Vector3( 0.0f, 10.0f, -25.0f );

            GlobalState.start( () => {
                t0 += Time.deltaTime * 1.0f;
                t0 = Mathf.Clamp01( t0 );
                var lp = Vector3.Lerp( sl, el, t0 );

                t1 += Time.deltaTime * 20.0f;
                t1 %= 360;
                var q = Quaternion.AngleAxis( t1, Vector3.up );
                var p = q * Vector3.Lerp( pos, posE, t0 );
                p_.camera_.transform.localPosition = p;
                p_.camera_.transform.rotation = Quaternion.LookRotation( p_.human_.transform.position + lp - p_.camera_.transform.position, p_.human_.transform.up );
                return true;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return null;
        }
    }
    Human human_;
    SphereField field_;
    State state_;
}
