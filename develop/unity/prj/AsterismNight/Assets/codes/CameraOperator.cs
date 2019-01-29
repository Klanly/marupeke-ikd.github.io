using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラ操作
public class CameraOperator {

    public void setSpeedScales( float x, float y )
    {
        speedScaleX_ = x;
        speedScaleY_ = y;
    }

	public void update () {
        cameraMove();

        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            prePos_ = Input.mousePosition;
        }
	    if ( Input.GetMouseButton( 0 ) == true ) {
            var curPos = Input.mousePosition;
            var dif = curPos - prePos_;
            if ( dif.magnitude > 0.0f ) {
                var rot = Camera.main.transform.rotation.eulerAngles;
                rot.y -= dif.x * speedScaleX_;
                rot.x += dif.y * speedScaleY_;
                Camera.main.transform.rotation = Quaternion.Euler( rot );
            }
            prePos_ = curPos;
        }
	}

    void cameraMove()
    {
        var rot = Camera.main.transform.rotation.eulerAngles;
        rot.y += 0.1f;
        Camera.main.transform.rotation = Quaternion.Euler( rot );
    }

    Vector3 prePos_;
    float speedScaleX_ = 1.0f;
    float speedScaleY_ = 1.0f;
}
