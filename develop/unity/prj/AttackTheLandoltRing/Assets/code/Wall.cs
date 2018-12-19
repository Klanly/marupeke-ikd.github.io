using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField]
    WallMesh mesh_;

    [SerializeField]
    Transform rotZTrans_;

    [SerializeField]
    Transform posTrans_;

    [SerializeField]
    Detonator detonator_ = null;

    [SerializeField]
    AudioSource audioSource_ = null;


    public WallMesh getMesh()
    {
        return mesh_;
    }

    public void setPolerPos( float radius, float degree )
    {
        radius_ = radius;
        degree_ = degree;

        rotZTrans_.localRotation = Quaternion.Euler( 0.0f, 0.0f, degree );
        posTrans_.localPosition = new Vector3( 0.0f, radius, 0.0f );
    }

    public void setRadiusPos( float radius )
    {
        setPolerPos( radius, degree_ );
    }

    public void toGameOver( float delay )
    {
        if ( state_ == null )
            state_ = new GameOverDelay( this, delay );
    }

    public bool isCollideEnemy( Vector3 pos, float radius )
    {
        if ( bDestroyed_ == true )
            return false;

        float dist = ( posTrans_.position - pos ).magnitude;
        float wallRadius = mesh_.getCollideRadius();
        return ( dist <= radius + wallRadius );
    }

    public void addDamage()
    {
        hp_--;
        if ( hp_ == 0 ) {
            // 爆発
            explode();
        } else {
            // マテリアルを赤寄りへ
            Color y = new Color( 0.9f, 0.7f, 0.0f, 1.0f );
            Color r = new Color( 0.9f, 0.2f, 0.0f, 1.0f );
            float t = ( float )hp_ / initHp_;
            mesh_.setColor( Color.Lerp( y, r, 1.0f - t ) );
        }
    }

    public bool isCriticalDamage()
    {
        return ( hp_ == 0 );
    }

    // 爆発
    void explode()
    {
        if ( bDestroyed_ == false ) {
            detonator_.Explode();
            audioSource_.Play();
            bDestroyed_ = true;
            mesh_.gameObject.SetActive( false );
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    float radius_;
    float degree_;
    State state_;
    bool bDestroyed_ = false;
    int initHp_ = 3;
    int hp_ = 3;


    // ゲームオーバー遅延爆発
    class GameOverDelay : State
    {
        public GameOverDelay( Wall manager, float delay )
        {
            manager_ = manager;
            delay_ = delay;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            delay_ -= Time.deltaTime;
            if ( delay_ <= 0.0f ) {
                manager_.explode();
                return null;
            }
            return this;
        }

        Wall manager_;
        float delay_ = 0.0f;
    }
}
