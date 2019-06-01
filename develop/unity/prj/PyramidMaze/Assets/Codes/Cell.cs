using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 壁メッシュ情報
//  生成する壁ポリゴンの頂点情報（位置、UV等）を保持
public class WallMesh {
    //1 -- 3 
    //| ＼ |
    //0 -- 2
    public Vector3[] vertices_ = new Vector3[ 4 ] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    public Vector2[] uvs_ = new Vector2[ 4 ] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
    public Vector3 normal_ = Vector3.zero;

    // 頂点座標を生成
    public void createVertex( Vector3 center, float halfLen, Vector3 axisX, Vector3 axisY ) {
        vertices_[ 0 ] = center + (  axisX - axisY ) * halfLen;
        vertices_[ 1 ] = center + (  axisX + axisY ) * halfLen;
        vertices_[ 2 ] = center + ( -axisX - axisY ) * halfLen;
        vertices_[ 3 ] = center + ( -axisX + axisY ) * halfLen;
        normal_ = Vector3.Cross( axisY, axisX ).normalized;
    }

    public int appendVertices( ref List<Vector3> list, ref List<Vector3> normalList, ref List<Vector2> uvList ) {
        // 内側
        list.Add( vertices_[ 0 ] );
        list.Add( vertices_[ 1 ] );
        list.Add( vertices_[ 2 ] );
        list.Add( vertices_[ 1 ] );
        list.Add( vertices_[ 3 ] );
        list.Add( vertices_[ 2 ] );

        for ( int i = 0; i < 6; ++i )
            normalList.Add( -normal_ );

        var uvs = new Vector2[] {
            new Vector2( 0.0f, 0.0f ),
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 1.0f, 1.0f ),
        };
        uvList.Add( uvs[ 0 ] );
        uvList.Add( uvs[ 1 ] );
        uvList.Add( uvs[ 2 ] );
        uvList.Add( uvs[ 1 ] );
        uvList.Add( uvs[ 3 ] );
        uvList.Add( uvs[ 2 ] );

        // 外側
        list.Add( vertices_[ 0 ] );
        list.Add( vertices_[ 2 ] );
        list.Add( vertices_[ 1 ] );
        list.Add( vertices_[ 1 ] );
        list.Add( vertices_[ 2 ] );
        list.Add( vertices_[ 3 ] );

        for ( int i = 0; i < 6; ++i )
            normalList.Add( normal_ );

        uvList.Add( uvs[ 2 ] );
        uvList.Add( uvs[ 0 ] );
        uvList.Add( uvs[ 3 ] );
        uvList.Add( uvs[ 3 ] );
        uvList.Add( uvs[ 0 ] );
        uvList.Add( uvs[ 1 ] );

        return 12;
    }
}

// 迷路を作る時の部屋
public class Cell {
    public int groupId_;    // グループId。連結している同一のセル番号。0はグループ未所属
    public int level_;      // セルの段数。地面のセルが0で上方向に昇順
    public int x_;          // セルの位置X
    public int z_;          // セルの位置Z
    public int num_;        // 部屋の個数
    public float len_ = 1.0f;   // セルの一辺の長さ
    public Vector3 localPos_ = Vector3.zero;    // 部屋の中心点の位置
    public Cell[] link_ = new Cell[ 12 ];
    public List<WallMesh> wallMeshes_ = new List<WallMesh>();   // ルール：隣り合う部屋の壁は番手が低い部屋が持つ

    // 位置を取得
    public Vector3Int getCoord() {
        return new Vector3Int( x_, level_, z_ );
    }

