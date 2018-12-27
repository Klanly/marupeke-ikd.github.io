using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーボードコントローラ
//
//  キーボードの各キーにキューブの回転操作を対応

public class CubeKeyboardController : CubeController {

    public CubeKeyboardController( int n )
    {
        n_ = n;
    }

    // コントローラのイベントを取得
    override public void getInputEvents(ref List<CubeEvent> events)
    {
        // アサインされている全キーをチェック
        foreach( var assign in assigns_ ) {
            if ( Input.GetKeyDown( assign.Key ) == true ) {
                // 指定のイベントタイプに対応したイベントを発行
                events.Add( CubeEventFactory.create( n_, assign.Value ) );
            }
        }
    }

    // キーアサイン
    public void setKey( KeyCode key, CubeEventType eventType )
    {
        assigns_[ key ] = eventType;
    }

    int n_;
    Dictionary<KeyCode, CubeEventType> assigns_ = new Dictionary<KeyCode, CubeEventType>();
}
