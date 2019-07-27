using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// チャンク内ブロック敷き詰め
public class ChunkBlocks : MonoBehaviour
{
    [SerializeField]
    BlockUnit blockPrefb_;

    [SerializeField]
    int size_ = 8;

    [SerializeField]
    float blockSize_ = 1.0f;

    // 再セット
    public void resetBlocks( Block[,] blocks, Vector2Int indexPos, Vector2 OriginOffset, float chunkUnitSize, bool isActive = true ) {
        // ブロック配列の参照範囲を算出
        Vector2Int startBlockIdx = indexPos * size_;
        startBlockIdx = Clamps.Vec2Int.clamp( startBlockIdx, Vector2Int.zero, new Vector2Int( blocks.GetLength( 0 ) - 1, blocks.GetLength( 1 ) - 1 ) );
        Vector2Int endBlockIdx = startBlockIdx + Vector2Int.one * size_;
        endBlockIdx = Clamps.Vec2Int.clamp( endBlockIdx, Vector2Int.zero, new Vector2Int( blocks.GetLength( 0 ), blocks.GetLength( 1 ) ) );

        // ブロック再配置
        clearAll();
        for ( int y = startBlockIdx.y; y < endBlockIdx.y; ++y ) {
            for ( int x = startBlockIdx.x; x < endBlockIdx.x; ++x ) {
                blockUnits_[ x - startBlockIdx.x, y - startBlockIdx.y ].setBlock( blocks[x, y] );
            }
        }

        // チャンク位置を変更
        transform.localPosition = new Vector3( OriginOffset.x + chunkUnitSize * indexPos.x, 0.0f, OriginOffset.y + chunkUnitSize * indexPos.y );
        gameObject.SetActive( true );
    }

    // ブロックをすべてクリア（Emptyに）
    void clearAll() {
        for ( int y = 0; y < size_; ++y ) {
            for ( int x = 0; x < size_; ++x ) {
                blockUnits_[ x, y ].allBlockOff();
            }
        }
    }

    private void Awake() {
        blockUnits_ = new BlockUnit[ size_, size_ ];
        for ( int y = 0; y < size_; ++y ) {
            for ( int x = 0; x < size_; ++x ) {
                var obj = PrefabUtil.createInstance( blockPrefb_, transform );
                obj.transform.localPosition = new Vector3( x * blockSize_, 0.0f, y * blockSize_ );
                Block b = new Block();
                b.type_ = Block.Type.Juel0;
                obj.setBlock( b );

                blockUnits_[ x, y ] = obj;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    BlockUnit[,] blockUnits_;
}
