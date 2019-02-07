using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Button easy_;

    [SerializeField]
    UnityEngine.UI.Button normal_;

    [SerializeField]
    UnityEngine.UI.Button hard_;

    [SerializeField]
    UnityEngine.UI.Button junkie_;

    // Use this for initialization
    void Start () {
        state_ = new Title( this );
	}
	
	// Update is called once per frame
	void Update () {
        state_ = state_.update();
	}

    class Title : State
    {
        GameStateManager manager_;
        int gimicNum_ = 0;

        public Title( GameStateManager manager )
        {
            manager_ = manager;
        }

        protected override State innerInit()
        {
            manager_.easy_.onClick.AddListener( () => { gimicNum_ = 1; Parameters.remainSec_g = 180; } );
            manager_.normal_.onClick.AddListener( () => { gimicNum_ = 2; Parameters.remainSec_g = 180; } );
            manager_.hard_.onClick.AddListener( () => { gimicNum_ = 3; Parameters.remainSec_g = 180; } );
            manager_.junkie_.onClick.AddListener( () => { gimicNum_ = 14; Parameters.remainSec_g = 1200; } );

            return this;
        }

        protected override State innerUpdate()
        {
            if ( gimicNum_ > 0 ) {
                Parameters.gimicNum_g = gimicNum_;
                UnityEngine.SceneManagement.SceneManager.LoadScene( "gameMain" );
            }
            return this;
        }
    }
    State state_;
}
