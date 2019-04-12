using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {

    [SerializeField]
    string seName_;

    public void playSE() {
        if ( bPlaying_ == true )
            return;
        float sec = SoundAccessor.getInstance().playSE( seName_ );
        bPlaying_ = true;
        GlobalState.wait( sec, () => {
            bPlaying_ = false;
            return false;
        } );
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool bPlaying_ = false;
}
