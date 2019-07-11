using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// デバッグ用の矢印
//  指定の方向に矢印を描画
public class DebugArrowLine : GLLine {
    public static DebugArrowLine create( Vector3 start, Vector3 end ) {
        var obj = new GameObject( "DebugArrowLine" );
        var arrow = obj.AddComponent<DebugArrowLine>();
        arrow.start_ = start;
        arrow.end_ = end;

        StaticGLLines.getInstance().addLine( arrow );

        return arrow;
    }

    public Vector3 Start { set { start_ = value; } get { return start_; } }
    public Vector3 End { set { end_ = value; } get { return end_; } }

    // 描画（GLLinesから呼ばれる）
    override public bool draw() {
        GL.Color( color_ );
        GL.Vertex3( start_.x, start_.y, start_.z );
        GL.Vertex3( end_.x, end_.y, end_.z );
        return true;
    }

    Color color_ = Color.white;
    Vector3 start_ = Vector3.zero;
    Vector3 end_ = Vector3.one;
}