using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// OBB2D
//  中心座標とX軸の方向ベクトル、X軸Y軸の長さで表現

public class OBB2D : Shape2D {
	public Vector2 Center {
		set { c_ = value; }
		get { return c_; }
	}
	public Vector2 XAxis {
		set {
			x_ = value.normalized;
			y_.x = -x_.y;
			y_.y = x_.x;
		}
		get { return x_; }
	}
	public Vector2 YAxis {
		set {
			y_ = value.normalized;
			x_.x = y_.y;
			x_.y = -y_.x;
		}
		get { return y_; }
	}
	public Vector2 HalfLen {
		set {
			len_ = value;
		}
		get {
			return len_;
		}
	}

	public bool collide(Vector2 point) {
		Vector2 cp = Vector2.zero;
		float dist = CollideUtil.closestPointOBB2D_Point( this, point, out cp );
		return dist <= 0.0f;
	}

	public bool collide( Circle2D r )
	{
		Vector2 cp = Vector2.zero;
		float dist = CollideUtil.closestPointOBB2D_Point( this, r.Center, out cp );
		return dist <= r.Radius;
	}

	public bool collide(AABB2D r) {
		var x = tmpObb_.XAxis;
		x.x = 1.0f; x.y = 0.0f;
		tmpObb_.Center = r.Center;
		tmpObb_.XAxis = x;
		tmpObb_.HalfLen = r.Len * 0.5f;
		return collide( tmpObb_ );
	}

	public bool collide(OBB2D r) {
		// 円での一次近似判定
		var centerV = Center - r.Center;
		float myLen = HalfLen.magnitude;
		float otherLen = r.HalfLen.magnitude;
		if (myLen + otherLen < centerV.magnitude) {
			return false;
		}

		// 分離軸判定
		float dist = Mathf.Abs( Vector2.Dot( x_, r.XAxis ) * r.HalfLen.x ) + ( Mathf.Abs( Vector2.Dot( x_, r.YAxis ) * r.HalfLen.y ) ) + len_.x;
		float centerDist = Mathf.Abs( Vector2.Dot( x_, centerV ) );
		if ( dist < centerDist ) {
			return false;
		}
		dist = Mathf.Abs( Vector2.Dot( y_, r.XAxis ) * r.HalfLen.x ) + ( Mathf.Abs( Vector2.Dot( y_, r.YAxis ) * r.HalfLen.y ) ) + len_.y;
		centerDist = Mathf.Abs( Vector2.Dot( y_, centerV ) );
		if (dist < centerDist) {
			return false;
		}
		dist = Mathf.Abs( Vector2.Dot( r.XAxis, x_ ) * HalfLen.x ) + ( Mathf.Abs( Vector2.Dot( r.XAxis, y_ ) * HalfLen.y ) + r.HalfLen.x );
		centerDist = Mathf.Abs( Vector2.Dot( r.XAxis, centerV ) );
		if (dist < centerDist) {
			return false;
		}
		dist = Mathf.Abs( Vector2.Dot( r.YAxis, x_ ) * HalfLen.x ) + ( Mathf.Abs( Vector2.Dot( r.YAxis, y_ ) * HalfLen.y + r.HalfLen.y ) );
		centerDist = Mathf.Abs( Vector2.Dot( r.YAxis, centerV ) );
		if (dist < centerDist) {
			return false;
		}
		return true;
	}

	public Vector2[] getVertices() {
		Vector2 min = c_ - x_ * len_.x - y_ * len_.y;
		return new Vector2[] {
			min,
			min + y_ * len_.y * 2.0f,
			c_ + y_ * len_.y + x_ * len_.x,
			min + x_ * len_.x * 2.0f
		};
	}

	Vector2 c_ = Vector2.zero;
	Vector2 x_ = new Vector2( 1.0f, 0.0f );
	Vector2 y_ = new Vector2( 0.0f, 1.0f );
	Vector2 len_ = Vector2.one;
	static OBB2D tmpObb_ = new OBB2D();	// 計算用テンポラリ
}
