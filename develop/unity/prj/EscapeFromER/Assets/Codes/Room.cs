using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    bool bActiveWarning_ = false;
    bool preActiveWarning_ = false;

    [SerializeField]
    Material wallMaterial_;

    [SerializeField]
    Texture2D normalWallTex_;

    [SerializeField]
    Texture2D warningTex_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( bActiveWarning_ != preActiveWarning_ ) {
            if ( bActiveWarning_ == true ) {
                wallMaterial_.SetTexture( "_MainTex", warningTex_ );
            } else {
                wallMaterial_.SetTexture( "_MainTex", normalWallTex_ );
            }
        }
        preActiveWarning_ = bActiveWarning_;

    }
}
