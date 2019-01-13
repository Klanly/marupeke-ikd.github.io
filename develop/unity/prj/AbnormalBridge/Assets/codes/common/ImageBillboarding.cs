using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// イメージをビルボーディングする
//  イメージじゃなくても使えますよ

public class ImageBillboarding : MonoBehaviour {

    [SerializeField]
    GameObject target_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position );
	}
}
