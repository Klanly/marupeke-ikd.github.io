using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLine : MonoBehaviour {

    [SerializeField]
    TrailLine trailLine_;

    [SerializeField]
    Vector3 initPos_;

    [SerializeField]
    Vector3 initVec_;

    [SerializeField]
    float gravity_ = 9.81f;

    [SerializeField]
    float planetaryRadius_ = 1.0f; // 惑星半径

    [SerializeField]
    float targetHeight_ = 2.0f;     // ターゲット高

    [SerializeField]
    float width_ = 0.025f;

    [SerializeField]
    float stepSec_ = 0.02f;

	// Use this for initialization
	void Start () {
        // 軌道計算
        var data = FallLine.calcOrbit( planetaryRadius_, initPos_, initVec_, gravity_, stepSec_, targetHeight_ );

        // ライン作成
        trailLine_.setup( data.orbit_, width_ );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
