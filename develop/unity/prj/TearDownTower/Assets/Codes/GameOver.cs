using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	[SerializeField]
	TextMesh text_;

	public System.Action FinishCallback { set { finishCallback_ = value;  } }

	private void Awake() {
		text_.gameObject.SetActive( false );
	}

	// Use this for initialization
	void Start () {
		text_.gameObject.SetActive( true );
		Color c = text_.color;
		text_.color = Color.clear;
		GlobalState.time( 1.5f, (sec, t) => {
			if ( this == null )
				return false;
			text_.color = Color.Lerp( Color.clear, c, t );
			return true;
		} ).wait( 4.0f )
		.nextTime( 1.5f, (sec, t) => {
			if ( this == null )
				return false;
			text_.color = Color.Lerp( c, Color.clear, t );
			return true;
		} ).wait( 1.0f )
		.finish(()=> {
			if ( this != null && finishCallback_ != null )
				finishCallback_();
		} );
	}

	System.Action finishCallback_;
}
