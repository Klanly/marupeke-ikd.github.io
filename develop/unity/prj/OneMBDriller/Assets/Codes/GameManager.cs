using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject diamondPref_;

    void Start()
    {
        var distributer = new BlockDistributer();
        var bp = new BlockFieldParameter();
        bp.regionMin_ = new Vector2( 0.0f, 0.0f );
        bp.regionMax_ = new Vector2( 1024.0f, 1024.0f );
        bp.sepX_ = 1024;
        bp.sepY_ = 1024;
        bp.diamondNum_ = 5;
        bp.diamondInterval_ = 200.0f;
        bp.diamondIntervalForPlayer_ = 150.0f;
        bp.diamondHP_ = 100;

        var blocks = distributer.createField( bp );

        for ( int y = 0; y < bp.sepY_; ++y ) {
            for ( int x = 0; x < bp.sepX_; ++x ) {
                if ( blocks[ x, y ] != null ) {
                    var type = blocks[ x, y ].type_;
                    switch ( type ) {
                        case Block.Type.Juel0:
                            createDiamond( bp, x, y, blocks[ x, y ] );
                            break;
                    }
                }
            }
        }
    }

    void createDiamond(BlockFieldParameter bp, int x, int y, Block block ) {
        var obj = PrefabUtil.createInstance( diamondPref_, transform );
        obj.transform.localPosition = new Vector3( x + 0.5f, 0.5f, y + 0.5f );
    }

    void Update()
    {
        
    }
}
