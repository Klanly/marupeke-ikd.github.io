using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRotation : MonoBehaviour {

    [SerializeField]
    Renderer renderer_ = null;

    [SerializeField]
    Color color0_ = Color.white;

    [SerializeField]
    Color color1_ = Color.white;

    [SerializeField]
    float loopSec_ = 1.0f;  // 1ループにかかる秒

	// Use this for initialization
	void Start () {
        if ( renderer_ as SpriteRenderer != null ) {
            updator_ = spriteUpdate;
            spriteRenderer_ = renderer_ as SpriteRenderer;
        } else {
            updator_ = defaultUpdate;
        }
	}

    void spriteUpdate() {
        if ( loopSec_ > 0.0f ) {
            t_ += Time.deltaTime / loopSec_;
            float t0 = Lerps.Float.loopCircle( 0, 1, t_ );
            spriteRenderer_.color = Color.Lerp( color0_, color1_, t0 );
        }
    }

    void defaultUpdate () {
        t_ += Time.deltaTime / loopSec_;
        float t0 = Lerps.Float.loopCircle( 0, 1, t_ );
        var mat = renderer_.material;
        mat.color = Color.Lerp( color0_, color1_, t0 );
        renderer_.material = mat;
	}

    void Update() {
        updator_();
    }

    float t_;
    System.Action updator_;
    SpriteRenderer spriteRenderer_;
}
