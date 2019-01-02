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
            rotType_ = rotType;
            colIndices_.Add( colIndex );
        }
        public RotateUnit(FaceType face, CubeRotationType rotType, int[] colIndices)
        {
            face_ = face;
            rotType_ = rotType;
            colIndices_.AddRange( colIndices );
        }
        public string getRotateCode()
        {
            string[] names = new string[ 6 ] { "L", "R", "D", "U", "F", "B" };
            bool bFoward = true;
            if ( face_ == FaceType.FaceType_Right || face_ == FaceType.FaceType_Up || face_ == FaceType.FaceType_Back )
                bFoward = false;
            string indicesStr = "(";
            for ( int i = 0; i < colIndices_.Count; ++i ) {
                indicesStr += colIndices_[ i ] + 1;
                if ( i + 1 != colIndices_.Count ) {
                    indicesStr += ":";
                }
            }
            indicesStr += ")";
            string[] rotNamesFoward  = new string[ 7 ] { "", "", "w", "3", "'", "w'", "3'" };
            string[] rotNamesInverse = new string[ 7 ] { "", "'", "w'", "3'", "", "w", "3" };
            int rotIndex = CubeRotateUtil.Util.getRotateIndex( rotType_ );
            return names[ (int)face_ ] + indicesStr + ( bFoward ? rotNamesFoward[ rotIndex ] : rotNamesInverse[ rotIndex ] );
        }
        public int[] getInvColIndices( int n )
        {
            var indices = new int[ colIndices_.Count ];
            for ( int i = 0; i < colIndices_.Count; ++i ) {
                indices[ i ] = n - 1 - colIndices_[ i ];
            }
            return indices;
        }
        public AxisType getRotateAxis()
        {
            switch ( face_ ) {
                case FaceType.FaceType_Left:
                case FaceType.FaceType_Right:
                    return AxisType.AxisType_X;
                case FaceType.FaceType_Down:
                case FaceType.FaceType_Up:
                    return AxisType.AxisType_Y;
                case FaceType.FaceType_Front:
                case FaceType.FaceType_Back:
                    return AxisType.AxisType_Z;
            }
            return AxisType.AxisType_X;
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
                cube.setFaceColor( ( FaceType )i, idx, ( FaceType )faces[ idx ] );
            }
        }

        return true;
    }

    // 軸回転情報からRotateUnitを生成
    public static RotateUnit convAxisRotateToRotUnit( int n, AxisType axis, CubeRotationType rotType, int[] colIndices )
    {
        FaceType face = FaceType.FaceType_None;
        int[] invColIndices = new int[ colIndices .Length ];
        for ( int i = 0; i < colIndices.Length; ++i ) {
            invColIndices[ i ] = n - colIndices[ i ] - 1;
        }
        bool bSingle = ( colIndices.Length == 1 );
        bool useFoward = ( bSingle == true && colIndices[ 0 ] == 0 );   // Idx0の1列のみ順回転
        switch( axis ) {
            case AxisType.AxisType_X:
                face = useFoward ? FaceType.FaceType_Left : FaceType.FaceType_Right;
                break;
            case AxisType.AxisType_Y:
                face = useFoward ? FaceType.FaceType_Down : FaceType.FaceType_Up;
                break;
            case AxisType.AxisType_Z:
                face = useFoward ? FaceType.FaceType_Front : FaceType.FaceType_Back;
                break;
        }
        return new RotateUnit( face, rotType, useFoward ? colIndices : invColIndices );
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
            for ( int idx = 0; idx < faces.GetLength( 1 ); ++idx ) {
                FaceType f = faces[ i, idx ];
                flist.Add( ( int )f );
            }
            pieces[ pieceNames[ i ] ] = flist;
        }
        root[ "Pieces" ] = pieces;

        var solveList = new List<string>();
        foreach( var s in solve ) {
            solveList.Add( s.getRotateCode() );
        }

        root[ "Solve" ] = solveList;

        return MiniJSON.Json.Serialize( root );
    }

    bool bLoaded_ = false;
    int n_ = 3;
    List<int>[] list_ = new List<int>[ 6 ];
    List<string> solve_ = null;
}
