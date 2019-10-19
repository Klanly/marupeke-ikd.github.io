using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 柵

public class Railling : MonoBehaviour
{
	[SerializeField]
	GameObject poleWood_;

	[SerializeField]
	GameObject railWood_;

	public float PoleHeight { get { return poleHeight_; } }
	float poleHeight_ = 1.0f;

	float railWoodOffsetH_ = 0.3f;

	public void create( Railling preRail, Vector2 pos, float poleHeight )
	{
		poleHeight_ = poleHeight;
		poleWood_.transform.localScale = new Vector3( 1.0f, 1.0f, poleHeight );
		transform.position = new Vector3( pos.x, pos.y, 0.0f );

		if ( preRail != null ) {
			float h0 = preRail.PoleHeight - railWoodOffsetH_;
			float h1 = PoleHeight - railWoodOffsetH_;
			var p0 = preRail.transform.position;
			p0.z = -h0;
			var p1 = new Vector3( pos.x, pos.y, -h1 );
			Vector3 foward = p1 - p0;
			float len = foward.magnitude;
			railWood_.transform.forward = foward;
			railWood_.transform.localPosition = new Vector3( 0.0f, 0.0f, -h1 );
			railWood_.transform.localScale = new Vector3( 1.0f, 1.0f, len );
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
