using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAutoOperator {
    public CubeAutoOperator(　Cube cube, CubePracticeData practiceData_, bool isInverse = false )
    {
        cube_ = cube;
        bInverse_ = isInverse;
        solve_ = practiceData_.getRotateList();
        if ( bInverse_ == true ) {
            var invList = new List<CubePracticeData.RotateUnit>();
            for ( int i = solve_.Count - 1; i >= 0; --i ) {
                invList.Add( solve_[ i ].getInvRotUnit() );
            }
            solve_ = invList;
        }
    }

    // 更新
    public bool update()
    {
        if ( bInitialize_ == false ) {
            bInitialize_ = true;
            AxisType axis;
            CubeRotationType rotType;
            int[] colIndices = null;
            solve_[ idx_ ].getAxisRotColindicesSet( cube_.getN(), out axis, out rotType, out colIndices );
            cube_.onRotation( axis, colIndices, rotType, rotate );
        }
        return !bFinish_;
    }

    void rotate()
    {
        idx_++;
        if ( idx_ >= solve_.Count ) {
            bFinish_ = true;
        }
        if ( bFinish_ == false ) {
            AxisType axis;
            CubeRotationType rotType;
            int[] colIndices = null;
            solve_[ idx_ ].getAxisRotColindicesSet( cube_.getN(), out axis, out rotType, out colIndices );
            cube_.onRotation( axis, colIndices, rotType, rotate );
        }
    }

    bool bInitialize_ = false;
    bool bInverse_ = false;
    bool bFinish_ = false;
    List<CubePracticeData.RotateUnit> solve_ = new List<CubePracticeData.RotateUnit>();
    Cube cube_;
    int idx_ = 0;
}
