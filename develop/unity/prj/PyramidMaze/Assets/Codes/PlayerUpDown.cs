using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpDown : MonoBehaviour {

    public System.Action< System.Action > FallCallback { set { fallCallback_ = value; } }
    public System.Action< System.Action > JumpCallback { set { jumpCallback_ = value; } }
    System.Action<System.Action> fallCallback_;
    System.Action<System.Action> jumpCallback_;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bEnable_ == false )
            return;

        // カメラの視点方向でジャンプか飛び降りか決める
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            if ( Camera.main.transform.forward.y < -0.3f ) {
                bEnable_ = false;
                // 飛び降り
                fallCallback_( () => {
                    bEnable_ = true;
                } );
            } else if ( Camera.main.transform.forward.y > 0.3f ) {
                bEnable_ = false;
                // ジャンプ
                jumpCallback_( () => {
                    bEnable_ = true;
                } );
            }
        }
	}
    bool bEnable_ = true;
}
