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
		mat.SetVector( "_CameraPosInOther", Camera.main.transform.position );
		
		windowRenderer_.material = mat;
	}
}
