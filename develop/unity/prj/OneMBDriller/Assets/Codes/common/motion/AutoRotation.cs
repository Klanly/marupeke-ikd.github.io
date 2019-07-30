using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アタッチしたGameObjectを自動回転
public class AutoRotation : MonoBehaviour {

    [SerializeField]
    float rotAngPerSec_ = 5.0f;

    [SerializeField]
    Vector3 rotAxis_ = Vector3.up;

    [SerializeField]
    Vector3 rotAxisRand_ = Vector3.zero;

    // Use this for initialization
    void Start () {
        rotAxis_ += Vector3Util.mul( Randoms.Vec3.valueCenter() * 2.0f, rotAxisRand_ );
    }
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = Quaternion.AngleAxis( Time.deltaTime * rotAngPerSec_, rotAxis_ ) * transform.localRotation;	
	}
}
