using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatoLamp : MonoBehaviour {

    [SerializeField]
    GameObject lampIn_;

    [SerializeField]
    SpriteRenderer renderer_;

	// Use this for initialization
	void Start () {
        color_ = renderer_.color;
    }
	
	// Update is called once per frame
	void Update () {
        r_ += Time.deltaTime * 720.0f;
        r_ %= 720;
        lampIn_.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, r_ );

        color_.a = 0.5f + 0.25f * ( 1.0f + Mathf.Cos( r_ * Mathf.Deg2Rad ) );
        renderer_.color = color_;
    }

    float r_ = 0.0f;
    Color color_;
}
