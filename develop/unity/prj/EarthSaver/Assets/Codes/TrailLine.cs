using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 外部指定トレイルライン
public class TrailLine : MonoBehaviour {

    [SerializeField]
    LineRenderer renderer_;

    public void setup( List<Vector3> positions, float width ) {
        renderer_.positionCount = positions.Count;
        renderer_.SetPositions( positions.ToArray() );
        renderer_.startWidth = width;
        renderer_.endWidth = width;
    }

    public void setAlpha( float a ) {
        var mat = renderer_.material;
        var color = mat.GetColor( "_TintColor" );
        color.a = a;
        mat.SetColor( "_TintColor", color );
        renderer_.material = mat;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
