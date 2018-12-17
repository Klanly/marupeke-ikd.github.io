using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Blower
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // クリックしたらブロー発生
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            var vpPos = Camera.main.ScreenToViewportPoint( Input.mousePosition );
            vpPos.x -= 0.5f;
            vpPos.y -= 0.5f;
            vpPos.x *= ( float )Screen.width / Screen.height;
            vpPos.z = 0.0f;

            // ブローパワー係数算出
            float power = 0.0f;
            float dist = vpPos.magnitude;
            float r1 = 0.1f;
            if ( dist <= r1 )
                power = 1.0f;
            else if ( dist >= 0.5f )
                power = 0.0f;
            else
                power = -( dist - r1 ) / ( 0.5f - r1 ) + 1.0f;
            blowTasks_.Add( new BlowTask( -vpPos, blowPower_ * power, blowDecRate_ ) );
        }

        updateTask();
    }
}
