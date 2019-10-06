using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AABB2D
//
//  左下隅（最小座標）-右上隅（最大座標）で表現

public class AABB2D : Shape2D {
	public Vector2 Center {
		set {
			center_ = value;
		}
		get {
			return center_;
		}
	} 
	public Vector2 HalfLen {
		set {
			len_ = value;
		}
		get {
			return len_;
		}
	}
    public Vector2 Min {
        get {
            return center_ - len_;
        }
    }
    public Vector2 Max {
        get {
			return center_ + len_;
		}
	}
    public Vector2 Len {
        get {
            return len_ * 2.0f;
        }
    }
    public AABB2D() {

    }
    public AABB2D( float minX, float minY, float maxX, float maxY ) {
		Swaps.minMax( ref minX, ref maxX );
		Swaps.minMax( ref minY, ref maxY );
		center_.x = ( maxX + minX ) * 0.5f;
		center_.y = ( maxY + minY ) * 0.5f;
		len_.x = ( maxX - minX ) * 0.5f;
		len_.y = ( maxY - minY ) * 0.5f;
    }

	public bool collide(Shape2D r) {
		return r.collide( this );
	}

	public bool collide( Vector2 point ) {
        if ( Vector2Util.orMin( point, Min ) == true )
            return false;
        if ( Vector2Util.orMax( point, Min ) == true )
            return false;
        return true;
    }
    public bool collide( AABB2D r ) {
        if ( Vector2Util.orMin( Max, r.Min ) == true )
            return false;
        if ( Vector2Util.orMin( r.Max, Min ) == true )
            return false;
        return true;
    }
	public bool collide(Circle2D r)
	{
		return r.collide( this );
	}
	public bool collide(OBB2D r)
	{
		return r.collide( this );
	}
	public Vector2 distance( Vector2 point, ref Vector2 normal ) {
        if ( collide( point ) == true ) {
            // めり込んでいるので中心点から外へ向かう方向を法線とする
            normal = ( point - Center ).normalized;
            return point;
        }

        var colPos = new Vector2(
            point.x < Min.x ? Min.x : ( point.x > Max.x ? Max.x : point.x ),
            point.y < Min.y ? Min.y : ( point.y > Max.y ? Max.y : point.y )
        );

        normal.x = ( colPos.x <= Min.x ? -1.0f : ( colPos.x >= Max.x ? 1.0f : 0.0f ) );
        normal.y = ( colPos.y <= Min.y ? -1.0f : ( colPos.y >= Max.y ? 1.0f : 0.0f ) );
        normal = normal.normalized;

        return colPos;
    }


    public void set( Vector2 center, Vector2 halfLen ) {
		center_ = center;
		len_ = halfLen;	// half
    }

    public Vector2[] getVertices() {
        return new Vector2[] {
            Min,
			new Vector2( Min.x, Max.y ),
			Max,
			new Vector2( Max.x, Min.y )
        };
    }
	Vector2 center_ = Vector2.zero;
	Vector2 len_ = Vector2.zero;	// half
}
