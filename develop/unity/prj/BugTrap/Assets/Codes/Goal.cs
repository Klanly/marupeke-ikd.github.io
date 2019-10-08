using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : FieldObject
{
	[SerializeField]
	float colX_;

	[SerializeField]
	float colY_;

	private void Awake()
	{
		shapeGroup_.addShape( col_ );
	}

	void Start()
    {
		xy_.x = colX_;
		xy_.y = colY_;
		col_.HalfLen = xy_;
	}

	void Update()
    {
		xy_.x = colX_;
		xy_.y = colY_;
		col_.HalfLen = xy_;
		col_.Center = Vector3Util.toVector2XZ( transform.position );
		col_.XAxis = Vector3Util.toVector2XZ( transform.forward );
		updateEntry();

		// 虫を捉えたかチェック
		if (objectManager_ == null)
			return;

		var list = objectManager_.checkCollideToBug( this );
		if ( list != null ) {
			foreach( var o in list ) {
				var bug = o as Bug;
				if ( bug != null ) {
					objectManager_.catchBug( bug );
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		GizmosUtil.drawOBB2D( col_, Color.white );
	}

	OBB2D col_ = new OBB2D();
	Vector2 xy_ = Vector2.zero;
}
