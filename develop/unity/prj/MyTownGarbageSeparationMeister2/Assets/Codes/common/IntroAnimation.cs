using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// イントロアニメーション
//
//  追加したコンポーネントの位置、回転、スケールのイントロアニメーションを行えます。
//  アニメーションが終了したらコンポーネント自体を破棄する事も出来ます。

public class IntroAnimation : MonoBehaviour {

    [SerializeField]
    bool startOnAwake_ = true;

    [Header("Position")]

    [SerializeField]
    bool usePosition_ = false;

    [SerializeField]
    bool positionAsLocal_ = true;

    [SerializeField]
    float positionWait_ = 0.0f;

    public AnimationCurve positionCurveX_;
    public AnimationCurve positionCurveY_;
    public AnimationCurve positionCurveZ_;

    [Header( "Rotation" )]

    [SerializeField]
    bool useRotation_ = false;

    [SerializeField]
    bool rotationAsLocal_ = true;

    [SerializeField]
    float rotationWait_ = 0.0f;

    public AnimationCurve rotationCurveX_;
    public AnimationCurve rotationCurveY_;
    public AnimationCurve rotationCurveZ_;

    [Header( "Scale" )]

    [SerializeField]
    bool useScale_ = false;

    [SerializeField]
    bool scaleAsAbsolute_ = true;

    [SerializeField]
    float scaleWait_ = 0.0f;

    public AnimationCurve scaleCurveX_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );
    public AnimationCurve scaleCurveY_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );
    public AnimationCurve scaleCurveZ_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );

    void setAnimState( float waitSec, AnimationCurve anim, System.Action< float > callback )
    {
        if ( anim.keys.Length == 0 )
            return;
        float t_ = 0.0f;
        float end_ = anim.keys[ anim.keys.Length - 1 ].time;
        GlobalState.wait( waitSec, () => {
            t_ += Time.deltaTime * timeScale_;
            if ( t_ >= end_ )
                t_ = end_;
            float v = anim.Evaluate( t_ );
            callback( v );
            return !( t_ >= end_ );
        } );
    }

    void setAnimState3(float waitSec, AnimationCurve animX, AnimationCurve animY, AnimationCurve animZ, System.Action<float, float, float> callback)
    {
        bool bX = animX.keys.Length > 0;
        bool bY = animY.keys.Length > 0;
        bool bZ = animZ.keys.Length > 0;
        float t_ = 0.0f;
        float end_ = Mathf.Max(
            ( bX ? animX.keys[ animX.keys.Length - 1 ].time : 0.0f ),
            ( bY ? animY.keys[ animY.keys.Length - 1 ].time : 0.0f ),
            ( bZ ? animZ.keys[ animZ.keys.Length - 1 ].time : 0.0f )
        );
        GlobalState.wait( waitSec, () => {
            t_ += Time.deltaTime * timeScale_;
            if ( t_ >= end_ )
                t_ = end_;
            float vX = animX.Evaluate( t_ );
            float vY = animY.Evaluate( t_ );
            float vZ = animZ.Evaluate( t_ );
            callback( vX, vY, vZ );
            return !( t_ >= end_ );
        } );
    }

    void startAnimation()
    {
        if ( usePosition_ == true ) {
            var p = new Vector3();
            if ( positionAsLocal_ == true ) {
                setAnimState3( positionWait_, positionCurveX_, positionCurveY_, positionCurveZ_, (x, y, z) => {
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    transform.localPosition = p;
                } );
            } else {
                setAnimState3( positionWait_, positionCurveX_, positionCurveY_, positionCurveZ_, (x, y, z) => {
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    transform.position = p;
                } );
            }
        }
        if ( useRotation_ == true ) {
            if ( rotationAsLocal_ == true ) {
                setAnimState3( rotationWait_, rotationCurveX_, rotationCurveY_, rotationCurveZ_, (x, y, z) => {
                    transform.localRotation = Quaternion.Euler( x, y, z );
                } );
            } else {
                setAnimState3( rotationWait_, rotationCurveX_, rotationCurveY_, rotationCurveZ_, (x, y, z) => {
                    transform.rotation = Quaternion.Euler( x, y, z );
                } );
            }
        }
        var initScale = transform.localScale;
        if ( useScale_ == true ) {
            var p = new Vector3();
            if ( scaleAsAbsolute_ == true ) {
                setAnimState3( scaleWait_, scaleCurveX_, scaleCurveY_, scaleCurveZ_, (x, y, z) => {
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    transform.localScale = p;
                } );
            } else {
                setAnimState3( scaleWait_, scaleCurveX_, scaleCurveY_, scaleCurveZ_, (x, y, z) => {
                    p.x = x * initScale.x;
                    p.y = y * initScale.y;
                    p.z = z * initScale.z;
                    transform.localScale = p;
                } );
            }
        }
    }

    // Use this for initialization
    void Start () {
        if ( startOnAwake_ == true )
            startAnimation();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    float timeScale_ = 1.0f;
}
