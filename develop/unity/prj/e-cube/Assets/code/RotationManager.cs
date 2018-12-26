using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転動作管理人
class RotationManager
{
    public RotationManager(Cube parent)
    {
        parent_ = parent;
    }

    // 回転設定と行動開始
    public void run(AxisType axis, int[] colIndices, CubeRotationType rotType, float defDegPerFrame)
    {
        defDegPerFrame_ = defDegPerFrame;

        // 回転メソッドを設定
        if ( axis == AxisType.AxisType_X )
            rotMethod_ = new RotationMethod_AxisX( colIndices, rotType, parent_ );
        else if ( axis == AxisType.AxisType_Y )
            rotMethod_ = new RotationMethod_AxisY( colIndices, rotType, parent_ );
        else if ( axis == AxisType.AxisType_Z )
            rotMethod_ = new RotationMethod_AxisZ( colIndices, rotType, parent_ );
    }

    // 回転更新
    public bool update()
    {
        if ( isRun() == true ) {
            rotMethod_ = rotMethod_.update( defDegPerFrame_ );
        }
        return isRun();
    }

    // 回転中？
    public bool isRun()
    {
        return ( rotMethod_ != null );
    }

    // 回転処理スキップ
    public void skip()
    {
        // 現在の回転を最後まで回す
        rotMethod_ = rotMethod_.update( 1000.0f );
    }

    Cube parent_;
    RotationMethod rotMethod_;
    float defDegPerFrame_ = 1.0f;
}
