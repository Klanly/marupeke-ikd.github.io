using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 移動数値
//
//  指定の目標値に向かって動く。途中で目標値が変わったらそれをあらたなゴールとして動く。
//  前の目標値のrate倍だけ値を縮める。

public class MoveValue {
    public MoveValue( float initAim, float rate, float minDef )
    {
        setRate( rate );

        if ( minDef <= 0.0f )
            minDef = 0.01f;
        minDef_ = minDef;

        cur_ = initAim;
        setAim( initAim );
    }

    public void setAim( float aim )
    {
        aim_ = aim;
    }

    public void setRate( float rate )
    {
        if ( rate <= 0.0f )
            rate = 0.01f;
        else if ( rate >= 1.0f )
            rate = 1.0f;
        rate_ = rate;
    }

    // 値を更新
    public float update()
    {
        float def = aim_ - cur_;
        if ( Mathf.Abs( def ) <= minDef_ ) {
            // 目標に届いた
            cur_ = aim_;
        } else {
            cur_ += def * rate_;
        }
        return cur_;
    }

    float aim_ = 0.0f;
    float rate_ = 0.25f;
    float minDef_ = 0.01f;
    float cur_ = 0.0f;
}
