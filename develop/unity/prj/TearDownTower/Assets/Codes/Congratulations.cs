using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Congratulations : MonoBehaviour {

	[SerializeField]
	SpriteRenderer renderer_;

	// Use this for initialization
	void Start () {
		var scale = transform.localScale;
		var initScale = new Vector3( 20.0f, 20.0f, 20.0f );
		Color initColor = renderer_.color;
		initColor.a = 0.0f;
		Color color = renderer_.color;
		GlobalState.time( 1.6f, (sec, t) => {
			if ( this == null )
				return false;
			transform.localScale = Lerps.Vec3.easeOutStrong( initScale, scale, t );
			renderer_.color = Color.Lerp( initColor, color, t );
			return true;
		} );
	}
}
