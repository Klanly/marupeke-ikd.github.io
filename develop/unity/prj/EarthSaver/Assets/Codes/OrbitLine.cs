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

    public class Parameter {
        public Vector3 initPos_;
        public Vector3 initVec_;
        public float gravity_;
        public float planetaryRadius_;
        public float targetHeight_;
        public float stepSec_;
    }

    // セットアップ
    public void setup( Parameter param ) {
        param_ = param;
        // 軌道計算
        data_ = FallLine.calcOrbit( param_.planetaryRadius_, param_.initPos_, param_.initVec_, param_.gravity_, param_.stepSec_, param_.targetHeight_ );
        // ライン作成
        trailLine_.setup( data_.orbit_, width_ );
    }

    // データ取得
    public FallLine.Data getData() {
        return data_;
    }

    public void setActiveLine( bool isActive ) {
        if ( isActive == true ) {
            trailLine_.setAlpha( 1.0f );
        } else {
            trailLine_.setAlpha( 0.05f );
        }
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    FallLine.Data data_ = null;
    Parameter param_ = new Parameter();
    Material lineMat_;
}
