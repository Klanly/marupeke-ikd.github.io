using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブイベントファクトリ
//
//  CubeEventTypeからCubeEventを生成

public class CubeEventFactory {

    // イベント作成
    static public CubeEvent create( int n, CubeEventType eventType )
    {
        // 回転軸と角度角度を取得
        AxisType axis;
        CubeRotationType rotType;
        int[] colIndices;
        EventUtil.convEventToRotInfo( n, eventType, out axis, out rotType, out colIndices );


        return  new CubeEvent_Rotate( axis, rotType, colIndices );
    }
}
