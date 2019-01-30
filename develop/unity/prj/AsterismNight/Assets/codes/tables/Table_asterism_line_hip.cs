using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星座恒星間ヒッパルコスデータ

public class Table_asterism_line_hip : Table {

    private Table_asterism_line_hip() { }

    static public Table_asterism_line_hip getInstance()
    {
        if ( table_ == null ) {
            table_ = new Table_asterism_line_hip();
            table_.create( "tables/asterism_line_hip.tsv" );
        }
        return table_;
    }

    public class Data
    {
        public string id_;      // id
        public string shortName_;      // 星座略名
        public int startHipId_;    // 開始点星ヒッパルコスId
        public int endHipId_;     // 終点星ヒッパルコスId
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
        d.shortName_ = values[ "shortName" ].sVal_;
        d.startHipId_ = values[ "startHipId" ].iVal_;
        d.endHipId_ = values[ "endHipId" ].iVal_;

        data_[ dataNum_ ] = d;
        if ( hipIds_ .ContainsKey( d.shortName_ ) == false ) {
            hipIds_[ d.shortName_ ] = new HashSet<int>();
        }
        hipIds_[ d.shortName_ ].Add( d.startHipId_ );
        hipIds_[ d.shortName_ ].Add( d.endHipId_ );
        
        if ( astLines_.ContainsKey( d.shortName_ ) == false ) {
            astLines_[ d.shortName_ ] = new List<Data>();
        }
        astLines_[ d.shortName_ ].Add( d );
        dataNum_++;
    }

    // データを取得
    public Data getData( int index )
    {
        if ( index >= data_.Length )
            return null;
        return data_[ index ];
    }

    // 星座略名から恒星のヒッパルコスId群を取得
    public List<int> getStarHipIndicesFromShortName( string shortName )
    {
        if ( hipIds_.ContainsKey( shortName ) == false )
            return null;
        var list = new List<int>();
        foreach( var hipId in hipIds_[ shortName ] ) {
            list.Add( hipId );
        }
        return list;
    }

    // 星座略名から恒星間ラインヒッパルコスIdペアを取得
    public List< Data > getLinesFromShortName( string shortName )
    {
        if ( astLines_.ContainsKey( shortName ) == false )
            return null;
        return astLines_[ shortName ];
    }

    static Table_asterism_line_hip table_;
    int dataNum_ = 0;
    Data[] data_;
    Dictionary<string, HashSet<int>> hipIds_ = new Dictionary<string, HashSet<int>>();  // key: shortName
    Dictionary<string, List<Data>> astLines_ = new Dictionary<string, List<Data>>();  // 星座別ライン群 key: shortName
}