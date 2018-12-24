using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    [SerializeField]
    int n_;

    [SerializeField]
    Transform body_;

    [SerializeField]
    NormalPiece piecePrefab_;

    private void Awake()
    {
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
                }
            }
        }
    }
 
    void Start () {
        
	}
	
	void Update () {
		
	}
}
