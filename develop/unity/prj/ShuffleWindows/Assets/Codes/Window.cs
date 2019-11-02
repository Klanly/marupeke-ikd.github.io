using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
	[SerializeField]
	GameObject framePrefab_;

	[SerializeField]
	MeshRenderer windowRenderer_;

	[SerializeField]
	float windowWidth_ = 5.0f;

	[SerializeField]
	float windowHeight_ = 4.0f;

	// LR
	[SerializeField]
	float upMergin_ = 0.3f;

	[SerializeField]
	float downMergin_ = 0.5f;

	[SerializeField]
	float ticknessLR_ = 0.3f;

	[SerializeField]
	float dimLengthLR_ = 1.2f;

	// UP
	[SerializeField]
	float dimLengthUp_ = 1.0f;

	[SerializeField]
	float ticknessUp_ = 0.3f;

	// Down
	[SerializeField]
	float dimLengthDown_ = 0.7f;

	[SerializeField]
	float ticknessDown_ = 0.3f;

	// Other
	[SerializeField]
	Window otherWindow_;


	private void Awake()
	{
		if ( otherWindow_ == null) {
			otherWindow_ = this;
		}
	}

	void Start()
    {
		// LR
		{
			float ty = ( upMergin_ - downMergin_ ) * 0.5f;
			float tx_L = -( windowWidth_ + ticknessLR_ ) * 0.5f;
			float tx_R = ( windowWidth_ + ticknessLR_ ) * 0.5f;
			float tz = -dimLengthLR_ * 0.5f;
			float sy = ( upMergin_ + windowHeight_ + downMergin_ );
			float sx = ticknessLR_;
			float sz = dimLengthLR_;
			var L = PrefabUtil.createInstance( framePrefab_, transform, new Vector3( tx_L, ty, tz ) );
			L.transform.localScale = new Vector3( sx, sy, sz );
			var R = PrefabUtil.createInstance( framePrefab_, transform, new Vector3( tx_R, ty, tz ) );
			R.transform.localScale = new Vector3( sx, sy, sz );
		}

		// U
		{
			float ty = ( windowHeight_ + ticknessUp_ ) * 0.5f;
			float tx = 0.0f;
			float tz = -dimLengthUp_ * 0.5f;
			float sy = ticknessUp_;
			float sx = windowWidth_;
			float sz = dimLengthUp_;
			var U = PrefabUtil.createInstance( framePrefab_, transform, new Vector3( tx, ty, tz ) );
			U.transform.localScale = new Vector3( sx, sy, sz );
		}

		// D
		{
			float ty = -( windowHeight_ + ticknessDown_ ) * 0.5f;
			float tx = 0.0f;
			float tz = -dimLengthDown_ * 0.5f;
			float sy = ticknessDown_;
			float sx = windowWidth_;
			float sz = dimLengthDown_;
			var D = PrefabUtil.createInstance( framePrefab_, transform, new Vector3( tx, ty, tz ) );
			D.transform.localScale = new Vector3( sx, sy, sz );
		}

		// Window
		{
			windowRenderer_.transform.localScale = new Vector3( windowWidth_, windowHeight_, 1.0f );
		}
	}

	void Update()
    {
		var x = otherWindow_.transform.right * otherWindow_.windowWidth_;
		var y = otherWindow_.transform.up * otherWindow_.windowHeight_;
		var z = otherWindow_.transform.forward;
		Vector4 p = otherWindow_.transform.position;
		p.w = 1.0f;

		var mat = windowRenderer_.material;
		mat.SetVector( "_OtherXAxis", x );
		mat.SetVector( "_OtherYAxis", y );
		mat.SetVector( "_OtherZAxis", z );
		mat.SetVector( "_OtherPos", p );

		// 相手の世界でのカメラ位置を算出
		//  1. 窓の中心からカメラまでの距離Lを算出
		//  2. カメラまでのベクトルをローカル空間へ移動（ただしスケールしない）
		//  3. ローカルベクトルを相手の世界へ（ただしスケールしない）
		//  4. 相手の中心から遷移後ベクトル×Lが相手の空間でのカメラ位置
		var meToCamera = Camera.main.transform.position - transform.position;
		float L = meToCamera.magnitude;
		var myInvQ = transform.rotation;
		myInvQ.x *= -1.0f;  // 反転
		myInvQ.y *= -1.0f;
		myInvQ.z *= -1.0f;
		var otherQ = otherWindow_.transform.rotation;
		var vInMe = myInvQ * meToCamera.normalized;
		var vInOther = otherQ * vInMe * L;
		var camPos = otherWindow_.transform.position + vInOther;
		mat.SetVector( "_CameraPosInOther", camPos );
		
		windowRenderer_.material = mat;
	}
}
