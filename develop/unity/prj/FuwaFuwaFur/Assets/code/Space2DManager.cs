using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2D空間アクティブ管理人
//
//  指定の空間(LB - RT)に1ユニット縦横サイズMの正方形空間を敷き詰め、
//  縦横サイズNのビジブル空間V（中心点P）が含まれるユニットをアクティブに、そうでない空間を非アクティブに切り替えます。

public class Space2DManager {
    public Space2DManager( Vector2 lb, Vector2 rt,  float unitSize )
    {
        LB_ = Vector2.Min( lb, rt );
        RT_ = Vector2.Max( lb, rt );
        WN_ = ( int )( ( rt.x - lb.x + 0.01f ) / unitSize );    // あまりきっちりだと誤差で範囲が足りなくなる事があるので0.01足してます
        HN_ = ( int )( ( rt.y - lb.y + 0.01f ) / unitSize );
        WN_ = WN_ == 0 ? 1 : WN_;
        HN_ = HN_ == 0 ? 1 : HN_;
        USz_ = unitSize;
    }

    // ビジブル領域を更新
    //  size: ビジブル矩形の辺の長さ。0以上（0も認める）
    public void setVisibleRegion( Vector2 center, float size )
    {
        visibleRegionPos_ = center;
        if ( size <= 0.0f )
            size = 0.0f;
        visibleSize_ = size;
        visibleHalfSize_.x = visibleHalfSize_.y = size * 0.5f;
    }

    // アクティブユニット更新
    public void update( out List<int> newActiveIndices, out List<int> newNoneActiveIndices )
    {
        bool bExistActiveRegion = true;
        // ビジブル左下点と右上点のインデックスを算出
        Vector2 VLB = visibleRegionPos_ - visibleHalfSize_;
        int LBXOfs = calcIdxOffsetX( VLB );
        int LBYOfs = calcIdxOffsetY( VLB );
        if ( LBXOfs == -2 || LBYOfs == -2 ) {
            // 全領域非アクティブ
            bExistActiveRegion = false ;
        }
        if ( LBXOfs == -1 )
            LBXOfs = 0;
        if ( LBYOfs == -1 )
            LBYOfs = 0;
        Vector2 VRT = visibleRegionPos_ + visibleHalfSize_;
        int RTXOfs = calcIdxOffsetX( VRT );
        int RTYOfs = calcIdxOffsetY( VRT );
        if ( RTXOfs == -1 || RTYOfs == -1 ) {
            // 全領域非アクティブ
            bExistActiveRegion = false;
        }
        if ( RTXOfs == -2 )
            RTXOfs = WN_ - 1;
        if ( RTYOfs == -2 )
            RTYOfs = HN_ - 1;

        HashSet<int> activeIdx = new HashSet<int>();
        List<int> newActiveIdx = new List<int>();
        List<int> newNoneActiveIdx = new List<int>();

        // アクティブユニット存在？
        if ( bExistActiveRegion == true ) {
            // アクティブユニットを更新
            for ( int y = LBYOfs; y <= RTYOfs; ++y ) {
                for ( int x = LBXOfs; x <= RTXOfs; ++x ) {
                    int idx = y * WN_ + x;
                    activeIdx.Add( idx );
                    if ( activeIdx_.Contains( idx ) == false ) {
                        newActiveIdx.Add( idx );    // 新しくアクティブになった
                    } else {
                        activeIdx_.Remove( idx );   // 残った物は非アクティブになる
                    }
                }
            }
            if ( activeIdx_.Count > 0 ) {
                // 非アクティブに変わったユニットがある
                foreach ( var i in activeIdx_ )
                    newNoneActiveIdx.Add( i );
            }

            // アクティブユニット更新
            activeIdx_ = activeIdx;

        } else {
            // アクティブユニットが一つもない（範囲外例外）
            // 既存の全アクティブユニットが非アクティブに
            foreach ( var i in activeIdx_ )
                newNoneActiveIdx.Add( i );

            activeIdx_.Clear();
        }

        // 新しくアクティブになった事を報告
        newActiveIndices = newActiveIdx;

        // 新しく非アクティブになった事を報告
        newNoneActiveIndices = newNoneActiveIdx;

    }

    // ユニット数を取得
    public int getUnitNum()
    {
        return WN_ * HN_;
    }

    // 指定点のインデックスを算出
    public int calcPointIndex( Vector2 p )
    {
        // はみ出した場合はクランプする
        int x = calcIdxOffsetX( p );
        if ( x == -1 )
            x = 0;
        else if ( x == -2 )
            x = WN_ - 1;
        int y = calcIdxOffsetY( p );
        if ( y == -1 )
            y = 0;
        else if ( y == -2 )
            y = HN_ - 1;
        return y * WN_ + x;
    }

    // 指定位置の横オフセットインデックスを取得
    //  左範囲外は-1、右範囲外は-2を返す
    int calcIdxOffsetX( Vector2 p )
    {
        if ( p.x < LB_.x )
            return -1;  // 左はみ出し
        if ( p.x > RT_.x )
            return -2;  // 右はみ出し
        if ( p.x == RT_.x )
            return WN_ - 1;     // 右境界線は有効
        return ( int )( ( p.x - LB_.x ) / USz_ );
    }

    // 指定位置の縦オフセットインデックスを取得
    //  下範囲外は-1、上範囲外は-2を返す
    int calcIdxOffsetY( Vector2 p )
    {
        if ( p.y < LB_.y )
            return -1;  // 下はみ出し
        if ( p.y > RT_.y )
            return -2;  // 上はみ出し
        if ( p.y == RT_.y )
            return HN_ - 1;     // 上境界線は有効
        return ( int )( ( p.y - LB_.y ) / USz_ );
    }

    Vector2 LB_, RT_;   // 左下、右上座標
    int WN_ = 0;        // 管理領域の横ユニット数
    int HN_ = 0;        // 管理領域の縦ユニット数
    float USz_;         // ユニットサイズ
    Vector2 visibleRegionPos_ = Vector2.zero;   // ビジブル空間中心点
    float visibleSize_;         // ビジブル空間サイズ
    HashSet<int> activeIdx_ = new HashSet<int>();   // 現在のアクティブ領域インデックス

    Vector2 visibleHalfSize_ = Vector2.zero;     // ビジブル空間の半サイズ（計算用）
}
