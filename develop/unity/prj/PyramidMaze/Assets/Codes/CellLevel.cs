using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1レベル分の情報

public class CellLevel {
    public int level_;      // 段数
    public Cell[,] cells_;  // セル情報(z,x)
    public int edgeNum_;    // 1辺の部屋の数

    public CellLevel( int level, int totalLevelNum ) {
        level_ = level;
        int n = totalLevelNum - level;
        cells_ = new Cell[ n, n ];
        for ( int z = 0; z < n; ++z ) {
            for ( int x = 0; x < n; ++x ) {
                cells_[ z, x ] = new Cell();
            }
        }
        edgeNum_ = n;
    }
}
