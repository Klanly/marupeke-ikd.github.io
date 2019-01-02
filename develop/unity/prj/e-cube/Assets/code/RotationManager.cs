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
    public void run(AxisType axis, int[] colIndices, CubeRotationType rotType, float defDegPerFrame, System.Action rotateFinishCallback = null )
    {
        // 回転タスクを再初期化
        defDegPerFrame_ = defDegPerFrame;

        // 回転メソッドを設定
        RotationMethod method = null;
        if ( axis == AxisType.AxisType_X )
            method = new RotationMethod_AxisX( colIndices, rotType, parent_ );
        else if ( axis == AxisType.AxisType_Y )
            method = new RotationMethod_AxisY( colIndices, rotType, parent_ );
        else if ( axis == AxisType.AxisType_Z )
            method = new RotationMethod_AxisZ( colIndices, rotType, parent_ );
        var callback = rotateFinishCallback;
        method.setRotateFinishCallback( () => {
            if ( callback != null )
                callback();
        });
        rotMethod_.Add( method );
    }

    // 回転更新
    public bool update()
    {
        if ( rotMethod_.Count > 0 ) {
            while ( true ) {
                // 対象が無くなったら抜ける
                if ( rotMethod_.Count == 0 )
                    break;

                // 最新が存在していなかったら削除
                if ( rotMethod_[ 0 ] == null ) {
                    rotMethod_.RemoveAt( 0 );
                    continue;
                }

                //  もしより新しい回転が積まれていたら(->Count >= 2)
                //  今の回転を直ちに終了させる
                if ( rotMethod_.Count >= 2 ) {
                    skip( rotMethod_[ 0 ] );
                    rotMethod_.RemoveAt( 0 );
                    continue;
                }

                // 最新の回転を更新
                rotMethod_[ 0 ] = rotMethod_[ 0 ].update( defDegPerFrame_ );

                // 更新の結果リストにより最新の回転が積まれていなかったら終わり
                if ( rotMethod_.Count <= 1 )
                    break;
            }
        }
        return isRun();
    }

    // 回転中？
    public bool isRun()
    {
        return ( rotMethod_.Count > 0 );
    }

    // 回転処理スキップ
    protected void skip( RotationMethod skipMethod )
    {
        // 現在の回転を最後まで回す
        skipMethod.update( 1000.0f );
    }

    Cube parent_;
    List< RotationMethod > rotMethod_ = new List<RotationMethod>();
    float defDegPerFrame_ = 1.0f;
}
