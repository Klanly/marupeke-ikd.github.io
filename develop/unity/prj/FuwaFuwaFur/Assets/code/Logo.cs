using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour {

    [SerializeField]
    SpriteRenderer sp_;

    [SerializeField]
    float sizeTime_ = 3.0f;

    [SerializeField]
    float size_ = 4.0f;

    [SerializeField]
    float upDownTime_ = 2.0f;

    // Use this for initialization
    void Start () {
        logoPos_ = sp_.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        offset_.y = ( 1.0f + size_ * Mathf.Cos( t / sizeTime_ * Mathf.PI * 2.0f ) ) * Mathf.Sin( t / upDownTime_ * Mathf.PI * 2.0f );
        sp_.transform.position = logoPos_ + offset_;
    }

    Vector3 offset_ = Vector3.zero;
    Vector3 logoPos_;
    float t = 0.0f;
}
