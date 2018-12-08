using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField]
    GameObject model_;

    [SerializeField]
    Missile missile_;

    [SerializeField]
    AudioSource audioSource_;

    [SerializeField]
    AudioClip shotSe_;

    [SerializeField]
    Detonator detonator_ = null;

    [SerializeField]
    AudioSource explodeAudioSource_ = null;


    public void emit( int id, int index, System.Action<Missile, LandoltEnemy, bool> resultCallback, bool isPlaySe )
    {
        Missile obj = Instantiate<Missile>( missile_ );
        obj.set( id, index, resultCallback );
        obj.shot( transform, model_.transform.up, 20.0f );

        if ( isPlaySe == true )
            audioSource_.PlayOneShot( shotSe_ );
    }

    public void toGameOver( float delay )
    {
        if ( bGameOver_ == false ) {
            bGameOver_ = true;
            state_ = new WaitState( delay, new GameOver( this ) );
        }
    }

    void explode()
    {
        explodeAudioSource_.Play();
        detonator_.Explode();
        model_.SetActive( false );
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
    }

    State state_;
    bool bGameOver_ = false;


    // GameOver
    class GameOver : State
    {
        public GameOver( Cannon manager )
        {
            manager_ = manager;
        }

        // 内部初期化
        override protected void innerInit()
        {
            manager_.explode();

        }

        Cannon manager_;
    }
}
