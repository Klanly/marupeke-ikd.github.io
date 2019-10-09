using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGroup2D : Shape2D {
	public void addShape( Shape2D shape ) {
		shapes_.Add( shape );
	}

	public void removeShape( Shape2D shape ) {
		for ( int i = 0; i < shapes_.Count; ++i ) {
			if ( shapes_[ i ] == shape ) {
				shapes_.Remove( shape );
				return;
			}
		}
	}

	public void removeAll() {
		shapes_.Clear();
	}

	public bool collide( ShapeGroup2D group ) {
		var list = group.shapes_;
		int listSz = list.Count;
		for ( int i = 0; i < listSz; ++i ){
			int shapeSz = shapes_.Count;
			for (int j = 0; j < shapeSz; ++j ) {
				if (shapes_[ j ].collide( list[ i ] ) == true) {
					return true;
				}
			}
		}
		return false;
	}

	public bool collide(Shape2D r) {
		return r.collide( this );
	}

	public bool collide( Vector2 point ) {
		foreach ( var s in shapes_ ) {
			if ( s.collide( point ) == true ) {
				return true;
			}
		}
		return false;
	}

	public bool collide( AABB2D r ) {
		foreach (var s in shapes_) {
			if (s.collide( r ) == true) {
				return true;
			}
		}
		return false;
	}

	public bool collide( Circle2D r ) {
		foreach (var s in shapes_) {
			if (s.collide( r ) == true) {
				return true;
			}
		}
		return false;
	}

	public bool collide( OBB2D r ) {
		foreach (var s in shapes_) {
			if (s.collide( r ) == true) {
				return true;
			}
		}
		return false;
	}

	List<Shape2D> shapes_ = new List<Shape2D>();
}
