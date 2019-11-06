using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	float moveSpeed_ = 0.1f;

	[SerializeField]
	float radius_ = 3.0f;

	[SerializeField]
	WindowRoom windowRoom_ = null;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if ( windowRoom_.isClear() == false ) {
			if (Input.GetMouseButtonDown( 0 ) == true) {
				Ray ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
				RaycastHit hit;
				if (Physics.Raycast( ray, out hit ) == true) {
					if (hit.collider.tag == "window") {
						Window w = hit.collider.gameObject.GetComponentInParent<Window>();
						if (w != null) {
							if (selectWindow_ == null) {
								w.activeCursor( true );
								selectWindow_ = w;
							} else {
								// 双方のウィンドウが保持している他者のウィンドウを交換
								var other0 = selectWindow_.getOtherWindow();
								var other1 = w.getOtherWindow();
								selectWindow_.setOtherWindow( other1 );
								w.setOtherWindow( other0 );

								if ( selectWindow_.isCorrect() == true ) {
									selectWindow_.removeCollider();
								}
								if ( w.isCorrect() == true ) {
									w.removeCollider();
								}

								selectWindow_.activeCursor( false );
								selectWindow_ = null;
							}
						}
					}
				}
				// すべてそろった？
				if (windowRoom_.checkClear() == true) {
					windowRoom_.toClear();
					transform.localPosition = Vector3.zero;	// 中央へ
				}
			}
			// 部屋内をちょっと動けるように
			float dx = 0.0f;
			float dz = 0.0f;
			if (Input.GetKey( KeyCode.W )) { dz += 1.0f; }
			if (Input.GetKey( KeyCode.S )) { dz -= 1.0f; }
			if (Input.GetKey( KeyCode.D )) { dx += 1.0f; }
			if (Input.GetKey( KeyCode.A )) { dx -= 1.0f; }

			var fv = Camera.main.transform.forward;
			fv.y = 0.0f;
			fv.Normalize();
			var rv = Camera.main.transform.right;
			rv.y = 0.0f;
			rv.Normalize();
			var p = transform.localPosition;
			Vector3 np = p + ( fv * dz + rv * dx ).normalized * moveSpeed_;
			if (np.magnitude > radius_) {
				np = np.normalized * radius_;
			}
			transform.localPosition = np;

			// Spaceで中央に戻る
			if (Input.GetKey( KeyCode.Space )) {
				transform.localPosition = Vector3.zero;
			}
		}
	}

	Window selectWindow_ = null;
}
