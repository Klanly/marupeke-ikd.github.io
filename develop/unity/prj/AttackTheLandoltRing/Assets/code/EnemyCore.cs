using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour {

    [SerializeField]
    SphereCollider collider_;

    [SerializeField]
    MeshRenderer coreMesh_;

    public void setup( System.Action destroyCallback )
    {
        rotUnit_.x = ( Random.value - 1.0f ) * 3.0f;
        rotUnit_.z = ( Random.value - 1.0f ) * 3.0f;
        destroyCallback_ = destroyCallback;
    }

    public float getRadius()
    {
        return collider_.radius;
    }

    public void setCoreColor( Color color )
    {
        coreMesh_.material.color = color;
    }

    public void setLimitAction()
    {
        if ( bLimitAction_ == false ) {
            bLimitAction_ = true;
            rotUnit_.x *= 10.0f;
            rotUnit_.z *= 10.0f;
        }
    }

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        coreMesh_.gameObject.transform.Rotate( rotUnit_ );
//        rotUnit_ += rotUnit_;
    }

    void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Missile" ) {
            destroyCallback_();
            DestroyObject( gameObject );
        }
    }

    Vector3 rotUnit_ = new Vector3();
    System.Action destroyCallback_;
    bool bLimitAction_ = false;
}
