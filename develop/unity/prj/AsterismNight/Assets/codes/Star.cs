using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星
public class Star : MonoBehaviour {

    [SerializeField]
    int hipId_ = 0;

    [SerializeField]
    float lat_ = 0.0f;

    [SerializeField]
    float longi_ = 0.0f;

    [SerializeField]
    MeshRenderer renderer_;

    public void setHipId( int hipId )
    {
        hipId_ = hipId;
    }

    public void setPolerCoord( float lat, float longi )
    {
        lat_ = lat;
        longi_ = longi;
    }

    public void setColor( Vector3 color )
    {
        var mat = renderer_.material;
        mat.color = new Color( color.x, color.y, color.z );
        renderer_.material = mat;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
