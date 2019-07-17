using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アタッチしたGameObjectを自動回転
public class AutoRotation : MonoBehaviour {

    [SerializeField]
    float rotAngPerSec_ = 5.0f;

    [SerializeField]
    Vector3 rotAxis_ = Vector3.up;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = Quaternion.AngleAxis( Time.deltaTime * rotAngPerSec_, rotAxis_ ) * transform.localRotation;	
	}
}
