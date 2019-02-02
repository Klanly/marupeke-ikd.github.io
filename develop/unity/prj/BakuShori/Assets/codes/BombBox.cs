using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾箱
//
//  表面にタイマー、ギミックボックス、各Answer、ギミックネジを持つ
//  また内部に赤青線を保持している。

public class BombBox : Entity {

    void Awake()
    {
        ObjectType = EObjectType.BombBox;
    }

    // Entityを登録
    override public bool setEntity( int index, Entity entity )
    {
        // BombBoxはAnswer以外は登録できない
        if (
            entity.ObjectType != EObjectType.GimicAnswer &&
            entity.ObjectType != EObjectType.GimicBoxAnswer &&
            entity.ObjectType != EObjectType.ScrewAnswer
        ) {
            return false;
        }
        return base.setEntity( index, entity );
    }

    // ギミックネジを取得
    public List<GimicScrew> getGimicScrewes()
    {
        return gimicScrews_;
    }

    List<GimicScrew> gimicScrews_ = new List<GimicScrew>();
}
