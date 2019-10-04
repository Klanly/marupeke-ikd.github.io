using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突判定ヘルパー

public class CollideUtil {

    // レイと平面の衝突
    //  colPos: 衝突点
    //  start : レイの開始点
    //  dir   : レイの方向
    //  p0    : 平面上の一点
    //  n     : 平面の法線
    // 戻り値 : 衝突していたらtrue
    static public bool colPosRayPlane( out Vector3 colPos, Vector3 start, Vector3 dir, Vector3 p0, Vector3 n )
    {
        float b = Vector3.Dot( dir, n );
        if ( Mathf.Abs( b ) < 0.00001f ) {
            // レイと平面が平行と判断
            colPos = Vector3.zero;
            return false;
        }
        float a = Vector3.Dot( p0 - start, n ) / Vector3.Dot( dir, n );
        colPos = start + a * dir;
        return a >= 0.0f;
    }

	// 点とAABB2Dの最短距離
	static public float distPoint_AABB2D( Vector2 p, AABB2D aabb, out Vector2 colPos ) {
		if ( p.x < aabb.Min.x ) {
			if ( p.y < aabb.Min.y ) {
				// 左下領域
				colPos = aabb.Min;
				return ( colPos - p ).magnitude;
			}
			if ( p.y > aabb.Max.y ) {
				// 左上領域
				colPos.x = aabb.Min.x;
				colPos.y = aabb.Max.y;
				return ( colPos - p ).magnitude;
			}
			// 左真ん中領域
			colPos.x = aabb.Min.x;
			colPos.y = p.y;
			return ( colPos.x - p.x );
		}
		if( p.x > aabb.Max.x ) {
			if (p.y < aabb.Min.y) {
				// 右下領域
				colPos.x = aabb.Max.x;
				colPos.y = aabb.Min.y;
				return ( colPos - p ).magnitude;
			}
			if (p.y > aabb.Max.y) {
				// 右上領域
				colPos = aabb.Max;
				return ( colPos - p ).magnitude;
			}
			// 右真ん中領域
			colPos.x = aabb.Max.x;
			colPos.y = p.y;
			return ( p.x - colPos.x );
		}
		if (p.y < aabb.Min.y) {
			// 中下領域
			colPos.x = p.x;
			colPos.y = aabb.Min.y;
			return colPos.y - p.y;
		}
		if (p.y > aabb.Max.y) {
			// 中上領域
			colPos.x = p.x;
			colPos.y = aabb.Max.y;
			return p.y - colPos.y;
		}
		// 右真ん中領域（衝突）
		float toL = p.x - aabb.Min.x;
		float toR = aabb.Max.x - p.x;
		float toD = p.y - aabb.Min.y;
		float toU = aabb.Max.y - p.y;
		if ( toL < toR && toL < toD && toL < toU ) {
			// 左
			colPos.x = aabb.Min.x;
			colPos.y = p.y;
			return -toL;
		}
		if ( toR < toD && toR < toU ) {
			// 右
			colPos.x = aabb.Max.x;
			colPos.y = p.y;
			return -toR;
		}
		if ( toD < toU ) {
			// 下
			colPos.x = p.x;
			colPos.y = aabb.Min.y;
			return -toD;
		}
		// 上
		colPos.x = p.x;
		colPos.y = aabb.Max.y;
		return -toU;
	}

	// OBB2Dと点の最接近点
	//  戻り値：最接近点から点までの距離（符号付き）
	static public float closestPointOBB2D_Point( OBB2D obb, Vector2 p, out Vector2 cp )
	{
		Vector2 d = p - obb.Center;
		int insertCount = 0;
		Vector2 q = obb.Center;
		float distX = Vector2.Dot( d, obb.XAxis );
		if (distX > obb.HalfLen.x) {
			distX = obb.HalfLen.x;
		}
		else if (distX < -obb.HalfLen.x) {
			distX = -obb.HalfLen.x;
		} else {
			insertCount++;
		}
		q += obb.XAxis * distX;

		float distY = Vector2.Dot( d, obb.YAxis );
		if (distY > obb.HalfLen.y) {
			distY = obb.HalfLen.y;
		} else if (distY < -obb.HalfLen.y) {
			distY = -obb.HalfLen.y;
		} else {
			insertCount++;
		}
		q += obb.YAxis * distY;
		cp = q;

		if ( insertCount == 2 ) {
			// めり込んでる
			var p1 = obb.Center + obb.XAxis * distX + obb.YAxis * obb.HalfLen.y * ( distY >= 0.0f ? 1.0f : -1.0f );
			var p2 = obb.Center + obb.YAxis * distY + obb.XAxis * obb.HalfLen.x * ( distY >= 0.0f ? 1.0f : -1.0f );
			float len1 = ( p1 - p ).magnitude;
			float len2 = ( p2 - p ).magnitude;
			if ( len1 < len2 ) {
				cp = p1;
				return -len1;
			}
			cp = p2;
			return -len2;
		}

		return ( cp - p ).magnitude;
	}
}
