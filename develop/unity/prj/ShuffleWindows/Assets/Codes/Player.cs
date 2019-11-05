using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
			Ray ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
			RaycastHit hit;
			if ( Physics.Raycast( ray, out hit ) == true ) {
				if ( hit.collider.tag == "window" ) {
					Window w = hit.collider.gameObject.GetComponentInParent< Window >();
					if ( w != null ) {
						if (selectWindow_ == null) {
							w.activeCursor( true );
							selectWindow_ = w;
						} else {
							// 双方のウィンドウが保持している他者のウィンドウを交換
							var other0 = selectWindow_.getOtherWindow();
							var other1 = w.getOtherWindow();
							selectWindow_.setOtherWindow( other1 );
							w.setOtherWindow( other0 );
							selectWindow_.activeCursor( false );
							selectWindow_ = null;
						}
					}
				}
			}
		}
    }

	Window selectWindow_ = null;
}
