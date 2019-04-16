using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCursor : MonoBehaviour {

    [SerializeField]
    SpriteRenderer renderer_;

    [SerializeField]
    float speed_ = 1.0f;

    public void remove( float fadeSec ) {
        var color = renderer_.color;
        GlobalState.time( fadeSec, (sec, t) => {
            color.a = Lerps.Float.linear( 1.0f, 0.0f, t );
            renderer_.color = color;
            return true;
        } ).finish(()=> {
            Destroy( gameObject );
        } );
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        defRot_ = Quaternion.Euler( 0.0f, speed_ * Time.deltaTime, 0.0f );
        transform.localRotation = transform.localRotation * defRot_;
    }

    Quaternion defRot_;
}
