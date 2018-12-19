using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSprite : MonoBehaviour {

    [SerializeField]
    SpriteRenderer sp_;

	// Use this for initialization
	void Start () {
        Color c = sp_.color;
        c.a = 0.0f;
        sp_.color = c;
    }

    // Update is called once per frame
    void Update () {
        if ( bTrans_ == true ) {
            t += Time.deltaTime;
            float d = t / tm;
            if ( d >= 1.0f ) {
                d = 1.0f;
                bTrans_ = false;
            }
            Color c = sp_.color;
            c.a = d;
            sp_.color = c;
        }
    }

    bool bTrans_ = true;
    float t = 0.0f;
    float tm = 2.0f;
}
