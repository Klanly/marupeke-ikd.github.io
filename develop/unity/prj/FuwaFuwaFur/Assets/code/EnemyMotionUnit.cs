using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の動きユニット
// 周期的に動く物である前提

public class EnemyMotionUnit {

    // 今の時刻での位置を取得
    public Vector3 getCurPos()
    {
        return getMyCurPos() + getChildrenCurPos();
    }

    // 自分の今の位置を取得
    virtual protected Vector3 getMyCurPos()
    {
        return Vector3.zero;
    }

    // 自分の位置を更新
    virtual protected Vector3 updateMyPos( float dt )
    {
        // dt時間に対する位置をここで算出
        return Vector3.zero;
    }

    // 位置を更新
    public Vector3 update() {
        float dt = Time.deltaTime;
        Vector3 p = updateMyPos( dt ) + updateChildrenTotalPos();
        return p;
	}

    // 合成子モーションを追加
    public void setChildMotion( EnemyMotionUnit motion )
    {
        childrenMotion_.Add( motion );
    }

    // 子のトータル位置を更新
    protected Vector3 updateChildrenTotalPos()
    {
        Vector3 p = Vector3.zero;
        foreach ( var c in childrenMotion_ ) {
            p += c.update();
        }
        return p;
    }

    // 子の現在位置を取得
    protected Vector3 getChildrenCurPos()
    {
        Vector3 p = Vector3.zero;
        foreach ( var c in childrenMotion_ ) {
            p += c.getMyCurPos();
        }
        return p;
    }

    List<EnemyMotionUnit> childrenMotion_ = new List<EnemyMotionUnit>();
}
