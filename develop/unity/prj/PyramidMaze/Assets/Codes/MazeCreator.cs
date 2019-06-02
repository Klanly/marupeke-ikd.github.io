using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 迷路生成者

public class MazeCreator : MonoBehaviour {
    // 生成迷路情報
    public class Parameter {
        public int level_ = 1;      // 段数
        public List<CellLevel> cellLevel_ = new List<CellLevel>();     // Level
        public float roomWidthX_ = 1.0f;    // 部屋のX軸方向の幅
        public float roomWidthZ_ = 1.0f;    // 部屋のZ軸方向の幅
        public float roomHeight_ = 1.0f;    // 部屋の高さ
        public int groupCountMin_ = 5;      // 作成時の最小連続接続数
        public int groupCountMax_ = 10;      // 作成時の最大連続接続数
        public bool useLevel0Holl_ = false;     // Level0の壁に穴を空ける？
        public bool bReady_ = false;

        public Cell getTopCell() {
            return cellLevel_[ level_ - 1 ].cells_[ 0, 0 ];
        }

        public bool isReady() {
            return bReady_;
        }
    }

    // 生成
    static public void create( ref Parameter param ) {

        // 段数分の部屋を用意
        var noGroupCells = new HashSet<Cell>();
        for ( int i = 0; i < param.level_; ++i ) {
            var cellLevel = new CellLevel( i, param.level_ );
            for ( int z = 0; z < cellLevel.edgeNum_; ++z ) {
                for ( int x = 0; x < cellLevel.edgeNum_; ++x ) {
                    var cell = cellLevel.cells_[ z, x ];
                    cell.groupId_ = 0;
                    cell.level_ = i;
                    cell.x_ = x;
                    cell.z_ = z;
                    cell.num_ = param.level_ - i;
                    cell.len_ = param.roomWidthX_;
                    cell.localPos_.x = param.roomWidthX_ * 0.5f * i + x * param.roomWidthX_;
                    cell.localPos_.z = param.roomWidthZ_ * 0.5f * i + z * param.roomWidthZ_;
                    cell.localPos_.y = param.roomHeight_ * ( 0.5f + i );
                    noGroupCells.Add( cell );
                }
            }
            param.cellLevel_.Add( cellLevel );
        }

        // 検索用
        var offsets = new Vector3Int[] {
                    new Vector3Int( -1, 1, -1 ),    // 天井
                    new Vector3Int(  0, 1, -1 ),
                    new Vector3Int( -1, 1,  0 ),
                    new Vector3Int(  0, 1,  0 ),
                    new Vector3Int(  0, -1, 0 ),    // 床
                    new Vector3Int(  1, -1, 0 ),
                    new Vector3Int(  0, -1, 1 ),
                    new Vector3Int(  1, -1, 1 ),
                    new Vector3Int( -1,  0,  0 ),    // 壁
                    new Vector3Int(  1,  0,  0 ),
                    new Vector3Int(  0,  0, -1 ),
                    new Vector3Int(  0,  0,  1 ),
                };

        // まだ連結が無い部屋を起点に数歩分進行
        var cellList = new List<Cell>();
        int curGroupId = 0;
        while( noGroupCells.Count > 0 ) {
            curGroupId++;
            // 一つランダムに選択
            cellList.Clear();
            foreach ( var c in noGroupCells ) {
                cellList.Add( c );
            }
            var startCell = ListUtil.random( cellList );
            startCell.groupId_ = curGroupId;
            noGroupCells.Remove( startCell );

            // 進行
            int groupCount = Random.Range( param.groupCountMin_, param.groupCountMax_ + 1 );
            Cell curCell = startCell;
            for ( int i = 0; i < groupCount; ++i ) {
                // 周囲のセルの内未グループもしくは自分以外のグループを収集
                var adjCells = new List<Cell>();    // x, level, z
                foreach ( var offset in offsets ) {
                    Vector3Int adjPos = curCell.getCoord() + offset;
                    if ( isValidateCellPos( adjPos, param ) == true ) {
                        Cell adjCell = param.cellLevel_[ adjPos.y ].cells_[ adjPos.z, adjPos.x ];
                        if ( adjCell.groupId_ == 0 || adjCell.groupId_ != curGroupId ) {
                            adjCells.Add( adjCell );    // 妥当セル見つけた
                        }
                    }
                }

                // 妥当セルが無い場合はこのグループの進行終了
                if ( adjCells.Count == 0 )
                    break;

                // リンクしたセルを互いに記録
                Cell nextCell = ListUtil.random( adjCells );
                curCell.link( nextCell );
                nextCell.link( curCell );
                noGroupCells.Remove( nextCell );

                // 相手が別所属グループだった時は進行終了
                if ( nextCell.groupId_ != 0 && curCell.groupId_ != nextCell.groupId_ )
                    break;

                curCell = nextCell;
                curCell.groupId_ = curGroupId;
            }
        }

        if ( param.useLevel0Holl_ == true ) {
            // Level0の任意の部屋の外側の壁を外す（スタート地点）
            {
                int xSize = param.cellLevel_[ 0 ].cells_.GetLength( 0 );
                int zSize = param.cellLevel_[ 0 ].cells_.GetLength( 1 );
                int x = 0;
                int z = 0;
                int wallIdx = 0;
                switch ( Random.Range( 0, 4 ) ) {
                    case 0: // L
                        x = 0;
                        z = Random.Range( 0, zSize );
                        wallIdx = 8;
                        break;
                    case 1:  // R
                        x = xSize - 1;
                        z = Random.Range( 0, zSize );
                        wallIdx = 9;
                        break;
                    case 2: // B
                        x = Random.Range( 0, xSize );
                        z = 0;
                        wallIdx = 10;
                        break;
                    case 3: // T
                        x = Random.Range( 0, xSize );
                        z = zSize - 1;
                        wallIdx = 11;
                        break;
                }
                var startCell = param.cellLevel_[ 0 ].cells_[ z, x ];
                startCell.link_[ wallIdx ] = startCell; // 自分を入れておくことにする！
            }
        }

        // 壁メッシュデータ作成
        foreach ( var cl in param.cellLevel_ ) {
            int xSz = cl.cells_.GetLength( 0 );
            int zSz = cl.cells_.GetLength( 1 );
            for ( int z = 0; z < zSz; ++z ) {
                for ( int x = 0; x < xSz; ++x ) {
                    var cell = cl.cells_[ z, x ];
                    cell.createWalls();
                }
            }
        }

        param.bReady_ = true;
    }

    // セルの位置は妥当？
    static bool isValidateCellPos( Vector3Int pos, Parameter param ) {
        if ( pos.y < 0 || pos.y >= param.level_ )
            return false;   // Level範囲外
        int n = param.level_ - pos.y;   // その段の個数
        if ( pos.x < 0 || pos.z < 0 || pos.x >= n || pos.z >= n )
            return false;   // 段内座標範囲外
        return true;
    }
}
