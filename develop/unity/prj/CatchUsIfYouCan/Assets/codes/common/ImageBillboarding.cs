using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// イメージをビルボーディングする
//  イメージじゃなくても使えますよ

public class ImageBillboarding : MonoBehaviour {

    [SerializeField]
    GameObject target_;

    [SerializeField]
    bool useCameraUp_ = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( useCameraUp_ == true ) {
            target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position - target_.transform.position, Camera.main.transform.up );
        } else {
            target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position - target_.transform.position );
        }
	}
}
