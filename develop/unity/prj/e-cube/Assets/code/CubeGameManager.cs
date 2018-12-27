using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブゲーム管理人

public class CubeGameManager : MonoBehaviour {

    [SerializeField]
    Cube cubePrefab_;

    [SerializeField]
    bool bStopControllerActiveWhenComplete_;    // 揃った時にキー入力を無効にするか？

	// Use this for initialization
	void Start () {
        cube_ = Instantiate<Cube>( cubePrefab_ );
        cube_.transform.parent = transform;
        cube_.transform.localPosition = Vector3.zero;
        controllerManager_.initialize( cube_ );

        // TEST
        var keyboardCont = new CubeKeyboardController( cube_.getN() );
        keyboardCont.setKey( KeyCode.D, CubeEventType.Rot_R );
        keyboardCont.setKey( KeyCode.C, CubeEventType.Rot_IR );
        keyboardCont.setKey( KeyCode.X, CubeEventType.Rot_Inv_Right | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.S, CubeEventType.Rot_Right | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.Z, CubeEventType.Rot_IL );
        keyboardCont.setKey( KeyCode.A, CubeEventType.Rot_L );
        controllerManager_.joinController( keyboardCont );
        controllerManager_.setActive( true );
        controllerManager_.setAutoNonActiveWhenComplete( bStopControllerActiveWhenComplete_ );

        cube_.onRotation( AxisType.AxisType_X, new int[ 1 ] { 0 }, CubeRotationType.CRT_Plus_90 );
	}
	
	void Update () {
        controllerManager_.update();
    }

    CubeControllerManager controllerManager_ = new CubeControllerManager();
    Cube cube_;
}
