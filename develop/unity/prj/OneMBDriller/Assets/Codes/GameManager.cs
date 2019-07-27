using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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


    private void Awake() {

        // ブロックを配置
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

        blocks_ = distributer.createField( bp );

        for ( int y = 0; y < bp.sepY_; ++y ) {
            for ( int x = 0; x < bp.sepX_; ++x ) {
                if ( x % 2 == 1 && y % 5 == 0 )
                    blocks_[ x, y ].type_ = Block.Type.Juel1;
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
    Dictionary<Vector2, ChunkBlocks> activeChunkRoots_ = new Dictionary<Vector2, ChunkBlocks>();
    Block[,] blocks_;
    BlockCollideManager collideManager_ = new BlockCollideManager();
}
