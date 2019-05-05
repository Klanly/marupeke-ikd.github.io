using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour {

    [SerializeField]
    GameObject model_;

    [SerializeField]
    float rotSpeed_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var q = model_.transform.localRotation;
        model_.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, Time.deltaTime * rotSpeed_ ) * q;
    }
}
