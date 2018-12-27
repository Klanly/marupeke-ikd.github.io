using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブコントローラ
//
//  キューブを回転したり見る位置を変更する仮想コントローラ
//  このクラスを派生してキーボードやマウスなどで操作する
//  コントローラを作成する

public class CubeController {

    // コントローラ有効？
    public bool isActive()
    {
        return bActive_;
    }

    // コントローラのON/OFFを設定
    public void setActive( bool isActive )
    {
        bActive_ = isActive;
    }

    // コントローラのイベントを取得
    virtual public void getInputEvents( ref List<CubeEvent> events )
    {
    }

    bool bActive_ = true;
}
