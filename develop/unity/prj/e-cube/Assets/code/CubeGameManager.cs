using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブゲーム管理人

public class CubeGameManager : MonoBehaviour {

    [SerializeField]
    Cube cubePrefab_;

    [SerializeField]
    bool bStopControllerActiveWhenComplete_;    // 揃った時にキー入力を無効にするか？

    [SerializeField]
    int n_ = 3;

    [SerializeField]
    CubeCamera camera_;

	// Use this for initialization
	void Start () {
        cube_ = Instantiate<Cube>( cubePrefab_ );
        cube_.transform.parent = transform;
        cube_.transform.localPosition = Vector3.zero;
        cube_.initialize( n_ );

        controllerManager_.initialize( cube_ );

        // TEST
        var mouseCont = new CubeMouseController( cube_, camera_ );
        var keyboardCont = new CubeKeyboardController( cube_.getN() );

        // X軸回転
        keyboardCont.setKey( KeyCode.E, CubeEventType.Rot_R );
        keyboardCont.setKey( KeyCode.C, CubeEventType.Rot_IR );
        keyboardCont.setKey( KeyCode.X, CubeEventType.Rot_Inv_Right | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.W, CubeEventType.Rot_Right | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.Z, CubeEventType.Rot_IL );
        keyboardCont.setKey( KeyCode.Q, CubeEventType.Rot_L );
        // Y軸回転
        keyboardCont.setKey( KeyCode.Q, KeyCode.V, CubeEventType.Rot_U );
        keyboardCont.setKey( KeyCode.E, KeyCode.V, CubeEventType.Rot_IU );
        keyboardCont.setKey( KeyCode.D, KeyCode.V, CubeEventType.Rot_Inv_Up | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.A, KeyCode.V, CubeEventType.Rot_Up | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.C, KeyCode.V, CubeEventType.Rot_ID );
        keyboardCont.setKey( KeyCode.Z, KeyCode.V, CubeEventType.Rot_D );
        // Z軸回転
        keyboardCont.setKey( KeyCode.Q, KeyCode.B, CubeEventType.Rot_B );
        keyboardCont.setKey( KeyCode.E, KeyCode.B, CubeEventType.Rot_IB );
        keyboardCont.setKey( KeyCode.D, KeyCode.B, CubeEventType.Rot_Inv_Back | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.A, KeyCode.B, CubeEventType.Rot_Back | CubeEventType.RotCol_2 | CubeEventType.RotDeg_90 );
        keyboardCont.setKey( KeyCode.C, KeyCode.B, CubeEventType.Rot_IF );
        keyboardCont.setKey( KeyCode.Z, KeyCode.B, CubeEventType.Rot_F );

        controllerManager_.joinController( keyboardCont );
        controllerManager_.joinController( mouseCont );
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
