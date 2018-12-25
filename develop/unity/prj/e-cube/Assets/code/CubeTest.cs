using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour {

    [SerializeField]
    Cube cube_;

    [SerializeField]
    bool bApplyParameters_ = false;

    [SerializeField]
    CubeRotationType rotType_ = CubeRotationType.CRT_Plus_90;

    [SerializeField]
    AxisType axis_ = AxisType.AxisType_X;

    [SerializeField]
    float defDegPerFrame_ = 10.0f;

    [SerializeField]
    int[] colIndices_;

    void Start () {
		
	}
	
	void Update () {
        if ( bApplyParameters_ == true ) {
            bApplyParameters_ = false;

            cube_.RotDegPerFrame = defDegPerFrame_;
            cube_.onRotation( axis_, colIndices_, rotType_ );
        }
    }

}
