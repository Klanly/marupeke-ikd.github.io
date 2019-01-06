using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 練習データ管理人

public class CubePracticeData {

    public enum RotDir : int
    {
        RotDir_Clockwise90 = 0,         // 時計回り90度
        RotDir_Clockwise180 = 1,        // 時計回り180度
        RotDir_CounterClockwise90 = 2,  // 反時計回り90度
        RotDir_CounterClockwise180 = 3  // 反時計回り180度
    }

    public class RotateUnit
    {
        private RotateUnit()
        {

        }

        public RotateUnit(FaceType face, RotDir rotDir)
        {
            face_ = face;
            rotDir_ = rotDir;
            colIndices_.Add( 1 );
        }
        public RotateUnit(FaceType face, RotDir rotDir, int colIndex)
        {
            face_ = face;
            rotDir_ = rotDir;
            colIndices_.Add( colIndex );
        }
        public RotateUnit(FaceType face, RotDir rotDir, int[] colIndices)
        {
            face_ = face;
            rotDir_ = rotDir;
            colIndices_.AddRange( colIndices );
        }

        // 回転コードを取得
        public string getRotateCode()
        {
            string[] names = new string[ 6 ] { "L", "R", "D", "U", "F", "B" };
            string indicesStr = "(";
            for ( int i = 0; i < colIndices_.Count; ++i ) {
                indicesStr += colIndices_[ i ] + 1;
                if ( i + 1 != colIndices_.Count ) {
                    indicesStr += ":";
                }
            }
            indicesStr += ")";
            string[] rotNames = new string[ 4 ] { "", "2", "'", "2'" };
            return names[ ( int )face_ ] + indicesStr + rotNames[ (int)rotDir_ ];
        }

        // 回転方向シンボルマークを取得
        public string getRotDirSymbolMark()
        {
            switch ( rotDir_ ) {
                case RotDir.RotDir_Clockwise90: return "";
                case RotDir.RotDir_Clockwise180: return "2";
                case RotDir.RotDir_CounterClockwise90: return "'";
                case RotDir.RotDir_CounterClockwise180: return "2'";
            }
            return "";
        }

        // 回転タイプを取得
        public CubeRotationType getRotType()
        {
            bool bLDF = ( face_ == FaceType.FaceType_Left || face_ == FaceType.FaceType_Down || face_ == FaceType.FaceType_Front );
            if ( bLDF == true ) {
                switch ( rotDir_ ) {
                    case RotDir.RotDir_Clockwise90: return CubeRotationType.CRT_Plus_90;
                    case RotDir.RotDir_Clockwise180: return CubeRotationType.CRT_Plus_180;
                    case RotDir.RotDir_CounterClockwise90: return CubeRotationType.CRT_Minus_90;
                    case RotDir.RotDir_CounterClockwise180: return CubeRotationType.CRT_Minus_180;
                }
            } else {
                switch ( rotDir_ ) {
                    case RotDir.RotDir_Clockwise90: return CubeRotationType.CRT_Minus_90;
                    case RotDir.RotDir_Clockwise180: return CubeRotationType.CRT_Minus_180;
                    case RotDir.RotDir_CounterClockwise90: return CubeRotationType.CRT_Plus_90;
                    case RotDir.RotDir_CounterClockwise180: return CubeRotationType.CRT_Plus_180;
                }
            }
            return CubeRotationType.CRT_0;
        }

        // 反転インデックスを取得
        public int[] getInvColIndices( int n )
        {
            var indices = new int[ colIndices_.Count ];
            for ( int i = 0; i < colIndices_.Count; ++i ) {
                indices[ i ] = n - 1 - colIndices_[ i ];
            }
            return indices;
        }

        // 軸・回転タイプ・インデックスのセットを取得
        public void getAxisRotColindicesSet(int n, out AxisType axis, out CubeRotationType rotType, out int[] colIndices)
        {
            // 登録されているフェイスを基準とする
            // LDF: Clockwise -> Plus  : CounterClockwise -> Minus
            // RUB: Clockwise -> Minus : CounterClockwise -> Plus
            axis = getRotateAxis();
            rotType = getRotType();
            bool bLDF = ( face_ == FaceType.FaceType_Left || face_ == FaceType.FaceType_Down || face_ == FaceType.FaceType_Front );
            colIndices = ( bLDF ? colIndices_.ToArray() : getInvColIndices( n ) );
        }

        // 回転軸を取得
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

        // 反転するRotUnitを取得
        public RotateUnit getInvRotUnit()
        {
            var ru = new RotateUnit();
            ru.face_ = face_;
            ru.rotDir_ = getInvRotDir( rotDir_ );
            for( int i = 0; i < colIndices_.Count; ++i ) {
                ru.colIndices_.Add( colIndices_[ i ] );
            }
            return ru;
        }

