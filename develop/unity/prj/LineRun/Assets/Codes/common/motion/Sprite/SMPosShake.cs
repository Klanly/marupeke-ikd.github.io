using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトモーション [位置シェイク]
public class SMPosShake : MonoBehaviour
{
    [SerializeField]
    float maxPosX_ = 2.0f;     // X軸方向の揺れ量

    [SerializeField]
    float dampSecX_ = 2.0f;     // X軸の振動がほぼ0になるまでの時間

    [SerializeField]
    float freqX_ = 10.0f;   // X軸方向の頻度

    [SerializeField]
    float maxPosY_ = 2.0f;     // Y軸方向の揺れ量

    [SerializeField]
    float dampSecY_ = 2.0f;     // Y軸の振動がほぼ0になるまでの時間

    [SerializeField]
    float freqY_ = 10.0f;   // Y軸方向の頻度


    private void Awake() {
        defPos_ = transform.localPosition;
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
        pos_.x = updatePos( t_, defPos_.x, maxPosX_, dampSecX_, freqX_ );
        pos_.y = updatePos( t_, defPos_.y, maxPosY_, dampSecY_, freqY_ );
        pos_.z = defPos_.z;
        transform.localPosition = pos_;
    }

    float updatePos(float t, float defPos, float maxPos, float dumpSec, float freq) {
        if ( t >= dumpSec ) {
            return defPos;
        }
        float a = Mathf.Log( 0.001f ) / dumpSec;
        float r = Mathf.Exp( a * t );
        return defPos + r * maxPos * Mathf.Cos( 2.0f * Mathf.PI * freq * t );
    }

    float t_ = 0.0f;
    Vector3 defPos_ = Vector3.zero;
    Vector3 pos_ = Vector3.zero;
}
