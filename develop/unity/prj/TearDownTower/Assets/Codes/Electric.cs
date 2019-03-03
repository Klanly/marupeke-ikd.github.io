using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour {

	[SerializeField]
	GameObject[] electrics_;

	// Use this for initialization
	void Start () {
		foreach ( var obj in electrics_ )
			obj.SetActive( false );
		electrics_[ curIdx_ ].SetActive( true );
	}
	
	// Update is called once per frame
	void Update () {
		if ( wait_ == 0 ) {
			electrics_[ curIdx_ ].SetActive( false );
			curIdx_ = Random.Range( 0, electrics_.Length );
			electrics_[ curIdx_ ].SetActive( true );
			wait_ = 2;
		} else
			wait_--;
	}

	int curIdx_ = 0;
	int wait_ = 0;
}
