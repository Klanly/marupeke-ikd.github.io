using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text timeText_;

    public void start()
    {
        bMove_ = true;
    }

    public void stop()
    {
        bMove_ = false;
    }

    public void clear()
    {
        curSec_ = 0.0f;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bMove_ == false )
            return;
        curSec_ += Time.deltaTime;
        timeText_.text = string.Format( "{0:00}:{1:00}:{2:00}", ( int )( curSec_ / 60.0f ), ( int )( curSec_ % 60 ), ( int )( ( curSec_ * 100.0f ) % 100 ) );
    }

    bool bMove_ = false;
    float curSec_ = 0.0f;
}
