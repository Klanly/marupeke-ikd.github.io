using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾箱ファクトリー
public class BombBoxFactory : MonoBehaviour {

    [SerializeField]
    BombBox[] bombBoxPrefabs_;

    public enum BonbBoxType : int
    {
        Metal = 0
    }

    // 生成
    public BombBox create( LayoutSpec spec )
    {
        int id = ( int )spec.bombBoxType_;
        if ( id >= bombBoxPrefabs_.Length )
            return null;
        var obj = Instantiate<BombBox>( bombBoxPrefabs_[ id ] );
        return obj;
    }
}
