using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t_ += Time.deltaTime * Mathf.PI * 2.0f * 10.0f;
        scale_.z = 1.0f + 0.2f * ( 1.0f + Mathf.Sin( t_ ) );
        transform.localScale = scale_;
	}

    Vector3 scale_ = Vector3.one;
    float t_ = 0.0f;
}
