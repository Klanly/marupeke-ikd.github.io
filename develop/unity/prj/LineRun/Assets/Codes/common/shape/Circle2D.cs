using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Circle2D
//
//  円：中心と半径で表現

public class Circle2D : Shape2D
{
	public float Radius {
		set { r_ = value; }
		get { return r_; }
	}

	public Vector2 Center {
		set { c_ = value; }
		get { return c_; }
	}

	public bool collide(Shape2D r) {
		return r.collide( this );
	}

	public bool collide(Vector2 point) {
		if (( point - Center ).magnitude <= r_)
			return true;
		return false;
	}

	public bool collide(AABB2D r)
	{
		Vector2 colPos = Vector2.zero;
		float d = CollideUtil.distPoint_AABB2D( Center, r, out colPos );
		return ( d <= Radius );
	}

	public bool collide(Circle2D r)
	{
		return ( r.Radius + Radius ) >= ( r.Center - Center ).magnitude;
	}

	public bool collide(OBB2D r ) {
		Vector2 cp = Vector2.zero;
		float dist = CollideUtil.closestPointOBB2D_Point( r, Center, out cp );
		return dist <= Radius;
	}
	float r_ = 0.0f;
	Vector2 c_ = Vector2.zero;
}
