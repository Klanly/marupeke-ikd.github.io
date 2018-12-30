using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 練習データ管理人

public class CubePracticeData {

    // 練習データをロード
    public bool load( string dataName )
    {
        // TEST
        string json = "{\"N\" : 3,\"Pieces\" : {\"Left\" : [2,2,2,2,2,2,2,2,2],\"Right\" : [2,2,2,2,2,2,2,2,2],\"Down\" : [2,2,2,2,2,2,2,2,2],\"Up\" : [2,2,2,2,2,2,2,2,2],\"Front\" : [2,2,2,2,2,2,2,2,2],\"Back\" : [2,2,2,2,2,2,2,2,2]},\"Solve\" : [ \"R\", \"R'\", \"L\", \"L'\", \"D\", \"D'\", \"U\", \"U'\", \"F\", \"F'\", \"B\", \"B'\" ]}";
        var obj = MiniJSON.Json.Deserialize( json );

        // 練習データはJSON形式
        // var obj = MiniJSON.Json.Deserialize( "AssetBundles/PracticeDataBundles/" + dataName + ".json.bytes" );
        var parameters = obj as Dictionary<string, object>;
        if ( parameters == null ) {
            return false;
        }

        n_ = ToVal.Conv.toInt( parameters[ "N" ].ToString(), 3 );
        var pieces_ = parameters[ "Pieces" ] as Dictionary<string, object>;
        if ( pieces_ == null )
            return false;

        string[] faceNames = new string[ 6 ] {
            "Left", "Right", "Down", "Up", "Front", "Back"
        };
        list_ = new List<int>[ 6 ];
        for ( int i = 0; i < 6; ++i ) {
            var l = pieces_[ faceNames[ i ] ] as List< object >;
            if ( l == null )
                return false;
            list_[ i ] = new List<int>();
            for ( int j = 0; j < l.Count; ++j ) {
                list_[ i ].Add( ToVal.Conv.toInt( l[ j ].ToString(), 0 ) );
            }
        }
        var solve = parameters[ "Solve" ] as List<object>;
        if ( solve == null )
            return false;
        solve_ = new List<string>();
        for ( int i = 0; i < solve.Count; ++i ) {
            solve_.Add( solve[ i ].ToString() );
        }

        bLoaded_ = true;

        return true;
    }

    // Cube数を取得
    public int getN()
    {
        return n_;
    }

    // Cubeにピースのフェイス面を設定
    public bool setPiecesOnCube( Cube cube )
    {
        if ( bLoaded_ == false )
            return false;

        for ( int i = 0; i < 6; ++i ) {
            var faces = list_[ i ];
            for ( int idx = 0; idx < faces.Count; ++idx ) {
                cube.setPieceFace( (FaceType)i, idx, (FaceType)faces[ idx ] );
            }
        }

        return true;
    }

    bool bLoaded_ = false;
    int n_ = 3;
    List<int>[] list_ = new List<int>[ 6 ];
    List<string> solve_ = null;
}
