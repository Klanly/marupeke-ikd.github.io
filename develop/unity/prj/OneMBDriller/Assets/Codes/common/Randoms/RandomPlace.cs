using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランダム配置
//  矩形範囲でランダムに点を配置する
public class RandomPlace
{
    // 除外範囲シェイプ
    public class IgnoreShape {
        public virtual bool isIgnore( Vector2 min, Vector2 max ) {
            return true;
        }
        public virtual bool isIgnore( Vector2 pos ) {
            return true;
        }
    }

    // 除外円領域
    public class IgnoreCircle : IgnoreShape {
        public IgnoreCircle() {
        }
        public IgnoreCircle( Vector2 center, float r ) {
            center_ = center;
            r_ = r;
        }
        public override bool isIgnore( Vector2 min, Vector2 max ) {
            if ( !isIgnore( min ) || !isIgnore( max ) || !isIgnore( min.x,  max.y ) || !isIgnore( max.x, min.y ) ) {
                return false;
            }
            return true;
        }
        public override bool isIgnore( Vector2 pos ) {
            return ( pos - center_ ).magnitude <= r_;
        }
        bool isIgnore( float x, float y ) {
            float dx = x - center_.x;
            float dy = y - center_.y;
            return dx * dx + dy * dy <= r_ * r_;
        }
        public Vector2 center_ = Vector2.zero;
        public float r_;
    }

    // 除外矩形領域
    public class IgnoreRect : IgnoreShape {
        public IgnoreRect() {
        }
        public IgnoreRect( Vector2 min, Vector2 max ) {
            min_ = min;
            max_ = max;
            Swaps.minMax( ref min_, ref max_ );
        }
        public override bool isIgnore(Vector2 min, Vector2 max) {
            if ( !isIgnore( min ) || !isIgnore( max ) || !isIgnore( min.x, max.y ) || !isIgnore( max.x, min.y ) ) {
                return false;
            }
            return true;
        }
        bool isIgnore( float x, float y ) {
            return ( x >= min_.x && x <= max_.x && y >= min_.y && y <= max_.y );
        }
        public override bool isIgnore( Vector2 pos ) {
            return ( pos.x >= min_.x && pos.x <= max_.x && pos.y >= min_.y && pos.y <= max_.y );
        }
        Vector2 min_ = Vector2.zero;
        Vector2 max_ = Vector2.zero;
    }

    // 一定距離以上離れてランダムに配置
    //  領域の大きさに対してdistが長い場合、指定数が生成される保障は無くなります
    //  min : 領域の左下座標
    //  max : 領域の右上座標
    //  dist: 最短隔離距離
    //  num : 発生する点の数
    static public List< Vector2 > distanceBase( Vector2 min, Vector2 max, float dist, int num, List< IgnoreShape > ignoreShapes = null ) {
        Swaps.minMax( ref min, ref max );
        dist = Mathf.Clamp( dist, 0.00001f, Mathf.Abs( dist ) );
        var list = new List<Vector2>();
        if ( ignoreShapes == null ) {
            ignoreShapes = new List<IgnoreShape>();
        }
        var wh = max - min; // 幅高

        // 領域の幅高どちらも最短距離に満たない場合は1点しか作成できない（とする）
        if ( max.x - min.x < dist && max.y - min.y < dist ) {
            num = 1;
        }

        // 領域をdist幅以上の区画に分割
        int sepNumX = ( int )( wh.x / dist );
        int sepNumY = ( int )( wh.y / dist );
        sepNumX = sepNumX == 0 ? 1 : sepNumX;
        sepNumY = sepNumY == 0 ? 1 : sepNumY;
        sepNumX = ( sepNumX > 128 ? 128 : sepNumX );
        sepNumY = ( sepNumY > 128 ? 128 : sepNumY );
        int totalCellNum = sepNumX * sepNumY;
        float unitX = wh.x / sepNumX;
        float unitY = wh.y / sepNumY;
        var unit = new Vector2( unitX, unitY );

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

            // セルが除外範囲内の場合はスキップ
            foreach ( var ig in ignoreShapes ) {
                if ( ig.isIgnore( cellPos, cellPos + unit ) == true ) {
                    // 除外
                    checkNum++;
                    i--;
                }
            }

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
                    } else {
                        // 点が除外範囲内の場合は不採用
                        foreach ( var ig in ignoreShapes ) {
                            if ( ig.isIgnore( p ) == true ) {
                                isValid = false;
                                break;
                            }
                        }
                        if ( isValid == false ) {
                            break;
                        }
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
