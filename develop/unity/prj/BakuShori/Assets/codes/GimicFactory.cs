using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックファクトリー
public class GimicFactory : MonoBehaviour {

    [SerializeField]
    Gimic[] gimicPrefabs_;

    public enum GimicType : int
    {
        NineNumbers = 0,  // 9つの数ボタン
        TypeNum = 1
    }

    // 生成
    public List<Gimic> create(LayoutSpec spec)
    {
        if ( gimicPrefabs_.Length < ( int )GimicType.TypeNum )
            return null;

        List<GimicType> typeList = spec.gimicTypes_;
        if ( spec.gimicRandomType_ == true ) {
            if ( spec.gimicNum_ == 0 )
                return null;
            var r = new System.Random( spec.seed_ );
            typeList = new List<GimicType>();
            for ( int i = 0; i < spec.gimicNum_; ++i ) {
                typeList.Add( ( GimicType )( r.Next() % ( int )GimicType.TypeNum ) );
            }
        }
        if ( typeList == null )
            return null;

        var list = new List<Gimic>();
        for ( int i = 0; i < spec.gimicNum_; ++i ) {
            list.Add( Instantiate<Gimic>( gimicPrefabs_[ ( int )typeList[ i ] ] ) );
        }
        return list;
    }
}
