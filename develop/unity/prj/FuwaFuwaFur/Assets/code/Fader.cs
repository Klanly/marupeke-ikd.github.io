using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour {

    [SerializeField]
    SpriteRenderer sprite_;

    [SerializeField]
    float moveRate_ = 0.05f;

    public void setFade( float aim, float moveRate = 0.0f )
    {
        value_.setAim( aim );
        if ( moveRate > 0.0f && moveRate < 1.0f ) {
            value_.setRate( moveRate_ );
            moveRate_ = moveRate;
        }
    }
	// Use this for initialization
	void Start () {
        value_ = new MoveValue( 0.0f, moveRate_, 0.001f );
    }
	
	// Update is called once per frame
	void Update () {
        Color c = sprite_.color;
        c.a = value_.update();
        sprite_.color = c;
    }
    MoveValue value_;
}
