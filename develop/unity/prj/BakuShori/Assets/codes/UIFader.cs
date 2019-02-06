using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Image image_;

    public void fade(Color color, float sec, System.Action finishCallback)
    {
        Color startColor = image_.color;
        GlobalState.time( sec, (_, t) => {
            image_.color = Color.Lerp( startColor, color, t );
            return true;
        } ).finish(()=> {
            if ( finishCallback != null )
                finishCallback();
        } );
    }

    // Use this for initialization
    void Start () {
        GlobalState.time( 2.0f, (sec, t) => {
            image_.color = Color.Lerp( Color.black, Color.clear, t );
            return true;
        } );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
