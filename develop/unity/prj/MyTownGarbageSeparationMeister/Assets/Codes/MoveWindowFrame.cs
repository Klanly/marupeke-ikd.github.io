using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindowFrame : MonoBehaviour
{
    [SerializeField]
    WindowFrame frame_;

    [SerializeField]
    Vector2 size_ = Vector2.one;

    [SerializeField]
    float startRotSec_ = 0.5f;

    [SerializeField]
    float startExpandSec_ = 1.0f;

    [SerializeField]
    bool debugStart_ = false;

    [SerializeField]
    bool debugExit_ = false;

    public void start( System.Action<bool> finishCallback = null ) {
        if ( bMoving_ == true ) {
            if ( finishCallback != null )
                finishCallback( false );
            return;
        }
        bMoving_ = true;

        // ピボットを真ん中へ
        Vector2 sp = Vector2.one;
        frame_.setSize( sp );
        frame_.setPivotRate( 0.5f, 0.5f, true );

        // くるっと回って
        Quaternion s = Quaternion.Euler( 0.0f, 0.0f, 90.0f );
        Quaternion e = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
        GlobalState.time( startRotSec_, (sec, t) => {
            frame_.transform.localRotation = Lerps.Quaternion.easeInOut( s, e, t );
            return true;
        }, () => {
            frame_.setPivotRate( 0.0f, 0.0f, true );
        } ).nextTime( startExpandSec_, (sec, t) => {
            var p = Lerps.Vec2.easeOut( sp, size_, t );
            frame_.setSize( p );
            return true;
        } ).finish(()=> {
            bMoving_ = false;
            finishCallback( true );
        } );
    }

    public void exit() {
        if ( bMoving_ == true )
            return;
        bMoving_ = true;

        // ピボットを左端へ
        frame_.setPivotRate( 0.0f, 0.0f, true );

        // 左上にシュリンクしてくるっと回って終わる
        Vector2 sp = Vector2.one;
        Quaternion s = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
        Quaternion e = Quaternion.Euler( 0.0f, 0.0f, 90.0f );
        GlobalState.time( startExpandSec_, (sec, t) => {
            var p = Lerps.Vec2.easeOut( size_, sp, t );
            frame_.setSize( p );
            return true;
        }, () => {
            frame_.setPivotRate( 0.5f, 0.5f, true );
        } ).nextTime( startRotSec_, (sec, t) => {
            frame_.transform.localRotation = Lerps.Quaternion.easeInOut( s, e, t );
            return true;
        } ).finish( () => {
            bMoving_ = false;
        } );
    }

    void Start()
    {
        
    }

    void Update()
    {
        if ( debugStart_ == true ) {
            debugStart_ = false;
            start();
        }        
        if ( debugExit_ == true ) {
            debugExit_ = false;
            exit();
        }
    }

    bool bMoving_ = false;
}
