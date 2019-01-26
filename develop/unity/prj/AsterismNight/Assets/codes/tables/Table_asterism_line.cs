using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星間ラインデータ

public class Table_asterism_line : Table {

    private Table_asterism_line() { }

    static public Table_asterism_line getInstance()
    {
        if ( table_ == null ) {
            table_ = new Table_asterism_line();
            table_.create( "tables/asterism_line.tsv" );
        }
        return table_;
    }

    public class Data
    {
        public string id_;      // id
        public int astId_;      // 星座Id
        public float startLong_;    // 開始点経度
        public float startLat_;     // 開始点緯度
        public float endLong_;    // 終点経度
        public float endLat_;     // 終点緯度
    }

    // データ格納前コール
    protected override void preStore(int dataNum)
    {
        data_ = new Data[ dataNum ];
    }

    // 1レコードを格納
    protected override void storeData( Dictionary<string, Val> values )
    {
        var d = new Data();
        d.id_ = values[ "id" ].sVal_;
        d.astId_ = values[ "astId" ].iVal_;
        d.startLong_ = values[ "startLong" ].fVal_;
        d.startLat_ = values[ "startLat" ].fVal_;
        d.endLong_ = values[ "endLong" ].fVal_;
        d.endLat_ = values[ "endLat" ].iVal_;

        data_[ dataNum_ ] = d;
        dataNum_++;
    }

    // データ格納後コール
    //  派生クラスで必要に応じて使用
    protected override void postStore()
    {
        // 星座別のラインデータをまとめる
        for( int i = 0; i < data_.Length; ++i ) {
            var d = data_[ i ];
            if ( astDict_.ContainsKey( d.astId_ ) == false ) {
                astDict_[ d.astId_ ] = new List<int>();
            }
            astDict_[ d.astId_ ].Add( i );
        }
    }

    // データを取得
    public Data getData( int index )
    {
        if ( index >= data_.Length )
            return null;
        return data_[ index ];
    }

    // 星座Idを構成するラインデータインデックス配列を取得
    public List<int> getAstIndices( int astId )
    {
        if ( astId < 1 || astId > 89 )
            return null;
        return astDict_[ astId ];
    }

    static Table_asterism_line table_;
    int dataNum_ = 0;
    Data[] data_;
    Dictionary<int, List<int>> astDict_ = new Dictionary<int, List<int>>();
}