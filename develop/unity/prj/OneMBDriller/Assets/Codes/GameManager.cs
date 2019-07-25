using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    BlockUnit blockPref_;

    void Start()
    {
        var distributer = new BlockDistributer();
        var bp = new BlockFieldParameter();
        bp.regionMin_ = new Vector2( 0.0f, 0.0f );
        bp.regionMax_ = new Vector2( 1024.0f, 1024.0f );
        bp.sepX_ = 1024;
        bp.sepY_ = 1024;
        bp.diamond_.num_ = 50;
        bp.diamond_.interval_ = 50.0f;
        bp.diamond_.intervalForPlayer_ = 350.0f;
        bp.diamond_.HP_ = 100;
        bp.sapphire_.num_ = 250;
        bp.sapphire_.interval_ = 20.0f;
        bp.sapphire_.intervalForPlayer_ = 0.0f;
        bp.sapphire_.HP_ = 50;
 
        var blocks = distributer.createField( bp );

        for ( int y = 0; y < bp.sepY_; ++y ) {
            for ( int x = 0; x < bp.sepX_; ++x ) {
                if ( blocks[ x, y ] != null ) {
                    var type = blocks[ x, y ].type_;
                    switch ( type ) {
                        case Block.Type.Juel0:
                            createDiamond( bp, x, y, blocks[ x, y ] );
                            break;
                        case Block.Type.Juel1:
                            createSapphire( bp, x, y, blocks[ x, y ] );
                            break;
                    }
                }
            }
        }
    }

    void createDiamond(BlockFieldParameter bp, int x, int y, Block block ) {
        var obj = PrefabUtil.createInstance( blockPref_, transform );
        obj.transform.localPosition = new Vector3( x, 0.0f, y );
        obj.setBlock( block );
    }

    void createSapphire(BlockFieldParameter bp, int x, int y, Block block) {
        var obj = PrefabUtil.createInstance( blockPref_, transform );
        obj.transform.localPosition = new Vector3( x, 0.0f, y );
        obj.setBlock( block );
    }

    void Update()
    {
        
    }
}
