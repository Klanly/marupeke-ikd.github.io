using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 練習データ管理人

public class CubePracticeData {

    public class RotateUnit
    {
        public RotateUnit(FaceType face, CubeRotationType rotType)
        {
            face_ = face;
            rotType_ = rotType;
            colIndices_.Add( 1 );
        }
        public RotateUnit(FaceType face, CubeRotationType rotType, int colIndex )
        {
            face_ = face;
            rotType = rotType_;
            colIndices_.Add( colIndex );
        }
        public FaceType face_;             // 回転フェイス
        public CubeRotationType rotType_;  // 回転タイプ
        public List<int> colIndices_ = new List<int>();      // 回転列
    }

    // 練習データをロード
    public void load( string dataName, System.Action< bool > callback )
    {
        System.Action<bool> _callback = callback;
        ResourceLoader.getInstance().loadAsync<TextAsset>( dataName, (_res, _obj) => {
            if ( _res == false ) {
                _callback( false );
                return;
            }

            // 練習データはJSON形式
            string json = _obj.text;
            var obj = MiniJSON.Json.Deserialize( json );
            var parameters = obj as Dictionary<string, object>;
            if ( parameters == null ) {
                return;
            }

            n_ = ToVal.Conv.toInt( parameters[ "N" ].ToString(), 3 );
            var pieces_ = parameters[ "Pieces" ] as Dictionary<string, object>;
            if ( pieces_ == null )
                return;

            string[] faceNames = new string[ 6 ] {
            "Left", "Right", "Down", "Up", "Front", "Back"
        };
            list_ = new List<int>[ 6 ];
            for ( int i = 0; i < 6; ++i ) {
                var l = pieces_[ faceNames[ i ] ] as List<object>;
                if ( l == null )
                    return;
                list_[ i ] = new List<int>();
                for ( int j = 0; j < l.Count; ++j ) {
                    list_[ i ].Add( ToVal.Conv.toInt( l[ j ].ToString(), 0 ) );
                }
            }
            var solve = parameters[ "Solve" ] as List<object>;
            if ( solve == null )
                return;
            solve_ = new List<string>();
            for ( int i = 0; i < solve.Count; ++i ) {
                solve_.Add( solve[ i ].ToString() );
            }

            bLoaded_ = true;

            _callback( true );
        } );
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

    // 練習データをCubeから作成
    public static string createDataStrFromCube( Cube cube, List< RotateUnit > solve )
    {
        var root = new Dictionary<string, object>();
        root[ "N" ] = cube.getN();
        var pieces = new Dictionary<string, object>();
        string[] pieceNames = new string[ 6 ] {
            "Left", "Right", "Down", "Up", "Front", "Back"
        };

        var faces = cube.getFaces();
        for ( int i = 0; i < 6; ++i ) {
            var flist = new List<int>();
            for ( int idx = 0; idx < faces.GetLength( 0 ); ++idx ) {
                FaceType f = faces[ i, idx ];
                flist.Add( ( int )f );
            }
            pieces[ pieceNames[ i ] ] = flist;
        }
        root[ "Pieces" ] = pieces;

        string[] faceNames = new string[ 6 ] {
            "L", "R", "D", "U", "F", "B"
        };
        var solveList = new List<string>();
        foreach( var s in solve ) {
            string name = faceNames[ ( int )s.face_ ];
            string indices = name + "(";
            for ( int c = 0; c < s.colIndices_.Count; ++c ) {
                indices += s.colIndices_[ c ];
                if ( c + 1 < s.colIndices_.Count )
                    indices += ":";
            }
            indices += ")";
            switch ( s.rotType_ ) {
                case CubeRotationType.CRT_Plus_90: indices += "'"; break;
                case CubeRotationType.CRT_Plus_180: indices += "w'"; break;
                case CubeRotationType.CRT_Plus_270: indices += "3'"; break;
                case CubeRotationType.CRT_Minus_90: indices += ""; break;
                case CubeRotationType.CRT_Minus_180: indices += "w"; break;
                case CubeRotationType.CRT_Minus_270: indices += "3"; break;
            }
            solveList.Add( indices );
        }

        root[ "Solve" ] = solveList;

        return MiniJSON.Json.Serialize( root );
    }

    bool bLoaded_ = false;
    int n_ = 3;
    List<int>[] list_ = new List<int>[ 6 ];
    List<string> solve_ = null;
}
