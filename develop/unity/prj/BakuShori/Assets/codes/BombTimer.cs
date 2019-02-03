using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimer : MonoBehaviour {

    [SerializeField]
    TextMesh text_;

    public void startTimer( int sec )
    {
        setTime( sec );
        bTimer_ = true;
        sec_ = sec;
    }

    void setTime( int sec )
    {
        if ( sec < 0 )
            return;

        int minute = sec / 60;
        int s = sec % 60;
        text_.text = string.Format( "{0:00}:{1:00}", minute, s );
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if ( bTimer_ == false )
            return;

        sec_ -= Time.deltaTime;
        setTime( ( int )sec_ );
    }

    bool bTimer_ = true;
    float sec_ = 180.0f;
}
