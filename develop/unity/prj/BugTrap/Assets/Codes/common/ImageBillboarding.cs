using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// イメージをビルボーディングする
//  イメージじゃなくても使えますよ

public class ImageBillboarding : MonoBehaviour {

    [SerializeField]
    GameObject target_ = null;

    [SerializeField]
    bool useCameraUp_ = false;

    [SerializeField]
    bool useUserUp_ = false;

    // ユーザアップベクトルを設定
    public void setUserUp( Vector3 up )
    {
        userUp_ = up;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( useCameraUp_ == true ) {
            target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position - target_.transform.position, Camera.main.transform.up );
        } else if ( useUserUp_ == true ) {
            target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position - target_.transform.position, userUp_ );
        } else {
            target_.transform.rotation = Quaternion.LookRotation( Camera.main.transform.position - target_.transform.position );
        }
    }

    Vector3 userUp_ = Vector3.up;
}
