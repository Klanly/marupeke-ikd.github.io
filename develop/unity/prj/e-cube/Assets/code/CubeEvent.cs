using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブイベント
//
//  キューブを操作するイベント群

public class CubeEvent {
    public virtual void action( Cube cube )
    {

    }
}

// キューブを回転させる
public class CubeEvent_Rotate : CubeEvent
{
    public CubeEvent_Rotate( AxisType axis, CubeRotationType rotType, int[] colIndices )
    {
        axis_ = axis;
        rotType_ = rotType;
        colIndices_ = colIndices;
    }

    public override void action( Cube cube )
    {
        cube.onRotation( axis_, colIndices_, rotType_ );
    }

    AxisType axis_;
    CubeRotationType rotType_;
    int[] colIndices_;
}