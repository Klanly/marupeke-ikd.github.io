using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	void Start () {
        state_ = new Gaming( this );
	}
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    Speaker isSelectSpeaker() {
        // レイ飛ばす
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit = new RaycastHit();
        if ( Physics.Raycast( ray, out hit ) == true ) {
            return hit.transform.GetComponent<Speaker>();
        }
        return null;
    }

    class Gaming : State< Player > {
        public Gaming(Player parent) : base( parent ) { }
        protected override State innerInit() {
            return null;
        }
        protected override State innerUpdate() {
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                // スピーカ選択？
                var speaker = parent_.isSelectSpeaker();
                if ( speaker != null ) {
                    speaker.playSE();
                }
            }
            return this;
        }
    }

    State state_;
}
