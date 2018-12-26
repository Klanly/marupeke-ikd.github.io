using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転メソッド
//  指定角度分回転させるが最後に回転角度をリセットしてカラーだけ変える
class RotationMethod
{
    public virtual RotationMethod update(float defDeg) { return null; }

    protected RotationMethod(int[] colIndices, CubeRotationType rotType, Cube cube)
    {
        cube_ = cube;
        rotType_ = rotType;
        aimRad_ = Mathf.Abs( (int)rotType ) * Mathf.Deg2Rad;
        if ( ( int )rotType < 0.0f )
            sign_ = -1.0f;
    }

    // 回転が終了したらピースフェイスを置き換え
    protected void replacePieceFaces(AxisType axis)
    {
        int n = cube_.getN();
        var pieceMap = cube_.getPieceMap();
        var infos = new List<NormalPiece.TransInfo>();
        foreach ( var p in pieces_ ) {
            // 回転先の情報を取得
            // イテレーション中にセット出来ないので取得のみ
            var coord = p.getCoord();
            infos.Add( p.calcRotateInfo( n, coord, axis, rotType_ ) );
        }
        // フェイス置き換え
        foreach ( var info in infos ) {
            uint hash = NormalPiece.convCoordToHash( info.transCoord_ );
            if ( pieceMap.ContainsKey( hash ) == true ) {
                var targetPiece = pieceMap[ NormalPiece.convCoordToHash( info.transCoord_ ) ];
                targetPiece.setFaceColors( info.transFaceType_ );
                targetPiece.resetRotate();    // ピースの姿勢をリセット
            }
        }
    }

    protected Cube cube_;
    protected CubeRotationType rotType_;
    protected List<NormalPiece> pieces_ = new List<NormalPiece>();
    protected float comRad_ = 0.0f;
    protected float aimRad_ = 0.0f;     // 目標角度
    protected float sign_ = 1.0f;
}

// X軸回転
class RotationMethod_AxisX : RotationMethod
{
    public RotationMethod_AxisX(int[] colIndices, CubeRotationType rotType, Cube cube) : base( colIndices, rotType, cube )
    {
        NormalPiece[,,] pieces = cube.getPieceList();
        foreach ( var x in colIndices ) {
            for ( int y = 0; y < pieces.GetLength( 1 ); ++y ) {
                for ( int z = 0; z < pieces.GetLength( 2 ); ++z ) {
                    if ( pieces[ x, y, z ] != null )
                        pieces_.Add( pieces[ x, y, z ] );
                }
            }
        }
        var cubeData = cube.getCubeData();
        cubeData.onRotation( AxisType.AxisType_X, colIndices, rotType );
    }

    public override RotationMethod update(float defDeg)
    {
        bool bFinish = false;
        float defRad = Mathf.Abs( defDeg ) * Mathf.Deg2Rad;
        if ( defRad + comRad_ >= aimRad_ ) {
            defRad = aimRad_ - comRad_;
            comRad_ = aimRad_;
            defDeg = defRad * Mathf.Rad2Deg;
            bFinish = true;
        } else {
            comRad_ += defRad;
        }
        Vector3 tmp = new Vector3();
        float c = Mathf.Cos( defRad * sign_ );
        float s = Mathf.Sin( defRad * sign_ );
        foreach ( var p in pieces_ ) {
            // ピース座標回転
            Vector3 pos = p.transform.localPosition;
            tmp.x = pos.x;
            tmp.y = pos.y * c + pos.z * s;
            tmp.z = -pos.y * s + pos.z * c;
            p.transform.localPosition = tmp;

            // ピースローカル回転
            p.transform.localRotation = Quaternion.Euler( -defDeg * sign_, 0.0f, 0.0f ) * p.transform.localRotation;
        }

        if ( bFinish == true )
            replacePieceFaces( AxisType.AxisType_X );

        return ( bFinish ? null : this );
    }
}

