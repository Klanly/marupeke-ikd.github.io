using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックボックスアンサー
//
//  ギミックボックスを開ける答え
//  子として自分のターゲットであるギミックボックスのみ登録できる

public class GimicBoxAnswer : Answer {

    void Awake()
    {
        ObjectType = EObjectType.GimicBoxAnswer;
        childrenEntities_ = new List<Entity>();
        childrenEntities_.Add( null );
    }

    // Entityを登録
    override public bool setEntity(int index, Entity entity )
    {
        // 自分のターゲットであるGimicBox以外は登録できない
        if ( index != 0 || entity.ObjectType != EObjectType.GimicBox || entity.Index != Index )
            return false;
        return base.setEntity( index, entity );
    }
}
