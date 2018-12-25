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

    private void Awake()
    {
        rotationManager_ = new RotationManager( this );

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
    }

    // ピースを登録
    void registerPiece(NormalPiece p)
    {
        Vector3Int coord = p.getCoord();
        uint hash = p.getCoordHash();
        pieces_[ coord.x, coord.y, coord.z ] = p;
        piecesMap_[ hash ] = p;
    }

    void Start () {
    }
	
	void Update () {
        rotationManager_.update();
    }

    NormalPiece[,,] pieces_;
    Dictionary<uint, NormalPiece> piecesMap_ = new Dictionary<uint, NormalPiece>();
    RotationManager rotationManager_;
}
