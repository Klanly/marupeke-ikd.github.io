using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2次元プリミティブ基底
//  コリジョンメソッドを定義

public interface Shape2D
{
	bool collide(Vector2 point);
	bool collide(AABB2D r);
	bool collide(Circle2D r);
	bool collide(OBB2D r);
}
