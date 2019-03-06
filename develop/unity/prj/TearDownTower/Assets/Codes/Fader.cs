using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フェーダー

public class Fader : MonoBehaviour {

	[SerializeField]
	Camera camera_;

	[SerializeField]
	SpriteRenderer renderer_;

	[SerializeField]
	float fadeVal_;

	// フェード率を設定
	public void to( float rate, float fadeSec, System.Action finishCallback = null ) {
		rate_.setSec( fadeSec );
		rate_.setAim( rate );
		if ( finishCallback  != null ) {
			GlobalState.time( fadeSec, (sec, t) => {
				return true;
			} ).finish( finishCallback );
		}
	}

	// Use this for initialization
	void Update () {
		fadeVal_ = rate_.getCurVal();
		if ( rate_.getCurVal() < 0.001f ) {
			// 透明とみなしカメラを切る
			camera_.gameObject.SetActive( false );
		} else {
			camera_.gameObject.SetActive( true );
			var color = renderer_.color;
			color.a = fadeVal_;
			renderer_.color = color;
		}
	}

	MoveValueFloat rate_ = new MoveValueFloat( 1.0f, 1.0f );
}
