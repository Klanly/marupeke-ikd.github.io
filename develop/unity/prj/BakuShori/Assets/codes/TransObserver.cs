using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Targetの姿勢を追従する
public class TransObserver : MonoBehaviour {

    public void setTarget( Transform target )
    {
        target_ = target;
    }

	// Use this for initialization
	void Start () {
        transform.position = target_.position;
        transform.rotation = target_.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target_.position;
        transform.rotation = target_.rotation;
    }
    Transform target_;
}
