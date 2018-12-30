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
    Transform[] normals_; // 各フェイスの法線方向にあるオブジェクト（ワールド法線計算用）

    [SerializeField]
    NormalPiece piecePrefab_;

    public float RotDegPerFrame { set { rotDegPerFrame_ = value; } get { return rotDegPerFrame_; } }

    // 初期化
    public void initialize( int n )
    {
        n_ = n;
        rotationManager_ = new RotationManager( this );
        cubeData_ = new CubeData( n_ );

        // 回転グループ初期化
        pieces_ = new NormalPiece[ n_, n_, n_ ];

        // 全ピースをインスタンスする
        float removeDist = n_ * 0.5f - 1.0f;    // 内接球半径から1ピース分内側
        Vector3 center = new Vector3( n_ * 0.5f, n_ * 0.5f, n_ * 0.5f );
        for ( int z = 0; z < n_; ++z ) {
            for ( int y = 0; y < n_; ++y ) {
                for ( int x = 0; x < n_; ++x ) {
                    // 内接球の内側にあるピースは除く
                    Vector3 pos = new Vector3( x + 0.5f, y + 0.5f, z + 0.5f );
                    float r = ( pos - center ).magnitude;
                    if ( r < removeDist )
                        continue;

                    NormalPiece p = Instantiate<NormalPiece>( piecePrefab_ );
                    p.transform.parent = body_;
                    p.initialize( n_, new Vector3Int( x, y, z ) );

                    // ピース回転グループに登録
                    registerPiece( p );
                }
            }
        }

        bInitialized_ = true;
    }


    // キューブのピース長を取得
    public int getN()
    {
        return n_;
    }

    // ピースリストを取得
    public NormalPiece[,,] getPieceList()
    {
        return pieces_;
    }

    // ピースマップを取得
    public Dictionary<uint, NormalPiece> getPieceMap() {
        return piecesMap_;
    }

    // キューブデータを取得
    public CubeData getCubeData()
    {
        return cubeData_;
    }

    // 現在の各面の情報を取得
    public FaceType[,] getFaces()
    {
        return cubeData_.getFaces();
    }

    // レイの先にあるピースを取得
    public NormalPiece ray( Ray r, out FaceType collideFaceType )
    {
        collideFaceType = FaceType.FaceType_None;

        RaycastHit[] hitInfo = Physics.RaycastAll( r );
        if ( hitInfo.Length > 0 ) {
            float minDist_ = float.MaxValue;
            int idx = -1;
            NormalPiece hitPiece = null;
            for ( int i = 0; i < hitInfo.Length; ++i ) {
                var piece = hitInfo[ i ].collider.transform.parent.gameObject.GetComponent<NormalPiece>();
                if ( piece == null )
                    continue;
                if ( hitInfo[ i ].distance < minDist_ ) {
                    minDist_ = hitInfo[ i ].distance;
                    hitPiece = piece;
                    idx = i;
                }
            }

            if ( hitPiece != null ) {
                // レイが衝突したフェイスを特定
                var nm = hitInfo[ idx ].normal;
                var localNm = body_.InverseTransformDirection( nm );
                Vector3[] dirs = new Vector3[ 6 ] {
                    Vector3.left,
                    Vector3.right,
                    Vector3.down,
                    Vector3.up,
                    Vector3.back,
                    Vector3.forward
                };
                float dotVal = -2.0f;
                int dirIdx = -1;
                for ( int i = 0; i < dirs.Length; ++i ) {
                    float dot = Vector3.Dot( localNm, dirs[ i ] );
                    if ( dot > dotVal ) {
                        dotVal = dot;
                        dirIdx = i;
                    }
                }
                collideFaceType = ( FaceType )dirIdx;
            }
            return hitPiece;
        }
        return null;
    }

    // 各フェイスの法線ターゲットのワールド座標を取得
    public Vector3[] getFaceNormalsTargetPosInWorld()
    {
        return  new Vector3[ 6 ] {
            normals_[ 0 ].position,
            normals_[ 1 ].position,
            normals_[ 2 ].position,
            normals_[ 3 ].position,
            normals_[ 4 ].position,
            normals_[ 5 ].position,
        };
    }

    // キューブの中心点の座標を取得
    public Vector3 getBodyPos()
    {
        return body_.transform.position;
    }

    // キューブのピースのフェイス面を個別設定
    public void setPieceFace(FaceType face, int idx, FaceType faceColor)
    {
        var coord = NormalPiece.convFaceTypeAndIndexToCoord( n_, face, idx );
        var piece = pieces_[ coord.x, coord.y, coord.z ];
        piece.setFaceColor( face, faceColor );
    }

    // 回転指示
    // axis   : 回転軸
    // colIdx : 列番号配列
    // rotType: 回転方向及び角度
    // defDegPerFrame: 1フレーム当たりの回転角度
    public virtual void onRotation( AxisType axis, int[] colIndices, CubeRotationType rotType )
    {
        HashSet<int> colHash = new HashSet<int>();
        List<int> ary = new List<int>();
        foreach ( var c in colIndices ) {
            if ( c >= 0 && c < n_ && colHash.Contains( c ) == false ) {
                ary.Add( c );
                colHash.Add( c );
            }
        }

        // 回転タスクを再初期化
        // 既に回転タスクが動いていたらスキップ
        if ( rotationManager_.isRun() == true ) {
            rotationManager_.skip();
        }
        rotationManager_.run( axis, ary.ToArray(), rotType, Mathf.Abs( rotDegPerFrame_ ) );
    }

    // キューブが揃っている？
    public bool isComplete()
    {
        return cubeData_.isComplete();
    }

    private void Awake()
    {
    }

    // ピースを登録
    void registerPiece(NormalPiece p)
    {
        Vector3Int coord = p.getCoord();
        uint hash = p.getCoordHash();
        pieces_[ coord.x, coord.y, coord.z ] = p;
        piecesMap_[ hash ] = p;
    }

    private void Start () {
        if ( bInitialized_ == false ) {
            initialize( n_ );
        }
    }
	
	void Update () {
        rotationManager_.update();
    }

    bool bInitialized_ = false;
    NormalPiece[,,] pieces_;
    Dictionary<uint, NormalPiece> piecesMap_ = new Dictionary<uint, NormalPiece>();
    RotationManager rotationManager_;
    CubeData cubeData_;
}
