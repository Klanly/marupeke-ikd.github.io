using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightLine : MonoBehaviour {

    [SerializeField]
    Transform target_;

    [SerializeField]
    float offset_;

    [SerializeField]
    TextMesh fontText_;

    [SerializeField]
    float fontOffsetX_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos = target_.position;
        float targetY = targetPos.y + offset_;
        Vector3 pos = transform.position;
        if ( pos.y < targetY )
            pos.y = targetY;
        transform.position = pos;

        Vector3 fontPos = fontText_.transform.position;
        fontPos.x = targetPos.x + fontOffsetX_;
        fontText_.transform.position = fontPos;

        fontText_.text = string.Format( "{0:0.###}m", pos.y / 1000.0f );
    }
}
