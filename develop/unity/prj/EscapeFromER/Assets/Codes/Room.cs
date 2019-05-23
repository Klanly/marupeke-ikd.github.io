using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    bool bActiveWarning_ = false;
    bool preActiveWarning_ = false;

    [SerializeField]
    MeshRenderer renderer_;

    // [SerializeField]
    Material wallMaterial_;

    // [SerializeField]
    Material floorMaterial_;

    [SerializeField]
    Texture2D normalWallTex_;

    [SerializeField]
    Texture2D warningTex_;

    [SerializeField]
    Texture2D diactiveTex_;

    [SerializeField]
    float speed_ = 0.01f;

    [SerializeField]
    GameObject breakRoom_;

    public void activeWarning( bool isActive ) {
        bActiveWarning_ = isActive;
    }

    // 部屋解放
    public void deactivate( System.Action finishCallback ) {
        bDeactivate_ = true;
        renderer_.gameObject.SetActive( false );
        breakRoom_.SetActive( true );
        Destroy( breakRoom_, 5.0f );
        finishCallback();
    }

    private void Awake() {
        floorMaterial_ = new Material( renderer_.materials[ 0 ] );
        wallMaterial_ = new Material( renderer_.materials[ 1 ] );
        renderer_.materials[ 0 ] = floorMaterial_;
       // renderer_.materials[ 1 ] = wallMaterial_;
    }
    // Use this for initialization
    void Start () {
        wallMaterial_.SetTextureOffset( "_MainTex", Vector2.zero );
	}
	
	// Update is called once per frame
	void Update () {
        if ( bDeactivate_ == true )
            return;

		if ( bActiveWarning_ != preActiveWarning_ ) {
            var mat = renderer_.materials[ 1 ];
            if ( bActiveWarning_ == true ) {
                mat.SetTexture( "_MainTex", warningTex_ );
            } else {
                offset_ = Vector2.zero;
                mat.SetTexture( "_MainTex", normalWallTex_ );
            }
            renderer_.materials[ 1 ] = mat;
        }
        preActiveWarning_ = bActiveWarning_;

        if ( bActiveWarning_ == true ) {
            offset_.x += speed_;
            var mat = renderer_.materials[ 1 ];
            mat.SetTextureOffset( "_MainTex", offset_ );
            renderer_.materials[ 1 ] = mat;
        }
    }

    Vector2 offset_ = Vector2.zero;
    bool bDeactivate_ = false;
}
