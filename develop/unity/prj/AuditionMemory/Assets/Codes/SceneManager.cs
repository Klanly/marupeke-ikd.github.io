using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    TitleManager titleManagerPrefab_;

    [SerializeField]
    GameManager gameManagerPrefab_;

    // Use this for initialization
    void Start () {
        state_ = new Title( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    class Title : State< SceneManager > {
        public Title( SceneManager parent ) : base( parent ) { }
        protected override State innerInit() {
            titleManager_ = Instantiate<TitleManager>( parent_.titleManagerPrefab_ );
            titleManager_.FinishCallback = (level) => {
                Destroy( titleManager_.gameObject );
                setNextState( new Gaming( parent_, level ) );
            };
            return this;
        }
        TitleManager titleManager_;
    }

    class Gaming : State< SceneManager > {
        public Gaming(SceneManager parent, string level ) : base( parent ) {
            level_ = level;
        }
        protected override State innerInit() {
            gameManager_ = Instantiate<GameManager>( parent_.gameManagerPrefab_ );
            gameManager_.setup( level_ );
            gameManager_.FinishCallback = () => {
                Destroy( gameManager_.gameObject );
                setNextState( new Title( parent_ ) );
            };
            return this;
        }
        GameManager gameManager_;
        string level_;
    }

    State state_;
}
