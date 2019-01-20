using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : SphereSurfaceObject {

    [SerializeField]
    Animator animatior_;

    [SerializeField]
    float boostRate_ = 3.0f;

    [SerializeField]
    float downRate_ = 0.5f;

    [SerializeField]
    float lrBoostRate_ = 3.0f;


    public enum ActionState : int
    {
        ActionState_Idle = 0,
        ActionState_Run = 1
    }

    // 行動変更
    public void setAction( ActionState state )
    {
        animatior_.SetInteger( "state", ( int )state );
    }

    // 宝を捕まえる
    public void catchMe( GameObject treasure )
    {
        // 相手から自分の所へピョーン
        treasure.transform.parent = transform;
        var initP = treasure.transform.localPosition;
        float th = Mathf.PI * 0.25f + 0.5f * Random.value * Mathf.PI;
        float dist = 30.0f;
        var ctP = new Vector3( Mathf.Cos( th ) * dist, Mathf.Sin( th ) * dist, 0.0f );
        var endP = Vector3.zero;
        float t = 0.0f;
        GlobalState.start( () => {
            treasure.transform.localPosition = Bezier3D.getPos( initP, ctP, ctP, endP, t );
            t += 0.02f;
            if ( t >= 1.0f )
                return false;
            return true;
        } ).finish( () => {
            Destroy( treasure.gameObject );
        } );
    }

    // Update is called once per frame
    void Update()
    {
        innerUpdate();
    }

    override protected void innerUpdate()
    {
        // 上キーが押されていたらスピードを上げる
        bool speedUp = Input.GetKey( KeyCode.UpArrow );
        bool speedDown = Input.GetKey( KeyCode.DownArrow );
        cont_.setSpeed( speed_ * (
                speedUp
                ? boostRate_
                : ( speedDown ? downRate_ : 1.0f )
            )
        );

        // 左コントロールが押されていたらLRブースト
        bool boost = Input.GetKey( KeyCode.LeftControl );

        // 左右キーが押されていたら進行方向に対して左右に少し曲がる
        float lr = 0.0f;
        if ( Input.GetKey( KeyCode.LeftArrow ) == true ) {
            lr = -lrSpeed_ * Time.deltaTime * ( boost ? lrBoostRate_ : 1.0f );
        } else if ( Input.GetKey( KeyCode.RightArrow ) == true ) {
            lr = lrSpeed_ * Time.deltaTime * ( boost ? lrBoostRate_ : 1.0f );
        }
        var tangent = transform.forward * speed_ + transform.right * lr;
        cont_.setDir( tangent );

        // モーションを変更
        animatior_.SetFloat( "speed", speedUp ? 5.0f : 3.0f );

        base.innerUpdate();
    }

}
