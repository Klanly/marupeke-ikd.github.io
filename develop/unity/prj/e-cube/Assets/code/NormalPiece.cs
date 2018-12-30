using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 通常ピースクラス
//
//  ピースの色や回転を制御

public class NormalPiece : MonoBehaviour
{

    [SerializeField]
    Renderer renderer_;

    [SerializeField]
    Collider collider_;

    [SerializeField]
    List<Material> materials_;

    [SerializeField]
    Material blackMaterial_;

    // 座標変換したピースの情報
    public class TransInfo
    {
        public Vector3Int transCoord_;     // 座標変換後のピース座標
        public FaceType[] transFaceType_ = new FaceType[ 6 ];  // 座標変換後のFaceType対応 transFaceType[ FaceType_Front ] -> FaceType_Left など

        // 回転後のピース座標を計算
        public static Vector3Int calcTransCoord(int n, AxisType axis, CubeRotationType rotType, Vector3Int coord)
        {
            // Sin, Cosテーブル
            int[] sin = new int[ ( int )CubeRotationType.CRT_NUM ] {
                0, 1, 0, -1, -1, 0, 1
            };
            int[] cos = new int[ ( int )CubeRotationType.CRT_NUM ] {
                1, 0, -1, 0, 0, -1, 0
            };
            float ox = coord.x + 0.5f - n * 0.5f;
            float oy = coord.y + 0.5f - n * 0.5f;
            float oz = coord.z + 0.5f - n * 0.5f;
            float rx = 0.0f;
            float ry = 0.0f;
            float rz = 0.0f;
            int e = 0;
            switch ( rotType ) {
                case CubeRotationType.CRT_0:
                    e = 0;
                    break;
                case CubeRotationType.CRT_Plus_90:
                    e = 1;
                    break;
                case CubeRotationType.CRT_Plus_180:
                    e = 2;
                    break;
                case CubeRotationType.CRT_Plus_270:
                    e = 3;
                    break;
                case CubeRotationType.CRT_Minus_90:
                    e = 4;
                    break;
                case CubeRotationType.CRT_Minus_180:
                    e = 5;
                    break;
                case CubeRotationType.CRT_Minus_270:
                    e = 6;
                    break;
            };
            int c = cos[ e ];
            int s = sin[ e ];
            if ( axis == AxisType.AxisType_X ) {
                rx = ox;
                ry = oy * c + oz * s;
                rz = -oy * s + oz * c;
            } else if ( axis == AxisType.AxisType_Y ) {
                ry = oy;
                rx = ox * c - oz * s;
                rz = ox * s + oz * c;
            } else {
                rz = oz;
                rx = ox * c + oy * s;
                ry = -ox * s + oy * c;
            }
            return new Vector3Int(
                ( int )( rx + n * 0.5f ),
                ( int )( ry + n * 0.5f ),
                ( int )( rz + n * 0.5f )
            );
        }

