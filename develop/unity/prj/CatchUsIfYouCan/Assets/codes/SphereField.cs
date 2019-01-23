using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球形フィールド

public class SphereField : MonoBehaviour {

    [SerializeField]
    GameObject fieldRoot_;

    [SerializeField]
    float radius_ = 300.0f;

    public void setRadius( float radius )
    {
        radius_ = radius;
    }

    public float getRadius()
    {
        return radius_;
    }

	// Use this for initialization
	void Start () {
        fieldRoot_.transform.localScale = Vector3.one * radius_;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
