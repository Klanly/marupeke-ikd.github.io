using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AABB
//
//  左下隅（最小座標）-右上隅（最大座標）で表現

public class AABB {
    public Vector3 Min {
        set {
            min_ = Vector3.Min( value, max_ );
            max_ = Vector3.Max( value, max_ );
        }
        get {
            return min_;
        }
    }
    public Vector3 Max {
        set {
            min_ = Vector3.Min( value, min_ );
            max_ = Vector3.Max( value, min_ );
        }
        get {
            return max_;
        }
    }
    public Vector3 Center {
        get {
            return ( min_ + max_ ) * 0.5f;
        }
    }
    public Vector3 Len {
        get {
            return ( max_ - min_ );
        }
    }
    public bool collide( Vector3 point ) {
        if ( Vector3Util.orMin( point, min_ ) == true )
            return false;
        if ( Vector3Util.orMax( point, min_ ) == true )
            return false;
        return true;
    }
    public bool collide( AABB r ) {
        if ( Vector3Util.orMin( max_, r.min_ ) == true )
            return false;
        if ( Vector3Util.orMin( r.max_, min_ ) == true )
            return false;
        return true;
    }
    public void set( List<Vector3> list ) {
        Vector3Util.calcRegion( list, out min_, out max_ );
    }
    public Vector3[] getVertices() {
        return new Vector3[] {
            new Vector3( min_.x, min_.y, min_.z ),
            new Vector3( max_.x, min_.y, min_.z ),
            new Vector3( min_.x, max_.y, min_.z ),
            new Vector3( max_.x, max_.y, min_.z ),
            new Vector3( min_.x, min_.y, max_.z ),
            new Vector3( max_.x, min_.y, max_.z ),
            new Vector3( min_.x, max_.y, max_.z ),
            new Vector3( max_.x, max_.y, max_.z ),
        };
    }

    Vector3 min_ = Vector3.zero;
    Vector3 max_ = Vector3.zero;
}
