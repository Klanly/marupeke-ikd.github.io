using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    int xNum_ = 8;

    [SerializeField]
    int yNum_ = 8;

    [SerializeField]
    float unitWidth_ = 2.0f;

    [SerializeField]
    float unitHeight_ = 3.7f;

    [SerializeField]
    Transform clientRoot_;

    public int XNum { get { return xNum_; } }
    public int YNum { get { return yNum_; } }
    public float UnitWidth { get { return unitWidth_; } }
    public float UnitHeight { get { return unitHeight_; } }
    public Transform ClientRoot { get { return clientRoot_; } }
    public PaiObject[,] Box { get { return box_; } }

    public Vector3 getPos( int idxX, int idxY ) {
        return new Vector3(
            unitWidth_ * ( idxX + 0.5f ),
            unitHeight_ * ( idxY + 0.5f ),
            0.0f
        );
    }

    // 牌を追加
    public void addPai( PaiObject pai, Vector2Int idx ) {
        box_[ idx.x, idx.y ] = pai;
        pai.transform.SetParent ( clientRoot_.transform );
        pai.setIdx( idx );
    }

    // フィールドボックスを更新
    public void updateBox( System.Action finishCallback, int rensa = 1 ) {
        // 連鎖がある
        var checker = new PaiGroupChecker();
        List<List<PaiObject>> paiSetList = null;
        List<MenzenSet> menzenSetList = null;
        checker.check( box_, out paiSetList, out menzenSetList );

        // 点数計算
        calclineDeleteScore( rensa, paiSetList );

        // 面子設定
        addMenzenSet( menzenSetList );

        // TODO:
        foreach ( var list in paiSetList ) {
            foreach ( var p in list ) {
                if ( box_[ p.Index.x, p.Index.y ] != null ) {
                    Destroy( box_[ p.Index.x, p.Index.y ].gameObject );
                    box_[ p.Index.x, p.Index.y ] = null;
                }
            }
        }

        // 全体フォール（非同期）
        allFall( ( res ) => {
            if ( res == true ) {
                updateBox( finishCallback, rensa + 1 );
            } else {
                finishCallback();
            }
        } );
    }

    // ライン消しの点数計算
    void calclineDeleteScore( int rensa, List<List<PaiObject>> paiSetList ) {
        // 1ライン点数 s
        // 3牌: 100
        // 4牌: 500
        // 5牌: 2000
        // 6牌: 5000
        // 7牌: 10000
        // 8牌: 20000
        // 同時削除ライン数 l
        // 同時削除ライン倍数 t = 2 * l - 1
        // 連鎖倍率 r = 1, 4, 8, ...
        //
        // 削除スコア ds = t * l * Σs 
        // (ex)
        //  3牌, 4牌同時消し、3連鎖目
        //   ds = 3 * ( 4 - 1 ) * ( 100 + 500 ) = 5400

        var lineScore = new Dictionary<int, int> {
            { 3, 100 },
            { 4, 500 },
            { 5, 2000 },
            { 6, 5000 },
            { 7, 10000 },
            { 8, 20000 },
        };
        int s = 0;
        foreach ( var ps in paiSetList ) {
            s += lineScore[ ps.Count ];
        }
        int t = paiSetList.Count * 2 - 1;
        int r = ( rensa == 1 ? 1 : ( rensa - 1 ) * 4 );
        int ds = r * t * s;

        if ( ds > 0 ) {
            Debug.Log( "rensa: " + rensa + ", linescore: " + s + ", set num: " + paiSetList.Count + ", score: " + ds );
            GameManager.getInstance().addScore( ds, rensa );
        }
    }

    //
    // 面子設定
    void addMenzenSet( List<MenzenSet> menzenSetList ) {
        // 面子登録
        //  面子になった牌の引き渡し
        GameManager.getInstance().addMentsu( menzenSetList );
    }


    // 全牌をフォール
    void allFall( System.Action< bool > result ) {
        PaiMoveUtil moveUtil = new PaiMoveUtil();
        moveUtil.PaiH = UnitHeight;
        moveUtil.UnitX = UnitWidth;
        moveUtil.UnitY = UnitHeight;

        // y=1から検索していき、牌の下が開いていたら落下可能位置まで移動。
        var bottoms = new List<int>();
        for ( int x = 0; x < box_.GetLength( 0 ); ++x ) {
            int b = 0;
            if ( box_[ x, 0 ] != null ) {
                b = 1;
            }
            bottoms.Add( b );
        }
        bool isFall = false;
        for ( int y = 1; y < box_.GetLength( 1 ); ++y ) {
            for ( int x = 0; x < box_.GetLength( 0 ); ++x ) {
                if ( box_[ x, y ] == null ) {
                    continue;
                }
                if ( y != bottoms[ x ] ) {
                    // bottomsまで落ちる確定
                    // 位置を移動
                    box_[ x, bottoms[ x ] ] = box_[ x, y ];
                    box_[ x, y ] = null;
                    var idx = new Vector2Int( x, bottoms[ x ] );
                    box_[ x, bottoms[ x ] ].setIdx( idx );

                    // 移動
                    //  TODO: 移動モーションに投げても良いかも
                    box_[ x, bottoms[ x ] ].transform.position = moveUtil.convIdxToPos( idx );
                    bottoms[ x ]++; // 底を更新

                    isFall = true;  // 落下検知
                } else {
                    bottoms[ x ]++; // 底を更新
                }
            }
        }

        result( isFall );
    }

    private void Awake() {
        box_ = new PaiObject[ xNum_, yNum_ + 2 ];  // 上2個の所から発生
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    PaiObject[,] box_;
}
