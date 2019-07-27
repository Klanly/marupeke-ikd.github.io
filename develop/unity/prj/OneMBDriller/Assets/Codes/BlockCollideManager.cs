using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールドブロックとの衝突判定管理人
public class BlockCollideManager
{
    // セットアップ
    public void setup( Block[,] blocks, float unitSize) {
        blocks_ = blocks;
        unit_ = unitSize;
        sep_.x = blocks.GetLength( 0 );
        sep_.y = blocks.GetLength( 1 );
        fieldMax_ = sep_ - Vector2Int.one;
    }

    // 円との衝突
    public Block toCircleCollide( Vector2 center, float radius, ref Vector2 penetration ) {
        var cIdx = center / unit_;
        var spX = center.x - radius;
        var spY = center.y - radius;
        var epX = center.x + radius;
        var epY = center.y + radius;
        Vector2Int minIdx = new Vector2Int( Mathf.FloorToInt( spX / unit_ ), Mathf.FloorToInt(  spY / unit_ ) );
        Vector2Int maxIdx = new Vector2Int( Mathf.FloorToInt( epX / unit_ ), Mathf.FloorToInt( epY / unit_ ) );
        minIdx = Clamps.Vec2Int.clamp( minIdx, Vector2Int.zero, fieldMax_ );
        maxIdx = Clamps.Vec2Int.clamp( maxIdx, Vector2Int.zero, fieldMax_ );

        for ( int y = minIdx.y; y <= maxIdx.y; ++y ) {
            for ( int x = minIdx.x; x <= maxIdx.x; ++x ) {
                if ( blocks_[ x, y ].type_ == Block.Type.Empty ) {
                    // 対象がいなかった
                    continue;
                }
                // 対象までの距離から衝突判定
                var aabb = blockIdxToRegion( x, y );
                Vector2 n = Vector2.zero;
                var colPos = aabb.distance( center, ref n );
                float d = ( colPos - center ).magnitude;
                if ( d <= radius ) {
                    // 衝突
                    penetration = -n * ( radius - d );
                    return blocks_[x, y];
                }
            }
        }

        return null;
    }

    AABB2D blockIdxToRegion(int x, int y) {
        return new AABB2D( x * unit_, y * unit_, ( x + 1 ) * unit_, ( y + 1 ) * unit_ );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Block[,] blocks_;
    Vector2Int sep_ = new Vector2Int( 1, 1 );
    Vector2Int fieldMax_ = new Vector2Int( 1, 1 );
    float unit_;
}
