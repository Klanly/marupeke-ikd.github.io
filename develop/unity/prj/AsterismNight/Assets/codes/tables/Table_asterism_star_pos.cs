using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星座恒星位置データ

public class Table_asterism_star_pos : Table {

    private Table_asterism_star_pos() { }

    static public Table_asterism_star_pos getInstance()
    {
        if ( table_ == null ) {
            table_ = new Table_asterism_star_pos();
            table_.create( "tables/asterism_star_pos.tsv" );
        }
        return table_;
    }

    public class Data
    {
        public int hipId_;      // ヒッパルコスid
        public int longHour_;      // 経度時
        public int longMinute_;    // 経度分
        public float longSec_;     // 経度秒
        public float long_;        // 経度
        public int latDeg_;       // 緯度（整数度）
        public int latMinute_;    // 緯度分
        public float latSec_;     // 緯度秒
        public float lat_;        // 緯度
        public float magnitude_;    // 見かけ等級
        public float stellarParallax_;  // 年周視差
        public float longProperMotion_; // 赤経方向固有運動
        public float latProperMotion_;  // 赤緯方向固有運動
        public float bvColorIndex_;     // B-V色指数
        public float viColorIndex_;     // V-I色指数
        public string spectralClassification_;  // スペクトル分類
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
        d.longHour_ = values[ "longHour" ].iVal_;
        d.longMinute_ = values[ "longMinute" ].iVal_;
        d.longSec_ = values[ "longSec" ].fVal_;
        d.long_ = values[ "long" ].fVal_;
        d.latDeg_ = values[ "latDeg" ].iVal_;
        d.latMinute_ = values[ "latMinute" ].iVal_;
        d.latSec_ = values[ "latSec" ].fVal_;
        d.lat_ = values[ "lat" ].fVal_;
        d.magnitude_ = values[ "magnitude" ].fVal_;
        d.stellarParallax_ = values[ "stellarParallax" ].fVal_;
        d.longProperMotion_ = values[ "longProperMotion" ].fVal_;
        d.latProperMotion_ = values[ "latProperMotion" ].fVal_;
        d.bvColorIndex_ = values[ "bvColorIndex" ].fVal_;
        d.viColorIndex_ = values[ "viColorIndex" ].fVal_;
        d.spectralClassification_ = values[ "spectralClassification" ].sVal_;

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

    // ヒッパルコスIdから取得
    public Data getDataFromHipId( int hipId )
    {
        if ( dict_.ContainsKey( hipId ) == false )
            return null;
        return dict_[ hipId ];
    }

    static Table_asterism_star_pos table_;
    int dataNum_ = 0;
    Data[] data_;
    Dictionary<int, Data> dict_ = new Dictionary<int, Data>();    // key: ヒッパルコスId
}