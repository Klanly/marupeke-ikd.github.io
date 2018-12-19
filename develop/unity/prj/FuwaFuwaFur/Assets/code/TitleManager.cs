using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public bool isFinish()
    {
        return bFinish_;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetMouseButtonDown( 0 ) == true )
            bFinish_ = true;
	}

    bool bFinish_ = false;
}
