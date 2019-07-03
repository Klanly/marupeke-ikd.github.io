using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UIイントロカラーアニメーション
//
//  追加したコンポーネントのImageのカラーにイントロアニメーションを加えます。
//  アニメーションが終了したらコンポーネント自体を破棄する事も出来ます。

[RequireComponent( typeof( RectTransform ), typeof(UnityEngine.UI.Text) )]
public class UIIntroTextColorAnimation : MonoBehaviour {

    [SerializeField]
    bool startOnAwake_ = true;

    [Header( "Color/Alpha" )]

    [SerializeField]
    BlendType blendType_ = BlendType.Absolute;

    [SerializeField]
    float wait_ = 0.0f;

    public AnimationCurve r_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );
    public AnimationCurve g_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );
    public AnimationCurve b_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );
    public AnimationCurve a_ = new AnimationCurve( new Keyframe( 0.0f, 1.0f ), new Keyframe( 1.0f, 1.0f ) );

    [SerializeField]
    bool useAutoDestroy_ = false;

    [SerializeField]
    float destroyWait_ = 0.0f;


    enum BlendType
    {
        Absolute,   // 上書き
        Add,        // 元の色に加算
        Mult,       // 元の色に乗算
    }

    void setAnimState4(float waitSec, AnimationCurve animR, AnimationCurve animG, AnimationCurve animB, AnimationCurve animA, System.Action<float, float, float, float> callback)
    {
        bool bR = animR.keys.Length > 0;
        bool bG = animG.keys.Length > 0;
        bool bB = animB.keys.Length > 0;
        bool bA = animA.keys.Length > 0;
        float t_ = 0.0f;
        float end_ = Mathf.Max(
            ( bR ? animR.keys[ animR.keys.Length - 1 ].time : 0.0f ),
            ( bG ? animG.keys[ animG.keys.Length - 1 ].time : 0.0f ),
            ( bB ? animB.keys[ animB.keys.Length - 1 ].time : 0.0f ),
            ( bA ? animA.keys[ animA.keys.Length - 1 ].time : 0.0f )
        );
        GlobalState.wait( waitSec, () => {
            t_ += Time.deltaTime * timeScale_;
            if ( t_ >= end_ )
                t_ = end_;
            float vR = animR.Evaluate( t_ );
            float vG = animG.Evaluate( t_ );
            float vB = animB.Evaluate( t_ );
            float vA = animA.Evaluate( t_ );
            callback( vR, vG, vB, vA );
            if ( t_ >= end_ && useAutoDestroy_ == true ) {
                Destroy( this.gameObject, destroyWait_ );
            }
            return !( t_ >= end_ );
        } );
    }

    void startAnimation()
    {
        var image = GetComponent<UnityEngine.UI.Text>();
        var c = image.color;

        if ( blendType_ == BlendType.Absolute ) {
            c.r = r_.Evaluate( 0.0f );
            c.g = g_.Evaluate( 0.0f );
            c.b = b_.Evaluate( 0.0f );
            c.a = a_.Evaluate( 0.0f );
            image.color = c;
            setAnimState4( wait_, r_, g_, b_, a_, (r, g, b, a) => {
                c.r = r;
                c.g = g;
                c.b = b;
                c.a = a;
                image.color = c;
            } );
        } else if ( blendType_ == BlendType.Add ) {
            var c0 = c;
            c0.r = r_.Evaluate( 0.0f );
            c0.g = g_.Evaluate( 0.0f );
            c0.b = b_.Evaluate( 0.0f );
            c0.a = a_.Evaluate( 0.0f );
            image.color = c + c0;
            setAnimState4( wait_, r_, g_, b_, a_, (r, g, b, a) => {
                c.r = c0.r + r;
                c.g = c0.g + g;
                c.b = c0.b + b;
                c.a = c0.a + a;
                image.color = c;
            } );
        } else if ( blendType_ == BlendType.Mult ) {
            var c0 = c;
            c0.r = r_.Evaluate( 0.0f );
            c0.g = g_.Evaluate( 0.0f );
            c0.b = b_.Evaluate( 0.0f );
            c0.a = a_.Evaluate( 0.0f );
            image.color = c * c0;
            setAnimState4( wait_, r_, g_, b_, a_, (r, g, b, a) => {
                c.r = c0.r * r;
                c.g = c0.g * g;
                c.b = c0.b * b;
                c.a = c0.a * a;
                image.color = c;
            } );
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
