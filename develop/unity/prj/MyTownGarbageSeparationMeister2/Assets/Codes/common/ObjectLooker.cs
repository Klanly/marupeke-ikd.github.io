using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットを一定の距離と角度でルックアットし追従
//
//  位置の追従のみでターゲットの回転やスケールには影響されない
//  通常コンポーネントとしてカメラにアタッチ

public class ObjectLooker : MonoBehaviour {

	[SerializeField]
	GameObject target_;

	public void setTarget( GameObject target ) {
		target_ = target;
	}

	public void setOffset( Vector3 offset ) {
		offset_ = offset;
	}
	
	void Update () {
		if ( target_ == null )
			return;
		transform.position = target_.transform.position + offset_;
		transform.rotation = Quaternion.LookRotation( -offset_ );
	}

	Vector3 offset_ = Vector3.one;
}
