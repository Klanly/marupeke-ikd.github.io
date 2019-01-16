using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBar : MonoBehaviour {

    [SerializeField]
    GameObject fireTop_;

    [SerializeField]
    GameObject fireBody_;

    [SerializeField]
    Transform root_;

    [SerializeField]
    bool playOnAwake = true;

    [SerializeField]
    FireDirection dir_ = FireDirection.DIR_RIGHT;

    [SerializeField]
    float speed_ = 5.0f;

    [SerializeField]
    float dist_ = 100.0f;

    public enum FireDirection
    {
        DIR_RIGHT,
        DIR_LEFT,
        DIR_UP,
        DIR_DOWN
    }

    public void fire(FireDirection dir, Vector3 pos, float speed, float dist )
    {
        dir_ = dir;
        transform.position = pos;
        speed_ = speed;
        dist_ = dist;
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if ( playOnAwake == false )
            return;

        if ( bFirst_ == false && prePlayOnAwake_ == false && playOnAwake == true ) {
            bFirst_ = true;
            bFinish_ = false;
            Destroy( fireTopObj_.gameObject );
        }
        prePlayOnAwake_ = playOnAwake;

        if ( bFinish_ == true )
            return;

        if ( bFirst_ == true ) {
            bFirst_ = false;
            fireTopObj_ = Instantiate<GameObject>( fireTop_ );
            fireTopObj_.transform.parent = root_;
            initPos_ = transform.position;
            fireTopObj_.transform.position = initPos_;
            switch ( dir_ ) {
                case FireDirection.DIR_RIGHT:
                    direction_ = Vector3.right;
                    break;
                case FireDirection.DIR_LEFT:
                    direction_ = Vector3.left;
                    root_.transform.localRotation = Quaternion.Euler( 0.0f, 180.0f, 0.0f );
                    break;
                case FireDirection.DIR_UP:
                    direction_ = Vector3.forward;
                    root_.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
                    break;
                case FireDirection.DIR_DOWN:
                    direction_ = Vector3.back;
                    root_.transform.localRotation = Quaternion.Euler( 0.0f, -90.0f, 0.0f );
                    break;
            }
            curDist_ = 0.0f;
        }

        curDist_ += speed_ * Time.deltaTime;
        if ( curDist_ > dist_ )
            curDist_ = dist_;
        fireTopObj_.transform.position = initPos_ + curDist_ * direction_;
        if ( curDist_ >= dist_ ) {
            bFinish_ = true;
        }
	}

    bool prePlayOnAwake_ = false;
    bool bFirst_ = true;
    bool bFinish_ = false;
    float curDist_ = 0.0f;
    GameObject fireTopObj_;
    List< GameObject > fireBodies_;
    Vector3 direction_ = Vector3.zero;
    Vector3 initPos_;
}
