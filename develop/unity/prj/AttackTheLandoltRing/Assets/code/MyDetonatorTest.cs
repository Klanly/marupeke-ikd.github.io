using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDetonatorTest : MonoBehaviour {

    [SerializeField]
    Detonator detonator_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown( 0 ) == true ) {
            var obj = Instantiate<Detonator>( detonator_ );
            obj.transform.localPosition = Vector3.zero;

            obj.Explode();
        }
	}
}
