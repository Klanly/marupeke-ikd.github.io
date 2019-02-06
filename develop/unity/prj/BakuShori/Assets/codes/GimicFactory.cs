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
    public List<Gimic> create(LayoutSpec spec, GimicSpec gimicSpec )
    {
        if ( gimicPrefabs_.Length < ( int )GimicType.TypeNum )
            return null;

        var r = new System.Random( spec.gimicSeed_ );
        List<GimicType> typeList = spec.gimicTypes_;
        if ( spec.gimicRandomType_ == true ) {
            if ( spec.gimicNum_ == 0 )
                return null;
            typeList = new List<GimicType>();
            for ( int i = 0; i < spec.gimicNum_; ++i ) {
                typeList.Add( ( GimicType )( r.Next() % ( int )GimicType.TypeNum ) );
            }
        }
        if ( typeList == null )
            return null;

        var list = new List<Gimic>();
        for ( int i = 0; i < spec.gimicNum_; ++i ) {
            var obj = Instantiate<Gimic>( gimicPrefabs_[ ( int )typeList[ i ] ] );
            obj.setParam( r.Next(), gimicSpec );
            if ( obj.ObjectType == Entity.EObjectType.Empty )
                obj.ObjectType = Entity.EObjectType.Gimic;
            list.Add( obj );
        }
        return list;
    }
}
