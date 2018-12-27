using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブコントローラ管理人

public class CubeControllerManager {

    // 初期化
    public void initialize( Cube cube )
    {
        cube_ = cube;
    }

    // 監視するコントローラを追加
    public void joinController(CubeController controller)
    {
        controllers_.Add( controller );
    }

    // コントローラ有効？
    public bool isActive()
    {
        return bActive_;
    }

    // 全コントローラのON/OFFを設定
    public void setActive( bool isActive )
    {
        bActive_ = isActive;
    }

    // キューブが揃った時にコントローラを自動的に非アクティブにするか？
    public void setAutoNonActiveWhenComplete( bool isYes )
    {
        bAutoNonActive_ = isYes;
    }

    // 更新
    public void update()
    {
        if ( bActive_ == true ) {
            // コントローラからのイベントを取得
            events_.Clear();
            foreach ( var c in controllers_ ) {
                if ( c.isActive() == true ) {
                    c.getInputEvents( ref events_ );
                }
            }

            // イベントに沿ったキューブ操作を行う
            foreach ( var e in events_ ) {
                e.action( cube_ );

                // イベントの結果キューブが揃ったら続くイベントはキャンセル
                if ( bAutoNonActive_ == true && cube_.isComplete() == true ) {
                    bActive_ = false;   // コントローラはOFFに
                    break;
                }
            }
        }
    }

    Cube cube_; // 操作対象キューブ
    bool bActive_;  // コントローラ有効？
    bool bAutoNonActive_ = true;   // 揃った時に非アクティブにする？
    List<CubeController> controllers_ = new List<CubeController>();
    List<CubeEvent> events_ = new List<CubeEvent>();
}
