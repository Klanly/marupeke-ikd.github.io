using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorButton : MonoBehaviour {

    [SerializeField]
    SpriteRenderer renderer_;

    [SerializeField]
    List< Sprite > sprites_;

    [SerializeField]
    Collider collider_;

    [SerializeField]
    int number_;

    public System.Action< string > OnClick { set { onClick_ = value; } }
    System.Action<string> onClick_;

    private void Awake() {
        renderer_.sprite = sprites_[ number_ ];        
    }

    void Update () {
		if ( Input.GetMouseButtonDown( 0 ) == true ) {
            Ray ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
            RaycastHit hit;
            if ( collider_.Raycast( ray, out hit, 100.0f ) == true ) {
                collider_.enabled = false;
                string[] strs = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
                if ( onClick_ != null )
                    onClick_( strs[ number_ ] );
                Color white = Color.white;
                Color gray = Color.gray;
                GlobalState.time( 0.125f, (sec, t) => {
                    renderer_.color = Color.Lerp( white, gray, t );
                    return true;
                }).nextTime( 0.125f, (sec, t) => {
                    renderer_.color = Color.Lerp( gray, white, t );
                    return true;
                } ).finish( () => {
                    collider_.enabled = true;
                } );
            }
        }
	}
}
