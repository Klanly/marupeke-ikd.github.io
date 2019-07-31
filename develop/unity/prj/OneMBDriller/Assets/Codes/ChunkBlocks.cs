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

    [SerializeField]
    TextMesh position_;

    // 再セット
    public void resetBlocks( Block[,] blocks, Vector2Int indexPos, Vector2 OriginOffset, float chunkUnitSize, bool isActive = true ) {
		// ブロック配列の参照範囲を算出
		indexPos_ = indexPos;
		startBlockIdx_ = indexPos * size_;
        startBlockIdx_ = Clamps.Vec2Int.clamp( startBlockIdx_, Vector2Int.zero, new Vector2Int( blocks.GetLength( 0 ) - 1, blocks.GetLength( 1 ) - 1 ) );
        endBlockIdx_ = startBlockIdx_ + Vector2Int.one * size_;
        endBlockIdx_ = Clamps.Vec2Int.clamp( endBlockIdx_, Vector2Int.zero, new Vector2Int( blocks.GetLength( 0 ), blocks.GetLength( 1 ) ) );

        // ブロック再配置
        clearAll();
        for ( int y = startBlockIdx_.y; y < endBlockIdx_.y; ++y ) {
            for ( int x = startBlockIdx_.x; x < endBlockIdx_.x; ++x ) {
                blockUnits_[ x - startBlockIdx_.x, y - startBlockIdx_.y ].setBlock( blocks[x, y] );
            }
        }

        // チャンク位置を変更
        transform.localPosition = new Vector3( OriginOffset.x + chunkUnitSize * indexPos.x, 0.0f, OriginOffset.y + chunkUnitSize * indexPos.y );
        gameObject.SetActive( true );

        // 減点座標を変更
        position_.text = string.Format("{0},{1}", startBlockIdx_.x, startBlockIdx_.y );
    }

	// 指定ブロックの状態を更新
	public bool updateBlock( Block block ) {
		var idx = block.getIdx();
		var refIdx = idx - startBlockIdx_;
		if (refIdx.x < 0 || refIdx.y < 0 || refIdx.x >= size_ || refIdx.y >= size_)
			return false;   // 範囲外
		blockUnits_[ refIdx.x, refIdx.y ].setBlock( block );
		return true;
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
                Block b = new Block( Block.Type.Juel0, x, y );
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
	Vector2Int indexPos_ = Vector2Int.zero; // チャンクId
	Vector2Int startBlockIdx_ = Vector2Int.zero;    // チャンク左下スタートブロックId
	Vector2Int endBlockIdx_ = Vector2Int.zero;		// チャンク右上チャンクブロックId
}
