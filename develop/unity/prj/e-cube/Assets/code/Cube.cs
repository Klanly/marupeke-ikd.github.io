using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    [SerializeField]
    float rotDegPerFrame_ = 10.0f;

    [SerializeField]
    int n_;

    [SerializeField]
    Transform body_;

    [SerializeField]
    NormalPiece piecePrefab_;

    public float RotDegPerFrame { set { rotDegPerFrame_ = value; } get { return rotDegPerFrame_; } }

    private void Awake()
    {
        rotationManager_ = new RotationManager( this );

        // 回転グループ初期化
        pieces_ = new NormalPiece[ n_, n_, n_ ];

        // 全ピースをインスタンスする
        float ofs = ( n_ - 1 ) * 0.5f;
        for ( int z = 0; z < n_; ++z ) {
            for ( int y = 0; y < n_; ++y ) {
                for ( int x = 0; x < n_; ++x ) {
                    NormalPiece p = Instantiate<NormalPiece>( piecePrefab_ );
                    p.transform.parent = body_;
                    Vector3 pos = new Vector3( x - ofs, y - ofs, z - ofs );
                    p.transform.localPosition = pos;

                    if ( x == 0 ) {
                        // Right Off
                        p.enableFaceColor( FaceType.FaceType_Right, false );
                    } else if ( x < n_ - 1 ) {
                        // Left and Right Off
                        p.enableFaceColor( FaceType.FaceType_Left, false );
                        p.enableFaceColor( FaceType.FaceType_Right, false );
                    } else {
                        // Left Off
                        p.enableFaceColor( FaceType.FaceType_Left, false );
                    }

                    if ( y == 0 ) {
                        // Up Off
                        p.enableFaceColor( FaceType.FaceType_Up, false );
                    } else if ( y < n_ - 1 ) {
                        // Down and Up Off
                        p.enableFaceColor( FaceType.FaceType_Down, false );
                        p.enableFaceColor( FaceType.FaceType_Up, false );
                    } else {
                        // Down Off
                        p.enableFaceColor( FaceType.FaceType_Down, false );
                    }

                    if ( z == 0 ) {
                        // Back Off
                        p.enableFaceColor( FaceType.FaceType_Back, false );
                    } else if ( z < n_ - 1 ) {
                        // Front and Back Off
                        p.enableFaceColor( FaceType.FaceType_Front, false );
                        p.enableFaceColor( FaceType.FaceType_Back, false );
                    } else {
                        // Front Off
                        p.enableFaceColor( FaceType.FaceType_Front, false );
                    }
                    p.updateMaterials();

                    // ピース回転グループに登録
                    registerPiece( p, x, y, z );
                }
            }
        }
    }

    // ピースを登録
    void registerPiece( NormalPiece p, int x, int y, int z )
    {
        pieces_[ x , y, z ] = p;
    }

    // 回転指示
    // axis   : 回転軸
    // colIdx : 列番号配列
    // rotType: 回転方向及び角度
    // defDegPerFrame: 1フレーム当たりの回転角度
    public virtual void onRotation( AxisType axis, int[] colIndices, CubeRotationType rotType )
    {
        List<int> ary = new List<int>();
        foreach( var c in colIndices ) {
            if ( c >= 0 && c < n_ )
                ary.Add( c );
        }

        // 回転タスクを再初期化
        // 既に回転タスクが動いていたらスキップ
        if ( rotationManager_.isRun() == true ) {
            rotationManager_.skip();
        }
        rotationManager_.run( axis, ary.ToArray(), rotType, Mathf.Abs( rotDegPerFrame_ ) );
    }

    void Start () {
    }
	
	void Update () {
        rotationManager_.update();
    }

    // 回転メソッド
    class RotationMethod
    {
        protected RotationMethod( int[] colIndices, float aimDeg, NormalPiece[,,] pieces )
        {
            aimRad_ = Mathf.Abs( aimDeg ) * Mathf.Deg2Rad;
            if ( aimDeg < 0.0f )
                sign_ = -1.0f;
        }
        public virtual RotationMethod update( float defDeg ) { return null; }
        protected void updateRot( NormalPiece p )
        {

        }
        protected List<NormalPiece> pieces_ = new List<NormalPiece>();
        protected float comRad_ = 0.0f;
        protected float aimRad_ = 0.0f;     // 目標角度
        protected float sign_ = 1.0f;
    }

    // X軸回転
    class RotationMethod_AxisX : RotationMethod
    {
        public RotationMethod_AxisX(int[] colIndices, float aimDeg, NormalPiece[,,] pieces) : base( colIndices, aimDeg, pieces )
        {
            foreach ( var x in colIndices ) {
                for ( int y = 0; y < pieces.GetLength( 1 ); ++y ) {
                    for ( int z = 0; z < pieces.GetLength( 2 ); ++z ) {
                        pieces_.Add( pieces[x, y, z] );
                    }
                }
            }
        }
        public override RotationMethod update( float defDeg ) {
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
                tmp.y =  pos.y * c + pos.z * s;
                tmp.z = -pos.y * s + pos.z * c;
                p.transform.localPosition = tmp;

                // ピースローカル回転
                p.transform.localRotation = Quaternion.Euler( -defDeg * sign_, 0.0f, 0.0f ) * p.transform.localRotation;
            }
            return ( bFinish ? null : this );
        }
    }

    // Y軸回転
    class RotationMethod_AxisY : RotationMethod
    {
        public RotationMethod_AxisY(int[] colIndices, float aimDeg, NormalPiece[,,] pieces) : base( colIndices, aimDeg, pieces )
        {
            foreach ( var y in colIndices ) {
                for ( int x = 0; x < pieces.GetLength( 0 ); ++x ) {
                    for ( int z = 0; z < pieces.GetLength( 2 ); ++z ) {
                        pieces_.Add( pieces[ x, y, z ] );
                    }
                }
            }
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
            return ( bFinish ? null : this );
        }
    }

    // Z軸回転
    class RotationMethod_AxisZ : RotationMethod
    {
        public RotationMethod_AxisZ(int[] colIndices, float aimDeg, NormalPiece[,,] pieces) : base( colIndices, aimDeg, pieces )
        {
            foreach ( var z in colIndices ) {
                for ( int x = 0; x < pieces.GetLength( 0 ); ++x ) {
                    for ( int y = 0; y < pieces.GetLength( 1 ); ++y ) {
                        pieces_.Add( pieces[ x, y, z ] );
                    }
                }
            }
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
                tmp.x =  pos.x * c + pos.y * s;
                tmp.y = -pos.x * s + pos.y * c;
                p.transform.localPosition = tmp;

                // ピースローカル回転
                p.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, -defDeg * sign_ ) * p.transform.localRotation;
            }
            return ( bFinish ? null : this );
        }
    }

    // 回転動作管理人
    class RotationManager
    {
        public RotationManager( Cube parent )
        {
            parent_ = parent;
        }

        // 回転設定と行動開始
        public void run( AxisType axis, int[] colIndices, CubeRotationType rotType, float defDegPerFrame )
        {
            defDegPerFrame_ = defDegPerFrame;

            // 回転メソッドを設定
            if ( axis == AxisType.AxisType_X )
                rotMethod_ = new RotationMethod_AxisX( colIndices, ( float )rotType, parent_.pieces_ );
            else if ( axis == AxisType.AxisType_Y )
                rotMethod_ = new RotationMethod_AxisY( colIndices, ( float )rotType, parent_.pieces_ );
            else if ( axis == AxisType.AxisType_Z )
                rotMethod_ = new RotationMethod_AxisZ( colIndices, ( float )rotType, parent_.pieces_ );

            // 回転後の回転対象ピース及びそのカラー情報を書き出し
        }

        // 回転更新
        public bool update()
        {
            if ( isRun() == true ) {
                rotMethod_ = rotMethod_.update( defDegPerFrame_ );
            }
            return isRun();
        }

        // 回転中？
        public bool isRun()
        {
            return ( rotMethod_ != null );
        }

        // 回転処理スキップ
        public void skip()
        {
            // 現在の回転を最後まで回す
            rotMethod_ = rotMethod_.update( 1000.0f );
        }

        Cube parent_;
        RotationMethod rotMethod_;
        float defDegPerFrame_ = 1.0f;
    }

    NormalPiece[,,] pieces_;
    RotationManager rotationManager_;
}
