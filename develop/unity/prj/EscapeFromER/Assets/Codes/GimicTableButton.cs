using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicTableButton : MonoBehaviour {

    [SerializeField]
    Collider collider_;

    public System.Action< string > OnClick { set { onClick_ = value; } }
    System.Action< string > onClick_;

    // Use this for initialization
    void Start () {
        offPos_ = transform.localPosition;
        cursorCenter_ = new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f );
    }
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown( 0 ) == true ) {
            Ray ray = Camera.main.ScreenPointToRay( cursorCenter_ );
            RaycastHit hit;
            if ( collider_.Raycast( ray, out hit, 100.0f ) == true ) {
                if ( onClick_ != null )
                    onClick_( name );
                collider_.enabled = false;
                GlobalState.wait( 1.0f, () => {
                    if ( this == null )
                        return false;
                    collider_.enabled = true;
                    return false;
                } );
                Vector3 offset = new Vector3( 0.0f, -0.1f, 0.0f );
                GlobalState.time( 0.25f, (sec, t) => {
                    transform.localPosition = offPos_ + Lerps.Vec3.easeOut( Vector3.zero, offset, t );
                    return true;
                } ).nextTime( 0.25f, (sec, t) => {
                    transform.localPosition = offPos_ + Lerps.Vec3.easeOut( offset, Vector3.zero, t );
                    return true;
                } );
            }
        }
	}

    Vector3 offPos_ = Vector3.zero;
    Vector3 cursorCenter_ = Vector3.zero;
}