// Y軸回転
class RotationMethod_AxisY : RotationMethod
{
    public RotationMethod_AxisY(int[] colIndices, CubeRotationType rotType, Cube cube) : base( colIndices, rotType, cube )
    {
        NormalPiece[,,] pieces = cube.getPieceList();
        foreach ( var y in colIndices ) {
            for ( int x = 0; x < pieces.GetLength( 0 ); ++x ) {
                for ( int z = 0; z < pieces.GetLength( 2 ); ++z ) {
                    if ( pieces[ x, y, z ] != null )
                        pieces_.Add( pieces[ x, y, z ] );
                }
            }
        }
        var cubeData = cube.getCubeData();
        cubeData.onRotation( AxisType.AxisType_Y, colIndices, rotType );
    }

    public override RotationMethod update(float defDeg)
    {
        bool bFinish = false;
        float defRad = Mathf.Abs( defDeg ) * Mathf.Deg2Rad;
        if ( defRad + comRad_ >= aimRad_ ) {
            defRad = aimRad_ - comRad_;
            comRad_ = aimRad_;
            defDeg = defRad * Mathf.Rad2Deg;
            bFinish = true;
        } else {
            comRad_ += defRad;
        }
        Vector3 tmp = new Vector3();
        float c = Mathf.Cos( defRad * sign_ );
        float s = Mathf.Sin( defRad * sign_ );
        foreach ( var p in pieces_ ) {
            // ピース座標回転
            Vector3 pos = p.transform.localPosition;
            tmp.y = pos.y;
            tmp.x = pos.x * c - pos.z * s;
            tmp.z = pos.x * s + pos.z * c;
            p.transform.localPosition = tmp;

            // ピースローカル回転
            p.transform.localRotation = Quaternion.Euler( 0.0f, -defDeg * sign_, 0.0f ) * p.transform.localRotation;
        }

        if ( bFinish == true )
            replacePieceFaces( AxisType.AxisType_Y );

        return ( bFinish ? null : this );
    }
}

// Z軸回転
class RotationMethod_AxisZ : RotationMethod
{
    public RotationMethod_AxisZ(int[] colIndices, CubeRotationType rotType, Cube cube) : base( colIndices, rotType, cube )
    {
        NormalPiece[,,] pieces = cube.getPieceList();
        foreach ( var z in colIndices ) {
            for ( int x = 0; x < pieces.GetLength( 0 ); ++x ) {
                for ( int y = 0; y < pieces.GetLength( 1 ); ++y ) {
                    if ( pieces[ x, y, z ] != null )
                        pieces_.Add( pieces[ x, y, z ] );
                }
            }
        }
        var cubeData = cube.getCubeData();
        cubeData.onRotation( AxisType.AxisType_Z, colIndices, rotType );
    }

    public override RotationMethod update(float defDeg)
    {
        bool bFinish = false;
        float defRad = Mathf.Abs( defDeg ) * Mathf.Deg2Rad;
        if ( defRad + comRad_ >= aimRad_ ) {
            defRad = aimRad_ - comRad_;
            comRad_ = aimRad_;
            defDeg = defRad * Mathf.Rad2Deg;
            bFinish = true;
        } else {
            comRad_ += defRad;
        }
        Vector3 tmp = new Vector3();
        float c = Mathf.Cos( defRad * sign_ );
        float s = Mathf.Sin( defRad * sign_ );
        foreach ( var p in pieces_ ) {
            // ピース座標回転
            Vector3 pos = p.transform.localPosition;
            tmp.z = pos.z;
            tmp.x = pos.x * c + pos.y * s;
            tmp.y = -pos.x * s + pos.y * c;
            p.transform.localPosition = tmp;

            // ピースローカル回転
            p.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, -defDeg * sign_ ) * p.transform.localRotation;
        }

        if ( bFinish == true )
            replacePieceFaces( AxisType.AxisType_Z );

        return ( bFinish ? null : this );
    }
}

