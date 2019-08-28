using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AABB2D
//
//  左下隅（最小座標）-右上隅（最大座標）で表現

public class AABB2D {
    public Vector2 Min {
        set {
            min_ = value;
            Swaps.minMax( ref min_, ref max_ );
        }
        get {
            return min_;
        }
    }
    public Vector2 Max {
        set {
            max_ = value;
            Swaps.minMax( ref min_, ref max_ );
        }
        get {
            return max_;
        }
    }
    public Vector2 Center {
        get {
            return ( min_ + max_ ) * 0.5f;
        }
    }
    public Vector2 Len {
        get {
            return ( max_ - min_ );
        }
    }
    public AABB2D() {

    }
    public AABB2D( float minX, float minY, float maxX, float maxY ) {
        min_.x = minX;
        min_.y = minY;
        max_.x = maxX;
        max_.y = maxY;
        Swaps.minMax( ref min_, ref max_ );
    }
    public bool collide( Vector2 point ) {
        if ( Vector2Util.orMin( point, min_ ) == true )
            return false;
        if ( Vector2Util.orMax( point, min_ ) == true )
            return false;
        return true;
    }
    public bool collide( AABB2D r ) {
        if ( Vector2Util.orMin( max_, r.min_ ) == true )
            return false;
        if ( Vector2Util.orMin( r.max_, min_ ) == true )
            return false;
        return true;
    }
    public Vector2 distance( Vector2 point, ref Vector2 normal ) {
        if ( collide( point ) == true ) {
            // めり込んでいるので中心点から外へ向かう方向を法線とする
            normal = ( point - Center ).normalized;
            return point;
        }

        var colPos = new Vector2(
            point.x < min_.x ? min_.x : ( point.x > max_.x ? max_.x : point.x ),
            point.y < min_.y ? min_.y : ( point.y > max_.y ? max_.y : point.y )
        );

        normal.x = ( colPos.x <= min_.x ? -1.0f : ( colPos.x >= max_.x ? 1.0f : 0.0f ) );
        normal.y = ( colPos.y <= min_.y ? -1.0f : ( colPos.y >= max_.y ? 1.0f : 0.0f ) );
        normal = normal.normalized;

        return colPos;
    }


    public void set( List<Vector2> list ) {
        Vector2Util.calcRegion( list, out min_, out max_ );
    }
    public Vector2[] getVertices() {
        return new Vector2[] {
            new Vector2( min_.x, min_.y ),
            new Vector2( max_.x, min_.y ),
            new Vector2( min_.x, max_.y ),
            new Vector2( max_.x, max_.y )
        };
    }

    Vector2 min_ = Vector2.zero;
    Vector2 max_ = Vector3.zero;
}
