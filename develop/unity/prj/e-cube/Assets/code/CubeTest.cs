using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour {

    [SerializeField]
    Cube cube_;

    [SerializeField]
    GameObject facePrefab_;

    [SerializeField]
    float pieceLen_ = 1.0f;

    [SerializeField]
    Transform faceRoot_;

    [SerializeField]
    bool bApplyParameters_ = false;

    [SerializeField]
    bool bRandomRotate_ = false;

    [SerializeField]
    int shuffleNum_ = 1;

    [ SerializeField]
    CubeRotationType rotType_ = CubeRotationType.CRT_Plus_90;

    [SerializeField]
    AxisType axis_ = AxisType.AxisType_X;

    [SerializeField]
    float defDegPerFrame_ = 10.0f;

    [SerializeField]
    int[] colIndices_;

    void Start () {
        int n = cube_.getN();
        float offset = 0.5f;
        faces_[ 0 ] = new Face( cube_, FaceType.FaceType_Left, "Left", faceRoot_, new Vector3( - pieceLen_ * n - offset, 0.0f, 0.0f), pieceLen_, facePrefab_, n );
        faces_[ 1 ] = new Face( cube_, FaceType.FaceType_Right, "Right", faceRoot_, new Vector3( pieceLen_ * n + offset, 0.0f, 0.0f ), pieceLen_, facePrefab_, n );
        faces_[ 2 ] = new Face( cube_, FaceType.FaceType_Down, "Down", faceRoot_, new Vector3( 0.0f, -pieceLen_ * n - offset, 0.0f ), pieceLen_, facePrefab_, n );
        faces_[ 3 ] = new Face( cube_, FaceType.FaceType_Up, "Up", faceRoot_, new Vector3( 0.0f, pieceLen_ * n + offset, 0.0f ), pieceLen_, facePrefab_, n );
        faces_[ 4 ] = new Face( cube_, FaceType.FaceType_Front, "Front", faceRoot_, new Vector3( 0.0f, 0.0f, 0.0f ), pieceLen_, facePrefab_, n );
        faces_[ 5 ] = new Face( cube_, FaceType.FaceType_Back, "Back", faceRoot_, new Vector3( pieceLen_ * n + offset * 2, pieceLen_ * n + offset * 2, 0.0f ), pieceLen_, facePrefab_, n );
        updateFaceState();
    }

    void Update () {
        if ( bApplyParameters_ == true ) {
            bApplyParameters_ = false;

            cube_.RotDegPerFrame = defDegPerFrame_;
            cube_.onRotation( axis_, colIndices_, rotType_ );
            updateFaceState();
        }

        if ( bRandomRotate_ == true ) {
            AxisType[] axiss = new AxisType[ 3 ] {
                AxisType.AxisType_X,
                AxisType.AxisType_Y,
                AxisType.AxisType_Z,
            };
            CubeRotationType[] rotTypes = new CubeRotationType[ 6 ] {
                CubeRotationType.CRT_Plus_90,
                CubeRotationType.CRT_Plus_180,
                CubeRotationType.CRT_Plus_270,
                CubeRotationType.CRT_Minus_90,
                CubeRotationType.CRT_Minus_180,
                CubeRotationType.CRT_Minus_270,
            };
            bRandomRotate_ = false;
            for ( int i = 0; i < shuffleNum_; ++i ) {
                cube_.RotDegPerFrame = defDegPerFrame_;
                cube_.onRotation( axiss[ Random.Range( 0, 3 ) ], new int[ 1 ] { Random.Range( 0, cube_.getN() ) }, rotTypes[ Random.Range( 0, 6 ) ] );
                updateFaceState();
            }
        }
    }

    void updateFaceState()
    {
        for ( int i = 0; i < 6; ++i )
            faces_[ i ].update();
    }

    class Face
    {
        public Face( Cube cube, FaceType faceType, string name, Transform root, Vector3 pos, float len, GameObject facePrefab, int n )
        {
            n_ = n;
            cube_ = cube;
            faceType_ = faceType;
            renderers_ = new MeshRenderer[ n * n ];
            GameObject faceRoot_ = new GameObject( name );
            faceRoot_.transform.parent = root;
            faceRoot_.transform.localPosition = Vector3.zero;
            int idx = 0;
            for ( int y = 0; y < n; ++y ) {
                for ( int x = 0; x < n; ++x ) {
                    var obj = GameObject.Instantiate<GameObject>( facePrefab );
                    obj.transform.parent = faceRoot_.transform;
                    obj.transform.localPosition = ( new Vector3( x * len, y * len, 0.0f ) ) + pos;

                    var renderer = obj.GetComponent<MeshRenderer>();
                    if ( renderer != null )
                        renderers_[ idx ] = renderer;
                    idx++;
                }
            }
        }

        public void update()
        {
            var data = cube_.getCubeData();
            var faceData_ = data.getFaces();
            for ( int i = 0; i < n_ * n_; ++i ) {
                var faceType = faceData_[ ( int )faceType_, i ];
                var m = renderers_[ i ].material;
                m.color = faceColors_[ ( int )faceType ];
                renderers_[ i ].material = m;
            }
        }

        int n_;
        Cube cube_;
        FaceType faceType_;
        MeshRenderer[] renderers_;
        GameObject faceRoot_;
        Color[] faceColors_ = CubeDefines.faceColors_;
    }

    Face[] faces_ = new Face[ 6 ];
}
