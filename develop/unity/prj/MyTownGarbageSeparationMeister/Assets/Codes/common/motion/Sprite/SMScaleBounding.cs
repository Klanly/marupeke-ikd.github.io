using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトモーション [スケールバウンド]
//
//  指定軸のスケールをバネのようにバウンドさせる

public class SMScaleBounding : MonoBehaviour
{
    [SerializeField]
    float maxScaleX_ = 1.5f;    // X軸の最大スケール値

    [SerializeField]
    float dampSecX_ = 2.0f;     // X軸の振動がほぼ0になるまでの時間

    [SerializeField]
    float freqX_ = 1.0f;        // X軸の周波数

    [ SerializeField ]
    float maxScaleY_ = 1.5f;    // Y軸の最大スケール値

    [SerializeField]
    float dampSecY_ = 2.0f;     // Y軸の振動がほぼ0になるまでの時間

    [SerializeField]
    float freqY_ = 1.0f;        // Y軸の周波数


    private void Awake() {
        defScale_ = transform.localScale;
    }

    private void OnEnable() {
        t_ = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t_ += Time.deltaTime;
        scale_.x = updateScale( t_, defScale_.x, maxScaleX_, dampSecX_, freqX_ );
        scale_.y = updateScale( t_, defScale_.y, maxScaleY_, dampSecY_, freqY_ );
        scale_.z = defScale_.z;
        transform.localScale = scale_;
    }

    float updateScale( float t, float defScale, float maxScale, float dumpSec, float freq ) {
        if ( t >= dumpSec ) {
            return defScale;
        }
        float a = Mathf.Log( 0.001f ) / dumpSec;
        float r = Mathf.Exp( a * t );
        return defScale * ( 1.0f + r * maxScale * Mathf.Cos( 2.0f * Mathf.PI * freq * t ) );
    }

    float t_ = 0.0f;
    Vector3 defScale_ = Vector3.one;
    Vector3 scale_ = Vector3.one;
}
