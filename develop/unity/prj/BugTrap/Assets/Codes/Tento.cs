using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tento : Bug
{
	[SerializeField]
	float colRadius_ = 1.0f;

	private void Awake()
	{
		col_.Radius = colRadius_;
		shapeGroup_.addShape( col_ );
		state_ = new StartIdle( this );
	}

	void Update()
	{
		col_.Center = Vector3Util.toVector2XZ( transform.position );
		updateEntry();
		curCollIndex_ = ( curCollIndex_ + 1 ) % 2;
		preCollideMap_[ curCollIndex_ ].Clear();
	}

	// 歩いている時に障害物に当たった
	protected override bool onCollideWalkAvoidObject(List<FieldObject> collideObjects, ref State<Bug> state)
	{
		bool isStopWalk = false;
		foreach( var fo in collideObjects ) {
			if ( isPreCollide( fo ) == true ) {
				preCollideMap_[ curCollIndex_ ].Add( fo );
				continue;	// 直前に当たっている
			}
			Wood w = fo as Wood;
			if ( w != null ) {
				preCollideMap_[ curCollIndex_ ].Add( fo );
				// 木の衝突点での法線で反射
				var n = w.getNormal();
				float dot = Vector3.Dot( n, transform.forward );
				// 長辺側と短辺側に真っすぐ当たった場合は正面衝突と判定
				if ( Mathf.Abs( dot ) > 0.99f || Mathf.Abs( dot ) < 0.01 ) {
					// 正面衝突の場合は右に曲がる
					transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f ) * transform.localRotation;
					continue;
				}

				// 斜め反射
				if ( Vector3.Dot( n, fo.transform.position - transform.position ) > 0 ) {
					n *= -1.0f;
				}
				transform.rotation = Quaternion.LookRotation( Vector3Util.reflect( transform.forward, n ) );
			}
		}
		return isStopWalk;
	}

	bool isPreCollide( FieldObject fo ) {
		return preCollideMap_[ ( curCollIndex_ + 1 ) % 2 ].Contains( fo );
	}

	private void OnDrawGizmos()
	{
		GizmosUtil.drawCircle( transform.position, colRadius_, Color.white, transform.rotation, transform.lossyScale );
	}

	Circle2D col_ = new Circle2D();
	HashSet<FieldObject>[] preCollideMap_ = new HashSet<FieldObject>[] {
		new HashSet<FieldObject>(),
		new HashSet<FieldObject>(),
	};
	int curCollIndex_ = 0;
}