    // 隣のセルとリンク（穴開き）
    public bool link( Cell adjCell ) {
        if ( adjCell.level_ < level_ - 1 || adjCell.level_ > level_ + 1 )
            return false;
        // 天井(0～3）
        if ( adjCell.level_ == level_ + 1 ) {
            // □2 □3
            // 　■
            // □0 □1
            int[] xs = new int[ 4 ] { -1, 0, -1, 0 };
            int[] zs = new int[ 4 ] { -1, -1, 0, 0 };
            for ( int i = 0; i < 4; ++i ) {
                if ( adjCell.x_ == x_ + xs[ i ] && adjCell.z_ == z_ + zs[ i ] ) {
                    link_[ i ] = adjCell;
                    return true;
                }
            }
            return false;
        }
        // 床(4～7)
        if ( adjCell.level_ == level_ - 1 ) {
            // □2 □3
            // 　■
            // □0 □1
            int[] xs = new int[ 4 ] { 0, 1, 0, 1 };
            int[] zs = new int[ 4 ] { 0, 0, 1, 1 };
            for ( int i = 0; i < 4; ++i ) {
                if ( adjCell.x_ == x_ + xs[ i ] && adjCell.z_ == z_ + zs[ i ] ) {
                    link_[ 4 + i ] = adjCell;
                    return true;
                }
            }
            return false;
        }
        // 壁(8～11)
        if ( adjCell.level_ == level_ ) {
            // 　 □3
            // 0□■□1
            //    □2
            int[] xs = new int[ 4 ] { -1, 1, 0, 0 };
            int[] zs = new int[ 4 ] {  0, 0, -1, 1 };
            for ( int i = 0; i < 4; ++i ) {
                if ( adjCell.x_ == x_ + xs[ i ] && adjCell.z_ == z_ + zs[ i ] ) {
                    link_[ 8 + i ] = adjCell;
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    // 壁を生成
    public void createWalls() {
        // ルール：隣り合う部屋の壁は番手が低い部屋が持つ
        // 　　　：壁側は勿論追加する
        var axisX = new Vector3( 1.0f, 0.0f, 0.0f );
        var axisY = new Vector3( 0.0f, 1.0f, 0.0f );
        var axisZ = new Vector3( 0.0f, 0.0f, 1.0f );

        Vector3[] offsets = new Vector3[] {
            ( - axisX - axisZ ) * len_ * 0.25f + axisY * len_ * 0.5f,    // 天井
            (   axisX - axisZ ) * len_ * 0.25f + axisY * len_ * 0.5f,
            ( - axisX + axisZ ) * len_ * 0.25f + axisY * len_ * 0.5f,
            (   axisX + axisZ ) * len_ * 0.25f + axisY * len_ * 0.5f,
            ( - axisX - axisZ ) * len_ * 0.25f - axisY * len_ * 0.5f,    // 床
            (   axisX - axisZ ) * len_ * 0.25f - axisY * len_ * 0.5f,
            ( - axisX + axisZ ) * len_ * 0.25f - axisY * len_ * 0.5f,
            (   axisX + axisZ ) * len_ * 0.25f - axisY * len_ * 0.5f,
            ( - axisX ) * len_ * 0.5f,  // 左壁
            (   axisX ) * len_ * 0.5f,  // 右壁
            ( - axisZ ) * len_ * 0.5f,  // 下壁
            (   axisZ ) * len_ * 0.5f,  // 上壁
        };
        float[] lens = new float[] {
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.25f,
            len_ * 0.5f,
            len_ * 0.5f,
            len_ * 0.5f,
            len_ * 0.5f,
        };
        Vector3[] axisXs = new Vector3[] {
            axisX,
            axisX,
            axisX,
            axisX,
            axisX,
            axisX,
            axisX,
            axisX,
            axisZ,
           -axisZ,
           -axisX,
            axisX,
        };
        Vector3[] axisYs = new Vector3[] {
            -axisZ,
            -axisZ,
            -axisZ,
            -axisZ,
             axisZ,
             axisZ,
             axisZ,
             axisZ,
             axisY,
             axisY,
             axisY,
             axisY,
        };
        bool[] createFlags = new bool[] {
            true,
            true,
            true,
            true,
            level_ == 0,
            level_ == 0,
            level_ == 0,
            level_ == 0,
            true,           // 左壁
            x_ == num_ - 1, // 右壁
            true,           // 下壁
            z_ == num_ - 1, // 上壁
        };

        for ( int i = 0; i < link_.Length; ++i ) {
            if ( link_[ i ] != null || createFlags[ i ] == false ) {
                continue;   // 穴開きか共通壁のため作る必要が無い
            }
            var p = localPos_ + offsets[ i ];
            var wall = new WallMesh();
            wall.createVertex( p, lens[ i ], axisXs[ i ], axisYs[ i ] );
            wallMeshes_.Add( wall );
        }
    }
}
