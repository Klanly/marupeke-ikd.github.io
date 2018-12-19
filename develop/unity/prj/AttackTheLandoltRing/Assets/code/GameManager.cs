using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    [SerializeField]
    WallOperator wallOperator_ = null;

    [SerializeField]
    LandoltEmitter enemyEmitter_ = null;

    [SerializeField]
    float fieldRadius_ = 100.0f;

    [SerializeField]
    int wallUnitDegree_ = 5;

    [SerializeField]
    float wallTickness_ = 6.0f;

    [SerializeField]
    AudioSource audioSource_;

    [SerializeField]
    AudioSource gameOverAudioSource_;

    [SerializeField]
    SpriteRenderer gameOverRenderer_;

    [SerializeField]
    ScoreManager scoreManager_;


    // ゲーム終了した？
    public bool isFinishGame()
    {
        return bFinishGame_;
    }

    // Use this for initialization
    void Start () {
        // スコアマネージャ
        scoreManager_.setup( fieldRadius_ );

        //壁オブジェクト作成
        wallOperator_.setup( wallUnitDegree_, fieldRadius_, wallTickness_ );
        wallOperator_.DestroyEnemyCallback = ( enemy, isCombo, isMultiHit ) => {
            scoreManager_.destroyEnemy( enemy, isCombo, isMultiHit );
        };

        // エミッター初期化
        enemyEmitter_.setup( 18, fieldRadius_ );
        enemyEmitter_.setEnemySpeed( 5.0f );
        enemyEmitter_.FieldOverExplodeCallback = fieldOverDamage;
        enemyEmitter_.ExplodeCallback = destroyEnemy;

        // ゲームスタート
        state_ = new GameStart( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    // ダメージ
    void  fieldOverDamage(Transform enemy, float radius )
    {
        if ( bGameOver_ == true )
            return;

        int criticalIdx = 0;
        if ( wallOperator_.fieldOverDamage( enemy, radius, ref criticalIdx ) == true ) {
            bGameOver_ = true;
            // ゲームオーバー処理
            audioSource_.Stop();
            enemyEmitter_.setEnableEmit( false );
            wallOperator_.toGameOver( criticalIdx );

            state_ = new WaitState( 3.0f, new GameOver( this ) );
        }
    }

    // 敵破壊
    void destroyEnemy( LandoltEnemy enemy )
    {
        // ここでスコアつけるのやめました
    }

    bool bGameOver_ = false;
    bool bFinishGame_ = false;
    State state_;


    // ステート
    class GameStart : State
    {
        public GameStart( GameManager manager )
        {
            manager_ = manager;
            manager_.wallOperator_.FinishIntroCallback = () => {
                bFinish_ = true;
            };
        }

        // 内部初期化
        override protected void innerInit()
        {
        }

        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                manager_.enemyEmitter_.setEnableEmit( true );
                manager_.audioSource_.Play();
                return null;
            }
            return this;
        }

        bool bFinish_ = false;
        GameManager manager_;
    }

    class GameOver : State
    {
        public GameOver(GameManager manager)
        {
            manager_ = manager;
        }

        // 内部初期化
        override protected void innerInit()
        {
            manager_.gameOverAudioSource_.Play();

            sprite_ = manager_.gameOverRenderer_.gameObject;
            sprite_.SetActive( true );
            sprite_.transform.localPosition = new Vector3( 0.0f, 0.0f, -100.0f );
            sprite_.transform.DOLocalMove( new Vector3( 0.0f, 0.0f, -10.0f ), 2.0f );

            mat_ = manager_.gameOverRenderer_.material;
            color_ = mat_.color;
            mat_.color = new Color( color_.r, color_.g, color_.b, 0.0f );
            mat_.DOColor( color_, 3.0f );
        }

        override protected State innerUpdate()
        {
            return new WaitState( 10.0f, null, () => {
                manager_.bFinishGame_ = true;
            } );
        }

        Color color_;
        Material mat_;
        bool bFinish_ = false;
        GameManager manager_;
        GameObject sprite_;
    }
}
