using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour {

	[SerializeField]
	float radius_ = 1.0f;

	[SerializeField]
	float distPerSec_ = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( radius_ == 0.0f )
			radius_ = 0.0001f;
		t_ += Time.deltaTime * distPerSec_ / radius_;
		transform.localPosition = new Vector3( radius_ * Mathf.Cos( t_ ), radius_ * Mathf.Sin( t_ ), 0.0f );
	}

	float t_ = 0.0f;
}
