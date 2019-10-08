using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWood : FieldObject
{
	[SerializeField]
	float colWidth_ = 0.2f;

	[SerializeField]
	Transform[] collisionPoses_ = new Transform[3];

	// Start is called before the first frame update
	void Start()
	{
		for ( int i = 0; i < collisionPoses_.Length; ++i ) {
			collisions_[ i ] = new OBB2D();
			collisions_[ i ].HalfLen = new Vector2( colWidth_, 1.0f );
			collisions_[ i ].Center = Vector3Util.toVector2XZ( collisionPoses_[ i ].transform.position );
			collisions_[ i ].XAxis = Vector3Util.toVector2XZ( collisionPoses_[ i ].transform.forward );
			shapeGroup_.addShape( collisions_[ i ] );
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < collisions_.Length; ++i) {
			collisions_[ i ].HalfLen = new Vector2( colWidth_, 1.0f );
			collisions_[ i ].Center = Vector3Util.toVector2XZ( collisionPoses_[ i ].transform.position );
			collisions_[ i ].XAxis = Vector3Util.toVector2XZ( collisionPoses_[ i ].transform.forward );
			shapeGroup_.addShape( collisions_[ i ] );
		}
		updateEntry();
	}

	public Vector3 getNormal()
	{
		return transform.forward;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < collisions_.Length; ++i) {
			GizmosUtil.drawOBB2D( collisions_[ i ], Color.white );
		}
	}

	OBB2D[] collisions_ = new OBB2D[3];
}
