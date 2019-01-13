using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 失敗時に現場へズームインするカメラ
public class ZoomInCamera : MonoBehaviour {

    [SerializeField]
    GameObject[] zoomCameraPoses_;

    public System.Action OnFinishMove { set { onFinishMove_ = value; } }

    // 初期化（動作スタート）
    public void setup( int positionIndex )
    {
        if ( bActive_ == true )
            return;
        bActive_ = true;

        idx_ = positionIndex;
        startP_ = Camera.main.transform.position;
        startQ_ = Camera.main.transform.rotation;
        endP_ = zoomCameraPoses_[ idx_ ].transform.position;
        endQ_ = zoomCameraPoses_[ idx_ ].transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		if ( bActive_ == true ) {
            t_ += 0.01f;
            if ( t_ >= 1.0f )
                t_ = 1.0f;
            // 位置と角度を同時にLerp
            Vector3 pos = Vector3.Lerp( startP_, endP_, t_ );
            Quaternion q = Quaternion.Slerp( startQ_, endQ_, t_ );
            Camera.main.transform.position = pos;
            Camera.main.transform.rotation = q;

            if ( t_ >= 1.0f ) {
                bActive_ = false;
                if ( onFinishMove_ != null )
                    onFinishMove_();
            }
        }
    }

    bool bActive_ = false;
    float t_ = 0.0f;
    int idx_ = 0;
    Quaternion startQ_;
    Quaternion endQ_;
    Vector3 startP_;
    Vector3 endP_;
    System.Action onFinishMove_;
}
