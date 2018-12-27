using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キューブデータ
public class CubeData {

    public CubeData( int n )
    {
        n_ = n;
        faces_ = new FaceType[ 6, n * n ];
        rotGroup_ = new List<Vector3Int>[ 3, n_ ];   // [ axis, face ]
        resetFaces();
        createRotateGroup();
    }

    // フェイスを取得
    public FaceType[,] getFaces()
    {
        return faces_;
    }

    // 全フェイスをリセット
    public void resetFaces()
    {
        for ( int idx = 0; idx < n_ * n_; ++idx ) {
            faces_[ ( int )FaceType.FaceType_Left, idx ] = FaceType.FaceType_Left;
            faces_[ ( int )FaceType.FaceType_Right, idx ] = FaceType.FaceType_Right;
            faces_[ ( int )FaceType.FaceType_Down, idx ] = FaceType.FaceType_Down;
            faces_[ ( int )FaceType.FaceType_Up, idx ] = FaceType.FaceType_Up;
            faces_[ ( int )FaceType.FaceType_Front, idx ] = FaceType.FaceType_Front;
            faces_[ ( int )FaceType.FaceType_Back, idx ] = FaceType.FaceType_Back;
        }
    }

    // ピース面座標ハッシュをピース面座標に変換
    public Vector3Int convHashToCoord( uint hash )
    {
        // 9bitずつ分割
        return new Vector3Int(
            ( int )( hash & 0x1ff),
            ( int )( ( hash >> 9 ) & 0x1ff ),
            ( int )( ( hash >> 18 ) & 0x1ff )
        );
    }

    // ピース面座標をFaceTypeとフェイス面座標に変換
    public void convCoordToFaceTypeAndFaceCoord( Vector3Int coord, out FaceType faceType, out Vector2Int faceCoord )
    {
        // coord -> FaceType + faceCoord
        //  FacyTypeはcoord内で0もしくは2*nの成分値とその座標軸から判断
        //  ex) ( x, 0, z ) -> y0はDown
        //  残った成分からfaceCoordへ
        //  ex) ( x, z ) -> D( ( x - 1 ) / 2, ( z - 1 ) / 2 )
        int n2 = n_ * 2;
        faceType = FaceType.FaceType_None;
        var v2 = new Vector2Int();

        if ( coord.x == 0 ) {
            // ( 0, y, z ) -> ( ( 2n - z - 1 ) / 2, ( y - 1 ) / 2 )
            faceType = FaceType.FaceType_Left;
            v2.x = ( n2 - coord.z - 1 ) / 2;
            v2.y = ( coord.y - 1 ) / 2;
        } else if ( coord.x == n2 ) {
            // ( n2, y, z ) -> ( ( z - 1 ) / 2, ( y - 1 ) / 2 )
            faceType = FaceType.FaceType_Right;
            v2.x = ( coord.z - 1 ) / 2;
            v2.y = ( coord.y - 1 ) / 2;
        } else if ( coord.y == 0 ) {
            // ( x, n2, z ) -> ( ( x - 1 ) / 2, ( n2 - z - 1 ) / 2 )
            faceType = FaceType.FaceType_Down;
            v2.x = ( coord.x - 1 ) / 2;
            v2.y = ( n2 - coord.z - 1 ) / 2;
        } else if ( coord.y == n2 ) {
            // ( x, 0, z ) -> ( ( x - 1 ) / 2, ( z - 1 ) / 2 )
            faceType = FaceType.FaceType_Up;
            v2.x = ( coord.x - 1 ) / 2;
            v2.y = ( coord.z - 1 ) / 2;
        } else if ( coord.z == 0 ) {
            // ( x, y, 0 ) -> ( ( x - 1 ) / 2, ( y - 1 ) / 2 )
            faceType = FaceType.FaceType_Front;
            v2.x = ( coord.x - 1 ) / 2;
            v2.y = ( coord.y - 1 ) / 2;
        } else if ( coord.z == n2 ) {
            // ( x, y, 2n ) -> ( x - 1 ) / 2, ( y - 1 ) / 2 )
            faceType = FaceType.FaceType_Back;
            v2.x = ( coord.x - 1 ) / 2;
            v2.y = ( coord.y - 1 ) / 2;
        }
        faceCoord = v2;
    }

    // ピース面座標をFaceTypeとFaceインデックスに変換
    public void convCoordToFaceTypeAndIndex( Vector3Int coord, out FaceType outFaceType, out int outIndex )
    {
        // coord -> FaceType + faceCoord
        Vector2Int faceCoord;
        convCoordToFaceTypeAndFaceCoord( coord, out outFaceType, out faceCoord );

        // faceCoord -> faceIndex
        outIndex = faceCoord.y * n_ + faceCoord.x;
    }

