using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : FieldObject
{
	[SerializeField]
	float colWidth_ = 1.0f;

	[SerializeField]
	GameObject[] rocks_;

	private void Awake()
	{
		for ( int i = 0; i < rocks_.Length; ++i ) {
			rocks_[ i ].SetActive( false );
		}
		rocks_[ Random.Range( 0, rocks_.Length ) ].SetActive( true );
	}

	// Start is called before the first frame update
	void Start()
	{
		col_.HalfLen = new Vector2( colWidth_, 1.0f );
		shapeGroup_.addShape( col_ );
	}

	// Update is called once per frame
	void Update()
	{
		col_.Center = Vector3Util.toVector2XZ( transform.position );
		col_.XAxis = Vector3Util.toVector2XZ( transform.forward );
		updateEntry();
	}

	public Vector3 getNormal()
	{
		return transform.forward;
	}

	private void OnDrawGizmos()
	{
		GizmosUtil.drawOBB2D( col_, Color.white );
	}

	OBB2D col_ = new OBB2D();
}
