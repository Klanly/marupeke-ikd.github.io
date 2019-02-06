using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        state_ = new Title();
	}
	
	// Update is called once per frame
	void Update () {
        state_ = state_.update();
	}

    class Title : State
    {
        protected override State innerUpdate()
        {
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                UnityEngine.SceneManagement.SceneManager.LoadScene( "gameMain" );
            }
            return this;
        }
    }
    State state_;
}
