using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックボックスファクトリ
public class GimicBoxFactory : MonoBehaviour {

    [SerializeField]
    GimicBox[] gimicBoxPrefabs_;

    public enum BoxType : int
    {
        Screw = 0,  // ギミックネジ
        TypeNum = 1
    }

    // 生成
    public List< GimicBox > create(LayoutSpec spec)
    {
        if ( gimicBoxPrefabs_.Length < ( int )BoxType.TypeNum )
            return null;

        List < GimicBoxFactory.BoxType > typeList = spec.gimicBoxTypes_;
        if ( spec.gimicBoxRandomType_ == true ) {
            if ( spec.gimicBoxNum_ == 0 )
                return null;
            var r = new System.Random( spec.seed_ );
            typeList = new List<BoxType>();
            for ( int i = 0; i < spec.gimicBoxNum_; ++i ) {
                typeList.Add( ( BoxType )( r.Next() % ( int )BoxType.TypeNum ) );
            }
        }
        if ( typeList == null )
            return null;

        var list = new List<GimicBox>();
        for ( int i = 0; i < spec.gimicBoxNum_; ++i ) {
            list.Add( Instantiate<GimicBox>( gimicBoxPrefabs_[ ( int )typeList[ i ] ] ) );
        }
        return list;
    }
}