    // ピース面座標ハッシュから所属するFaceTypeとFaceインデックスを算出
    public void convHashToFaceTypeAndIndex( uint hash, out FaceType outFaceType, out int outIndex )
    {
        // hash -> ( ix, iy, iz )
        var coord = convHashToCoord( hash );

        // coord -> FaceType + faceCoord
        // faceCoord -> faceIndex
        convCoordToFaceTypeAndIndex( coord, out outFaceType, out outIndex );
    }

    // 回転
    public void onRotation( AxisType axis, int[] colIndices, CubeRotationType rotType )
    {
        HashSet<int> colHash = new HashSet<int>();
        List<int> ary = new List<int>();
        foreach ( var c in colIndices ) {
            if ( c >= 0 && c < n_ && colHash.Contains( c ) == false ) {
                ary.Add( c );
                colHash.Add( c );
            }
        }

        // 回転軸とcolIndicesに対応したCoordを生成
        foreach ( var c in ary ) {
            List<Vector3Int> coords = getRotateGroup( axis, c );

            // 指定軸で回転
            List<Vector3Int> transCoords = rotateCoords( axis, rotType, coords );

            // coordsとtransCoordsの対応からfaces_を更新
            FaceType[] preFaceTypes = new FaceType[ transCoords.Count ];
            FaceType[] transFaceTypes = new FaceType[ transCoords.Count ];
            int[] transIndices = new int[ transCoords.Count ];
            for ( int i = 0; i < transCoords.Count; ++i ) {
                FaceType preFaceType;
                int preIndex;
                convCoordToFaceTypeAndIndex( coords[ i ], out preFaceType, out preIndex );
                preFaceTypes[ i ] = faces_[ ( int )preFaceType, preIndex ];
                convCoordToFaceTypeAndIndex( transCoords[ i ], out transFaceTypes[ i ], out transIndices[ i ] );
            }
            for ( int i = 0; i < transCoords.Count; ++i ) {
                faces_[ ( int )transFaceTypes[ i ], transIndices[ i ] ] = preFaceTypes[ i ];
            }
        }
    }

    // キューブが揃っている？
    public bool isComplete()
    {
        for ( int i = 0; i < 6; ++i ) {
            FaceType faceColor = faces_[ i, 0 ];
            for ( int idx = 1; idx < n_ * n_; ++idx ) {
                if ( faceColor != faces_[ i, idx ] )
                    return false;
            }
        }
        return true;
    }

    // 指定軸で回転
    List<Vector3Int> rotateCoords( AxisType axis, CubeRotationType rotTyep, List< Vector3Int > coords )
    {
        int[] cos = new int[ ( int )CubeRotationType.CRT_NUM ] {
            1, 0, -1, 0, 0, -1, 0
        };
        int[] sin = new int[ ( int )CubeRotationType.CRT_NUM ] {
            0, 1, 0, -1, -1, 0, 1
        };

        int e = 0;
        switch ( rotTyep ) {
            case CubeRotationType.CRT_0: e = 0; break;
            case CubeRotationType.CRT_Plus_90: e = 1; break;
            case CubeRotationType.CRT_Plus_180: e = 2; break;
            case CubeRotationType.CRT_Plus_270: e = 3; break;
            case CubeRotationType.CRT_Minus_90: e = 4; break;
            case CubeRotationType.CRT_Minus_180: e = 5; break;
            case CubeRotationType.CRT_Minus_270: e = 6; break;
        }
        int c = cos[ e ];
        int s = sin[ e ];
        var ret = new List<Vector3Int>();
        Vector3Int ofs = new Vector3Int( n_, n_, n_ );
        if ( axis == AxisType.AxisType_X ) {
            foreach ( var p in coords ) {
                Vector3Int p0 = p - ofs;
                int x = p0.x;
                int y =  p0.y * c + p0.z * s;
                int z = -p0.y * s + p0.z * c;
                p0.x = x;
                p0.y = y;
                p0.z = z;
                ret.Add( p0 + ofs );
            }
        } else if ( axis == AxisType.AxisType_Y) {
            foreach ( var p in coords ) {
                Vector3Int p0 = p - ofs;
                int y = p0.y;
                int x = p0.x * c - p0.z * s;
                int z = p0.x * s + p0.z * c;
                p0.x = x;
                p0.y = y;
                p0.z = z;
                ret.Add( p0 + ofs );
            }
        } else if ( axis == AxisType.AxisType_Z ) {
            foreach ( var p in coords ) {
                Vector3Int p0 = p - ofs;
                int z = p0.z;
                int x =  p0.x * c + p0.y * s;
                int y = -p0.x * s + p0.y * c;
                p0.x = x;
                p0.y = y;
                p0.z = z;
                ret.Add( p0 + ofs );
            }
        }
        return ret;
    }

