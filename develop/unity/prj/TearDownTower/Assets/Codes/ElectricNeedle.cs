using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNeedle : MonoBehaviour {

	[SerializeField]
	GameObject pos_;

	public void setup( float radius, float rotY ) {
		pos_.transform.localPosition = new Vector3( 0.0f, 0.0f, -radius );
		pos_.transform.localRotation = Quaternion.Euler( 0.0f, rotY, 0.0f );
	}

	public void setRot( float rot ) {
		transform.localRotation = Quaternion.Euler( 0.0f, rot, 0.0f );
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
