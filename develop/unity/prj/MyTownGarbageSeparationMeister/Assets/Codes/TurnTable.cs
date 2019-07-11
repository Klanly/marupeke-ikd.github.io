using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターンテーブル
public class TurnTable : MonoBehaviour
{
    [SerializeField]
    int cardPlaceNum_ = 5;

    [SerializeField]
    float turnSec_ = 2.0f;

    [SerializeField]
    Transform turnRoot_;

    [SerializeField]
    bool debugStart_ = false;


    public void turnNext( System.Action finishCallback ) {
        if ( bTurning_ == true )
            return;
        bTurning_ = true;
        float turnDeg = 360.0f / cardPlaceNum_;
        var sQ = turnRoot_.localRotation;
        var eQ = Quaternion.Euler( 0.0f, 0.0f, turnDeg ) * sQ;
        GlobalState.time( turnSec_, (sec, t) => {
            turnRoot_.localRotation = Lerps.Quaternion.easeInOut( sQ, eQ, t );
            return true;
        } ).finish(()=> {
            bTurning_ = false;
            if ( finishCallback != null )
                finishCallback();
        } );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( debugStart_ == true ) {
            debugStart_ = false;
            turnNext( null );
        }        
    }

    bool bTurning_ = false;
}
