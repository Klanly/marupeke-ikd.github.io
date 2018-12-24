using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 通常ピースクラス
//
//  ピースの色や回転を制御

public class NormalPiece : MonoBehaviour {

    [SerializeField]
    Renderer renderer_;

    [SerializeField]
    List<Material> materials_;

    [SerializeField]
    Material blackMaterial_;

    public void enableFaceColor( FaceType face, bool enable )
    {
        if ( enable == false )
            materials_[ ( int )face ] = blackMaterial_;
    }

    public void updateMaterials()
    {
        var materials = renderer_.materials;
        materials[ 0 ] = materials_[ ( int )FaceType.FaceType_Front ];
        materials[ 1 ] = materials_[ ( int )FaceType.FaceType_Down ];
        materials[ 2 ] = materials_[ ( int )FaceType.FaceType_Right ];
        materials[ 3 ] = materials_[ ( int )FaceType.FaceType_Up ];
        materials[ 4 ] = materials_[ ( int )FaceType.FaceType_Back ];
        materials[ 6 ] = materials_[ ( int )FaceType.FaceType_Left ];
        renderer_.materials = materials;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
