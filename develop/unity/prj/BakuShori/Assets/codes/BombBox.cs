using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾箱
//
//  表面にタイマー、ギミックボックス、各Answer、ギミックネジを持つ
//  また内部に赤青線を保持している。

public class BombBox : Entity {

    [SerializeField]
    BombBoxModel bombBoxModel_;

    void Awake()
    {
        ObjectType = EObjectType.BombBox;
    }

    // Entityを登録
    override public bool setEntity( int index, Entity entity )
    {
        // BombBoxはAnswer以外は登録できない
        if ( entity.isAnswer() == false )
        {
            return false;
        }
        return base.setEntity( index, entity );
    }

    // ギミックネジを取得
    public List<GimicScrew> getGimicScrewes()
    {
        return gimicScrews_;
    }

    // 箱を形成
    public void buildBox()
    {
        // 自分の直下にあるアンサー群は自分の子に
        foreach( var e in childrenEntities_ ) {
            if ( e != null && e.isAnswer() == true ) {
                e.transform.parent = transform;
            }
        }
    }

    private void Start()
    {
        int num = bombBoxModel_.getGimicBoxPlaceNum();
        for ( int i = 0; i < num; ++i )
           bombBoxModel_.openCover( i );
    }

    List<GimicScrew> gimicScrews_ = new List<GimicScrew>();
}
