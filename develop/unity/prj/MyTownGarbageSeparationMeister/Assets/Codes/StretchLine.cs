using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchLine : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f )]
    float rate_;    // 線の伸び率

    [SerializeField]
    int sepNum_ = 25;   // 線の分割数

    [SerializeField]
    Transform endTarget_;   // 終点のターゲットオブジェクト

    public float getLength() {
        if ( endTarget_ == null )
            return 0.0f;
        return ( transform.position - endTarget_.position ).magnitude;
    }

    public void setRate( float rate ) {
        rate_ = Mathf.Clamp01( rate );
        updateLine();
    }

    private void Awake() {
        line_ = GetComponent<LineRenderer>();
        line_.positionCount = 0;
    }

    void updateLine() {
        if ( endTarget_ == null ) {
            line_.positionCount = 0;
            return;
        }
        int createCount = ( int )( sepNum_ * rate_ );
        line_.positionCount = createCount + 1;
        preSPos_ = transform.position;
        preEPos_ = endTarget_.position;
        for ( int i = 0; i <= createCount; ++i ) {
            float t = ( float )i / sepNum_;
            line_.SetPosition( i, Vector3.Lerp( preSPos_, preEPos_, t ) );
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if ( endTarget_ != null ) {
            // 始点、終点、伸長レートに変化があった時だけ再構築
            if (
                ( transform.position - preSPos_ ).magnitude > 0.0001f ||
                ( endTarget_.transform.position - preEPos_ ).magnitude > 0.0001f ||
                preRate_ != rate_
            ) {
                updateLine();
            }
        }
    }

    LineRenderer line_;
    Vector3 preSPos_ = Vector3.zero;
    Vector3 preEPos_ = Vector3.zero;
    float preRate_ = 0.0f;
}
