using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<GlobalState> states = new List<GlobalState>();
		for ( int i = 0; i < 100; ++i ) {
			float sec = Random.value * 6.0f;
			states.Add( GlobalState.wait( sec, () => { Debug.Log( "Finish " + sec ); return false; } ) );
		}
		GlobalState.parallel( states.ToArray() ).finish( () => {
			Debug.Log( "Finish All" );
		} );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
