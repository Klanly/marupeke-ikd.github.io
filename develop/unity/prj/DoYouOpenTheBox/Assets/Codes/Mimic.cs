using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ミミック

public class Mimic : Treasurebox
{
    [SerializeField, Range( 0.0f, 1.0f )]
    float tooth_;

    [SerializeField]
    Transform toothUp_;

    [SerializeField]
    Transform toothDown_;

    [SerializeField]
    Vector3 toothUpDir_ = new Vector3( 0.0f, 0.0f, -0.01f );

    [SerializeField]
    Vector3 toothDownDir_ = new Vector3( 0.0f, 0.00f, -0.01f );

    // 箱よ開け
    override protected void openMotion(float sec) {
        float initToothVal = tooth_;
        GlobalState.time( sec * 0.5f, (_sec, t) => {
            setFlapAngle( Lerps.Float.easeInOut( 0.0f, 160.0f, t ) );
            setToothVal( initToothVal + Lerps.Float.easeOut01Strong( Mathf.Clamp01( t * 3.0f ) ) * ( 1.0f - initToothVal ) );
            return true;
        } );
    }

    public void setToothVal( float val ) {
        tooth_ = val;
    }

    private void OnValidate() {
        setAngle();
        setTooth();
    }

    void setTooth() {
        toothUp_.transform.localPosition = toothUpDir_ * tooth_;
        toothDown_.transform.localPosition = toothDownDir_ * tooth_;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        setAngle();
        setTooth();
    }
}
