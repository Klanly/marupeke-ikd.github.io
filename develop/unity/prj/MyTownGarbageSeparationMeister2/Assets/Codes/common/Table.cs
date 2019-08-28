using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// テーブルアクセスクラス
//
//  sheetconverterが出力したtsvデータを読み込んでテーブル化
public class Table {

    // 一時値
    protected class Val
    {
        public int iVal_;
        public float fVal_;
        public string sVal_;
    }

    // テーブルを同期読みで作成
    //  tableName: tsvファイル名
    protected bool create( string tableName )
    {
        var text = ResourceLoader.getInstance().loadSync<TextAsset>( tableName );
        if ( text == null ) {
            return false;
        }

        string[] lines = text.text.Split( '\n' );
        if ( lines.Length < 4 ) {
            // パラメータ数、データ数、パラメータ、型が揃っていない
            return false;
        }

        var paramNum = ToVal.Conv.toInt( lines[ 0 ], 0 );
        var dataNum = ToVal.Conv.toInt( lines[ 1 ], 0 );
        string[] parameters = lines[ 2 ].Split( '\t' );
        string[] types = lines[ 3 ].Split( '\t' );
        parameters[ parameters.Length - 1 ] = parameters[ parameters.Length - 1 ].Replace( "\r", "" );
        types[ types.Length - 1 ] = types[ types.Length - 1 ].Replace( "\r", "" );
        if (
            paramNum == 0 ||
            parameters.Length != paramNum ||
            types.Length != paramNum ||
            lines.Length != dataNum + 4
        ) {
            // パラメータが設定されていない
            // データ数が合っていない
            return false;
        }

        // データタイプに沿ったインデックスをセット
        Dictionary<string, Val> dict_ = new Dictionary<string, Val>();
        int[] typeIdx = new int[ types.Length ];
        for ( int i = 0; i < types.Length; ++i ) {
            switch ( types[ i ] ) {
                case "int": typeIdx[ i ] = 0; break;
                case "float": typeIdx[ i ] = 1; break;
                case "string": typeIdx[ i ] = 2; break;
                default: typeIdx[ i ] = 0; break;
            }
            dict_.Add( parameters[ i ], new Val() );
        }

        // データ格納前コール
        preStore( dataNum );

        for ( int i = 4; i < lines.Length; ++i ) {
            string[] datas = lines[ i ].Split( '\t' );
            if ( datas.Length != paramNum )
                return false;
            for ( int j = 0; j < datas.Length; ++j ) {
                switch ( typeIdx[ j ] ) {
                    case 0: dict_[ parameters[ j ] ].iVal_ = ToVal.Conv.toInt( datas[ j ], 0 ); break;
                    case 1: dict_[ parameters[ j ] ].fVal_ = ToVal.Conv.toFloat( datas[ j ], 0 ); break;
                    case 2: dict_[ parameters[ j ] ].sVal_ = datas[ j ]; break;
                }
            }

            // 値を格納者に渡す
            storeData( dict_ );
        }

        // データ格納後コール
        postStore();

        return true;
    }

    // データ格納前コール
    //  派生クラスで必要に応じて使用
    protected virtual void preStore( int dataNum )
    {
    }

    // データ格納後コール
    //  派生クラスで必要に応じて使用
    protected virtual void postStore()
    {
    }

    // 1レコードを格納
    //  派生クラスで具体的な格納を行う
    protected virtual void storeData( Dictionary<string, Val> values )
    {
    }
}
