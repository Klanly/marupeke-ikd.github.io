using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UnityEngine.SceneManagement.SceneManager.LoadScene( "gameMain" );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    class Title : State
    {
        protected override State innerInit()
        {
            return base.innerInit();
        }
    }
    State state_;
}
