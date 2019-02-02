using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックボックスファクトリ
public class GimicBoxFactory : MonoBehaviour {

    [SerializeField]
    GimicBox[] gimicBoxPrefabs_;

    [SerializeField]
    TrapFactory trapFactory_;

    public enum BoxType : int
    {
        Screw = 0,  // ギミックネジ
        TypeNum = 1
    }

    // 生成
    public List<GimicBox> create(LayoutSpec spec)
    {
        if ( gimicBoxPrefabs_.Length < ( int )BoxType.TypeNum ) {
            Debug.LogAssertion( "GimicFactory: error: lack of GimicBox Prefab." );
            return null;
        }

        List<GimicBoxFactory.BoxType> typeList = spec.gimicBoxTypes_;
        if ( spec.gimicBoxRandomType_ == true ) {
            if ( spec.gimicBoxNum_ == 0 ) {
                Debug.LogAssertion( "GimicFactory: error: lack of GimicBox num in spec." );
                return null;
            }
            var r = new System.Random( spec.seed_ );
            typeList = new List<BoxType>();
            for ( int i = 0; i < spec.gimicBoxNum_; ++i ) {
                typeList.Add( ( BoxType )( r.Next() % ( int )BoxType.TypeNum ) );
            }
        }
        if ( typeList == null ) {
            Debug.LogAssertion( "GimicFactory: error: failed to create typelist." );
            return null;
        }

        var trapRand = new System.Random( spec.trapSeed_ );
        var list = new List<GimicBox>();
        for ( int i = 0; i < spec.gimicBoxNum_; ++i ) {
            var obj = Instantiate<GimicBox>( gimicBoxPrefabs_[ ( int )typeList[ i ] ] );
            // トラップをアタッチ
            var trap = trapFactory_.create( spec );
            if ( trap == null ) {
                Debug.LogAssertion( "GimicFactory: error: no trap was created." );
                return null;
            }
            trap.setup( trapRand.Next() );
            obj.setTrap( trap );
            list.Add( obj );
        }
        return list;
    }
}