        // 回転後のフェイスを算出
        public static FaceType[] calcTransFaceType(AxisType axis, CubeRotationType rotType)
        {
            var f = new FaceType[ ( int )FaceType.FaceType_Num ] {
                FaceType.FaceType_Left,
                FaceType.FaceType_Right,
                FaceType.FaceType_Down,
                FaceType.FaceType_Up,
                FaceType.FaceType_Front,
                FaceType.FaceType_Back,
            };
            if ( axis == AxisType.AxisType_X ) {
                switch ( rotType ) {
                    case CubeRotationType.CRT_Plus_90:
                    case CubeRotationType.CRT_Minus_270:
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Front;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Down;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Up;
                        break;
                    case CubeRotationType.CRT_Plus_180:
                    case CubeRotationType.CRT_Minus_180:
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Up;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Down;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Front;
                        break;
                    case CubeRotationType.CRT_Plus_270:
                    case CubeRotationType.CRT_Minus_90:
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Front;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Up;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Down;
                        break;
                }
            } else if ( axis == AxisType.AxisType_Y ) {
                switch ( rotType ) {
                    case CubeRotationType.CRT_Plus_90:
                    case CubeRotationType.CRT_Minus_270:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Front;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Right;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Left;
                        break;
                    case CubeRotationType.CRT_Plus_180:
                    case CubeRotationType.CRT_Minus_180:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Right;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Left;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Front;
                        break;
                    case CubeRotationType.CRT_Plus_270:
                    case CubeRotationType.CRT_Minus_90:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Back;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Front;
                        f[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_Left;
                        f[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_Right;
                        break;
                }
            } else if ( axis == AxisType.AxisType_Z ) {
                switch ( rotType ) {
                    case CubeRotationType.CRT_Plus_90:
                    case CubeRotationType.CRT_Minus_270:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Up;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Down;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Right;
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Left;
                        break;
                    case CubeRotationType.CRT_Plus_180:
                    case CubeRotationType.CRT_Minus_180:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Right;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Left;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Down;
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Up;
                        break;
                    case CubeRotationType.CRT_Plus_270:
                    case CubeRotationType.CRT_Minus_90:
                        f[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_Down;
                        f[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_Up;
                        f[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_Left;
                        f[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_Right;
                        break;
                }
            }
            return f;
        }
    }

    // ピース座標ハッシュ値を座標に変換
    public static Vector3Int convHashToCoord(uint hash)
    {
        Vector3Int p = Vector3Int.zero;
        p.x = ( int )( hash & 0xff );
        p.y = ( int )( ( hash >> 8 ) & 0xff );
        p.z = ( int )( ( hash >> 16 ) & 0xff );
        return p;
    }

    // ピース座標をハッシュ値に変換
    public static uint convCoordToHash(Vector3Int coord)
    {
        return ( uint )(
            ( coord.x & 0xff ) |
            ( ( coord.y & 0xff ) << 8 ) |
            ( ( coord.z & 0xff ) << 16 )
        );
    }

    // FaceTypeとFaceインデックスからその面を保持するピース座標を算出
    static public Vector3Int convFaceTypeAndIndexToCoord(int n, FaceType faceType, int index)
    {
        int fcX = index % n;
        int fcY = index / n;

        Vector3Int coord = Vector3Int.zero;

        switch ( faceType ) {
            case FaceType.FaceType_Left:
                coord.x = 0;
                coord.y = fcY;
                coord.z = ( n - 1 ) - fcX;
                break;
            case FaceType.FaceType_Right:
                coord.x = n - 1;
                coord.y = fcY;
                coord.z = fcX;
                break;
            case FaceType.FaceType_Down:
                coord.x = fcX;
                coord.y = 0;
                coord.z = ( n - 1 ) - fcY;
                break;
            case FaceType.FaceType_Up:
                coord.x = fcX;
                coord.y = n - 1;
                coord.z = fcY;
                break;
            case FaceType.FaceType_Front:
                coord.x = fcX;
                coord.y = fcY;
                coord.z = 0;
                break;
            case FaceType.FaceType_Back:
                coord.x = fcX;
                coord.y = fcY;
                coord.z = n - 1;
                break;
        }

        return coord;
    }

    // 指定の回転をした時の置換先情報を返す
    public TransInfo calcRotateInfo(int n, Vector3Int coord, AxisType axis, CubeRotationType rotType)
    {
        TransInfo info = new TransInfo();
        info.transCoord_ = TransInfo.calcTransCoord( n, axis, rotType, coord );
        info.transFaceType_ = TransInfo.calcTransFaceType( axis, rotType );
        FaceType[] faceType = new FaceType[ info.transFaceType_.Length ];
        for ( int i = 0; i < info.transFaceType_.Length; ++i ) {
            faceType[ ( int )info.transFaceType_[ i ] ] = curFaceColors_[ i ];
        }
        info.transFaceType_ = faceType;
        return info;
    }

    // 初期化
    //  自分の初期ピース座標から自分自身のローカル位置、フェイスを決定する
    public void initialize(int n, Vector3Int pieceCoord, float modelScale = 1.0f)
    {
        coord_ = pieceCoord;

        // 初期座標に対応した面を自動設定
        if ( pieceCoord.x == 0 ) {
            // Right Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_None;
        } else if ( pieceCoord.x < n - 1 ) {
            // Left and Right Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_None;
            defaultFaceTypes_[ ( int )FaceType.FaceType_Right ] = FaceType.FaceType_None;
        } else {
            // Left Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Left ] = FaceType.FaceType_None;
        }

        if ( pieceCoord.y == 0 ) {
            // Up Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_None;
        } else if ( pieceCoord.y < n - 1 ) {
            // Down and Up Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_None;
            defaultFaceTypes_[ ( int )FaceType.FaceType_Up ] = FaceType.FaceType_None;
        } else {
            // Down Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Down ] = FaceType.FaceType_None;
        }

        if ( pieceCoord.z == 0 ) {
            // Back Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_None;
        } else if ( pieceCoord.z < n - 1 ) {
            // Front and Back Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_None;
            defaultFaceTypes_[ ( int )FaceType.FaceType_Back ] = FaceType.FaceType_None;
        } else {
            // Front Off
            defaultFaceTypes_[ ( int )FaceType.FaceType_Front ] = FaceType.FaceType_None;
        }
        setFaceColors( defaultFaceTypes_ );

        float ofs = ( n - 1 ) * 0.5f;
        initPos_ = new Vector3( pieceCoord.x - ofs, pieceCoord.y - ofs, pieceCoord.z - ofs );
        transform.localPosition = initPos_;
        initRot_ = transform.localRotation;

        // 内部ピースの場合はコライダーを切る
        //  内部ピース：ピース座標の成分に0もしくはn-1が一つも含まれていない
        if (
            coord_.x != 0 && coord_.x != n - 1 &&
            coord_.y != 0 && coord_.y != n - 1 &&
            coord_.z != 0 && coord_.z != n - 1
        ) {
            // 内部ピース
            collider_.gameObject.SetActive( false );
        }

        gameObject.name = "Piece(" + coord_.x + ", "+ coord_.y +", "+ coord_.z +")";
    }

    // 指定のフェイスに差し替え
    public void setFaceColors(FaceType[] faces)
    {
        Material[] materials = new Material[ 6 ];
        for ( int i = 0; i < faces.Length; ++i ) {
            // i -> 指定面
            var pasteFace = faces[ i ];
            if ( pasteFace == FaceType.FaceType_None )
                materials[ i ] = blackMaterial_;
            else
                materials[ i ] = materials_[ ( int )pasteFace ];
        }
        curFaceColors_ = faces;
        updateMaterials( materials );
    }

    // 指定のフェイスに差し替え
    public void setFaceColor( FaceType face, FaceType faceColor )
    {
        curFaceColors_[ ( int )face ] = faceColor;
        if ( faceColor == FaceType.FaceType_None )
            updateMaterial( face, blackMaterial_ );
        else
            updateMaterial( face, materials_[ ( int )faceColor ] );
    }

    // ピース座標を取得
    public Vector3Int getCoord()
    {
        return coord_;
    }

    // ピース座標のハッシュ値を取得
    public uint getCoordHash()
    {
        return convCoordToHash( coord_ );
    }

    // 姿勢をリセット
    public void resetRotate()
    {
        transform.localPosition = initPos_;
        transform.localRotation = initRot_;
    }

    // 指定マテリアルをフェイスに適用
    void updateMaterials(Material[] materials)
    {
        var faceMaterials = renderer_.materials;
        faceMaterials[ 0 ] = materials[ ( int )FaceType.FaceType_Front ];
        faceMaterials[ 1 ] = materials[ ( int )FaceType.FaceType_Down ];
        faceMaterials[ 2 ] = materials[ ( int )FaceType.FaceType_Right ];
        faceMaterials[ 3 ] = materials[ ( int )FaceType.FaceType_Up ];
        faceMaterials[ 4 ] = materials[ ( int )FaceType.FaceType_Back ];
        faceMaterials[ 6 ] = materials[ ( int )FaceType.FaceType_Left ];
        renderer_.materials = faceMaterials;
    }

    // 指定マテリアルをフェイスに適用
    void updateMaterial( FaceType face, Material material )
    {
        var faceMaterials = renderer_.materials;
        int[] idx = new int[ 6 ] { 6, 2, 1, 3, 0, 4 };
        int i = idx[ ( int )face ];
        faceMaterials[ i ] = material;
        renderer_.materials = faceMaterials;
    }

    // フェイスのON/OFF切り替え
    void enableFaceColor(bool[] enable)
    {
        Material[] materials = new Material[ 6 ];
        for ( int i = 0; i < materials.Length; ++i ) {
            materials[ i ] = ( enable[ i ] ? materials_[ i ] : blackMaterial_ );
        }
        updateMaterials( materials );
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    FaceType[] defaultFaceTypes_ = new FaceType[ 6 ] {
        FaceType.FaceType_Left,
        FaceType.FaceType_Right,
        FaceType.FaceType_Down,
        FaceType.FaceType_Up,
        FaceType.FaceType_Front,
        FaceType.FaceType_Back
    };
    Vector3 initPos_ = Vector3.zero;
    Quaternion initRot_ = Quaternion.identity;
    FaceType[] curFaceColors_;
    Vector3Int coord_ = Vector3Int.zero;
}
 