    // 回転軸とcolIndicesに対応したCoordを取得
    List<Vector3Int> getRotateGroup( AxisType axis, int colIndex )
    {
        return rotGroup_[ ( int )axis, colIndex ];
    }

    // 回転軸とcolIndicesに対応したCoordを生成
    void createRotateGroup()
    {
        int n2 = n_ * 2;
        for ( int i = 0; i < n_; ++i ) {
            // X軸i列目
            int x2 = i * 2 + 1;
            List<Vector3Int> groupX = new List<Vector3Int>();
            for ( int j = 0; j < n_; ++j ) {
                groupX.Add( new Vector3Int( x2, j * 2 + 1, 0 ) );    // Front
                groupX.Add( new Vector3Int( x2, n2, j * 2 + 1 ) );   // Up
                groupX.Add( new Vector3Int( x2, j * 2 + 1, n2 ) );   // Back
                groupX.Add( new Vector3Int( x2, 0, j * 2 + 1 ) );    // Down
            }
            if ( i == 0 ) {
                // Left
                for ( int z = 0; z < n_; ++z ) {
                    for ( int y = 0; y < n_; ++y ) {
                        groupX.Add( new Vector3Int( 0, y * 2 + 1, z * 2 + 1 ) );    // Left
                    }
                }
            } else if ( i == n_ - 1 ) {
                // Right
                for ( int z = 0; z < n_; ++z ) {
                    for ( int y = 0; y < n_; ++y ) {
                        groupX.Add( new Vector3Int( n2, y * 2 + 1, z * 2 + 1 ) );    // Left
                    }
                }
            }
            rotGroup_[ ( int )AxisType.AxisType_X, i ] = groupX;

            // Y軸i列目
            int y2 = i * 2 + 1;
            List<Vector3Int> groupY = new List<Vector3Int>();
            for ( int j = 0; j < n_; ++j ) {
                groupY.Add( new Vector3Int( j * 2 + 1, y2, 0 ) );    // Front
                groupY.Add( new Vector3Int( n2, y2, j * 2 + 1 ) );   // Right
                groupY.Add( new Vector3Int( j * 2 + 1, y2, n2 ) );   // Back
                groupY.Add( new Vector3Int( 0, y2, j * 2 + 1 ) );    // Left
            }
            if ( i == 0 ) {
                // Down
                for ( int z = 0; z < n_; ++z ) {
                    for ( int x = 0; x < n_; ++x ) {
                        groupY.Add( new Vector3Int( x * 2 + 1, 0, z * 2 + 1 ) );    // Down
                    }
                }
            } else if ( i == n_ - 1 ) {
                // Up
                for ( int z = 0; z < n_; ++z ) {
                    for ( int x = 0; x < n_; ++x ) {
                        groupY.Add( new Vector3Int( x * 2 + 1, n2, z * 2 + 1 ) );    // Up
                    }
                }
            }
            rotGroup_[ ( int )AxisType.AxisType_Y, i ] = groupY;

            // Z軸i列目
            int z2 = i * 2 + 1;
            List<Vector3Int> groupZ = new List<Vector3Int>();
            for ( int j = 0; j < n_; ++j ) {
                groupZ.Add( new Vector3Int( j * 2 + 1, n2, z2 ) );   // Up
                groupZ.Add( new Vector3Int( n2, j * 2 + 1, z2 ) );   // Right
                groupZ.Add( new Vector3Int( j * 2 + 1, 0, z2 ) );    // Down
                groupZ.Add( new Vector3Int( 0, j * 2 + 1, z2 ) );    // Left
            }
            if ( i == 0 ) {
                // Front
                for ( int y = 0; y < n_; ++y ) {
                    for ( int x = 0; x < n_; ++x ) {
                        groupZ.Add( new Vector3Int( x * 2 + 1, y * 2 + 1, 0 ) );    // Front
                    }
                }
            } else if ( i == n_ - 1 ) {
                // Back
                for ( int y = 0; y < n_; ++y ) {
                    for ( int x = 0; x < n_; ++x ) {
                        groupZ.Add( new Vector3Int( x * 2 + 1, y * 2 + 1, n2 ) );    // Back
                    }
                }
            }
            rotGroup_[ ( int )AxisType.AxisType_Z, i ] = groupZ;
        }
    }

    List<Vector3Int>[,] rotGroup_;   // [ axis, face ]
    FaceType[,] faces_;   // 現在のFaceカラー [FaceType, FaceIndex]
    int n_ = 3;

}
