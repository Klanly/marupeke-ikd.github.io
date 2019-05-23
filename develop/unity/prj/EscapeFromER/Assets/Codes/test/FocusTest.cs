using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusTest : MonoBehaviour {

    [SerializeField]
    Focus focus_;

    [SerializeField]
    bool bActive_ = false;

    [SerializeField]
    bool bFinish_ = false;

	// Use this for initialization
	void Start () {
        focus_.setSize( new Bounds( new Vector3( 1.0f, 2.0f, 3.0f ), new Vector3( 2.0f, 4.0f, 3.0f ) ) );

    }
	
	// Update is called once per frame
	void Update () {
        if ( bActive_ == true ) {
            bActive_ = false;
            focus_.active();
        }		
        if ( bFinish_ == true ) {
            bFinish_ = false;
            focus_.destroy();
        }
	}
}
