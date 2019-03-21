using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDescs : MonoBehaviour {

	[SerializeField]
	List<GameObject> descs_;

	private void Awake() {
		foreach ( var d in descs_ ) {
			d.SetActive( false );
		}
	}

	public bool setup( int index ) {
		if ( index >= descs_.Count )
			return false;
		foreach( var d in descs_ ) {
			d.SetActive( false );
		}
		descs_[ index ].SetActive( true );
		return true;
	}
}
