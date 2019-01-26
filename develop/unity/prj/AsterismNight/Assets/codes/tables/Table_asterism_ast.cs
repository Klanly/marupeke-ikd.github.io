using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星座基礎データ

public class Table_asterism_ast : Table {

    private Table_asterism_ast() { }

    static public Table_asterism_ast getInstance()
    {
        if ( table_ == null ) {
            table_ = new Table_asterism_ast();
            table_.create( "tables/asterism_ast.tsv" );
        }
        return table_;
    }

    public class Data
    {
        public string shortName_;  // 略名
        public string name_;       // 名前（英語）
        public string jpName_;     // 名前（日本名）
        public int longHour_;      // 経度時
        public int longMinute_;    // 経度分
        public int lat_;           // 緯度
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
        d.shortName_ = values[ "shortName" ].sVal_;
        d.name_ = values[ "name" ].sVal_;
        d.jpName_ = values[ "jpName" ].sVal_;
        d.longHour_ = values[ "longHour" ].iVal_;
        d.longMinute_ = values[ "longMinute" ].iVal_;
        d.lat_ = values[ "lat" ].iVal_;

        data_[ dataNum_ ] = d;
        dataNum_++;
    }

    // データを取得
    public Data getData( int index )
    {
        if ( index >= data_.Length )
            return null;
        return data_[ index ];
    }

    static Table_asterism_ast table_;
    int dataNum_ = 0;
    Data[] data_;
}