        // 反転する回転方向を取得
        public static RotDir getInvRotDir( RotDir rotDir )
        {
            switch ( rotDir ) {
                case RotDir.RotDir_Clockwise90: return RotDir.RotDir_CounterClockwise90;
                case RotDir.RotDir_Clockwise180: return RotDir.RotDir_CounterClockwise180;
                case RotDir.RotDir_CounterClockwise90: return RotDir.RotDir_Clockwise90;
                case RotDir.RotDir_CounterClockwise180: return RotDir.RotDir_Clockwise180;
            }
            return RotDir.RotDir_Clockwise90;
        }

        public bool isCorrect(int n, AxisType axis, CubeRotationType rotType, int[] colIndices) {
            AxisType myAxis;
            CubeRotationType myRotType;
            int[] myColIndices;
            getAxisRotColindicesSet( n, out myAxis, out myRotType, out myColIndices );
            if ( axis != myAxis )
                return false;
            if ( myRotType != rotType )
                return false;
            foreach ( int idx in colIndices ) {
                bool isDetect = false;
                foreach ( int myIdx in myColIndices ) {
                    if ( myIdx == idx ) {
                        isDetect = true;
                        break;
                    }
                }
                if ( isDetect == false )
                    return false;
            }
            foreach ( int myIdx in myColIndices ) {
                bool isDetect = false;
                foreach ( int idx in colIndices ) {
                    if ( myIdx == idx ) {
                        isDetect = true;
                        break;
                    }
                }
                if ( isDetect == false )
                    return false;
            }

            // 同じと判断
            return true;
        }

        public FaceType face_;  // 回転フェイス
        public RotDir rotDir_;  // 回転方向
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

    // RotateUnitリストを取得
    public List< RotateUnit > getRotateList()
    {
        var faceMap = new Dictionary<string, FaceType> {
            { "L", FaceType.FaceType_Left },
            { "R", FaceType.FaceType_Right },
            { "D", FaceType.FaceType_Down },
            { "U", FaceType.FaceType_Up },
            { "F", FaceType.FaceType_Front },
            { "B", FaceType.FaceType_Back },
        };
        var rotTypeMap = new Dictionary<string, RotDir> {
            { "", RotDir.RotDir_Clockwise90 },
            { "2", RotDir.RotDir_Clockwise180 },
            { "3", RotDir.RotDir_CounterClockwise90 },
            { "'", RotDir.RotDir_CounterClockwise90 },
            { "2'", RotDir.RotDir_CounterClockwise180 },
            { "3'", RotDir.RotDir_Clockwise90 },
        };
        var list = new List<RotateUnit>();
        foreach ( var s in solve_ ) {
            var colIndices = new List<int>();
            // R(0,1)' -> [R] [0,1] [']
            string[] strs = System.Text.RegularExpressions.Regex.Split( s, "[(]|[)]" );
            FaceType face = faceMap[ strs[ 0 ] ];
            RotDir rotDir = rotTypeMap[ strs[ 2 ] ];
            string[] cols = strs[ 1 ].Split( ',' );
            for ( int i = 0; i < cols.Length; ++i ) {
                colIndices.Add( ToVal.Conv.toInt( cols[ i ], 0 ) - 1 );
            }
            list.Add( new RotateUnit( face, rotDir, colIndices.ToArray() ) );
        }
        return list;
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
        // 軸回転情報はすべてLDFを基準とする(colIndicesを変更しない);
        FaceType face = FaceType.FaceType_None;
        switch( axis ) {
            case AxisType.AxisType_X:
                face = FaceType.FaceType_Left;
                break;
            case AxisType.AxisType_Y:
                face = FaceType.FaceType_Down;
                break;
            case AxisType.AxisType_Z:
                face = FaceType.FaceType_Front;
                break;
        }
        RotDir rotDir = RotDir.RotDir_Clockwise90;
        switch ( rotType ) {
            case CubeRotationType.CRT_Plus_90: rotDir = RotDir.RotDir_Clockwise90; break;
            case CubeRotationType.CRT_Plus_180: rotDir = RotDir.RotDir_Clockwise180; break;
            case CubeRotationType.CRT_Plus_270: rotDir = RotDir.RotDir_CounterClockwise90; break;
            case CubeRotationType.CRT_Minus_90: rotDir = RotDir.RotDir_CounterClockwise90; break;
            case CubeRotationType.CRT_Minus_180: rotDir = RotDir.RotDir_CounterClockwise180; break;
            case CubeRotationType.CRT_Minus_270: rotDir = RotDir.RotDir_Clockwise90; break;
        }
        return new RotateUnit( face, rotDir, colIndices );
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
