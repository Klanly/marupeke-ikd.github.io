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

    [SerializeField]
    GameObject particle_;

    [SerializeField]
    GameObject namePlateRoot_;

    [SerializeField]
    TextMesh namePlate_;


    public void setName( string name )
    {
        namePlateRoot_.gameObject.SetActive( true );
        namePlate_.text = name;
    }

    public void setHipId( int hipId )
    {
        hipId_ = hipId;
    }

    public int getHipId()
    {
        return hipId_;
    }

    public void setPolerCoord( float lat, float longi )
    {
        lat_ = lat;
        longi_ = longi;
    }

    public void runParticle()
    {
        particle_.SetActive( true );
    }

    public void setColorScale( float scale )
    {
        colorScale_ = scale;
        setColor( new Vector3( baseColor_.r, baseColor_.g, baseColor_.b ), alpha_ );
    }

    public void setColor( Vector3 color, float alpha )
    {
        baseColor_ = new Color( color.x, color.y, color.z, alpha );
        alpha_ = alpha;
        var mat = renderer_.material;
        var c = baseColor_ * colorScale_;
        c.a = alpha;
        mat.color = c;
        renderer_.material = mat;
    }

    public void setAlpha( float alpha )
    {
        alpha_ = alpha;
        var mat = renderer_.material;
        var color = mat.color;
        color.a = alpha;
        mat.color = color;
        renderer_.material = mat;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Color baseColor_;
    float alpha_ = 1.0f;
    float colorScale_ = 1.0f;
}
