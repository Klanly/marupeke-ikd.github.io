using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星座恒星ヒッパルコスデータ

public class Table_asterism_star_hip : Table {

    private Table_asterism_star_hip() { }

    static public Table_asterism_star_hip getInstance()
    {
        if ( table_ == null ) {
            table_ = new Table_asterism_star_hip();
            table_.create( "tables/asterism_star_hip.tsv" );
        }
        return table_;
    }

    public class Data
    {
        public int hipId_;      // ヒッパルコスid
        public string egStarName_;  // 恒星名（英語）
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
        d.hipId_ = values[ "hipId" ].iVal_;
        d.egStarName_= values[ "egStarName" ].sVal_;

        data_[ dataNum_ ] = d;
        dict_[ d.hipId_ ] = d;
        dataNum_++;
    }

    // データを取得
    public Data getData( int index )
    {
        if ( index >= data_.Length )
            return null;
        return data_[ index ];
    }

    // ヒッパルコスIdから名前を取得
    public string getName( int hipId )
    {
        if ( dict_.ContainsKey( hipId ) == false )
            return "";
        return dict_[ hipId ].egStarName_;
    }

    static Table_asterism_star_hip table_;
    int dataNum_ = 0;
    Data[] data_;
    Dictionary<int, Data> dict_ = new Dictionary<int, Data>();  // key: hipId
}