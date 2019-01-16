using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーボードコントローラ
//
//  キーボードの各キーにキューブの回転操作を対応

public class CubeKeyboardController : CubeController {

    class KeyInfo
    {
        public class HoldInfo
        {
            public HoldInfo(KeyCode holdKey, CubeEventType eventType )
            {
                holdKey_ = holdKey;
                eventType_ = eventType;
            }
            public KeyCode holdKey_;
            public CubeEventType eventType_;
        }
        public List< HoldInfo > holds_ = new List<HoldInfo>();
        public CubeEventType eventType_;
    }

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
                // ホールドキーをチェック
                bool useHold = false;
                foreach ( var hold in assign.Value.holds_) {
                    if ( Input.GetKey( hold.holdKey_ ) == true ) {
                        // ホールド確認
                        events.Add( CubeEventFactory.create( n_, hold.eventType_ ) );
                        useHold = true;
                        break;
                    }
                }
                if ( useHold == false ) {
                    // ホールド無しダイレクト入力
                    events.Add( CubeEventFactory.create( n_, assign.Value.eventType_ ) );
                }
            }
        }
    }

    // キーアサイン
    public void setKey( KeyCode key, CubeEventType eventType )
    {
        if ( assigns_.ContainsKey( key ) == false ) {
            assigns_[ key ] = new KeyInfo();
        }
        assigns_[ key ].eventType_ = eventType;
    }

    // ホールドキーアサイン
    //  ホールドキーを押したままにした時に有効となるキーをアサイン
    //  同じキーは許可しない
    public bool setKey(KeyCode key, KeyCode holdKey, CubeEventType eventType)
    {
        if ( key == holdKey )
            return false;

        if ( assigns_.ContainsKey( key ) == false ) {
            assigns_[ key ] = new KeyInfo();
        }

        bool isDetect = false;
        for ( int i = 0; i < assigns_[ key ].holds_.Count; ++i ) {
            if ( assigns_[ key ].holds_[ i ].holdKey_ == holdKey ) {
                // ホールドキーの差し替え
                assigns_[ key ].holds_[ i ].eventType_ = eventType;
                isDetect = true;
                break;
            }
        }
        if( isDetect == false ) {
            // 新規ホールドキー追加
            assigns_[ key ].holds_.Add( new KeyInfo.HoldInfo( holdKey, eventType ) );
        }
        return true;
    }

    int n_;
    Dictionary<KeyCode, KeyInfo> assigns_ = new Dictionary<KeyCode, KeyInfo>();
}
