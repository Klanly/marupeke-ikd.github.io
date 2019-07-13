﻿using System.Collections;
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
    List<CardPlace> cards_;

    [SerializeField]
    bool debugStart_ = false;


    public class Param {
        public List<Card.Param> cards = new List<Card.Param>();
    }


    public void setup( Param param ) {
        // カードを初期状態に
        foreach ( var c in cards_ ) {
            c.setActive( false );
        }
        param_ = param;
    }

    // 次のターンにする
    public bool turnNext( System.Action<Card.Param> finishCallback ) {
        if ( bTurning_ == true )
            return false;
        bTurning_ = true;

        if ( setNextCard() == false ) {
            return false;
        }

        float turnDeg = 360.0f / cardPlaceNum_;
        var sQ = turnRoot_.localRotation;
        var eQ = Quaternion.Euler( 0.0f, 0.0f, turnDeg ) * sQ;
        GlobalState.time( turnSec_, (sec, t) => {
            turnRoot_.localRotation = Lerps.Quaternion.easeInOut( sQ, eQ, t );
            return true;
        } ).finish(()=> {
            bTurning_ = false;
            if ( finishCallback != null )
                finishCallback( cards_[ curCardIdx_ ].getParam() );
        } );
        return true;
    }

    // 次のカードをセット
    bool setNextCard() {
        if ( curCardIdx_ + 1 >= param_.cards.Count )
            return false;
        curCardIdx_++;
        curCardPlaceIdx_++;
        curCardPlaceIdx_ = curCardPlaceIdx_ % cards_.Count;
        cards_[ curCardPlaceIdx_ ].setParam( param_.cards[ curCardIdx_ ] );
        cards_[ curCardPlaceIdx_ ].setActive( true );
        return true;
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

    Param param_;
    bool bTurning_ = false;
    int curCardPlaceIdx_ = -1;
    int curCardIdx_ = -1;
}
