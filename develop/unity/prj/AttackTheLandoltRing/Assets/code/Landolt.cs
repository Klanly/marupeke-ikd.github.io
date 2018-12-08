using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landolt : MonoBehaviour {

    [SerializeField]
    LandoltMesh mesh_;

    public void setup( float radius, float degree )
    {
        radius_ = radius;
        mesh_.setup( radius, degree, tickness_, 16 );
    }

    public float getTickness()
    {
        return tickness_;
    }

    public float getSpaceRadius()
    {
        return mesh_.getSpaceRadius();
    }

    public float getRadius()
    {
        return radius_ + tickness_;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float tickness_ = 4.0f;
    float radius_ = 5.0f;
}
