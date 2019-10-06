using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : FieldObject
{
	[SerializeField]
	float colWidth_ = 0.2f;

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
