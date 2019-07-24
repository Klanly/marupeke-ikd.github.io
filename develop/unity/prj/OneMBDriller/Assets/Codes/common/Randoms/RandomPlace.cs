using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランダム配置
//  矩形範囲でランダムに点を配置する
public class RandomPlace
{
    // 一定距離以上離れてランダムに配置
    //  領域の大きさに対してdistが長い場合、指定数が生成される保障は無くなります
    //  min : 領域の左下座標
    //  max : 領域の右上座標
    //  dist: 最短隔離距離
    //  num : 発生する点の数
    static public List< Vector2 > distanceBase( Vector2 min, Vector2 max, float dist, int num ) {
        Swaps.minMax( ref min, ref max );
        dist = Mathf.Clamp( dist, 0.0f, Mathf.Abs( dist ) );
        var list = new List<Vector2>();

        var wh = max - min; // 幅高

        // 領域の幅高どちらも最短距離に満たない場合は1点しか作成できない（とする）
        if ( max.x - min.x < dist && max.y - min.y < dist ) {
            list.Add( min + Randoms.Vec2.value( wh ) );
            return list;
        }

        // 距離がゼロなら単純に位置をランダムに作成
        if ( dist == 0.0f ) {
            for ( int i = 0; i < num; ++i ) {
                list.Add( min + Randoms.Vec2.value( wh ) );
            }
            return list;
        }

        // 領域をdist幅以上の区画に分割
        int sepNumX = ( int )( wh.x / dist );
        int sepNumY = ( int )( wh.y / dist );
        sepNumX = ( sepNumX > 128 ? 128 : sepNumX );
        sepNumY = ( sepNumY > 128 ? 128 : sepNumY );
        int totalCellNum = sepNumX * sepNumY;
        float unitX = wh.x / sepNumX;
        float unitY = wh.y / sepNumY;

        // 座標格納配列確保
        var cells = new Vector2[ sepNumX, sepNumY ];
        var pointExists = new bool[ sepNumX, sepNumY ];

        // セル座標番号をシャッフル
        var indices = new List<int>();
        ListUtil.numbering( ref indices, sepNumX * sepNumY );
        ListUtil.shuffle( ref indices );
        var colPoints = new Vector2[ 9 ];
        int checkNum = 0;   // チェックしたセルの数
        for ( int i = 0; i < num && checkNum < indices.Count; ++i ) {
            int e = indices[ checkNum ];
            int x = e % sepNumX;
            int y = e / sepNumX;
            var cellPos = new Vector2( x * unitX, y * unitY );

            // 周囲8セルの点の座標を取得
            int sx = x > 0 ? x - 1 : x;
            int ex = x < sepNumX - 1 ? x + 1 : x;
            int sy = y > 0 ? y - 1 : y;
            int ey = y < sepNumY - 1 ? y + 1 : y;
            int cpn = 0;  // 相手点の数
            for ( int oy = sy; oy <= ey; ++oy ) {
                for ( int ox = sx; ox <= ex; ++ox ) {
                    if ( pointExists[ ox, oy ] == true ) {
                        colPoints[ cpn ] = cells[ ox, oy ];
                        cpn++;
                    }
                }
            }

            const int tryNum = 16;  // 試行回数
            bool isValid = true;
            Vector2 p = Vector2.zero;
            for ( int t = 0; t < tryNum; ++t ) {
                // 設定点に対して周囲8セル内の点との距離をチェック
                p =  Vector2.one * cellPos + Randoms.Vec2.value( unitX, unitY );
                for ( int n = 0; n < cpn; ++n ) {
                    float d = ( colPoints[ n ] - p ).magnitude;
                    if ( d < dist ) {
                        // 不採用
                        isValid = false;
                        break;
                    }
                }
                if ( isValid == true )
                    break;
            }

            // 採用点が見つかったら格納
            if ( isValid == true ) {
                list.Add( p );
                cells[ x, y ] = p;
                pointExists[ x, y ] = true;
            } else {
                i--;    // numは進めない
            }
            checkNum++; // チェック数は採用不採用関係なくカウントアップ
        }

        return list;
    }
}
