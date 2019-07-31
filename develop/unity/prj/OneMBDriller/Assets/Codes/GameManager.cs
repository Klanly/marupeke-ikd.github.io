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

	[SerializeField]
	UnityEngine.UI.Text blockNumText_;

    [SerializeField]
    UnityEngine.UI.Image mapImage_;

    [SerializeField]
    UnityEngine.UI.Image arrowImage_;

    Texture2D mapTex_;

	// 破壊されたブロック数を設定
	public void setBrokenBlock( Block block ) {
		breakBlockNum_++;

        // 破壊ブロック座標を保持（塗りつぶし用）
        brokenBlockCoords_.Add( block.getIdx() );
	}

    // 破壊ブロック座標を塗りつぶし
    void paintBrokenTexture() {
        foreach ( var pos in brokenBlockCoords_ ) {
            mapTex_.SetPixel( pos.x, pos.y, Color.black );
        }
        mapTex_.Apply();
        brokenBlockCoords_.Clear();
    }

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
        bp.enemyBullet1_.num_ = 100000;
        bp.enemyBullet1_.interval_ = 0.5f;
        bp.enemyBullet1_.intervalForPlayer_ = 0.0f;
        bp.enemyBullet1_.HP_ = 10;
        bp.enemyBullet2_.num_ = 10000;
        bp.enemyBullet2_.interval_ = 0.5f;
        bp.enemyBullet2_.intervalForPlayer_ = 0.0f;
        bp.enemyBullet2_.HP_ = 10;

        blocks_ = distributer.createField( bp );

        for ( int y = 0; y < bp.sepY_; ++y ) {
            for ( int x = 0; x < bp.sepX_; ++x ) {
                if ( blocks_[x,y].type_ == Block.Type.Empty ) {
//                    if ( x % 2 == 0 && y % 3 == 0 )
//                        blocks_[ x, y ].type_ = Block.Type.Trap1;
                }
				if ( blocks_[ x, y ].type_ != Block.Type.Empty ) {
					totalBlockNum_++;
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

        // マップ用スプライト作成
        if ( mapTex_ == null ) {
            mapTex_ = new Texture2D( 1024, 1024, TextureFormat.RGBA32, false );
            Color32[] colors = new Color32[ 1024 * 1024 ];
            Color32 color = new Color32( 255, 255, 255, 255 );
            for ( int i = 0; i < 1024 * 1024; ++i ) {
                colors[ i ] = color;
            }
            mapTex_.SetPixels32( colors );
            mapTex_.Apply();
            var sprite = Sprite.Create( mapTex_, new Rect( 0.0f, 0.0f, 1024.0f, 1024.0f ), Vector2.zero );
            mapImage_.sprite = sprite;
        }

        // インディケーター初期設定
        setArrowPose( 0.0f, 0.0f, new Vector3( 0.0f, 0.0f, 1.0f ) );
    }

    void setArrowPose( float x, float y, Vector3 forward ) {
        var basePos = new Vector3( -256.0f, 0.0f, 0.0f );
        var pos = new Vector3( x / 4.0f, y / 4.0f, 0.0f );  // Worldが1024,1024に対しテクセル座標が256,256なので1/4している
        arrowImage_.rectTransform.localPosition = basePos + pos;

        forward = forward.normalized;
        float th = Mathf.Atan2( forward.z, forward.x );
        var rot = new Vector3( 0.0f, 0.0f, th / ( Mathf.PI * 2.0f ) * 360.0f );
        var q = Quaternion.Euler( rot );
        arrowImage_.rectTransform.rotation = q;
    }

    void updateBlockNumText() {
		float r = ( float )breakBlockNum_ / totalBlockNum_;
		blockNumText_.text = string.Format( "{0:#,0}/{1:#,0}({2:#.###%})", breakBlockNum_, totalBlockNum_, r );
	}

    void Start()
    {
    }

    void Update()
    {
        // チャンク座標更新
        chunkManager_.updateChunk( player_.transform.localPosition );

		// ブロック数表記更新
		updateBlockNumText();

        // 破壊ブロック位置ペイント
        paintBrokenTexture();

        // インディケータ更新
        var p = player_.transform.position;
        setArrowPose( p.x, p.z, player_.transform.forward );
    }

	SquareChunkManager chunkManager_ = new SquareChunkManager();
    Stack<ChunkBlocks> chunkRootStack_ = new Stack<ChunkBlocks>();
    Dictionary<Vector2Int, ChunkBlocks> activeChunkRoots_ = new Dictionary<Vector2Int, ChunkBlocks>();
    Block[,] blocks_;
    BlockCollideManager collideManager_ = new BlockCollideManager();
	int breakBlockNum_ = 0;
	int totalBlockNum_ = 0;
    List<Vector2Int> brokenBlockCoords_ = new List<Vector2Int>();
}
