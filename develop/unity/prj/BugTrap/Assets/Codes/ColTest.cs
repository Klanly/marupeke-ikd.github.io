using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTest : MonoBehaviour
{
	[SerializeField]
	Transform Point2D1;

	[SerializeField]
	Transform Point2D2;

	[SerializeField]
	Transform Circle2D1;
	[SerializeField]
	float circle2D1_Radius = 1;

	[SerializeField]
	Transform Circle2D2;
	[SerializeField]
	float circle2D2_Radius;

	[SerializeField]
	Transform AABB2D1;
	[SerializeField, Range( 0.0f, 10.0f )]
	float AABB2D1_LenX_;
	[SerializeField, Range( 0.0f, 10.0f )]
	float AABB2D1_LenY_;

	[SerializeField]
	Transform AABB2D2;
	[SerializeField, Range( 0.0f, 10.0f )]
	float AABB2D2_LenX_;
	[SerializeField, Range( 0.0f, 10.0f )]
	float AABB2D2_LenY_;

	[SerializeField]
	Transform OBB2D1;
	[SerializeField, Range( 0.0f, 10.0f )]
	float OBB2D1_LenX_;
	[SerializeField, Range( 0.0f, 10.0f )]
	float OBB2D1_LenY_;

	[SerializeField]
	Transform OBB2D2;
	[SerializeField, Range( 0.0f, 10.0f )]
	float OBB2D2_LenX_;
	[SerializeField, Range( 0.0f, 10.0f )]
	float OBB2D2_LenY_;


	void Start()
    {
        
    }

    void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
		// point1
		p1_.Radius = 0.0f;
		if (Point2D1 != null) {
			var p = Point2D1.transform.position;
			p1_.Center = p;
		}
		// point2
		p2_.Radius = 0.0f;
		if (Point2D2 != null) {
			var p = Point2D2.transform.position;
			p2_.Center = p;
		}
		// circle2D1
		c1_.Radius = circle2D1_Radius;
		if (Circle2D1 != null) {
			var p = Circle2D1.transform.position;
			c1_.Center = p;
		}
		// circle2D2
		c2_.Radius = circle2D2_Radius;
		if (Circle2D2 != null) {
			var p = Circle2D2.transform.position;
			c2_.Center = p;
		}
		// aabb2D1
		aabb1_.HalfLen = new Vector2( AABB2D1_LenX_, AABB2D1_LenY_ );
		if ( AABB2D1 != null) {
			aabb1_.Center = AABB2D1.transform.position;
		}
		// aabb2D2
		aabb2_.HalfLen = new Vector2( AABB2D2_LenX_, AABB2D2_LenY_ );
		if (AABB2D2 != null) {
			aabb2_.Center = AABB2D2.transform.position;
		}
		// obb2D1
		obb1_.HalfLen = new Vector2( OBB2D1_LenX_, OBB2D1_LenY_ );
		if (OBB2D1 != null) {
			obb1_.Center = OBB2D1.transform.position;
			obb1_.XAxis = OBB2D1.transform.right;
		}
		// obb2D2
		obb2_.HalfLen = new Vector2( OBB2D2_LenX_, OBB2D2_LenY_ );
		if (OBB2D2 != null) {
			obb2_.Center = OBB2D2.transform.position;
			obb2_.XAxis = OBB2D2.transform.right;
		}

		aabb1_.collide( obb2_ );

		List<Shape2D> shapes = new List<Shape2D>();
		shapes.Add( p1_ );
		shapes.Add( p2_ );
		shapes.Add( c1_ );
		shapes.Add( c2_ );
		shapes.Add( aabb1_ );
		shapes.Add( aabb2_ );
		shapes.Add( obb1_ );
		shapes.Add( obb2_ );

		if (Point2D1 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != p1_ && sh.collide( p1_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var p = Point2D1.transform.position;
			Gizmos.DrawWireSphere( p, 0.1f );
		}
		if (Point2D2 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != p2_ && sh.collide( p2_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var p = Point2D2.transform.position;
			Gizmos.DrawWireSphere( p, 0.1f );
		}
		if (Circle2D1 != null) {
			bool col = false;
			foreach ( var sh in shapes ) {
				if ( sh != c1_ && sh.collide( c1_ ) ) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var p = Circle2D1.transform.position;
			Vector3 s = new Vector3( 1.0f, 0.0f, 0.0f );
			for (int i = 1; i <= 20; ++i) {
				float a = (float)i / 20;
				Vector3 e = new Vector3( Mathf.Cos( Mathf.PI * 2 * a ), Mathf.Sin( Mathf.PI * 2 * a ) );
				Gizmos.DrawLine( circle2D1_Radius * s + p, circle2D1_Radius * e + p );
				s = e;
			}
		}
		if ( Circle2D2 != null ) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != c2_ && sh.collide( c2_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var p = Circle2D2.transform.position;
			Vector3 s = new Vector3( 1.0f, 0.0f, 0.0f );
			for (int i = 1; i <= 20; ++i) {
				float a = (float)i / 20;
				Vector3 e = new Vector3( Mathf.Cos( Mathf.PI * 2 * a ), Mathf.Sin( Mathf.PI * 2 * a ) );
				Gizmos.DrawLine( circle2D2_Radius * s + p, circle2D2_Radius * e + p );
				s = e;
			}
		}
		if (AABB2D1 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != aabb1_ && sh.collide( aabb1_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var vs = aabb1_.getVertices();
			Gizmos.DrawLine( vs[ 0 ], vs[ 1 ] );
			Gizmos.DrawLine( vs[ 1 ], vs[ 2 ] );
			Gizmos.DrawLine( vs[ 2 ], vs[ 3 ] );
			Gizmos.DrawLine( vs[ 3 ], vs[ 0 ] );
		}
		if (AABB2D2 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != aabb2_ && sh.collide( aabb2_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var vs = aabb2_.getVertices();
			Gizmos.DrawLine( vs[ 0 ], vs[ 1 ] );
			Gizmos.DrawLine( vs[ 1 ], vs[ 2 ] );
			Gizmos.DrawLine( vs[ 2 ], vs[ 3 ] );
			Gizmos.DrawLine( vs[ 3 ], vs[ 0 ] );
		}
		if (OBB2D1 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != obb1_ && sh.collide( obb1_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var vs = obb1_.getVertices();
			Gizmos.DrawLine( vs[ 0 ], vs[ 1 ] );
			Gizmos.DrawLine( vs[ 1 ], vs[ 2 ] );
			Gizmos.DrawLine( vs[ 2 ], vs[ 3 ] );
			Gizmos.DrawLine( vs[ 3 ], vs[ 0 ] );
		}
		if (OBB2D2 != null) {
			bool col = false;
			foreach (var sh in shapes) {
				if (sh != obb2_ && sh.collide( obb2_ )) {
					col = true;
					break;
				}
			}
			Gizmos.color = ( col ? Color.red : Color.white );
			var vs = obb2_.getVertices();
			Gizmos.DrawLine( vs[ 0 ], vs[ 1 ] );
			Gizmos.DrawLine( vs[ 1 ], vs[ 2 ] );
			Gizmos.DrawLine( vs[ 2 ], vs[ 3 ] );
			Gizmos.DrawLine( vs[ 3 ], vs[ 0 ] );
		}
	}

	Circle2D p1_ = new Circle2D();
	Circle2D p2_ = new Circle2D();
	Circle2D c1_ = new Circle2D();
	Circle2D c2_ = new Circle2D();
	AABB2D aabb1_ = new AABB2D();
	AABB2D aabb2_ = new AABB2D();
	OBB2D obb1_ = new OBB2D();
	OBB2D obb2_ = new OBB2D();
}
