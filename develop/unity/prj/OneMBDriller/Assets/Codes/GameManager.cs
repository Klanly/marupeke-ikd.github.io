using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MBSingleton<GameManager>
{
    [SerializeField]
    BlockUnit blockPref_;

    [SerializeField]
    ChunkBlocks chunkBlocksPref_;

    [SerializeField]
    Vector3 pos_;

    [SerializeField]
    float chunkSize_ = 8.0f;

    [SerializeField]
    GameObject chunkRoot_;

    [SerializeField]
    Player player_;

    [SerializeField]
    BlockEventManager blockEventManager_;

    // Playerを取得
    public Player getPlayer( int idx = 0 ) {
        return player_;
    }

    // チャンク内にブロックの状態リセットを要求
    public void updateBlock( Block block) {
		// ブロックの所属するチャンクに状態リセットを要求
		var idx = block.getIdx();
		var pos = new Vector3( idx.x + 0.5f, 0.0f, idx.y + 0.5f );
		var chunkIdx = chunkManager_.calcChunkId( pos );
		if (activeChunkRoots_.ContainsKey( chunkIdx ) == true) {
			activeChunkRoots_[ chunkIdx ].updateBlock( block );
		}
	}

	// チャンク内にブロックの状態リセットを要求
	public void updateBlocks( List<Block> blocks ) {
		foreach ( var b in blocks ) {
			// ブロックの所属するチャンクに状態リセットを要求
			var idx = b.getIdx();
			var pos = new Vector3( idx.x + 0.5f, 0.0f, idx.y + 0.5f );
			var chunkIdx = chunkManager_.calcChunkId( pos );
			if ( activeChunkRoots_.ContainsKey( chunkIdx ) == true ) {
				activeChunkRoots_[ chunkIdx ].updateBlock( b );
			}
		}
	}

    // ブロックイベント発動
    public void emitBlockEvent( Block block ) {
        blockEventManager_.emitEvent( block );
    }

    private void Awake() {

        // ブロックを配置
        var distributer = new BlockDistributer();
        var bp = new BlockFieldParameter();
        bp.regionMin_ = new Vector2( 0.0f, 0.0f );
        bp.regionMax_ = new Vector2( 1024.0f, 1024.0f );
        bp.sepX_ = 1024;
        bp.sepY_ = 1024;
        bp.diamond_.num_ = 250;
        bp.diamond_.interval_ = 50.0f;
        bp.diamond_.intervalForPlayer_ = 35.0f;
        bp.diamond_.HP_ = 5000;
        bp.sapphire_.num_ = 2500;
        bp.sapphire_.interval_ = 20.0f;
        bp.sapphire_.intervalForPlayer_ = 0.0f;
        bp.sapphire_.HP_ = 2500;

        blocks_ = distributer.createField( bp );

        for ( int y = 0; y < bp.sepY_; ++y ) {
            for ( int x = 0; x < bp.sepX_; ++x ) {
                if ( blocks_[x,y].type_ == Block.Type.Empty ) {
                    if ( x % 2 == 0 && y % 3 == 0 )
                        blocks_[ x, y ].type_ = Block.Type.Trap1;
                    if ( x % 2 != 0 && y % 3 != 0 )
                        blocks_[ x, y ].type_ = Block.Type.Trap0;
                }
            }
        }

        // チャンクストック作成
        for ( int i = 0; i < 12; ++i ) {
            var root = PrefabUtil.createInstance( chunkBlocksPref_, chunkRoot_.transform );
            chunkRootStack_.Push( root );
            root.transform.SetParent( chunkRoot_.transform );
            root.name = "nullChunk";
            root.gameObject.SetActive( false );
        }

        chunkManager_.ChangeChunkCallback = (acts, nonActs) => {
            // 削除対象になったチャンク領域を非アクティブに
            foreach ( var p in nonActs ) {
                if ( activeChunkRoots_.ContainsKey( p ) == true ) {
                    var root = activeChunkRoots_[ p ];
                    root.gameObject.SetActive( false );
                    root.name = "nullChunk";
                    activeChunkRoots_.Remove( p );
                    chunkRootStack_.Push( root );
                }
            }
            // 新規アクティブになったチャンク領域に対応したブロック情報を流し込み
            foreach ( var p in acts ) {
                if ( p.x < 0 || p.y < 0 )
                    continue;
                var root = chunkRootStack_.Pop();
                if ( root != null ) {
                    root.resetBlocks( blocks_, p, Vector2.zero, chunkSize_ );
                    activeChunkRoots_[ p ] = root;
                    root.name = p.ToString();
                }
            }
        };

        chunkManager_.setup( chunkSize_, 1, SquareChunkManager.PlaneType.XZ, Vector3.zero, player_.transform.localPosition );
        collideManager_.setup( blocks_, 1.0f );
        player_.setup( collideManager_ );
    }

    void Start()
    {
    }

    void Update()
    {
        // チャンク座標更新
        chunkManager_.updateChunk( player_.transform.localPosition );
    }

    SquareChunkManager chunkManager_ = new SquareChunkManager();
    Stack<ChunkBlocks> chunkRootStack_ = new Stack<ChunkBlocks>();
    Dictionary<Vector2Int, ChunkBlocks> activeChunkRoots_ = new Dictionary<Vector2Int, ChunkBlocks>();
    Block[,] blocks_;
    BlockCollideManager collideManager_ = new BlockCollideManager();
}
